using Game.State;

namespace Game.State.Resources
{
    public class CookedMeatStore : AbstractStore<int>
    {
        public override string GetTitle()
        {
            return "Cooked meat";
        }
    }
}

