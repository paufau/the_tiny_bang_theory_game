using Game.State;

namespace Game.State.Resources
{
    public class MeatStore : AbstractStore<int>
    {
        public override string GetTitle()
        {
            return "Raw meat";
        }
    }
}

