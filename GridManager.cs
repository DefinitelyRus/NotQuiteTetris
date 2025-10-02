using System.Numerics;

namespace NotQuiteTetris;

public static class GridManager {

	private static readonly Cell?[,] Grid = new Cell?[8, 8];

	public static int Width => Grid.GetLength(1); //X

	public static int Height => Grid.GetLength(0); //Y

	public static Vector2 Position { get; private set; } = new(100, 100);

	#region Block Control

	public static void TryPlaceBlock(Block block, int gridX, int gridY, bool v = false, int s = 0) {
		Log.Me(() => $"Checking block placement at ({gridX}, {gridY})...", v, s + 1);

		// Check if the block fits within the grid bounds
		bool isAtLeastZero = gridX >= 0 || gridY >= 0;
		bool withinBoundsX = gridX + block.Width < Width;
		bool withinBoundsY = gridY + block.Height < Height;
		if (!isAtLeastZero || !withinBoundsX || !withinBoundsY) {
			Log.Me($"Block placement at ({gridX}, {gridY}) is invalid. Block exceeds grid bounds.", v, s + 1);
			return;
		}

		// For each column...
		for (int blockX = 0; blockX < block.Width; blockX++) {

			// For each cell (row) in the column...
			for (int blockY = 0; blockY < block.Height; blockY++) {
				bool blockHasCell = block.HasCellAt(blockX, blockY);
				bool gridHasCell = Grid[gridX + blockX, gridY + blockY] != null;

				if (blockHasCell && gridHasCell) {
					Log.Me($"Block placement at ({gridX}, {gridY}) is invalid. Cell ({gridX + blockX}, {gridY + blockY}) is already occupied.", v, s + 1);
					return;
				}
			}
		}

		// Place block
		Log.Me(() => $"The block can be placed at ({gridX}, {gridY})!", v, s + 1);
		PlaceBlock(block, gridX, gridY, v, s + 1);

		Log.Me(() => "Done!", v, s + 1);
	}

	public static void PlaceBlock(Block block, int gridX, int gridY, bool v = false, int s = 0) {
		Log.Me(() => $"Placing block at ({gridX}, {gridY})...", v, s + 1);

		// For each column...
		for (int blockX = 0; blockX < block.Width; blockX++) {

			// For each cell (row) in the column...
			for (int blockY = 0; blockY < block.Height; blockY++) {

				Vector2 cellPosition = new(
					Position.X + (blockX + gridX) * (GridRenderer.CellSize + GridRenderer.Spacing),
					Position.Y + (blockY + gridY) * (GridRenderer.CellSize + GridRenderer.Spacing)
				);

				if (block.HasCellAt(blockX, blockY)) {
					Grid[gridX + blockX, gridY + blockY] = new Cell(cellPosition, block.Color);
				}
			}
		}

		Log.Me(() => $"Done!", v, s + 1);
	}

	#endregion

	#region Grid Control

	public static void ClearGrid(bool v = false, int s = 0) {
		Log.Me(() => "Clearing entire grid...", v, s + 1);
		Array.Clear(Grid, 0, Grid.Length);

		Log.Me(() => "Done!", v, s + 1);
	}

	#endregion

	#region Cell Control

	public static Cell? GetCell(int x, int y, bool v = false, int s = 0) {
		Log.Me(() => $"Getting cell at ({x}, {y}).", v, s + 1);
		return Grid[x, y];
	}

	public static void SetCell(int x, int y, Cell cell, bool v = false, int s = 0) {
		Log.Me(() => $"Setting cell at ({x}, {y})...", v, s + 1);
		Grid[x, y] = cell;

		Log.Me(() => "Done!", v, s + 1);
	}

	#endregion

	#region Row and Column Operations

	public static void ClearColumn(int x, bool v = false, int s = 0) {
		Log.Me(() => $"Clearing column {x}...", v, s + 1);
		for (int y = 0; y < Height; y++) {
			Grid[x, y] = null;
		}

		Log.Me(() => $"Column {x} cleared!", v, s + 1);
	}


	public static void ClearRow(int y, bool v = false, int s = 0) {
		Log.Me(() => $"Clearing row {y}...", v, s + 1);
		for (int x = 0; x < Width; x++) {
			Grid[x, y] = null;
		}

		Log.Me(() => $"Row {y} cleared!", v, s + 1);
	}


	public static bool IsColumnFull(int x, bool v = false, int s = 0) {
		Log.Me(() => $"Checking if column {x} is full...", v, s + 1);
		for (int y = 0; y < Height; y++) {
			if (Grid[x, y] == null) {
				Log.Me(() => $"Column {x} is not full!", v, s + 1);
				return false;
			}
		}

		Log.Me(() => $"Column {x} is full!", v, s + 1);
		return true;
	}


	public static bool IsRowFull(int y, bool v = false, int s = 0) {
		Log.Me(() => $"Checking if row {y} is full...", v, s + 1);
		for (int x = 0; x < Width; x++) {
			if (Grid[x, y] == null) {
				Log.Me(() => $"Row {y} is not full!", v, s + 1);
				return false;
			}
		}

		Log.Me(() => $"Row {y} is full!", v, s + 1);
		return true;
	}

	public static void ScanAndClearFullRowsAndColumns(bool v = false, int s = 0) {

		// Scan each column
		Log.Me(() => "Scanning for full columns...", v, s + 1);
		for (int x = 0; x < Width; x++) {
			if (IsColumnFull(x, v, s + 1)) {
				ClearColumn(x, v, s + 1);
			}
		}


		// Scan each row
		Log.Me(() => "Scanning for full rows...", v, s + 1);
		for (int y = 0; y < Height; y++) {
			if (IsRowFull(y, v, s + 1)) {
				ClearRow(y, v, s + 1);
			}
		}

		Log.Me(() => "Done!", v, s + 1);
	}

	#endregion

}
