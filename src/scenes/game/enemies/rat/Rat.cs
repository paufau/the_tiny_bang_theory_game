using Game.InputSystem;
using Game.Pawn.AI;
using Game.State;
using Godot;
using System;

public partial class Rat : CharacterBody2D
{
    private readonly int secondsForMovingAttempt = 1;
    private double secondsFromLastAttempt = 0;

    private navigation_pathfinder pathfinder;
    private Sprite2D sprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        pathfinder = GetNode<navigation_pathfinder>("Pathfinder");
        sprite = GetNode<Sprite2D>("Sprite2D");

        Position = StatesProvider.NavigatorState.GetRandomPointGlobalPosition();
        GoToRandomPoint();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        sprite.FlipH = Velocity.X > 0;

        secondsFromLastAttempt += delta;

        if (secondsFromLastAttempt < secondsForMovingAttempt) return;

        secondsFromLastAttempt = 0;
        if (!pathfinder.IsMoving)
        {
            GoToRandomPoint();
        }
    }

    private void GoToRandomPoint()
    {
        var goToPosition = StatesProvider.NavigatorState.GetRandomPointGlobalPosition();
        pathfinder.MoveTo(goToPosition);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        InputProcessor.OnGameClick(@event, (mouse) =>
        {
            if (mouse.ButtonIndex == MouseButton.Left &&
                GlobalPosition.DistanceTo(GetGlobalMousePosition()) < 32)
            {
                StatesProvider.Meat.Update(prev => prev + 1);
                QueueFree();
            }
        });
    }
}
