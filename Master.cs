using Raylib_cs;
namespace NotQuiteTetris;

public class Master
{
    public const bool LogBasic = true;

    public const bool LogUpdate = true;

	private static bool IsOnMainMenu = true;

	static void Main() {
		Log.Me(() => "Starting game...", LogBasic);
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		GameRenderer.Initialize(LogBasic);
		ResourceManager.Initialize(LogBasic);

		Log.Me(() => "Entering main game loop...", LogBasic);
		while (!Raylib.WindowShouldClose()) {
			Log.Me(() => "Updating game state...", LogUpdate);
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.RayWhite);

			if (!IsOnMainMenu) {
				InputManager.Update(LogUpdate);
				GameRenderer.Update(LogUpdate);

				ResourceManager.PlayMusic(LogUpdate);
			}
			else {
				Log.Me(() => "Rendering main menu...", LogUpdate);

				Raylib.DrawText("Not Quite Tetris", 50, 100, 50, Color.DarkBlue);
				Raylib.DrawText("Press ENTER to start", 70, 200, 20, Color.DarkGray);

				if (Raylib.IsKeyPressed(KeyboardKey.Enter)) {
					Log.Me(() => "Starting game...", LogBasic);
					IsOnMainMenu = false;
					GameManager.InitializeHand(LogBasic);
					GameManager.LoadGameProgress(LogBasic);
				}

				Raylib.EndDrawing();
			}

			Log.Me(() => "Frame complete.\n", LogUpdate);
		}
		Log.Me(() => "Exited main game loop. Saving game...", LogBasic);
		GameManager.SaveGameProgress(LogBasic);

		Log.Me(() => "Exiting game...", LogBasic);
		Raylib.CloseWindow();
	}
}
