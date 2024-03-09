using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class navigation_pathfinder : Node2D
{
    private CharacterBody2D parent;
    private Vector2? targetPosition;
    private List<Vector2> path = new();
    private int pathAccuracy = 1;
    public navigator nav;

    [Export]
    public float speed = 100;

    public bool IsMoving = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        parent = GetParent<CharacterBody2D>();
        nav = (navigator)GetTree().GetNodesInGroup("navigator")[0];
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        IsMoving = path.Count > 0;
        if (path.Count == 0) return;

        var goToPoint = path.First();

        parent.Velocity = parent.Position.DirectionTo(goToPoint) * speed;

        if (parent.Position.DistanceTo(goToPoint) < pathAccuracy)
        {
            path.Remove(goToPoint);
        } else
        {
            parent.MoveAndSlide();
        }
    }

    public void MoveTo(Vector2 point)
    {
        targetPosition = point;
        var currentPoint = nav.aStar.GetClosestPoint(parent.Position);
        var targetPoint = nav.aStar.GetClosestPoint((Vector2)targetPosition);
        path = nav.aStar.GetPointPath(currentPoint, targetPoint).ToList();
        IsMoving = path.Count > 0;

        if (!IsMoving)
        {
            targetPosition = null;
        }
    }
}
