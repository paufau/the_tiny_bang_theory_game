using Game.State;

namespace Game.State.Resources
{
    public class FuelStore : AbstractStore<int>
    {
        public override string GetTitle()
        {
            return "Fuel";
        }
    }
}

