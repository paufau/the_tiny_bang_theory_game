using Game.BuildingSystem;
using Game.State;
using Godot;
using System;
using System.Collections.Generic;

public partial class Bed : Node2D, IBuildHandler, IBreakHandler
{
    private int pawnsToSpawn = 2;
    private List<Node2D> pawnInstances = new();

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
        var pawn = ResourceLoader.Load<PackedScene>("res://src/scenes/game/pawn/pawn.tscn");
        for (var i = 0; i < pawnsToSpawn; i++)
        {
            var instance = (Node2D)pawn.Instantiate();
            instance.GlobalPosition = GlobalPosition;
            StatesProvider.world.AddChild(instance);
            pawnInstances.Add(instance);
        }
    }

    public void OnBreak()
    {
        foreach (var pawn in pawnInstances)
        {
            pawn.QueueFree();
        }
    }
}
