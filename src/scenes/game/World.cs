using Game.State;
using Godot;
using System;
using System.Linq;

public partial class World : Node2D
{
    private int ratsSpawnRateSeconds = 7;
    private double secondsFromLastSpawn = 0;
    private PackedScene ratScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        StatesProvider.world = this;
        ratScene = ResourceLoader.Load<PackedScene>("res://src/scenes/game/enemies/rat/rat.tscn");
        SpawnRat();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        secondsFromLastSpawn += delta;
        if (secondsFromLastSpawn > ratsSpawnRateSeconds)
        {
            SpawnRat();
        }
    }

    private void SpawnRat()
    {
        secondsFromLastSpawn = 0;
        var ratInstance = (Node2D)ratScene.Instantiate();
        AddChild(ratInstance);
    }

    public Node2D? GetNearestInGroup(string group, Vector2 position)
    {
        var nodesInGroup = GetTree().GetNodesInGroup(group);

        if (nodesInGroup.Count == 0) return null;

        Node2D? nearest = null;
        int pathLength = 0;

        foreach(Node2D node in nodesInGroup)
        {
            var path = StatesProvider.NavigatorState.GetPath(position, node.GlobalPosition);
            if ((nearest == null && path.Count > 0) || pathLength > path.Count)
            {
                nearest = node;
                pathLength = path.Count;
            } 
        }

        return nearest;
    }
}
