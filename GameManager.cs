using Raylib_cs;
namespace NotQuiteTetris;

public static class GameManager {
	public static readonly char[] BlockPalette = ['I', 'J', 'L', 'O', 'S', 'Z', '.'];

	public static Block?[] Hand { get; private set; } = new Block[4];

	public static bool IsGameOngoing { get; set; } = true;


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
			'J' => new JBlock(v, s + 1),
			'L' => new LBlock(v, s + 1),
			'O' => new OBlock(v, s + 1),
			'S' => new SBlock(v, s + 1),
			'Z' => new ZBlock(v, s + 1),
			'.' => new DotBlock(v, s + 1),
			_ => throw new Exception($"Block type \"{BlockPalette[index]}\" is not recognized."),
		};

		Log.Me(() => "Rotating block to random orientation...", v, s + 1);
		int rotation = Raylib.GetRandomValue(0, 3);
		block.RotateTo(rotation, v, s + 1);

		Log.Me(() => $"Got block with type {block.GetType().Name} with {rotation * 90}deg rotation.", v, s + 1);
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

		Block block = Hand[index]!;
		Log.Me(() => $"Using block at index {index} with type {block.GetType().Name}...", v, s + 1);

		//Place block
		int mouseX = Raylib.GetMouseX();
		int mouseY = Raylib.GetMouseY();
		int gridX = (mouseX - GridRenderer.PosX) / (GridRenderer.CellSize + GridRenderer.Spacing);
		int gridY = (mouseY - GridRenderer.PosY) / (GridRenderer.CellSize + GridRenderer.Spacing);
		bool toPlace = GridManager.TryPlaceBlock(block, gridX, gridY, v, s + 1);

		// Has space
		if (toPlace) {
			Log.Me(() => $"Placing block at ({gridX}, {gridY}).", v, s + 1);
			GridManager.PlaceBlock(block, gridX, gridY, v, s + 1);
		}

		// No space
		else {
			Log.Me(() => $"Unable to place block at ({gridX}, {gridY}).", v, s + 1);
			return;
		}

		Log.Me(() => "Removing block from hand...", v, s + 1);
		Hand[index] = null;

		// Scan for full rows/columns
		int clearedLines = GridManager.ScanAndClearFullLines(v, s + 1);

		// Update score
		if (clearedLines > 0) {
			Log.Me(() => $"Cleared {clearedLines} lines. Updating score...", v, s + 1);
			ScoreManager.AddScore(clearedLines, v, s + 1);
		}

		// Check for game over
		if (!GridManager.HasValidSpot(v, s + 1)) {
			Log.Me(() => "No valid spots remain. Game over!", v, s + 1);
			IsGameOngoing = false;
		}

		// Check for hand refill
		bool toRefill = true;
		foreach (Block? b in Hand) {
			if (b != null) {
				Log.Me(() => "At least one block remains in hand. Not refilling hand.", v, s + 1);
				toRefill = false;
				break;
			}
		}

		if (toRefill) {
			Log.Me(() => "Hand is empty. Refilling hand...", v, s + 1);
			InitializeHand(v, s + 1);
		}

		Log.Me(() => "Done!", v, s + 1);
	}


	public static void SaveGameProgress(bool v = false, int s = 0) {
		Log.Me(() => "Saving game progress...", v, s + 1);
		Dictionary<string, string> jsonDict = [];

		// Save grid state
		Log.Me(() => "Saving grid state...", v, s + 1);
		string gridString = GridManager.GetGridAsString(v, s + 1);
		jsonDict["grid"] = gridString;

		// Save hand state
		Log.Me(() => "Saving hand state...", v, s + 1);
		for (int i = 0; i < Hand.Length; i++) {
			Block? block = Hand[i];
			string blockKey = $"hand_{i}";

			if (block == null) {
				jsonDict[blockKey] = "null";
				continue;
			}

			string blockType = block.GetType().Name;
			int rotation = block.Rotation;
			string blockValue = $"{blockType},{rotation}";
			jsonDict[blockKey] = blockValue;
		}

		// Save scores
		Log.Me(() => "Saving score state...", v, s + 1);
		jsonDict["score"] = ScoreManager.Score.ToString();
		jsonDict["linesCleared"] = ScoreManager.LinesCleared.ToString();
		jsonDict["multiplier"] = ScoreManager.Multiplier.ToString();
		jsonDict["highScore"] = ScoreManager.HighScore.ToString();
		jsonDict["isGameOngoing"] = IsGameOngoing.ToString();

		string jsonString = System.Text.Json.JsonSerializer.Serialize(jsonDict);

		// Write to file
		Log.Me(() => "Writing to file...", v, s + 1);
		ResourceManager.SaveToFile(jsonString, v, s + 1);

		Log.Me(() => "Done!", v, s + 1);
	}


	public static bool LoadGameProgress(bool v = false, int s = 0) {
		Log.Me(() => "Loading game progress from file...", v, s + 1);
		string? jsonString = ResourceManager.LoadFromFile(v, s + 1);

		if (string.IsNullOrEmpty(jsonString)) {
			Log.Me(() => "No saved game found.", v, s + 1);
			return false;
		}

		// Parse JSON
		Dictionary<string, string> jsonDict;
		try {
			jsonDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString)!;
		}

		catch (Exception e) {
			Log.Me(() => $"Failed to parse saved game data: {e.Message}", v, s + 1);
			return false;
		}

		// Load grid state
		Log.Me(() => "Loading grid state...", v, s + 1);
		if (jsonDict.TryGetValue("grid", out string? gridString)) {
			if (!string.IsNullOrEmpty(gridString)) {
				GridManager.ParseStringAsGrid(gridString, v, s + 1);
			}
		}

		// Load hand state
		Log.Me(() => "Loading hand state...", v, s + 1);
		for (int i = 0; i < Hand.Length; i++) {
			string blockKey = $"hand_{i}";
			if (jsonDict.TryGetValue(blockKey, out string? blockValue)) {
				if (blockValue == "null") {
					Hand[i] = null;
					continue;
				}
				string[] parts = blockValue.Split(',');
				if (parts.Length != 2) continue;
				string blockType = parts[0];
				if (!int.TryParse(parts[1], out int rotation)) continue;
				Block? block = blockType switch {
					nameof(IBlock) => new IBlock(v, s + 1),
					nameof(JBlock) => new JBlock(v, s + 1),
					nameof(LBlock) => new LBlock(v, s + 1),
					nameof(OBlock) => new OBlock(v, s + 1),
					nameof(SBlock) => new SBlock(v, s + 1),
					nameof(ZBlock) => new ZBlock(v, s + 1),
					nameof(DotBlock) => new DotBlock(v, s + 1),
					_ => null,
					};
				if (block != null) {
					block.RotateTo(rotation, v, s + 1);
					Hand[i] = block;
				}
				else {
					Hand[i] = null;
				}
			}
			else {
				Hand[i] = null;
			}
		}

		// Load scores
		Log.Me(() => "Loading score state...", v, s + 1);
		int score = 0;
		bool hasScore = jsonDict.TryGetValue("score", out string? scoreStr);
		bool parsedScore = hasScore && int.TryParse(scoreStr, out score);
		if (hasScore && parsedScore) {
			ScoreManager.Score = score;
		}

		int linesCleared = 0;
		bool hasLinesCleared = jsonDict.TryGetValue("linesCleared", out string? linesClearedStr);
		bool parsedLinesCleared = hasLinesCleared && int.TryParse(linesClearedStr, out linesCleared);
		if (hasLinesCleared && parsedLinesCleared) {
			ScoreManager.LinesCleared = linesCleared;
		}

		int multiplier = 1;
		bool hasMultiplier = jsonDict.TryGetValue("multiplier", out string? multiplierStr);
		bool parsedMultiplier = hasMultiplier && int.TryParse(multiplierStr, out multiplier);
		if (hasMultiplier && parsedMultiplier) {
			ScoreManager.Multiplier = multiplier;
		}

		int highScore = 0;
		bool hasHighScore = jsonDict.TryGetValue("highScore", out string? highScoreStr);
		bool parsedHighScore = hasHighScore && int.TryParse(highScoreStr, out highScore);
		if (hasHighScore && parsedHighScore) {
			ScoreManager.HighScore = highScore;
		}

		bool isGameOngoing = true;
		bool hasIsGameOngoing = jsonDict.TryGetValue("isGameOngoing", out string? isGameOngoingStr);
		bool parsedIsGameOngoing = hasIsGameOngoing && bool.TryParse(isGameOngoingStr, out isGameOngoing);
		if (hasIsGameOngoing && parsedIsGameOngoing) {
			IsGameOngoing = isGameOngoing;
		}

		Log.Me(() => "Done!", v, s + 1);
		return true;
	}


	public static bool SoftReset(bool v = false, int s = 0) {
		Log.Me(() => "Performing soft reset...", v, s + 1);
		GridManager.ClearGrid(v, s + 1);
		InitializeHand(v, s + 1);
		ScoreManager.ResetScore(v, s + 1);
		IsGameOngoing = true;

		Log.Me(() => "Done!", v, s + 1);
		return true;
	}

	public static bool IsMouseInGrid(bool v = false, int s = 0) {
		Log.Me(() => "Checking if mouse is inside grid...", v, s + 1);
		int mouseX = Raylib.GetMouseX();
		int mouseY = Raylib.GetMouseY();
		bool withinBoundsX = mouseX >= GridRenderer.PosX && mouseX <= GridRenderer.EndPosX;
		bool withinBoundsY = mouseY >= GridRenderer.PosY && mouseY <= GridRenderer.EndPosY;
		bool inGrid = withinBoundsX && withinBoundsY;

		Log.Me(() => $"Done!", v, s + 1);
		return inGrid;
	}
}
