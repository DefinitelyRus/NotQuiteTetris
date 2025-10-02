

namespace NotQuiteTetris;

public static class GridManager {

	public static bool[][] Grid { get; } = [
		new bool[8],
		new bool[8],
		new bool[8],
		new bool[8],
		new bool[8],
		new bool[8],
		new bool[8],
		new bool[8]
	];

	public static bool GetCell(int x, int y) {
		return Grid[x][y];
	}

	public static void SetCell(int x, int y, bool value) {
		Grid[x][y] = value;
	}

	public static void ClearColumn(int x, bool v = false, int s = 0) {
		Log.Me(() => $"Clearing column {x}...", v, s + 1);
		for (int y = 0; y < Grid[x].Length; y++) {
			Grid[x][y] = false;
		}

		Log.Me(() => $"Column {x} cleared!", v, s + 1);
	}

	public static void ClearRow(int y, bool v = false, int s = 0) {
		Log.Me(() => $"Clearing row {y}...", v, s + 1);
		for (int x = 0; x < Grid.Length; x++) {
			Grid[x][y] = false;
		}
		Log.Me(() => $"Row {y} cleared!", v, s + 1);
	}

	public static void ClearGrid() {
		for (int x = 0; x < Grid.Length; x++) {
			for (int y = 0; y < Grid[x].Length; y++) {
				Grid[x][y] = false;
			}
		}
	}

	public static bool IsColumnFull(int x, bool v = false, int s = 0) {
		Log.Me(() => $"Checking if column {x} is full...", v, s + 1);
		for (int y = 0; y < Grid[x].Length; y++) {
			if (!Grid[x][y]) {
				Log.Me(() => $"Column {x} is not full!", v, s + 1);
				return false;
			}
		}

		Log.Me(() => $"Column {x} is full!", v, s + 1);
		return true;
	}

	public static bool IsRowFull(int y, bool v = false, int s = 0) {
		Log.Me(() => $"Checking if row {y} is full...", v, s + 1);
		for (int x = 0; x < Grid.Length; x++) {
			if (!Grid[x][y]) {
				Log.Me(() => $"Row {y} is not full!", v, s + 1);
				return false;
			}
		}

		Log.Me(() => $"Row {y} is full!", v, s + 1);
		return true;
	}
}
