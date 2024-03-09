using Game.Pawn.AI;
using Godot;

public partial class pawn_controller : CharacterBody2D
{
    public PawnAI AI;
    public navigation_pathfinder pathfinder;

    public override void _Ready()
    {
        TaskTracker.Instance().AddPawn(this);

        pathfinder = (navigation_pathfinder)GetNode<Node2D>("NavigationPathfinder");
        AI = (PawnAI)GetNode<Node>("AI");
    }

    public override void _Process(double delta)
    {

    }
}
