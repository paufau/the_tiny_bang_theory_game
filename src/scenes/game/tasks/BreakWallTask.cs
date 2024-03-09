using Game.Pawn.AI;
using Godot;

namespace Game.Task
{
    public class BreakWallTask : AbstractTask
    {
        private Vector2 wallPosition;
        private BreakProcessor breakProcessor;
        private bool isWallBroken = false;

        public override void Do()
        {
            isWallBroken = true;
            breakProcessor.BreakAt(wallPosition);
        }

        public override int GetPriority()
        {
            return 1;
        }

        public override bool IsDone()
        {
            return isWallBroken;
        }

        public override void Plan(pawn_controller pawn)
        {
            var goToTask = new GoToTask(wallPosition);
            goToTask.Plan(pawn);
            pawn.AI.AddTask(goToTask);
        }

        public BreakWallTask(Vector2 wallPosition, BreakProcessor breakProcessor)
        {
            this.wallPosition = wallPosition;
            this.breakProcessor = breakProcessor;
        }
    }
}

