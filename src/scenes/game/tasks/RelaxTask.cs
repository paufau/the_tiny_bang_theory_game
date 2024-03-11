using Game.State;
using Godot;

namespace Game.Task
{
	public class RelaxTask : AbstractTask
	{
        private bool isRelaxed = false;

        public override void Do()
        {
            isRelaxed = true;
        }

        public override int GetPriority()
        {
            return 0;
        }

        public override bool IsDone()
        {
            return isRelaxed;
        }

        public override void Plan(pawn_controller pawn)
        {
            var bed = StatesProvider.world.GetNearestInGroup("beds_placed", pawn.GlobalPosition);
            if (bed == null) return;

            var goToBed = new GoToTask(bed.GlobalPosition);
            goToBed.Plan(pawn);
            pawn.AI.AddTask(goToBed);
        }
    }
}

