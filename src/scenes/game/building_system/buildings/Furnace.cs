using Game.Task;
using Godot;
using System;

public partial class Furnace : Node2D
{
    private CookMeatTask cookMeatTask;

    public override void _Ready()
    {
        PlanTask();
    }

    private void PlanTask()
    {
        cookMeatTask = new CookMeatTask();
        cookMeatTask.SetSource(GlobalPosition);

        cookMeatTask.OnDone(PlanTask);

        TaskTracker.Instance().PlanTask(cookMeatTask);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
