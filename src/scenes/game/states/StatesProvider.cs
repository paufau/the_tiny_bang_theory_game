
using Game.State.Resources;

namespace Game.State
{
	public static class StatesProvider
	{
		public static bool isLoading = false;
		public static navigator NavigatorState;
		public static World world;
		public static Alert alert;
		public static BuildingUI buildingUI;
		public static TimeCycles timeCycles;

		public static RocksStore Rocks = new();
		public static FuelStore Fuel = new();
		public static MeatStore Meat = new();
		public static CookedMeatStore CookedMeat = new();
	}
}

