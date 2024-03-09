
namespace Game.Task
{
	public abstract class AbstractTask
	{
		public abstract int GetPriority();
		public abstract void Plan(pawn_controller pawn);
		public abstract void Do();
		public abstract bool IsDone();
	}
}

