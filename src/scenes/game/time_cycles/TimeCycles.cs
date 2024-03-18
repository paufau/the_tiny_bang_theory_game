using Game.State;
using Godot;
using System.Collections.Generic;

public partial class TimeCycles : Node
{
    public enum TimePeriodType
    {
        DAY, NIGHT, CUSTOM
    }

    public class TimePeriod
    {
        public TimePeriodType periodType;
        public double durationSeconds;
        public string name;

        public TimePeriod(double durationSeconds, string name, TimePeriodType periodType)
        {
            this.durationSeconds = durationSeconds;
            this.name = name;
            this.periodType = periodType;
        }
    }

    public delegate void UpdateTimeCycleEvent(TimePeriod nextPeriod);

    public event UpdateTimeCycleEvent OnUpdateTime;

    private List<TimePeriod> periods = new()
    {
        new TimePeriod(3, "Day", TimePeriodType.DAY),
        new TimePeriod(2, "Night", TimePeriodType.NIGHT),
    };

    private int currentPeriodIndex = -1;
    public TimePeriod currentPeriod;

    private double timeFromPeriotStart = 0;

    private void MoveToNextPeriod()
    {
        timeFromPeriotStart = 0;

        int nextPeriodIndex = currentPeriodIndex + 1;

        if (nextPeriodIndex >= periods.Count)
        {
            nextPeriodIndex = 0;
        }

        currentPeriod = periods[nextPeriodIndex];

        OnUpdateTime?.Invoke(currentPeriod);
        currentPeriodIndex = nextPeriodIndex;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        StatesProvider.timeCycles = this;
        MoveToNextPeriod();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        timeFromPeriotStart += delta;

        if (currentPeriod.durationSeconds <= timeFromPeriotStart)
        {
            MoveToNextPeriod();
        }
    }
}
