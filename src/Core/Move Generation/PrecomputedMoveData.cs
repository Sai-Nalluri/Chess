using System;

namespace Chess.Core;

public static class PrecomputedMoveData
{
    public static readonly int[] directionOffsets = { 8, -8, -1, 1, 7, -7, 9, -9 };
    public static readonly int[][] numSquaresToEdge = new int[64][];

    public static void ComputeMoveData()
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                // How far away the current square is from the edges of the board
                int numToNorth = 7 - rank;
                int numToSouth = rank;
                int numToWest = file;
                int numToEast = 7 - file;

                int squareIndex = rank * 8 + file;

                numSquaresToEdge[squareIndex] = [
                    numToNorth,
                    numToSouth,
                    numToWest,
                    numToEast,
                    Math.Min(numToNorth, numToWest), // North West
                    Math.Min(numToSouth, numToEast), // South East
                    Math.Min(numToNorth, numToEast), // North East
                    Math.Min(numToSouth, numToWest), // South West
                ];
            }
        }
    }
}