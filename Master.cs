using Raylib_cs;
namespace NotQuiteTetris;

public class Master
{
    public const bool LogBasic = true;

    public const bool LogUpdate = true;

	static void Main() {
		Log.Me(() => "Starting game...", LogBasic);
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		Raylib.SetTargetFPS(120);

		Log.Me(() => "Doing initial setup...", LogBasic);
		ResourceManager.Initialize(LogBasic);
		GameRenderer.Initialize(LogBasic);
		GameManager.InitializeHand(LogBasic);

		Log.Me(() => "Loading game from file...", LogBasic);
		GameManager.LoadGameProgress(LogBasic);

		Log.Me(() => "Entering main game loop...", LogBasic);
		while (!Raylib.WindowShouldClose()) {
			Log.Me(() => "Updating game state...", LogUpdate);
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.RayWhite);

			InputManager.Update(LogUpdate);
			GameRenderer.Update(LogUpdate);

			ResourceManager.PlayMusic(LogUpdate);

			Log.Me(() => "Frame complete.\n", LogUpdate);
		}
		Log.Me(() => "Exited main game loop. Saving game...", LogBasic);
		GameManager.SaveGameProgress(LogBasic);

		Log.Me(() => "Exiting game...", LogBasic);
		Raylib.CloseWindow();
	}
}
