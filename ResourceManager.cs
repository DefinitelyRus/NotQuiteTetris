using Raylib_cs;
namespace NotQuiteTetris;
internal static class ResourceManager {

	public static Dictionary<string, Texture2D> Textures = [];
	public static Dictionary<string, Sound> Sounds = [];

	public static Music MusicStream = new();

	public const string Title = "NotQuiteTetris";

	public static string SaveFilePath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Title + "save.json";


	public static void Initialize(bool v = false, int s = 0) {
		Log.Me(() => "Initializing resource manager...", v, s + 1);

		Log.Me(() => "Initializing audio device...", v, s + 1);
		Raylib.InitAudioDevice();
		Raylib.SetMasterVolume(0.6f);

		Log.Me(() => "Done!", v, s + 1);
	}


	public static void PreloadAssets(bool v = false, int s = 0) {
		Log.Me(() => "Preloading assets...", v, s + 1);

		// Load textures
		Log.Me(() => "Loading textures...", v, s + 1);
		string[] textureFiles = Directory.GetFiles("Assets\\Sprites", "*.png");
		foreach (string file in textureFiles) {
			string name = Path.GetFileNameWithoutExtension(file);
			Log.Me(() => $"Loading texture \"{name}\" from \"{file}\"...", v, s + 1);
			Texture2D texture = new();

			try {
				texture = Raylib.LoadTexture(file);
			}
			catch (AccessViolationException) {
				Log.Err($"Unable to access file \"{name}\" from \"{file}\".", true, s + 1);
				continue;
			}

			Textures[name] = texture;
		}

		// Load sounds
		Log.Me(() => "Loading sounds...", v, s + 1);
		string[] audioFiles = Directory.GetFiles("Assets\\Audio", "*.wav");
		foreach (string file in audioFiles) {
			string name = Path.GetFileNameWithoutExtension(file);
			Log.Me(() => $"Loading sound \"{name}\" from \"{file}\"...", v, s + 1);
			Sound sound = Raylib.LoadSound(file);
			Sounds[name] = sound;
		}

		Log.Me(() => "All assets loaded.", v, s + 1);
	}


	public static Texture2D GetTexture(string name, bool v = false, int s = 0) {
		Log.Me(() => $"Getting texture \"{name}\"...", v, s + 1);
		if (Textures.TryGetValue(name, out Texture2D texture)) return texture;

		Log.Err(() => $"Texture '{name}' not found in ResourceManager.", true, s + 1);
		return new();
	}


	public static Sound GetSound(string name, bool v = false, int s = 0) {
		Log.Me(() => $"Getting sound \"{name}\"...", v, s + 1);
		if (Sounds.TryGetValue(name, out Sound sound)) return sound;

		Log.Err(() => $"Sound '{name}' not found in ResourceManager.", true, s + 1);
		return new();
	}


	public static void PlaySound(string name, bool v = false, int s = 0) {
		Log.Me(() => $"Playing sound \"{name}\"...", v, s + 1);
		Sound sound = GetSound(name, v, s + 1);
		Raylib.PlaySound(sound);

		Log.Me(() => "Done!", v, s + 1);
	}


	public static void PlayMusic(bool v = false, int s = 0) {
		Raylib.UpdateMusicStream(MusicStream);
		if (Raylib.IsMusicStreamPlaying(MusicStream)) return;

		Log.Me(() => "Finding tetoris...", v, s + 1);
		string filePath = $"Assets\\Audio\\tetoris.wav";
		string fileName = Path.GetFileNameWithoutExtension(filePath);
		MusicStream = Raylib.LoadMusicStream(filePath);

		Raylib.PlayMusicStream(MusicStream);
		Log.Me(() => $"Now playing: {fileName}.", v, s + 1);
	}


	public static void SaveToFile(string content, bool v = false, int s = 0) {
		Log.Me(() => $"Saving to file \"{SaveFilePath}\"...", v, s + 1);

		try {
			File.WriteAllText(SaveFilePath, content);
		}

		catch (Exception e) {
			Log.Err(() => $"Failed to save to file \"{SaveFilePath}\": {e.Message}", true, s + 1);
			return;
		}
	}


	public static string? LoadFromFile(bool v = false, int s = 0) {
		Log.Me(() => $"Loading from file \"{SaveFilePath}\"...", v, s + 1);
		string content;

		try {
			content = File.ReadAllText(SaveFilePath);
		}

		catch (Exception e) {
			Log.Err(() => $"Failed to load from file \"{SaveFilePath}\": {e.Message}", true, s + 1);
			return null;
		}

		return content;
	}
}
