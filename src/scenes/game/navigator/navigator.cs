using Game.State;
using Godot;
using System.Collections.Generic;
using System.Linq;
using Utils.Graph;
using Utils.ID;

public partial class navigator : Node2D
{
    // fields
    
    private IDGenerator idGenerator = new();

    [Export]
    public TileMap tileMap;

    private int navLayer = 2;
    private int wallsLayer = 1;
    public AStar2D aStar = new();
    private Graph<NavigationPathPoint> aStarGraphRepresentation = new();


    private List<Vector2I> NeighborPoints = new()
    {
        Vector2I.Left,
        Vector2I.Left + Vector2I.Down,
        Vector2I.Left + Vector2I.Up,
        Vector2I.Right,
        Vector2I.Right + Vector2I.Down,
        Vector2I.Right + Vector2I.Up,
        Vector2I.Up,
        Vector2I.Down,
    };

    private Dictionary<float, Dictionary<float, NavigationPathPoint>> positionToId = new();

    // methods

    private Vector2? GetPointAt(Vector2 point, List<Vector2> allPoints)
    {
        foreach (var addedPoint in allPoints)
        {
            if (point.X == addedPoint.X && point.Y == addedPoint.Y)
            {
                return point;
            }
        }

        return null;
    }

    private List<Vector2> GetGroundedPoints()
    {
        List<Vector2> addedPoints = new();
        var navCells = tileMap.GetUsedCellsById(navLayer, sourceId: 0, atlasCoords: new Vector2I(6, 1));

        foreach (var cell in navCells)
        {
            var nextY = cell.Y + 1;

            while (tileMap.GetCellTileData(navLayer, new Vector2I(cell.X, nextY)) != null)
            {
                nextY++;
            }

            nextY--;

            var point = new Vector2I(cell.X, nextY);

            if (GetPointAt(point, addedPoints) == null)
            {
                addedPoints.Add(point);
            }
        }

        return addedPoints;
    }

    private Vector2I ToGroundedPoint(Vector2I mapPoint)
    {
        Vector2I iterPoint = new Vector2I((int)mapPoint.X, (int)mapPoint.Y);
        iterPoint.Y++;
        GD.Print(tileMap.GetUsedRect());
        while (tileMap.GetCellTileData(wallsLayer, iterPoint) == null)
        {
            iterPoint.Y++;
        }
        iterPoint.Y--;
        return iterPoint;
    }

    private Vector2 TileMapToGlobalCoords(Vector2I point)
    {
        return ToGlobal(tileMap.MapToLocal(point));
    }

    private Vector2 TileMapToGlobalCoords(Vector2 point)
    {
        return TileMapToGlobalCoords((Vector2I)point);
    }

    private List<Vector2> TileMapToGlobalCoords(List<Vector2I> tileMapPoints)
    {
        return tileMapPoints.ConvertAll(TileMapToGlobalCoords).ToList();
    }

    private List<Vector2> TileMapToGlobalCoords(List<Vector2> tileMapPoints)
    {
        return tileMapPoints.ConvertAll(TileMapToGlobalCoords).ToList();
    }

    private NavigationPathPoint? GetInstance(Vector2 point)
    {
        NavigationPathPoint pathPoint;
        Dictionary<float, NavigationPathPoint> xPoints;

        if (positionToId.TryGetValue(point.X, out xPoints))
        {
            if (xPoints.TryGetValue(point.Y, out pathPoint))
            {
                return pathPoint;
            }
        }

        return null;
    }

    private void RemoveInstance(Vector2 point)
    {
        NavigationPathPoint pathPoint;
        Dictionary<float, NavigationPathPoint> xPoints;

        if (positionToId.TryGetValue(point.X, out xPoints))
        {
            if (xPoints.TryGetValue(point.Y, out pathPoint))
            {
                xPoints.Remove(point.Y);
            }
        }
    }

    private NavigationPathPoint CreateInstance(Vector2 point)
    {
        NavigationPathPoint pathPoint;
        Dictionary<float, NavigationPathPoint> xPoints;

        if (positionToId.TryGetValue(point.X, out xPoints))
        {
            if (xPoints.TryGetValue(point.Y, out pathPoint))
            {
                return pathPoint;
            }
            else
            {
                pathPoint = new(idGenerator.Next(), point);
                xPoints.Add(point.Y, pathPoint);
                return pathPoint;
            }
        }
        else
        {
            positionToId.Add(point.X, new());
        }

        pathPoint = new(idGenerator.Next(), point);
        positionToId[point.X].Add(point.Y, pathPoint);
        return pathPoint;
    }

    private void DisconnectPoint(Vector2 mapPoint)
    {
        var pointInstance = GetInstance(mapPoint);
        if (pointInstance == null) return;

        aStar.RemovePoint(pointInstance.id);
        aStarGraphRepresentation.RemoveNode(pointInstance);
        RemoveInstance(pointInstance.position);
    }

    private NavigationPathPoint SafeAddPointInstance(Vector2I mapPoint)
    {
        NavigationPathPoint pointInstance = GetInstance(mapPoint);

        if (pointInstance != null) return pointInstance;

        pointInstance = CreateInstance(mapPoint);
        aStar.AddPoint(pointInstance.id, TileMapToGlobalCoords(pointInstance.position));
        aStarGraphRepresentation.AddNode(pointInstance);
        return pointInstance;
    }

    private NavigationPathPoint? AddPointWithChecks(Vector2I mapPoint)
    {
        var groundedPoint = ToGroundedPoint(mapPoint);

        Vector2I upperPoint = mapPoint + Vector2I.Up;
        var upperBlock = tileMap.GetCellTileData(wallsLayer, upperPoint);
        if (upperBlock != null)
        {
            // Has wall above
            if (groundedPoint.Y == mapPoint.Y)
            {
                return null;
            }
        } else
        {
            // Has empty space above
            DisconnectPoint(upperPoint);
        }

        return SafeAddPointInstance(groundedPoint);
    }

    private void ConnectGroundedPoints(List<Vector2> points)
    {
        foreach (var point in points)
        {
            AddPointWithChecks((Vector2I)point);
        }

        foreach (var point in points)
        {
            var pointInstance = GetInstance(point);
            if (pointInstance == null) continue;
            ConnectPointInstance(pointInstance);
        }
    }

    private void ConnectPointInstance(NavigationPathPoint pointInstance)
    {
        foreach (var shift in NeighborPoints)
        {
            var shiftedPoint = pointInstance.position + shift;
            var shiftedPointInstance = GetInstance(shiftedPoint);

            if (shiftedPointInstance == null) continue;

            aStar.ConnectPoints(pointInstance.id, shiftedPointInstance.id);
            aStarGraphRepresentation.Connect(pointInstance, shiftedPointInstance);
        }
    }

    public void AddPoint(Vector2 globalPoint)
    {
        var pointInstance = AddPointWithChecks(tileMap.LocalToMap(ToLocal(globalPoint)));

        if (pointInstance != null)
        {
            ConnectPointInstance(pointInstance);
        }
    }

    public void AddFloatingPoint(Vector2 globalPoint)
    {
        var mapPoint = tileMap.LocalToMap(ToLocal(globalPoint));
        var pointInstance = SafeAddPointInstance(mapPoint);
        pointInstance.isFloating = true;
        ConnectPointInstance(pointInstance);
    }

    public override void _Ready()
    {
        StatesProvider.NavigatorState = this;
        ConnectGroundedPoints(GetGroundedPoints());
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        foreach (var point in aStarGraphRepresentation.Nodes)
        {
            DrawCircle(
                TileMapToGlobalCoords(point.position),
                3,
                point.isFloating ? Colors.Orange : Colors.AliceBlue
            );
        }

        foreach (var connection in aStarGraphRepresentation.Lines)
        {
            DrawLine(
                TileMapToGlobalCoords(connection.Begin.position),
                TileMapToGlobalCoords(connection.End.position),
                Colors.Aqua,
                width: 3
            );
        }

        base._Draw();
    }
}

