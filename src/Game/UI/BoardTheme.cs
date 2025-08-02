using Avalonia.Media;
namespace Chess.UI;

public static class BoardTheme
{
    public static SquareColors lightSquares;
    public static SquareColors darkSquares;

    public struct SquareColors
    {
        public Color normal;
        public Color legal;
        public Color selected;
        public Color moveFromHighlight;
        public Color moveToHighlight;
    }
}