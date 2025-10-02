using Raylib_cs;
namespace NotQuiteTetris;

public class BlockSubclasses {
	public class IBlock : Block {
		public IBlock(
			bool v = false,
			int s = 0
		) : base(
			grid: [
				[true, true, true, true]
			],
			color: Color.SkyBlue,
			v: v,
			s: s + 1
		) {
			Log.Me(() => "Created IBlock.", v, s + 1);
		}
	}
}
