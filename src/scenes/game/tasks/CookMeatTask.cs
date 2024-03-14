using System;
using Game.State;
using Game.Task;
using Godot;

namespace Game.Task
{
    public class CookMeatTask : AbstractTask
	{
		public CookMeatTask()
		{
		}

        public override void Do()
        {

        }

        public override int GetPriority()
        {
            return 2;
        }

        public override bool IsDone()
        {
            return true;
        }

        public override bool IsReadyForAssigning()
        {
            var box = StatesProvider.world.GetNearestInGroup("box_placed",
                StatesProvider.NavigatorState.GetRandomPointGlobalPosition());

            return StatesProvider.Meat.value > 0 && box != null;
        }

        public override void Plan(pawn_controller pawn)
        {
            var box = StatesProvider.world.GetNearestInGroup("box_placed", pawn.GlobalPosition);

            if (box == null)
            {
                return;
            };

            var goToStore1 = new GoToTask(box.GlobalPosition);
            goToStore1.Plan(pawn);
            pawn.AI.AddTask(goToStore1);
            goToStore1.OnDone(() =>
            {
                if (!IsReadyForAssigning())
                {
                    pawn.AI.TerminateAllPlanned();
                    return;
                }
                StatesProvider.Meat.Update(prev => prev - 1);
            });

            var goToFurnace = new GoToTask(sourcePosition);
            goToFurnace.Plan(pawn);
            pawn.AI.AddTask(goToFurnace);

            var waitTask = new WaitTask(4);
            waitTask.Plan(pawn);
            pawn.AI.AddTask(waitTask);
            waitTask.OnDone(() =>
            {
                box = StatesProvider.world.GetNearestInGroup("box_placed", pawn.GlobalPosition);
                if (box == null)
                {
                    pawn.AI.TerminateAllPlanned();
                    StatesProvider.alert.ShowAlert("Cooked meat is lost, because there are no boxes to store it!");
                }
            });

            var goToStore2 = new GoToTask(box.GlobalPosition);
            goToStore2.Plan(pawn);
            pawn.AI.AddTask(goToStore2);
            goToStore2.OnDone(() =>
            {
                StatesProvider.CookedMeat.Update(prev => prev + 1);
            });
        }
    }
}

