using Game.BuildingSystem;
using Game.Task;
using Godot;

public partial class Furnace : Node2D, IBuildHandler
{
    private CookMeatTask cookMeatTask;

    private void PlanTask()
    {
        cookMeatTask = new CookMeatTask();
        cookMeatTask.SetSource(GlobalPosition);

        cookMeatTask.OnFinish(PlanTask);

        TaskTracker.Instance().PlanTask(cookMeatTask);
    }

    public void OnBuild()
    {
        PlanTask();
    }
}
