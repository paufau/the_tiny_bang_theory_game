using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Game.State;
using Game.Task;
using Godot;

namespace Game.Pawn.AI
{
	public partial class PawnAI : Node
	{
		private Queue<AbstractTask> tasks = new();
		private AbstractTask? active = null;
		private Action? onTaskDone = null;
		private bool shouldTerminate = false;

		public void AddTask(AbstractTask task, Action onDone)
		{
			onTaskDone = onDone;
			tasks.Enqueue(task);
		}

		public void AddTask(AbstractTask task)
		{
			tasks.Enqueue(task);
		}

		public void TerminateAllPlanned()
		{
			shouldTerminate = true;
		}

		private void _Terminate()
		{
            onTaskDone?.Invoke();
            active?.Finish();

            foreach (var task in tasks.ToList())
            {
                task.Finish();
            }

            tasks = new();
            active = null;
            onTaskDone = null;
			shouldTerminate = false;
        }

		public override void _Process(double delta)
		{
			if (active == null)
			{
				if (tasks.Count == 0)
				{
					onTaskDone?.Invoke();
					onTaskDone = null;
					return;
				};

				active = tasks.Dequeue();
				active.Do();
			}

			if (active.IsDone())
			{
				active.Complete();
                active.Finish();
                active = null;
			}

			if (shouldTerminate)
			{
				_Terminate();
			}
		}

        public override void _Ready()
        {
			StatesProvider.timeCycles.OnUpdateTime += HandleTimePeriodUpdate;
        }

        public override void _ExitTree()
        {
            StatesProvider.timeCycles.OnUpdateTime -= HandleTimePeriodUpdate;
        }

        private void HandleTimePeriodUpdate(TimeCycles.TimePeriod period)
		{
			if (period.periodType != TimeCycles.TimePeriodType.NIGHT) return;

			StatesProvider.CookedMeat.Update(prev => prev - 1);
		}
    }
}

