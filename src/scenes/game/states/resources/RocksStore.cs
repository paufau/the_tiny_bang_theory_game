using Game.State;

namespace Game.State.Resources
{
    public class RocksStore : AbstractStore<int>
    {
        public override string GetTitle()
        {
            return "Rocks";
        }
    }
}

