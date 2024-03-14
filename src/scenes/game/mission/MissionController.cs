using Game.State;
using Godot;
using System;
using System.Collections.Generic;

public partial class MissionController : Node2D
{
    private class Mission
    {
        public double durationSeconds;
        public List<string> onboardingText;
    }

    [Export]
    private ProgressBar progressBar;

    private double currentMissionSecondsProgress = 0;

    private int currentMissionIndex = 0;
    private List<Mission> missions = new()
    {
        new Mission()
        {
            durationSeconds = 60,
            onboardingText = new()
            //{
            //    "Welcome1",
            //    "Welcome1_2",
            //}
        },
        new Mission()
        {
            durationSeconds = 3 ,
            onboardingText = new()
            {
                "Welcome2",
            }
        },
    };

    public void FinishGame()
    {
        QueueFree();
    }

    public void RunMission(int index)
    {
        if (index >= missions.Count)
        {
            FinishGame();
            return;
        };

        var mission = missions[index];
        currentMissionIndex = index;
        currentMissionSecondsProgress = 0;

        StatesProvider.alert.ShowAlert(mission.onboardingText);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        RunMission(0);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        currentMissionSecondsProgress += delta;

        var currentMission = missions[currentMissionIndex];
        progressBar.Value = Mathf.Min(100, Mathf.Max(0, 100 * (currentMission.durationSeconds - currentMissionSecondsProgress) / currentMission.durationSeconds));

        if (currentMissionSecondsProgress > currentMission.durationSeconds)
        {
            RunMission(currentMissionIndex + 1);
        }
    }
}
