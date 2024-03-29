using Game.BuildingSystem;
using Game.InputSystem;
using Godot;
using System.Collections.Generic;

public partial class BuildingProcessor : Node2D
{
    private TileMap tileMap;
    private int tileSize;

    [Export]
    private navigator nav;

    private Occupancy occupancy = new();

    private int wallsLayer = 1;
    private int buildingLayer = 3;

    public Node2D? activeBuilding;
    public Building? activeBuildingIntent;

    public abstract class Building
    {
        public abstract PackedScene GetScene();
        public virtual Vector2I GetSize()
        {
            return Vector2I.One;
        }
    }

    public class B_Ladder : Building
    {
        public override PackedScene GetScene()
        {
            return ResourceLoader.Load<PackedScene>("res://src/scenes/game/building_system/buildings/ladder.tscn");
        }
    }

    public class B_Box : Building
    {
        public override PackedScene GetScene()
        {
            return ResourceLoader.Load<PackedScene>("res://src/scenes/game/building_system/buildings/Box.tscn");
        }
    }

    public class B_Bed : Building
    {
        public override PackedScene GetScene()
        {
            return ResourceLoader.Load<PackedScene>("res://src/scenes/game/building_system/buildings/Bed.tscn");
        }

        public override Vector2I GetSize()
        {
            return new Vector2I(3, 2);
        }
    }

    public class B_Furnace : Building
    {
        public override PackedScene GetScene()
        {
            return ResourceLoader.Load<PackedScene>("res://src/scenes/game/building_system/buildings/Furnace.tscn");
        }

        public override Vector2I GetSize()
        {
            return new Vector2I(1, 2);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        tileMap = GetParent<TileMap>();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed() || InputProcessor.isUIHovered) return;

        InputProcessor.OnGameClick(@event, (InputEventMouseButton mouseButton) =>
        {
            if (activeBuilding != null && mouseButton.ButtonIndex == MouseButton.Left)
            {
                BuildAt();
            }
            else if (activeBuilding != null && mouseButton.ButtonIndex == MouseButton.Right)
            {
                ClearBuildingIntent();
            }
        });
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (activeBuilding == null) return;

        var mousePosition = GetGlobalMousePosition();
        var snappedPosition = nav.SnapToNearestTileOrigin(mousePosition);
        activeBuilding.GlobalPosition = snappedPosition;

        QueueRedraw();
    }

    public override void _Draw()
    {
        if (activeBuilding == null) return;

        var occupiedCells = GetOccupiedCells();

        foreach (var occupiedCell in occupiedCells)
        {
            DrawRect(
                new Rect2(
                    nav.ToGlobalCoords(occupiedCell) - nav.tileMap.TileSet.TileSize / 2 ,
                    nav.tileMap.TileSet.TileSize),
                new Color(1f, 0f, 0f, 0.4f)
            );
        }
    }

    public Node2D InstantiateBuilding(Building building)
    {
        var instance = (Node2D)building.GetScene().Instantiate();
        AddChild(instance);
        return instance;
    }

    public void BuildAt()
    {
        var occupiedCells = GetOccupiedCells();

        if (occupiedCells.Count > 0)
        {
            return;
        }

        var buildingClone = (Node2D)activeBuilding.Duplicate();
        AddChild(buildingClone);

        if (buildingClone is IBuildHandler buildHandler)
        {
            buildHandler.OnBuild();
        }

        occupancy.Occupy(new Rect2I(
            position: nav.ToMapCoords(buildingClone.GlobalPosition),
            size: activeBuildingIntent.GetSize()));

        foreach(var group in buildingClone.GetGroups())
        {
            buildingClone.AddToGroup(group + "_placed");
        }
    }

    public void SetBuildingIntent(Building building)
    {
        if (activeBuilding != null)
        {
            ClearBuildingIntent();
        } 
        activeBuildingIntent = building;
        activeBuilding = InstantiateBuilding(building);
    }

    public void ClearBuildingIntent()
    {
        activeBuilding.QueueFree();
        activeBuilding = null;
        activeBuildingIntent = null;
        QueueRedraw();
    }

    private List<Vector2I> GetOccupiedCells()
    {
        var occupiedCells = occupancy.GetOccupiedCells(
            nav.ToMapCoords(activeBuilding.GlobalPosition),
            activeBuildingIntent.GetSize()
        );

        Occupancy.IterateCells(new Rect2I(
            nav.ToMapCoords(activeBuilding.GlobalPosition),
            activeBuildingIntent.GetSize()
        ), (x, y) =>
        {
            var checkVector = new Vector2I(x, y);
            if (tileMap.GetCellTileData(wallsLayer, checkVector) != null)
            {
                occupiedCells.Add(checkVector);
            }
        });

        return occupiedCells;
    }
}
