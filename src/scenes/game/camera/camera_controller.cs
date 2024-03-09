using Godot;
using System;

public partial class camera_controller : Camera2D
{
    private Vector2 initialMousePosition;
    private bool isMoving;

    public override void _Process(double delta)
    {
        MakeMovement();
    }

    private void MakeMovement()
    {
        Vector2 currentMousePosition = GetLocalMousePosition();

        if (isMoving)
        {
            Position = initialMousePosition - currentMousePosition;
        }

        bool isMovingButtonPressed = Input.IsMouseButtonPressed(MouseButton.Middle);
        if (isMovingButtonPressed && !isMoving)
        {
            isMoving = true;
            initialMousePosition = Position + currentMousePosition;
        }
        else if (isMoving && !isMovingButtonPressed)
        {
            isMoving = false;
        }
    }
}
