using Game.InputSystem;
using Game.State;
using Game.Task;
using Godot;
using Godot.Collections;
using System;

public partial class BreakProcessor : Node2D
{
    private TileMap tileMap;
    private int wallsLayer = 1;

    [Export]
    private navigator nav;

    private Dictionary<int, bool> plannedBreakings = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        tileMap = GetParent<TileMap>();
    }

    private bool isBreakingEnabled = false;

    public void EnableBreaking()
    {
        isBreakingEnabled = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed()) return;

        InputProcessor.OnGameClick(@event, (InputEventMouseButton mouseButton) =>
        {
            if (mouseButton.ButtonIndex == MouseButton.Right)
            {
                isBreakingEnabled = false;
                QueueRedraw();
            }
            else if (mouseButton.ButtonIndex == MouseButton.Left && isBreakingEnabled)
            {
                ScheduleDiggingTask();
            }
        });
    }

    private int GetTaskKey(Vector2 position)
    {
        return (int)(position.X * 1000 + position.Y);
    }

    private void ScheduleDiggingTask()
    {
        var mousePosition = GetGlobalMousePosition();
        var tilePotision = nav.ToMapCoords(mousePosition);

        var tileSourceId = tileMap.GetCellSourceId(wallsLayer, tilePotision);
        var tileSourceAtlasCoords = tileMap.GetCellAtlasCoords(wallsLayer, tilePotision);

        var taskKey = GetTaskKey(tilePotision);

        if (tileSourceId == -1 || plannedBreakings.TryGetValue(taskKey, out _)) return;

        plannedBreakings.Add(taskKey, true);

        var breakWallTask = new BreakWallTask(GetGlobalMousePosition(), this, () =>
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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (isBreakingEnabled)
        {
            QueueRedraw();
        }
    }

    public override void _Draw()
    {
        if (!isBreakingEnabled) return;

        var position = nav.SnapToNearestTile(GetGlobalMousePosition());
        var tileSize = tileMap.TileSet.TileSize;

        DrawRect(
            new Rect2(position - tileSize / 2, tileSize),
            Colors.DarkOrange
        );
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
        nav.AddPoint(point);
    }
}
