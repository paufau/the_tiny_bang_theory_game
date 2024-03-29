﻿using System;
using System.Collections.Generic;
using Godot;

namespace Game.BuildingSystem
{
	public delegate void OccupancyIterator(int x, int y);

	public class Occupancy
	{
		public static void IterateCells(Rect2I rect, OccupancyIterator iterator)
		{
            for (var x = rect.Position.X; x < rect.Position.X + rect.Size.X; x++)
            {
                for (var y = rect.Position.Y - rect.Size.Y; y < rect.Position.Y; y++)
                {
                    iterator.Invoke(x, y);
                }
            }
        }

		public Dictionary<int, Dictionary<int, bool>> occupancyMap = new();

		public void OccupyCell(int x, int y)
		{
            Dictionary<int, bool> xDict;

			if (!occupancyMap.TryGetValue(x, out xDict))
			{
				xDict = new();
				occupancyMap.Add(x, xDict);
			}

			xDict[y] = true;
		}

		public bool IsOccupied(int x, int y)
		{
            Dictionary<int, bool> xDict;

            if (occupancyMap.TryGetValue(x, out xDict))
            {
                return xDict.TryGetValue(y, out _);
            }

			return false;
        }

		public void Occupy(Rect2I rect)
		{
			IterateCells(rect, OccupyCell);
		}

		public void Occupy(Vector2I mapPosition)
		{
			OccupyCell(mapPosition.X, mapPosition.Y);
		}

        public List<Vector2I> GetOccupiedCells(Vector2I position, Vector2I size)
		{
			List<Vector2I> occupiedPositions = new();

			IterateCells(new Rect2I(position, size), (x, y) =>
			{
                if (IsOccupied(x, y))
                {
                    occupiedPositions.Add(new Vector2I(x, y));
                }
            });

			return occupiedPositions;
        }

		public List<Vector2I> GetOccupiedCells(Rect2I rect)
		{
			return GetOccupiedCells(rect.Position, rect.Size);
        }

		public List<Vector2I> GetOccupiedCells(Vector2I position)
		{
			return GetOccupiedCells(position, Vector2I.One);
        }
	}
}

