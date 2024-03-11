
using Godot;

namespace Game.State
{
	public static class StatesProvider
	{
		public static bool isLoading = false;
		public static navigator NavigatorState;
		public static World world;

		public static RocksState Rocks = new();
	}
}

