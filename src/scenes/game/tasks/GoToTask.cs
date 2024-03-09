using Godot;

namespace Game.Task
{
    public class GoToTask : AbstractTask
    {
        private pawn_controller pawn;
        private Vector2 goToPosition;

        public override int GetPriority()
        {
            return 1;
        }

        public override bool IsDone()
        {
            return !pawn.pathfinder.IsMoving;
        }

        public override void Plan(pawn_controller pawn)
        {
            this.pawn = pawn;
        }

        public override void Do()
        {
            pawn.pathfinder.MoveTo(goToPosition);
        }

        public GoToTask(Vector2 position)
        {
            goToPosition = position;
        }
    }
}

