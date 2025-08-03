using Avalonia.Media;
namespace Chess.UI;

public class BoardTheme
{
    public SquareColors lightSquares;
    public SquareColors darkSquares;

    public struct SquareColors
    {
        public SolidColorBrush normal;
        public SolidColorBrush legal;
        public SolidColorBrush selected;
        public SolidColorBrush moveFromHighlight;
        public SolidColorBrush moveToHighlight;
    }
}