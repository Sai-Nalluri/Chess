using System;
using System.Collections.Generic;

namespace Chess.Core;

public static class PrecomputedMoveData
{
    public static readonly int[] directionOffsets = { 8, -8, -1, 1, 7, -7, 9, -9 };
    public static readonly int[][] numSquaresToEdge = new int[64][];

    public static readonly int[][] knightSquares = new int[64][];
    public static readonly int[][] kingSquares = new int[64][];

    public static void ComputeMoveData()
    {
        int[] allKnightJumpOffsets = { 15, 17, 6, 10, -10, -6, -17, -15 };

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

                var legalKnightJumps = new List<int>();
                foreach (int knightJumpOffset in allKnightJumpOffsets)
                {
                    int knightJumpSquare = squareIndex + knightJumpOffset;
                    if (knightJumpSquare >= 0 && knightJumpSquare < 64)
                    {
                        int knightSquareRank = knightJumpSquare / 8;
                        int knightSquareFile = knightJumpSquare % 8;
                        // If the knight jumps more than 2 squares, that means it wraped around the board and is invalid
                        int maxMoveDistance = Math.Max(Math.Abs(rank - knightSquareRank), Math.Abs(file - knightSquareFile));
                        if (maxMoveDistance == 2)
                        {
                            legalKnightJumps.Add(knightJumpSquare);
                        }
                    }
                }
                knightSquares[squareIndex] = legalKnightJumps.ToArray();

                var legalKingMoves = new List<int>();
                foreach (int kingMoveOffset in directionOffsets)
                {
                    int kingMoveSquare = squareIndex + kingMoveOffset;
                    if (kingMoveSquare >= 0 && kingMoveSquare < 64)
                    {
                        int kingMoveRank = kingMoveSquare / 8;
                        int kingMoveFile = kingMoveSquare % 8;
                        // If the king moved more than one square, he wrapped around the board
                        int maxMoveDistance = Math.Max(Math.Abs(rank - kingMoveRank), Math.Abs(file - kingMoveFile));
                        if (maxMoveDistance == 1)
                        {
                            legalKingMoves.Add(kingMoveSquare);
                        }
                    }
                }
                kingSquares[squareIndex] = legalKingMoves.ToArray();
            }
        }
    }
}