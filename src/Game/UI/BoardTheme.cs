using Microsoft.Xna.Framework;

namespace Chess.UI;

public class BoardTheme
{
    public SquareColors lightSquares;
    public SquareColors darkSquares;

    public struct SquareColors
    {
        public Color normal;
        public Color legal;
        public Color selected;
        public Color moveFromHighlight;
        public Color moveToHighlight;
    }
}