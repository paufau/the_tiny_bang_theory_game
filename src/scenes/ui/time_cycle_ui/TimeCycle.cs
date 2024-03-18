using Game.State;
using Godot;
using System;

public partial class TimeCycle : Control
{
    private Label label;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        label = GetNode<Label>("Label");

        StatesProvider.timeCycles.OnUpdateTime += HandleTimeCycleUpdate;
        HandleTimeCycleUpdate(StatesProvider.timeCycles.currentPeriod);
    }

    public override void _ExitTree()
    {
        StatesProvider.timeCycles.OnUpdateTime -= HandleTimeCycleUpdate;
    }

    private void HandleTimeCycleUpdate(TimeCycles.TimePeriod period)
    {
        label.Text = period.name;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
