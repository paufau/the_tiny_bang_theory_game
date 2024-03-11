using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Task;
using Godot;

namespace Game.Pawn.AI
{
	public partial class PawnAI : Node
	{
		public delegate void TaskDoneDelegate();

		private Queue<AbstractTask> tasks = new();
		private AbstractTask? active = null;
		private TaskDoneDelegate? onTaskDone = null;

		public void AddTask(AbstractTask task, TaskDoneDelegate onDone)
		{
			onTaskDone = onDone;
			tasks.Enqueue(task);
		}

		public void AddTask(AbstractTask task)
		{
			tasks.Enqueue(task);
		}

		public override void _Process(double delta)
		{
			if (active == null)
			{
				if (tasks.Count == 0)
				{
					if (onTaskDone != null)
					{
						onTaskDone.Invoke();
						onTaskDone = null;
					}
					return;
				};

				active = tasks.Dequeue();
				active.Do();
			}

			if (active.IsDone())
			{
				active.Complete();
				active = null;
			}
		}

	}
}

