using Game.BuildingSystem;
using Game.State;
using Godot;
using System;

public partial class Ladder : Node2D, IBuildHandler
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void OnBuild()
    {
        StatesProvider.NavigatorState.AddFloatingPoint(GlobalPosition);
    }
}
