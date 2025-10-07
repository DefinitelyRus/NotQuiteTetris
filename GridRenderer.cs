using Raylib_cs;
namespace NotQuiteTetris;

public static class GridRenderer {

	public static int PosX => 100;

	public static int PosY => 100;

	public static int CellOutlineWidth => 4;

	public static int Spacing => 2;

	public static int CellSize => 50;
	
	public static int EndPosX => PosX + (CellSize + Spacing) * GridManager.Width - Spacing;

	public static int EndPosY => PosY + (CellSize + Spacing) * GridManager.Height - Spacing;

	public static bool IsMouseInsideGrid {
		get {
			int x = Raylib.GetMouseX();
			int y = Raylib.GetMouseY();
			return x >= PosX && x <= EndPosX && y >= PosY && y <= EndPosY;
		}
	}


	public static void Update(bool v = false, int s = 0) {
		Log.Me(() => "Rendering grid...", v, s + 1);

		for (int x = 0; x < GridManager.Width; x++) {
			for (int y = 0; y < GridManager.Height; y++) {
				Cell? cell = GridManager.GetCell(x, y, v, s + 1);

				int cellPosX = x * CellSize + PosX;
				int cellPosY = y * CellSize + PosY;
				Rectangle rect = new(cellPosX, cellPosY, CellSize, CellSize);

				// Has cell
				if (cell != null) {
					Log.Me(() => $"Drawing cell at ({x}, {y})...", v, s + 1);
					Raylib.DrawRectangle(cellPosX, cellPosY, CellSize, CellSize, cell.Color);
					Raylib.DrawRectangleLinesEx(rect, CellOutlineWidth, Color.Black);
				}
				
				// No cell
				else {
					Log.Me(() => $"Drawing empty cell at ({x}, {y})...", v, s + 1);
					Raylib.DrawRectangle(cellPosX, cellPosY, CellSize, CellSize, Color.DarkGray);
					Raylib.DrawRectangleLinesEx(rect, CellOutlineWidth, Color.Gray);
				}
			}
		}

		Log.Me(() => "Done!", v, s + 1);
	}


	public static void PreviewBlock(Block block, int gridX, int gridY, bool v = false, int s = 0) {
		Log.Me(() => $"Previewing block at ({gridX}, {gridY})...", v, s + 1);

		// For each column...
		for (int blockX = 0; blockX < block.Width; blockX++) {

			// For each cell (row) in the column...
			for (int blockY = 0; blockY < block.Height; blockY++) {
				int targetX = gridX + blockX;
				int targetY = gridY + blockY;

				// Skip if out of bounds
				bool belowZero = targetX < 0 || targetY < 0;
				bool beyondBounds = targetX >= GridManager.Width || targetY >= GridManager.Height;
				if (belowZero || beyondBounds) continue;

				bool blockHasCell = block.HasCellAt(blockX, blockY);
				bool gridHasCell = GridManager.GetCell(gridX + blockX, gridY + blockY, v, s + 1) != null;
				int cellPosX = (gridX + blockX) * CellSize + PosX;
				int cellPosY = (gridY + blockY) * CellSize + PosY;

				if (blockHasCell) {
					Log.Me(() => $"Rendering cell at ({cellPosX}, {cellPosY})...", v, s + 1);
					Color previewColor = gridHasCell ? Color.Red : Color.Green;
					Raylib.DrawRectangle(cellPosX, cellPosY, CellSize, CellSize, previewColor);
					Raylib.DrawRectangleLinesEx(new Rectangle(cellPosX, cellPosY, CellSize, CellSize), CellOutlineWidth, Color.Black);
				}
			}
		}

		Log.Me(() => "Done!", v, s + 1);
	}
}
