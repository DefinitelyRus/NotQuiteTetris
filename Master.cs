using System.Resources;
using Raylib_cs;

namespace NotQuiteTetris
{
    public class Master
    {

        public static bool LogBasic = true;

        public static bool LogUpdate = true;

		static void Main(string[] args) {
			Log.Me(() => "Starting game...", LogBasic);
			Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
			Raylib.InitWindow(1280, 720, "Not quite Tetris");
			Raylib.InitAudioDevice();
			Raylib.SetMasterVolume(0.6f);
			Raylib.SetTargetFPS(60);

			Log.Me(() => "Entering main game loop...", LogBasic);
			while (!Raylib.WindowShouldClose()) {
				Log.Me(() => "Updating game state...", LogUpdate);
				Raylib.BeginDrawing();
				Raylib.ClearBackground(Color.RayWhite);

				GridRenderer.Update(LogUpdate);

				Raylib.EndDrawing();
				Log.Me(() => "Frame complete.\n", LogUpdate);
			}

			Log.Me(() => "Exiting game...", LogBasic);
			Raylib.CloseWindow();
		}
    }
}
