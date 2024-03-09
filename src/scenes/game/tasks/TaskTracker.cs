using Game.Task;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    private List<AbstractTask> pendingTasks = new();

    public void AddPawn(pawn_controller pawn)
    {
        freePawns.Add(pawn);
    }

    public void PlanTask(AbstractTask task)
    {
        pendingTasks.Add(task);
        AssignPendingTask();
    }

    public void AssignPendingTask()
    {
        GD.Print("TASK", freePawns.Count, pendingTasks.Count);
        if (freePawns.Count == 0) return;
        if (pendingTasks.Count == 0) return;

        var pawn = freePawns.First();
        var task = pendingTasks.First();

        task.Plan(pawn);

        pendingTasks.Remove(task);
        freePawns.Remove(pawn);
        busyPawns.Add(pawn);

        pawn.AI.AddTask(task, () =>
        {
            GD.Print("IS DONE");
            busyPawns.Remove(pawn);
            freePawns.Add(pawn);
            Task.Run(() =>
            {
                AssignPendingTask();
            });
        });
    }
}