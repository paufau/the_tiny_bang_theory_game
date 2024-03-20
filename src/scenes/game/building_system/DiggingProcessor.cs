using Game.InputSystem;
using Game.State;
using Game.Task;
using Godot;
using Godot.Collections;
using System;

public partial class DiggingProcessor : Node2D
{
    private TileMap tileMap;
    private int wallsLayer = 1;

    [Export]
    private navigator nav;

    private TextureRect? diggingRect;

    private Dictionary<int, bool> plannedBreakings = new();

    private Vector2? initialDraggingPosition = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        tileMap = GetParent<TileMap>();
        diggingRect = GetNode<TextureRect>("DiggingRect");
    }

    private bool isBreakingEnabled = false;

    public void EnableBreaking()
    {
        isBreakingEnabled = true;
        diggingRect.Show();
    }

    public void DisableBreaking()
    {
        diggingRect.Hide();
        initialDraggingPosition = null;
        isBreakingEnabled = false;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!isBreakingEnabled) return;

        if (@event.IsPressed() && initialDraggingPosition == null)
        {
            InputProcessor.OnGameClick(@event, (InputEventMouseButton mouseButton) =>
            {
                if (mouseButton .ButtonIndex == MouseButton.Left)
                {
                    initialDraggingPosition = GetGlobalMousePosition();
                }
            });
            return;
        };

        if (@event.IsPressed()) return;

        InputProcessor.OnGameClick(@event, (InputEventMouseButton mouseButton) =>
        {
            if (mouseButton.ButtonIndex == MouseButton.Right)
            {
                DisableBreaking();
            }
            else if (mouseButton.ButtonIndex == MouseButton.Left)
            {
                ScheduleDiggingTask(GetDiggingRect());
                initialDraggingPosition = null;
            }
        });
    }

    private int GetTaskKey(Vector2 position)
    {
        return (int)(position.X * 1000 + position.Y);
    }

    private Rect2 GetDiggingRect()
    {
        Vector2 mousePosition = GetGlobalMousePosition();
        Vector2 initialPosition = mousePosition;

        Vector2 tileSizeX = new Vector2(nav.tileMap.TileSet.TileSize.X, 0);
        Vector2 tileSizeY = new Vector2(0, nav.tileMap.TileSet.TileSize.Y);

        if (initialDraggingPosition != null)
        {
            initialPosition = (Vector2)initialDraggingPosition;
        }

        initialPosition -= tileSizeY;

        Vector2 fixedMousePosition = mousePosition - tileSizeY;

        if (mousePosition.X > initialPosition.X)
        {
            fixedMousePosition += tileSizeX;
        }

        if (mousePosition.Y > initialPosition.Y)
        {
            fixedMousePosition += tileSizeY;
        }

        Rect2 rect = new(nav.SnapToNearestTileOrigin(initialPosition), nav.tileMap.TileSet.TileSize);
        rect = rect.Expand(nav.SnapToNearestTileOrigin(fixedMousePosition));

        return rect;
    }

    private void DrawDraggingRect()
    {
        var rect = GetDiggingRect();
        diggingRect.SetPosition(rect.Position);
        diggingRect.SetSize(rect.Size);
    }

    private void ScheduleDiggingTask(Rect2 rect)
    {
        var tileSize = nav.tileMap.TileSet.TileSize;

        for (var x = rect.Position.X + tileSize.X / 2; x < rect.Position.X + rect.Size.X; x += tileSize.X)
        {
            for(var y = rect.Position.Y + tileSize.Y / 2; y < rect.Position.Y + rect.Size.Y; y += tileSize.Y)
            {
                ScheduleDiggingTask(new Vector2(x, y));
            }
        }
    }

    private void ScheduleDiggingTask(Vector2 position)
    {
        var tilePotision = nav.ToMapCoords(position);

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

    public override void _Process(double delta)
    {
        if (isBreakingEnabled)
        {
            //diggingRect.GlobalPosition = nav.SnapToNearestTileOrigin(GetGlobalMousePosition());
            DrawDraggingRect();
        }
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
