using System.Numerics;
using System.Text;
using Raylib_cs;

namespace NotQuiteTetris;

public static class GridManager {

	private static readonly Cell?[,] Grid = new Cell?[8, 8];

	public static int Width => Grid.GetLength(1); //X

	public static int Height => Grid.GetLength(0); //Y

	public static Vector2 Position { get; private set; } = new(100, 100);

	#region Block Control

	public static bool TryPlaceBlock(Block block, int gridX, int gridY, bool v = false, int s = 0) {
		Log.Me(() => $"Checking block placement at ({gridX}, {gridY})...", v, s + 1);

		// Check if the block fits within the grid bounds
		bool isAtLeastZero = gridX >= 0 || gridY >= 0;
		bool withinBoundsX = gridX + block.Width <= Width;
		bool withinBoundsY = gridY + block.Height <= Height;

		if (!isAtLeastZero || !withinBoundsX || !withinBoundsY) {
			Log.Me($"Block placement at ({gridX}, {gridY}) is invalid. Block exceeds grid bounds.", v, s + 1);
			Log.Me($"Block size: ({block.Width}, {block.Height}), Grid size: ({Width}, {Height})", v, s + 1);
			Log.Me($"Target position plus block size: ({gridX + block.Width}, {gridY + block.Height})", v, s + 1);
			return false;
		}

		// For each column...
		for (int blockX = 0; blockX < block.Width; blockX++) {

			// For each cell (row) in the column...
			for (int blockY = 0; blockY < block.Height; blockY++) {
				bool blockHasCell = block.HasCellAt(blockX, blockY);
				int targetX = gridX + blockX;
				int targetY = gridY + blockY;
				bool gridHasCell = Grid[targetX, targetY] != null;

				if (blockHasCell && gridHasCell) {
					Log.Me($"Block placement at ({gridX}, {gridY}) is invalid. Cell ({targetX}, {targetY}) is already occupied.", v, s + 1);
					return false;
				}
			}
		}

		Log.Me(() => $"Done! The block can be placed at ({gridX}, {gridY}).", v, s + 1);
		return true;
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

	public static bool HasValidSpot(bool v = false, int s = 0) {
		Log.Me(() => "Scanning grid...", v, s + 1);

		bool emptyHand = true;

		// For each block in the hand...
		foreach (Block? block in GameManager.Hand) {
			if (block == null) continue;
			emptyHand = false;

			// For each column...
			for (int x = 0; x < Width; x++) {

				// For each cell (row) in the column...
				for (int y = 0; y < Height; y++) {
					Cell? cell = Grid[x, y];
					if (cell != null) {
						Log.Me(() => $"Cell at ({x}, {y}) is occupied with color {cell.Color}.", v, s + 1);
						continue;
					}
					
					else {
						Log.Me(() => $"Cell at ({x}, {y}) is empty.", v, s + 1);

						if (TryPlaceBlock(block, x, y, v, s + 1)) {
							Log.Me(() => $"Done! Block of type {block.GetType} can be placed at ({x}, {y}).", v, s + 1);
							return true;
						}
					}
				}
			}
		}

		if (emptyHand) {
			Log.Me(() => "Hand is empty, nothing to scan.", v, s + 1);
			return true;
		}

		Log.Me(() => "Done! There are no more valid slots for all blocks in the hand.", v, s + 1);
		return false;
	}

	public static void ClearGrid(bool v = false, int s = 0) {
		Log.Me(() => "Clearing entire grid...", v, s + 1);
		Array.Clear(Grid, 0, Grid.Length);

		Log.Me(() => "Done!", v, s + 1);
	}

	public static (int, int) GetGridCoordinates(Vector2 position, bool v = false, int s = 0) {
		Log.Me(() => $"Getting grid coordinates from position ({position.X}, {position.Y})...", v, s + 1);
		int x = (int) ((position.X - Position.X) / (GridRenderer.CellSize + GridRenderer.Spacing));
		int y = (int) ((position.Y - Position.Y) / (GridRenderer.CellSize + GridRenderer.Spacing));

		Log.Me(() => $"Done! Grid coordinates are ({x}, {y}).", v, s + 1);
		return (x, y);
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


	public static int ScanAndClearFullLines(bool v = false, int s = 0) {
		int clearedLines = 0;

		// Scan each column
		Log.Me(() => "Scanning for full columns...", v, s + 1);
		for (int x = 0; x < Width; x++) {
			if (IsColumnFull(x, v, s + 1)) {
				ClearColumn(x, v, s + 1);
				clearedLines++;
			}
		}


		// Scan each row
		Log.Me(() => "Scanning for full rows...", v, s + 1);
		for (int y = 0; y < Height; y++) {
			if (IsRowFull(y, v, s + 1)) {
				ClearRow(y, v, s + 1);
				clearedLines++;
			}
		}

		Log.Me(() => $"Done! Cleared {clearedLines} lines.", v, s + 1);
		return clearedLines;
	}


	/// <summary>
	/// Lazily generates a string representation of the grid. <br/><br/>
	/// It does not store cell-specific information, only whether a cell is occupied or empty,
	/// so occupied cells will always be represented by a random color.
	/// </summary>
	public static string GetGridAsString(bool v = false, int s = 0) {
		Log.Me(() => "Generating grid string representation...", v, s + 1);
		StringBuilder builder = new();

		for (int y = 0; y < Height; y++) {
			for (int x = 0; x < Width; x++) {
				Cell? cell = Grid[x, y];
				builder.Append(cell == null ? "." : "#");
			}
			builder.AppendLine();
		}

		string result = builder.ToString();
		Log.Me(() => "Done!", v, s + 1);
		return result;
	}


	/// <summary>
	/// Parses a string representation of the grid and updates the grid state accordingly.
	/// </summary>
	public static void ParseStringAsGrid(string gridString, bool v = false, int s = 0) {
		Log.Me(() => "Parsing grid string representation...", v, s + 1);
		string[] lines = gridString.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

		// For each line...
		for (int y = 0; y < Math.Min(lines.Length, Height); y++) {
			string line = lines[y];

			// For each character in the line...
			for (int x = 0; x < Math.Min(line.Length, Width); x++) {
				char c = line[x];

				if (c == '#') {
					int blockIndex = Raylib.GetRandomValue(0, GameManager.BlockPalette.Length - 1);
					char blockType = GameManager.BlockPalette[blockIndex];
					string blockName = blockType.ToString() + "Block";

					Color randomColor = GameRenderer.ColorPalette[blockName];
					Grid[x, y] = new Cell(new Vector2(
						Position.X + x * (GridRenderer.CellSize + GridRenderer.Spacing),
						Position.Y + y * (GridRenderer.CellSize + GridRenderer.Spacing)
					), Color.Gray);
				}

				else {
					Grid[x, y] = null;
				}
			}
		}
		Log.Me(() => "Done!", v, s + 1);
	}

	#endregion

}
