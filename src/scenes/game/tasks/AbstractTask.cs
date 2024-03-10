

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

