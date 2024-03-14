using System;
using Game.Pawn.AI;
using Game.State;
using Godot;

namespace Game.Task
{
    public class BreakWallTask : AbstractTask
    {
        private pawn_controller pawn;
        private Vector2 wallPosition;
        private BreakProcessor breakProcessor;
        private bool isWallBroken = false;
        private PackedScene breakingMask;
        private Node2D breakingMaskInstance;
        private Action onGainResource;

        public override void Do()
        {
            breakProcessor.BreakAt(wallPosition);

            var box = StatesProvider.world.GetNearestInGroup("boxes_placed", pawn.GlobalPosition);

            if (box != null)
            {
                var goToBox = new GoToTask(box.GlobalPosition);
                goToBox.Plan(pawn);
                goToBox.OnDone(onGainResource);
                pawn.AI.AddTask(goToBox);
            } else
            {
                StatesProvider.alert.ShowAlert("Resource is lost! Build a box in order to save it.");
            }

            isWallBroken = true;
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
            this.pawn = pawn;
            var goToTask = new GoToTask(wallPosition);
            goToTask.Plan(pawn);
            pawn.AI.AddTask(goToTask);
        }

        public BreakWallTask(Vector2 wallPosition, BreakProcessor breakProcessor, Action onGainResource)
        {
            breakingMask = ResourceLoader.Load<PackedScene>("res://src/scenes/game/building_system/BreakingMask.tscn");
            breakingMaskInstance = (Node2D)breakingMask.Instantiate();
            breakingMaskInstance.GlobalPosition = StatesProvider.NavigatorState.SnapToNearestTile(wallPosition);
            breakProcessor.AddChild(breakingMaskInstance);

            this.onGainResource = onGainResource;
            this.wallPosition = wallPosition;
            this.breakProcessor = breakProcessor;
            SetSource(wallPosition);
        }
    }
}

