using System.Numerics;
using Raylib_cs;

namespace NotQuiteTetris;

public static class InputManager {

	public const MouseButton M1 = MouseButton.Left;

	public const KeyboardKey BX = KeyboardKey.Grave;

	public const KeyboardKey B1 = KeyboardKey.One;

	public const KeyboardKey B2 = KeyboardKey.Two;

	public const KeyboardKey B3 = KeyboardKey.Three;

	public const KeyboardKey B4 = KeyboardKey.Four;

	public const KeyboardKey Reset = KeyboardKey.F5;

	public static int SelectedBlockIndex { get; private set; } = -1;

	public static void Update(bool v = false, int s = 0) {
		Log.Me(() => "Updating input...", v, s + 1);

		// Place block
		if (Raylib.IsMouseButtonPressed(M1) && GridRenderer.IsMouseInsideGrid && SelectedBlockIndex >= 0) {
			GameManager.UseBlock(SelectedBlockIndex, v, s + 1);
		}

		// Deselect block
		if (Raylib.IsKeyPressed(BX)) {
			SelectedBlockIndex = -1;
			Log.Me(() => "Deselected block.", v, s + 1);
		}

		// Select block 1
		if (Raylib.IsKeyPressed(B1)) {
			SelectedBlockIndex = 0;
			Log.Me(() => "Selected block 1.", v, s + 1);
		}

		// Select block 2
		if (Raylib.IsKeyPressed(B2)) {
			SelectedBlockIndex = 1;
			Log.Me(() => "Selected block 2.", v, s + 1);
		}

		// Select block 3
		if (Raylib.IsKeyPressed(B3)) {
			SelectedBlockIndex = 2;
			Log.Me(() => "Selected block 3.", v, s + 1);
		}

		// Select block 4
		if (Raylib.IsKeyPressed(B4)) {
			SelectedBlockIndex = 3;
			Log.Me(() => "Selected block 4.", v, s + 1);
		}

		// Reset game
		if (Raylib.IsKeyPressed(Reset)) {
			Log.Me(() => "Resetting game...", v, s + 1);
			GameManager.SoftReset(v, s + 1);
		}

		Log.Me(() => "Done!", v, s + 1);
	}
}
