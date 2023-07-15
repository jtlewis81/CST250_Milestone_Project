namespace CST250_Milestone_Project
{
	class Cell
	{
		public int Row { get; set; }
		public int Col { get; set; }
		public bool Visited { get; set; }
		public bool Live { get; set; }
		public int NumLiveNeighbors { get; set; }

		// constructor sets all properties to default values
		public Cell()
		{
			Row = -1;
			Col = -1;
			Visited = false;
			Live = false;
			NumLiveNeighbors = 0;
		}
	}
}