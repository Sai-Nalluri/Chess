namespace Chess.Core;

public static class BoardHelper
{
    public static int RankIndex(int squareIndex)
    {
        return squareIndex / 8;
    }
}