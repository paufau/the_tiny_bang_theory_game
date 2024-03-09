using Godot;
using System;

public partial class pawn_controller : CharacterBody2D
{
    private float speed = 100;

    [Export]
    public navigation_pathfinder pathfinder;

    public override void _Ready()
    {

    }

    private Vector2I nextPosition;

    public override void _Process(double delta)
    {
        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            var mousePosition = GetGlobalMousePosition();
            pathfinder.MoveTo(mousePosition);
        }
    }
}
