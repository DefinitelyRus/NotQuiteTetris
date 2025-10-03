using Raylib_cs;
namespace NotQuiteTetris;

public static class GridRenderer {

	public static int PosX => 100;

	public static int PosY => 100;

	public static int Padding => 5;

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

				// Has cell
				if (cell != null) {
					Log.Me(() => $"Drawing cell at ({x}, {y})...", v, s + 1);
					Raylib.DrawRectangle(cellPosX, cellPosY, CellSize, CellSize, cell.Color);
				}
				
				// No cell
				else {
					Log.Me(() => $"Drawing empty cell at ({x}, {y})...", v, s + 1);
					Rectangle rect = new(cellPosX, cellPosY, CellSize, CellSize);
					Raylib.DrawRectangle(cellPosX, cellPosY, CellSize, CellSize, Color.DarkGray);
					Raylib.DrawRectangleLinesEx(rect, Padding, Color.Gray);
				}
			}
		}

		Log.Me(() => "Done!", v, s + 1);
	}
}
