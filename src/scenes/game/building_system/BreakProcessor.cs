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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsMouseButtonPressed(MouseButton.Right))
        {
            if (isBreak)
            {
                BreakAt(GetGlobalMousePosition());
            } else
            {
                AddFloatingAt(GetGlobalMousePosition());
            }
            
        }

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
