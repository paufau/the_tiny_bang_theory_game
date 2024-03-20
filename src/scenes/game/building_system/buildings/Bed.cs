using Game.BuildingSystem;
using Game.State;
using Godot;
using System.Collections.Generic;

public partial class Bed : Node2D, IBuildHandler, IBreakHandler
{
    private int pawnsToSpawn = 2;
    private List<Node2D> pawnInstances = new();

    private static PackedScene pawnScene = ResourceLoader.Load<PackedScene>("res://src/scenes/game/pawn/pawn.tscn");

    public void OnBuild()
    {
        for (var i = 0; i < pawnsToSpawn; i++)
        {
            var instance = (Node2D)pawnScene.Instantiate();
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
