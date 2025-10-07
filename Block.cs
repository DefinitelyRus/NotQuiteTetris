using System.Numerics;
using Raylib_cs;

namespace NotQuiteTetris;

public abstract class Block {
	public int Width { get; private set; }

	public int Height { get; private set; } = -1;

	public Color Color { get; private set; }

	public bool[][] Grid { get; protected set; }

	public int Rotation { get; private set; } = 0; //0 = 0, 1 = 90, 2 = 180, 3 = 270

	public Block(bool[][] grid, Color color, bool v = false, int s = 0) {
		Log.Me(() => "Creating new block...", v, s + 1);

		// Validate row length (set Width)
		if (grid.Length == 0) throw new ArgumentException("Column length must not be 0.");
		Width = grid.Length;

		// Validate column lengths (set Height)
		foreach (bool[] column in grid) {

			// First column sets height.
			if (column.Length > 0 && Height == -1) Height = column.Length;

			// Update height if current column is taller.
			//else if (column.Length > Height) Height = column.Length;

			// Column length 0.
			else if (column.Length == 0) throw new ArgumentException("All column lengths must not be 0.");
		}

		Grid = grid;

		Color = color;

		Log.Me(() => "Done!", v, s + 1);
	}


	public bool HasCellAt(int x, int y) {
		return Grid[x][y];
	}


	public void Rotate(bool v = false, int s = 0) {
		Log.Me(() => "Rotating block...", v, s + 1);
		bool[][] newGrid = new bool[Height][];

		for (int i = 0; i < Height; i++) newGrid[i] = new bool[Width];

		// Rotate
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				newGrid[y][Width - 1 - x] = Grid[x][y];
			}
		}

		Grid = newGrid;

		// Swap dimensions
		(Height, Width) = (Width, Height);
		Rotation = (Rotation + 1) % 4;

		Log.Me(() => "Done!", v, s + 1);
	}


	public void RotateTo(int targetRotation, bool v = false, int s = 0) {
		if (targetRotation < 0 || targetRotation > 3) throw new ArgumentOutOfRangeException(nameof(targetRotation), "Target rotation must be between 0 and 3 (inclusive).");
		while (Rotation != targetRotation) Rotate(v, s + 1);
	}


	public Vector2 GetCenterPosition(bool v = false, int s = 0) {
		Log.Me(() => "Calculating center position...", v, s + 1);
		float x = (Width - 1) / 2f;
		float y = (Height - 1) / 2f;

		Log.Me(() => "Done!", v, s + 1);
		return new Vector2(x, y);
	}
}
