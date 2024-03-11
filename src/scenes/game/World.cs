using Game.State;
using Godot;
using System;
using System.Linq;

public partial class World : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        StatesProvider.world = this;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public Node2D? GetNearestInGroup(string group, Vector2 position)
    {
        var nodesInGroup = GetTree().GetNodesInGroup(group);

        if (nodesInGroup.Count == 0) return null;

        Node2D? nearest = null;
        int pathLength = 0;

        GD.Print("Nodes in group: ", nodesInGroup.Count);

        foreach(Node2D node in nodesInGroup)
        {
            var path = StatesProvider.NavigatorState.GetPath(position, node.GlobalPosition);
            GD.Print("Path: ", path.Count);
            if ((nearest == null && path.Count > 0) || pathLength > path.Count)
            {
                nearest = node;
                pathLength = path.Count;
            } 
        }

        return nearest;
    }
}
