using System.Data.Common;
using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;

namespace NotQuiteTetris;
public static class GameRenderer {

	private const int ResolutionX = 1280;

	private const int ResolutionY = 720;

	private const string Title = "Not Quite Tetris";

	private const int TargetFPS = 60;

	private const int GameElementSpacing = 100;

	private static (int, int) ScorePosition = (ResolutionX - 200, 20);

	private const string ScoreText = "Score: ";

	private static (int, int) LinesPosition = (ResolutionX - 200, 60);

	private const string LinesText = "Lines: ";

	private static (int, int) MultiplierPosition = (ResolutionX - 200, 100);

	private const string MultiplierText = "Multiplier: ";

	private static (int, int) HighScorePosition = (ResolutionX - 200, 140);

	private const string HighScoreText = "High Score: ";

	private const string WinSuffix = "You win!";

	private const string LoseSuffix = "You lose!";

	public static Texture2D BackgroundImage { get; private set; }


	public static Color BackgroundColor { get; private set; } = Color.Black;


	public static Dictionary<string, Color> ColorPalette { get; } = [];


	public static void Initialize(bool v = false, int s = 0) {
		Log.Me(() => "Initializing game renderer...", v, s + 1);
		Raylib.InitWindow(ResolutionX, ResolutionY, Title);
		Raylib.SetTargetFPS(TargetFPS);

		// Assign color palette
		ColorPalette["GridLines"] = Color.DarkGray;
		ColorPalette["Text"] = Color.White;
		ColorPalette["Background"] = Color.Black;
		ColorPalette["PreviewValid"] = Color.Lime;
		ColorPalette["PreviewInvalid"] = Color.Red;
		ColorPalette[".Block"] = Color.Gold;
		ColorPalette["OBlock"] = Color.Yellow;
		ColorPalette["IBlock"] = Color.SkyBlue;
		ColorPalette["TBlock"] = Color.Purple;
		ColorPalette["SBlock"] = Color.Lime;
		ColorPalette["ZBlock"] = Color.Pink;
		ColorPalette["JBlock"] = Color.DarkBlue;
		ColorPalette["LBlock"] = Color.Orange;
		ColorPalette["Empty"] = Color.LightGray;

		// Set background
		ResourceManager.PreloadAssets(v, s + 1);
		BackgroundImage = ResourceManager.GetTexture("background", v, s + 1);

		Log.Me(() => "Done!", v, s + 1);
	}


	public static void Update(bool v = false, int s = 0) {
		Log.Me(() => "Rendering frame...", v, s + 1);
		Raylib.BeginDrawing();

		// Draw background
		if (BackgroundImage.Width > 0 || BackgroundImage.Height > 0) {
			Log.Me(() => "Drawing background image...", v, s + 1);
			Raylib.DrawTexture(BackgroundImage, 0, 0, Color.White);
		}

		// Fallback: draw solid color
		else {
			Log.Warn(() => "No background image set, using solid color instead.", v, s + 1);
			Raylib.ClearBackground(BackgroundColor);
		}

		// Render grid
		Log.Me(() => "Updating grid renderer...", v, s + 1);
		GridRenderer.Update(false, s + 1);


		// Display hand preview
		Log.Me(() => "Displaying hand...", v, s + 1);
		for (int i = 0; i < GameManager.Hand.Length; i++) {
			Block? block = GameManager.Hand[i];
			if (block == null) continue;

			int posX = GridRenderer.EndPosX + GameElementSpacing;
			int posY = GridRenderer.PosY + i * (GridRenderer.CellSize / 2 + GridRenderer.Spacing) * 4;

			RenderBlockInHand(block, new Vector2(posX, posY), false, s + 1);
		}


		// Preview selected block
		bool hasSelectedBlock = InputManager.SelectedBlockIndex >= 0;
		bool mouseInGrid = GameManager.IsMouseInGrid(v, s + 1);
		bool isGameOngoing = GameManager.IsGameOngoing;
		if (hasSelectedBlock && mouseInGrid && isGameOngoing) {
			Log.Me(() => "Previewing selected block...", v, s + 1);
			Block? block = GameManager.Hand[InputManager.SelectedBlockIndex];

			if (block != null) {
				// Get grid position under mouse
				(int, int) gridPos = GridManager.GetGridCoordinates(Raylib.GetMousePosition(), v, s + 1);
				GridRenderer.PreviewBlock(block, gridPos.Item1, gridPos.Item2, v, s + 1);
			}
		}


		// Draw score
		Log.Me(() => "Drawing score...", v, s + 1);
		Raylib.DrawText(ScoreText + ScoreManager.Score, ScorePosition.Item1, ScorePosition.Item2, 20, ColorPalette["Text"]);
		Raylib.DrawText(LinesText + ScoreManager.LinesCleared, LinesPosition.Item1, LinesPosition.Item2, 20, ColorPalette["Text"]);
		Raylib.DrawText(MultiplierText + ScoreManager.Multiplier + "x", MultiplierPosition.Item1, MultiplierPosition.Item2, 20, ColorPalette["Text"]);
		Raylib.DrawText(HighScoreText + ScoreManager.HighScore, HighScorePosition.Item1, HighScorePosition.Item2, 20, ColorPalette["Text"]);

		// Draw game over text
		if (!isGameOngoing) {
			Log.Me(() => "Rendering game over text...", v, s + 1);
			bool won = ScoreManager.ScoreGoal <= ScoreManager.Score;
			string gameOverText = $"Game Over!\n{(won ? WinSuffix : LoseSuffix)}";
			int fontSize = 60;
			int textWidth = Raylib.MeasureText(gameOverText, fontSize);
			int textX = (ResolutionX - textWidth) / 2;
			int textY = (ResolutionY - fontSize) / 2;
			Raylib.DrawText(gameOverText, textX, textY, fontSize, ColorPalette["Text"]);

			if (won) ResourceManager.PlaySound("end_win", v, s + 1);
			else ResourceManager.PlaySound("end_lose", v, s + 1);
		}

		Raylib.EndDrawing();
		Log.Me(() => "Done!", v, s + 1);
	}


	private static void RenderBlockInHand(Block block, Vector2 position, bool v = false, int s = 0) {
		int posX = (int) position.X;
		int posY = (int) position.Y;

		Log.Me(() => $"Displaying a {block.Width}x{block.Height} block of type {block.GetType().Name} at position ({posX}, {posY})...", v, s + 1);

		// For each column...
		for (int x = 0; x < block.Width; x++) {

			// For each cell (row) in the column...
			for (int y = 0; y < block.Height; y++) {

				// If the block has a cell at this position, draw it.
				if (block.HasCellAt(x, y)) {
					Rectangle rect = new(
						posX + x * (GridRenderer.CellSize / 2 + GridRenderer.Spacing),
						posY + y * (GridRenderer.CellSize / 2 + GridRenderer.Spacing),
						GridRenderer.CellSize / 2,
						GridRenderer.CellSize / 2
					);

					Log.Me(() => "Drawing cell...", v, s + 1);
					Raylib.DrawRectangleRec(rect, block.Color);
				}
			}
		}

		Log.Me(() => "Done!", v, s + 1);
	}

}
