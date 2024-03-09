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

    private bool isBreak = true;

    private bool isPressed = false;

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton &&
            ((InputEventMouseButton)@event).ButtonIndex == MouseButton.Right &&
            !@event.IsPressed())
        {
            if (isBreak)
            {
                TaskTracker.Instance().PlanTask(new BreakWallTask(GetGlobalMousePosition(), this));
            }
            else
            {
                AddFloatingAt(GetGlobalMousePosition());
            }
        }
        base._Input(@event);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Space))
        {
            isBreak = !isBreak;
        }
    }

    public void BreakAt(Vector2 point)
    {
        tileMap.EraseCell(wallsLayer, tileMap.LocalToMap(ToLocal(point)));
        nav.AddPoint(point);
    }

    public void AddFloatingAt(Vector2 point)
    {
        nav.AddFloatingPoint(point);
    }
}
