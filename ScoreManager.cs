namespace NotQuiteTetris;
public static class ScoreManager {
	public static int Score { get; set; } = 0;

	public const int ScoreGoal = 5000;

	public static int Multiplier { get; set; } = 1;

	public static int LinesCleared { get; set; } = 0;

	public const int BaseScore = 100;

	public static int HighScore { get; set; } = 0;


	public static void AddScore(int linesCleared, bool v = false, int s = 0) {
		if (linesCleared <= 0) {
			return;
		}

		Log.Me(() => $"Adding score for clearing {linesCleared} line(s)...", v, s + 1);
		int scoreToAdd = BaseScore * linesCleared * Multiplier;
		Score += scoreToAdd;
		LinesCleared += linesCleared;

		Log.Me(() => $"Added {scoreToAdd} points. Total score is now {Score}.", v, s + 1);
		if (linesCleared >= 2) {
			Multiplier++;
			Log.Me(() => $"Increased multiplier to {Multiplier}.", v, s + 1);
		}

		if (Score > HighScore) {
			HighScore = Score;
			Log.Me(() => $"New high score: {HighScore}", v, s + 1);
		}

		// End game	if score goal reached
		if (Score >= ScoreGoal) {
			Log.Me(() => $"Score goal of {ScoreGoal} reached! Ending game...", v, s + 1);
			GameManager.IsGameOngoing = false;
		}
	}


	public static void ResetScore(bool v = false, int s = 0) {
		Log.Me(() => "Resetting score...", v, s + 1);
		Score = 0;
		Multiplier = 1;
		LinesCleared = 0;

		Log.Me(() => "Score reset.", v, s + 1);
	}


	public static void SaveScore(bool v = false, int s = 0) {
		Log.Me(() => "Saving score...", v, s + 1);
		if (Score > HighScore) {
			HighScore = Score;
			Log.Me(() => $"New high score: {HighScore}", v, s + 1);
		}
		
		else {
			Log.Me(() => $"Score of {Score} did not beat high score of {HighScore}.", v, s + 1);
		}

		Log.Me(() => "Done!", v, s + 1);
	}
}
