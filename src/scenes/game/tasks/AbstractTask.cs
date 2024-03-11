

using System;
using System.Collections.Generic;
using Game.State;
using Godot;

namespace Game.Task
{
	public abstract class AbstractTask
	{
		public abstract int GetPriority();
		public abstract void Plan(pawn_controller pawn);
		public abstract void Do();
		public abstract bool IsDone();

		private List<Action>? onDoneActions = null;

		public void OnDone(Action action)
		{
			if (onDoneActions == null)
			{
				onDoneActions = new();
			}
			onDoneActions.Add(action);
		}

		public void Complete()
		{
			if (onDoneActions == null) return;
			foreach(var action in onDoneActions)
			{
				action.Invoke();
			}
			onDoneActions = null;
		}

		public Vector2 sourcePosition;
		public void SetSource(Vector2 sourcePosition) {
			this.sourcePosition = sourcePosition;
		}
		public bool IsSourceReachable(Vector2 from)
		{
			return StatesProvider.NavigatorState.GetPath(from, sourcePosition).Count > 0;
		}
	}
}

