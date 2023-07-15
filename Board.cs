using System;

namespace CST250_Milestone_Project
{
	class Board
	{
		public int Size { get; set; }
		public Cell[,] Grid { get; set; }
		public double[] Difficulty
		{
			get { return new double[] { 0.12d, 0.21d, 0.30d }; }
			private set { }
		}

		// Constructor initializes a grid of cells, assigning their row and col property values. 
		public Board(int size)
		{
			Size = size;
			Grid = new Cell[size, size];
			for (int a = 0; a < size; a++)
			{
				for (int b = 0; b < size; b++)
				{
					Cell cell = new Cell();
					Grid[a, b] = cell;
					Grid[a, b].Row = a;
					Grid[a, b].Col = b;
				}
			}
		}

		// generate a number of "bombs" on random cells based on difficulty level
		public void SetupLiveNeighbors(int difficultyLevel)
		{
			// use a percentage of the total cells in the grid (Difficulty is an array of percentages)
			int liveNum = (int)(Size * Size * Difficulty[difficultyLevel]);

			// loop activates a random cell until liveNum count is reached
			for (int i = 0; i < liveNum; i++)
			{
				// pick a random cell
				int r1 = new Random().Next(0, Size);
				int r2 = new Random().Next(0, Size);

				// if it is not already live, make it live, else decerement the liveNum count to try again
				if (Grid[r1, r2].Live == false)
				{
					Grid[r1, r2].Live = true;
				}
				else
				{
					i--;
				}
			}
		}

		// populate NumLiveNeighbors on each cell
		public void CalculateLiveNeighbors()
		{
			// loop though the grid
			for (int a = 0; a < Size; a++)
			{
				for (int b = 0; b < Size; b++)
				{
					// default neighbor count
					int neighborCount = 0;

					// if current cell is live, set neighbors to 9, else...
					if (Grid[a, b].Live)
					{
						Grid[a, b].NumLiveNeighbors = 9;
					}
					else
					// loop through neighbor cells...
					{
						for (int x = a - 1; x < a + 2; x++)
						{
							for (int y = b - 1; y < b + 2; y++)
							{
								// if the neighbor coordinates are on the grid and...
								if (IsValidCell(x, y))
								{
									// are not the current cell and the cell is live
									if (Grid[x, y] != Grid[a, b] && Grid[x, y].Live)
									{
										//increment the neighbor count
										neighborCount++;
									}
								}
							}
						}
						// assign the final neighbor count to the cell
						Grid[a, b].NumLiveNeighbors = neighborCount;
					}
				}
			}
		}

		// check to see if a cell exists given row and column numbers
		public bool IsValidCell(int r, int c)
		{
			// check if each value is in range of the board size (0 to Size)
			if (r >= 0 && r < Size
				&& c >= 0 && c < Size)
			{
				return true;
			}
			// return false if the validation fails - one or both values of the coordinates were less than 0 or >= Size
			return false;
		}

		// Milestone 3 - recursive method to mark neighbors visited when a cell with no live neighbors is checked.
		public void FloodFill(int row, int col)
		{
			// mark the calling cell visited
			Grid[row, col].Visited = true;

			// if this cell has 0 neighbors, visit cells above, below, and beside
			if (Grid[row, col].NumLiveNeighbors == 0)
			{
				// if the 4-direction neighbor coordinates are valid, and the cell is not live, and not already visited
				if (IsValidCell(row - 1, col) && !Grid[row - 1, col].Live && !Grid[row - 1, col].Visited) { FloodFill(row - 1, col); }
				if (IsValidCell(row + 1, col) && !Grid[row + 1, col].Live && !Grid[row + 1, col].Visited) { FloodFill(row + 1, col); }
				if (IsValidCell(row, col - 1) && !Grid[row, col - 1].Live && !Grid[row, col - 1].Visited) { FloodFill(row, col - 1); }
				if (IsValidCell(row, col + 1) && !Grid[row, col + 1].Live && !Grid[row, col + 1].Visited) { FloodFill(row, col + 1); }
			}
			else // do nothing
			{
				return;
			}
		}
	}
}