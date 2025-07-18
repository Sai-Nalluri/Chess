namespace Chess.Core;


public static class BitBoardUtility
{
    public static void SetSquare(ref ulong bitboard, int squareIndex)
    {
        bitboard |= 1UL << squareIndex;
    }

    public static void ClearSquare(ref ulong bitboard, int squareIndex)
    {
        bitboard &= ~(1UL << squareIndex);
    }

    public static void ToggleSquare(ref ulong bitboard, int squareIndex)
    {
        bitboard ^= ~(1UL << squareIndex);
    }

    // Toggle 2 squares
    public static void ToggleSquare(ref ulong bitboard, int startSquareIndex, int targetSquareIndex)
    {
        bitboard ^= ~(1UL << startSquareIndex | 1UL << targetSquareIndex);
    }

    public static ulong Shift(ulong bitboard, int numToShift)
    {
        if (numToShift >= 0)
        {
            return bitboard <<= numToShift;
        }
        else
        {
            return bitboard >>= numToShift;
        }
    }

    public static bool IsSet(ulong bitboard, int squareIndex)
    {
        return (bitboard & (1UL << squareIndex)) != 0;
    }
}