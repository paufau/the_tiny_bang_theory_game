using System;
using Godot;

namespace Game.State
{
	public class RocksState
	{
		public int storedRocks = 0;

		public delegate void UpdateEventHandler(int next);
		public event UpdateEventHandler Update;

		public void Add(int amountToAdd)
		{
			storedRocks += amountToAdd;
			Update?.Invoke(storedRocks);
		}
	}
}

