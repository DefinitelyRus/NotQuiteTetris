using Raylib_cs;

namespace NotQuiteTetris;
public static class GameManager {
	public static readonly char[] BlockPalette = ['I', 'J', 'L', 'O', 'S', 'Z', '.'];

	public static Block?[] Hand { get; private set; } = new Block[4];


	
	public static void InitializeHand(bool v = false, int s = 0) {
		Log.Me(() => "Initializing hand...", v, s + 1);

		for (int i = 0; i < Hand.Length; i++) {
			Log.Me(() => $"Filling hand at index {i}...", v, s + 1);
			Hand[i] = GetRandomBlock(v, s + 1);
		}

		Log.Me(() => "Done!", v, s + 1);
	}

	private static Block GetRandomBlock(bool v = false, int s = 0) {
		Log.Me(() => "Getting random block...", v, s + 1);
		Block? block = null;
		int index = Raylib.GetRandomValue(0, BlockPalette.Length - 1);

		block = BlockPalette[index] switch {
			'I' => new IBlock(v, s + 1),
			_ => throw new Exception($"Block type \"{BlockPalette[index]}\" is not recognized."),
		};

		Log.Me(() => $"Got block with type {block.GetType}.", v, s + 1);
		return block;
	}

	public static void UseBlock(int index, bool v = false, int s = 0) {
		if (index < 0 || index >= Hand.Length) {
			throw new IndexOutOfRangeException($"Index {index} is out of bounds for hand of size {Hand.Length}.");
		}

		if (Hand[index] == null) {
			Log.Me(() => $"Hand at index {index} is empty.", v, s + 1);
			return;
		}

		Log.Me(() => $"Using block at index {index}...", v, s + 1);

		Block block = Hand[index]!;
		Log.Me(() => $"Using block of type {block.GetType}.", v, s + 1);

		Log.Me(() => "Removing block from hand...", v, s + 1);
		Hand[index] = null;

		Log.Me(() => "Done!", v, s + 1);
	}
}
