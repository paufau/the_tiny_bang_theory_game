using Game.Task;
using Godot;
using System;

public partial class BreakProcessor : Node2D
{
    private TileMap tileMap;
    private int wallsLayer = 1;

    [Export]
    private navigator nav;

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

        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Right)
            {
                isBreakingEnabled = false;
            } else if (mouseButton.ButtonIndex == MouseButton.Left && isBreakingEnabled)
            {
                TaskTracker.Instance().PlanTask(new BreakWallTask(GetGlobalMousePosition(), this));
            }
        }
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
        tileMap.EraseCell(wallsLayer, tileMap.LocalToMap(ToLocal(point)));
        nav.AddPoint(point);
    }
}
