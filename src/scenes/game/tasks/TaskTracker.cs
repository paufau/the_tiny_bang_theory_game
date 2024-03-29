using Game.Task;
using System.Collections.Generic;

public class TaskTracker
{
    private static TaskTracker _Instance;
    public static TaskTracker Instance()
    {
        if (_Instance == null)
        {
            _Instance = new();
        }

        return _Instance;
    }

    private List<pawn_controller> busyPawns = new();
    private List<pawn_controller> freePawns = new();

    public List<AbstractTask> pendingTasks = new();

    public void AddPawn(pawn_controller pawn)
    {
        freePawns.Add(pawn);
    }

    public void PlanTask(AbstractTask task)
    {
        pendingTasks.Add(task);
        AssignPendingTask();
    }

    public AbstractTask? GetAvailableTaskFor(pawn_controller pawn)
    {
        foreach (var task in pendingTasks)
        {
            if (task.IsSourceReachable(pawn.GlobalPosition) && task.IsReadyForAssigning())
            {
                return task;
            }
        }

        return null;
    }

    public void AssignPendingTask()
    {
        foreach (var pawn in new List<pawn_controller>(freePawns))
        {
            var task = GetAvailableTaskFor(pawn);

            if (task == null)
            {
                task = new RelaxTask();
            };

            task.Plan(pawn);

            pendingTasks.Remove(task);
            freePawns.Remove(pawn);
            busyPawns.Add(pawn);

            pawn.AI.AddTask(task);

            task.OnFinish(() =>
            {
                busyPawns.Remove(pawn);
                freePawns.Add(pawn);
            });
        }
    }
}
