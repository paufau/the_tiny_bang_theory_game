using Godot;
using System;

public partial class TaskAssigner : Node2D
{
    private double intervalSeconds = 0.1;
    private double lastCall = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        lastCall += delta;

        if (lastCall > intervalSeconds)
        {
            TaskTracker.Instance().AssignPendingTask();
            lastCall -= intervalSeconds;
        }

        QueueRedraw();
    }

    public override void _Draw()
    {
        foreach (var task in TaskTracker.Instance().pendingTasks)
        {
            DrawCircle(task.sourcePosition, 2, Colors.Blue);
        }
    }
}
