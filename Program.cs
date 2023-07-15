using System;

namespace CST250_Milestone_Project
{
	public class Program
	{
		private static bool endGame = false;
		private static int nonLiveCells = 0;
		private static int liveCells = 0;

		static void Main(string[] args)
		{
			Board board = new Board(5);
			board.SetupLiveNeighbors(0);
			board.CalculateLiveNeighbors();

			// track live and non-live cells
			foreach(Cell cell in board.Grid)
			{
				if (!cell.Live)
				{
					nonLiveCells++;
				}
				else
				{
					liveCells++;
				}
			}

			// added an output to tell the player how many bombs are on the board, since the original game offers that information.
			Console.WriteLine("There are {0} bombs in play!", liveCells);

			while (!endGame)
			{
				// Display current game board state
				PrintGameBoard(board);

				// get input
				Cell cell = GetUserInput(board);

				if (cell.Live)
				{
					//game over - lose
					Console.WriteLine("You stepped on a Bomb. Game over. You Lose.");
					PrintBoard(board);
					endGame = true;
				}
				else if (!cell.Visited)
				{
					// if the selected cell has no live neighbors
					if(cell.NumLiveNeighbors == 0)
					{
						// call a recursive method to clear all horizontally and vertically connected cells with no neighbors,
						// up to and including those that do have live neighbors, but are not live themselves
						board.FloodFill(cell.Row, cell.Col);
					}
					else // just mark as visited
					{
						cell.Visited = true;
					}
				}

				// call helper method to check if all non-live cells have been visited
				if (PlayerWins(board))
				{
					Console.WriteLine("All Bombs found. You Win!");
					endGame = true;
					PrintBoard(board);
				}

			}

			Console.ReadLine();
		}

		// print the board state to the console after each move (Milestone 2)
		private static void PrintGameBoard(Board board)
		{
			// print top edge of board
			PrintBoardTopOrBottom(board, true);

			// print each row/column for the size of the board
			for (int i = 0; i < board.Size; i++)
			{
				for (int j = 0; j < board.Size; j++)
				{
					// fill each cell with a marker

					// first determine and set a value
					string value = "";
					// if not visited, mark with "?"
					if (!board.Grid[i, j].Visited)
					{
						value = "?";
					}
					else // was already visited, mark based on number of visitors
					{
						int neighbors = board.Grid[i, j].NumLiveNeighbors;

						if (neighbors == 0)
						{
							value = "~";
						}
						else if (neighbors < 9)
						{
							value = neighbors.ToString();
						}
					}
					// print the value
					Console.Write("| " + value + " ");
				}

				// print board edge for each row, followed by the row number
				Console.WriteLine("| " + i);

				// print bottom edge of the board
				PrintBoardTopOrBottom(board, false);
			}
		}

		//Milestone 1 method, modified to match output for 0 neighbor cells as in Milestone 3
		// prints the board to the console, showing all live neighbor counts and bomb cells
		// only used on game over
		private static void PrintBoard(Board board)
		{
			PrintBoardTopOrBottom(board, true);

			for (int i = 0; i < board.Size; i++)
			{
				for (int j = 0; j < board.Size; j++)
				{
					int neighbors = board.Grid[i, j].NumLiveNeighbors;
					string value = "";

					if (neighbors == 0)
					{
						value = "~";
					}
					else	if (neighbors < 9)
					{
						value = neighbors.ToString();
					}
					else // is a bomb cell
					{
						value = "*";
					}						
					Console.Write("| " + value + " ");
				}
				Console.WriteLine("| " + i);

				PrintBoardTopOrBottom(board, false);
			}
		}
		
		// helper method to print top or bottom edge of board dynamically
		// handles boards up to size 100, if/else statement could be easily modified to handle up to 1000
		private static void PrintBoardTopOrBottom(Board board, bool isTopEdge)
		{
			// if method is called for the top edge, print the column numbers
			if (isTopEdge)
			{
				for (int i = 0; i < board.Size; i++)
				{
					if (i < 10)
					{
						Console.Write("+ " + i + " ");
					}
					else
					{
						Console.Write("+ " + i);
					}
				}
				Console.WriteLine("+");
			}

			// always print board edge
			for (int i = 0; i < board.Size; i++)
			{
				Console.Write("+---");
			}
			Console.WriteLine("+");
		}

		// Get user input for the row and column of a cell to play
		private static Cell GetUserInput(Board board)
		{
			// start with default row/col values
			int row = -1;
			int col = -1;

			// run while loop until the row and column are validated
			while (!board.IsValidCell(row,col))  
			{
				// get row input
				Console.Out.Write("Enter a row number:");
				while (row == -1)
				{
					// validate input value
					try
					{
						int input = int.Parse(Console.ReadLine());
						if (input >= 0 && input < board.Size)
						{
							// a valid input will update the row value and break the while loop
							row = input;
						}
						else
						{
							throw new Exception();
						}
					}
					catch
					{
						Console.Out.WriteLine("Invalid Entry. Please try again.");
					}
				}

				// get column input - works the same as getting the row input
				Console.Out.Write("Enter a column number:");
				while (col == -1)
				{
					try
					{
						int input = int.Parse(Console.ReadLine());
						if (input >= 0 && input < board.Size)
						{
							col = input;
						}
						else
						{
							throw new Exception();
						}
					}
					catch
					{
						Console.Out.WriteLine("Invalid Entry. Please try again.");
					}
				}
			}

			// return validated cell
			return board.Grid[row, col];
		}

		// helper method to validate that all non-live cells have been visited
		// old method I was using with a tracking counter no longer worked after implementing flood fill
		private static bool PlayerWins(Board board)
		{
			int cellsVisited = 0;

			// loop through the board and count cellsVisited
			foreach (Cell cell in board.Grid)
			{
				if (cell.Visited)
				{
					cellsVisited ++;

					
				}
			}
			// if the cellsVisited count matches the stored nonLiveCells count, the player wins
			if (cellsVisited == nonLiveCells)
			{
				return true;
			}
			// returns false if the count doesn't match
			return false;
		}
	}
}