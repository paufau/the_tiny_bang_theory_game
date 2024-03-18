

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

		private List<Action>? onSucceedActions = null;
		private List<Action>? onFinishedActions = null;

		public virtual bool IsReadyForAssigning()
		{
			return true;
		}

		public void OnSucceed(Action action)
		{
			if (onSucceedActions == null)
			{
				onSucceedActions = new();
			}
			onSucceedActions.Add(action);
		}

		public void OnFinish(Action action)
		{
			if (onFinishedActions == null)
			{
				onFinishedActions = new();
			}
            onFinishedActions.Add(action);
		}

		public void Complete()
		{
			if (onSucceedActions == null) return;
			foreach(var action in onSucceedActions)
			{
				action.Invoke();
			}
			onSucceedActions = null;
		}

		public Vector2 sourcePosition;
		public void SetSource(Vector2 sourcePosition) {
			this.sourcePosition = sourcePosition;
		}
		public bool IsSourceReachable(Vector2 from)
		{
			return StatesProvider.NavigatorState.GetPath(from, sourcePosition).Count > 0;
		}

        public void Finish()
        {
			if (onFinishedActions == null) return;
			foreach(var action in onFinishedActions)
			{
				action.Invoke();
			}
			onFinishedActions = null;
        }
    }
}

