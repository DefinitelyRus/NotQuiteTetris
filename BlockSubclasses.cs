namespace NotQuiteTetris;

public class IBlock : Block {
	public IBlock(
		bool v = false,
		int s = 0
	) : base(
		grid: [
			[true, true, true, true]
		],
		color: GameRenderer.ColorPalette["IBlock"],
		v: v,
		s: s + 1
	) {
		Log.Me(() => "Created IBlock.", v, s + 1);
	}
}


public class JBlock : Block {
	public JBlock(
		bool v = false,
		int s = 0
	) : base(
		grid: [
			[true],
			[true, true, true]
		],
		color: GameRenderer.ColorPalette["JBlock"],
		v: v,
		s: s + 1
	) {
		Log.Me(() => "Created JBlock.", v, s + 1);
	}
}

public class LBlock : Block {
	public LBlock(
		bool v = false,
		int s = 0
	) : base(
		grid: [
			[false, false, true],
			[true, true, true]
		],
		color: GameRenderer.ColorPalette["LBlock"],
		v: v,
		s: s + 1
	) {
		Log.Me(() => "Created LBlock.", v, s + 1);
	}
}


public class OBlock : Block {
	public OBlock(
		bool v = false,
		int s = 0
	) : base(
		grid: [
			[true, true],
			[true, true]
		],
		color: GameRenderer.ColorPalette["OBlock"],
		v: v,
		s: s + 1
	) {
		Log.Me(() => "Created OBlock.", v, s + 1);
	}
}


public class SBlock : Block {
	public SBlock(
		bool v = false,
		int s = 0
	) : base(
		grid: [
			[false, true, true],
			[true, true, false]
		],
		color: GameRenderer.ColorPalette["SBlock"],
		v: v,
		s: s + 1
	) {
		Log.Me(() => "Created SBlock.", v, s + 1);
	}
}


public class ZBlock : Block {
	public ZBlock(
		bool v = false,
		int s = 0
	) : base(
		grid: [
			[true, true, false],
			[false, true, true]
		],
		color: GameRenderer.ColorPalette["ZBlock"],
		v: v,
		s: s + 1
	) {
		Log.Me(() => "Created ZBlock.", v, s + 1);
	}
}


public class DotBlock : Block {
	public DotBlock(
		bool v = false,
		int s = 0
	) : base(
		grid: [
			[true]
		],
		color: GameRenderer.ColorPalette[".Block"],
		v: v,
		s: s + 1
	) {
		Log.Me(() => "Created DotBlock.", v, s + 1);
	}
}