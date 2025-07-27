using System;

namespace Chess.Core;

public static class PrecomputedMoveData
{
    public static readonly int[] directionOffsets = { 8, -8, -1, 1, 7, -7, 9, -9 };
    public static readonly int[][] numSquaresToEdge = new int[64][];

    static PrecomputedMoveData()
    {
        for (int file = 0; file < 8; file++)
        {
            for (int rank = 0; rank < 8; rank++)
            {
                int numNorth = 7 - rank;
                int numSouth = rank;
                int numWest = file;
                int numEast = 7 - file;

                int squareIndex = rank * 8 + file;

                numSquaresToEdge[squareIndex] = [
                    numNorth,
                    numSouth,
                    numWest,
                    numEast,
                    Math.Min(numNorth, numWest),
                    Math.Min(numSouth, numEast),
                    Math.Min(numNorth, numEast),
                    Math.Min(numSouth, numWest),
                ];
            }
        }
    }
}