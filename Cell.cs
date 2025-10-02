using System.Numerics;
using Raylib_cs;
namespace NotQuiteTetris;

public class Cell {
	public Vector2 Position { get; private set; }
	
	public Color Color { get; private set; }

	public Cell(Vector2 position, Color color) {
		Log.Me(() => "Creating new cell...");
		Position = position;
		Color = color;

		Log.Me(() => "Done!");
	}
}
