using Game.State;
using Game.Task;
using Godot;
using Godot.Collections;

public partial class DiggingProcessor : Node2D
{
    private TileMap tileMap;
    private int wallsLayer = 1;

    private Dictionary<int, bool> plannedBreakings = new();

    public override void _Ready()
    {
        tileMap = GetParent<TileMap>();
    }

    private int GetTaskKey(Vector2 position)
    {
        return (int)(position.X * 1000 + position.Y);
    }

    private void ScheduleDiggingTask(Rect2 rect)
    {
        var tileSize = StatesProvider.NavigatorState.tileMap.TileSet.TileSize;

        for (var x = rect.Position.X + tileSize.X / 2; x < rect.Position.X + rect.Size.X; x += tileSize.X)
        {
            for(var y = rect.Position.Y + tileSize.Y / 2; y < rect.Position.Y + rect.Size.Y; y += tileSize.Y)
            {
                ScheduleDiggingTask(new Vector2(x, y));
            }
        }
    }

    public void EnableBreaking()
    {
        StatesProvider.selectionHelper.StartSelection(ScheduleDiggingTask);
    }

    private void ScheduleDiggingTask(Vector2 position)
    {
        var tilePotision = StatesProvider.NavigatorState.ToMapCoords(position);

        var tileSourceId = tileMap.GetCellSourceId(wallsLayer, tilePotision);
        var tileSourceAtlasCoords = tileMap.GetCellAtlasCoords(wallsLayer, tilePotision);

        var taskKey = GetTaskKey(tilePotision);

        if (tileSourceId == -1 || plannedBreakings.TryGetValue(taskKey, out _)) return;

        plannedBreakings.Add(taskKey, true);

        var breakWallTask = new BreakWallTask(position, this, () =>
        {
            if (tileSourceId == 0 && tileSourceAtlasCoords == new Vector2I(39, 54))
            {
                StatesProvider.Fuel.Update(prev => prev + 1);
            } else
            {
                StatesProvider.Rocks.Update(prev => prev + 1);
            }
        });

        TaskTracker.Instance().PlanTask(breakWallTask);
    }

    public void BreakAt(Vector2 point)
    {
        var tilePosition = tileMap.LocalToMap(ToLocal(point));
        var taskKey = GetTaskKey(tilePosition);

        if (plannedBreakings.TryGetValue(taskKey, out _))
        {
            plannedBreakings.Remove(taskKey);
        }

        tileMap.EraseCell(wallsLayer, tilePosition);
        StatesProvider.NavigatorState.AddPoint(point);
    }
}
