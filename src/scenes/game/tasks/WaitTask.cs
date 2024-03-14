namespace Game.Task
{
    public class WaitTask : AbstractTask
    {
        private bool isDone = false;
        private double durationSeconds;

        public override void Do()
        {
            System.Threading.Tasks.Task
                .Delay((int)durationSeconds * 1000)
                .ContinueWith((_) =>
                {
                    isDone = true;
                });
        }

        public override int GetPriority()
        {
            return 0;
        }

        public override bool IsDone()
        {
            return isDone;
        }

        public override void Plan(pawn_controller pawn)
        {

        }

        public WaitTask(double durationSeconds)
        {
            this.durationSeconds = durationSeconds;
        }
    }
}

