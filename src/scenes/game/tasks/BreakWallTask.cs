using Game.Pawn.AI;
using Game.State;
using Godot;

namespace Game.Task
{
    public class BreakWallTask : AbstractTask
    {
        private Vector2 wallPosition;
        private BreakProcessor breakProcessor;
        private bool isWallBroken = false;
        private PackedScene breakingMask;
        private Node2D breakingMaskInstance;

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
            if (isWallBroken && breakingMaskInstance != null)
            {
                breakingMaskInstance.QueueFree();
                breakingMaskInstance = null;
            }
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
            breakingMask = ResourceLoader.Load<PackedScene>("res://src/scenes/game/building_system/BreakingMask.tscn");
            breakingMaskInstance = (Node2D)breakingMask.Instantiate();
            breakingMaskInstance.GlobalPosition = StatesProvider.NavigatorState.SnapToNearestTile(wallPosition);
            breakProcessor.AddChild(breakingMaskInstance);

            this.wallPosition = wallPosition;
            this.breakProcessor = breakProcessor;
            SetSource(wallPosition);
        }
    }
}

