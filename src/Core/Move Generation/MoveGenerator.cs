using System;
using System.Collections.Generic;
using System.Linq;
using static Chess.Core.PrecomputedMoveData;

namespace Chess.Core;


public class MoveGenerator
{
    public const int MaxMoves = 218;

    public enum PromotionMode { All, QueenOnly, QueenAndKnight };
    public PromotionMode promotionsToGenerate = PromotionMode.All;

    bool isWhiteToMove;
    int friendlyColor;
    int opponentColor;
    int friendlyColorIndex;
    int opponentColorIndex;
    ulong friendlyPieceBitboard;
    ulong opponentPieceBitboard;
    ulong emptySquares;
    ulong enemyPieces;
    ulong allPieceBitboards;
    int currentMoveIndex;

    Board board = new Board();

    public List<Move> GenerateMoves(Board board)
    {
        List<Move> moves = new List<Move>(MaxMoves);
        return GenerateMoves(board, moves);
    }

    public List<Move> GenerateMoves(Board board, List<Move> moves)
    {
        this.board = board;
        Init();

        GenerateKingMoves(moves);
        GeneratePawnMoves(moves);
        GenerateSlidingMoves(moves);
        GenerateKnightMoves(moves);

        return moves.GetRange(0, currentMoveIndex);
    }

    void GenerateKingMoves(List<Move> moves)
    {

    }
    void GenerateSlidingMoves(List<Move> moves)
    {
        PieceList rooks = board.Rooks[board.moveColorIndex];
        for (int index = 0; index < rooks.Count; index++)
        {
            GenerateSlidingPiecesMoves(rooks[index], 0, 4, moves);
        }

        PieceList bishops = board.Bishops[board.moveColorIndex];
        for (int index = 0; index < bishops.Count; index++)
        {
            GenerateSlidingPiecesMoves(bishops[index], 4, 8, moves);
        }

        PieceList queens = board.Queens[board.moveColorIndex];
        for (int index = 0; index < queens.Count; index++)
        {
            GenerateSlidingPiecesMoves(queens[index], 0, 8, moves);
        }
    }

    void GenerateSlidingPiecesMoves(int startSquare, int startDirectionIndex, int endDirectionIndex, List<Move> moves)
    {
        for (int directionIndex = startDirectionIndex; directionIndex < endDirectionIndex; directionIndex++)
        {
            int directionOffset = directionOffsets[directionIndex];
            for (int n = 0; n < numSquaresToEdge[startSquare][directionIndex]; n++)
            {
                int targetSquare = startSquare + directionOffset * (n + 1);
                int targetSquarePiece = board.Square[targetSquare];

                if (Piece.IsColor(targetSquarePiece, friendlyColor))
                {
                    break;
                }

                moves.Add(new Move(startSquare, targetSquare));
                currentMoveIndex++;

                bool isCapture = targetSquarePiece != Piece.None;

                if (isCapture)
                {
                    break;
                }
            }
        }
    }

    void GeneratePawnMoves(List<Move> moves)
    {
        int pushDir = board.isWhiteToMove ? 1 : -1;
        int pushOffset = pushDir * 8;

        int friendlyPawnPiece = Piece.MakePiece(Piece.Pawn, board.moveColor);
        ulong pawns = board.pieceBitboards[friendlyPawnPiece];

        ulong singlePush = BitBoardUtility.Shift(pawns, pushOffset) & emptySquares;
        ulong promotionRankMask = board.isWhiteToMove ? BitBoardUtility.Rank1 : BitBoardUtility.Rank8;

        ulong pushPromotions = singlePush & promotionRankMask;

        ulong singlePushNoPromotions = singlePush & ~promotionRankMask;

        // Single and double push
        while (singlePushNoPromotions != 0)
        {
            int targetSquare = BitBoardUtility.PopLSB(ref singlePushNoPromotions);
            int startSquare = targetSquare - pushOffset;
            moves.Add(new Move(startSquare, targetSquare));
            currentMoveIndex++;
        }

        // This is where pawns would land if they were push 2 squares from the correct ranks
        // & it to doublePush so only the right squares are made to a move
        ulong doublePushTargetRankMask = board.isWhiteToMove ? BitBoardUtility.Rank4 : BitBoardUtility.Rank5;
        ulong doublePush = BitBoardUtility.Shift(singlePush, pushOffset) & emptySquares & doublePushTargetRankMask;

        while (doublePush != 0)
        {
            int targetSquare = BitBoardUtility.PopLSB(ref doublePush);
            int startSquare = targetSquare - pushOffset * 2;
            moves.Add(new Move(startSquare, targetSquare, Move.PawnTwoUpFlag));
            currentMoveIndex++;
        }

        ulong captureEdgeFileMask1 = board.isWhiteToMove ? BitBoardUtility.notAFile : BitBoardUtility.notHFile;
        ulong captureEdgeFileMask2 = board.isWhiteToMove ? BitBoardUtility.notHFile : BitBoardUtility.notAFile;
        ulong captureA = BitBoardUtility.Shift(pawns & captureEdgeFileMask1, pushDir * 7) & enemyPieces;
        ulong captureB = BitBoardUtility.Shift(pawns & captureEdgeFileMask2, pushDir * 9) & enemyPieces;

        ulong capturePromotionA = captureA & promotionRankMask;
        ulong capturePromotionB = captureB & promotionRankMask;

        captureA &= ~promotionRankMask;
        captureB &= ~promotionRankMask;

        // Captures with no promotion
        while (captureA != 0)
        {
            int targetSquare = BitBoardUtility.PopLSB(ref captureA);
            int startSquare = targetSquare - pushDir * 7;
            moves.Add(new Move(startSquare, targetSquare));
            currentMoveIndex++;
        }

        while (captureB != 0)
        {
            int targetSquare = BitBoardUtility.PopLSB(ref captureB);
            int startSquare = targetSquare - pushDir * 9;
            moves.Add(new Move(startSquare, targetSquare));
            currentMoveIndex++;
        }

        // Plain promotions
        while (pushPromotions != 0)
        {
            int targetSquare = BitBoardUtility.PopLSB(ref pushPromotions);
            int startSquare = targetSquare - pushOffset;
            GeneratePromotions(startSquare, targetSquare, moves);
        }

        while (capturePromotionA != 0)
        {
            int targetSquare = BitBoardUtility.PopLSB(ref capturePromotionA);
            int startSquare = targetSquare - pushDir * 7;
            GeneratePromotions(startSquare, targetSquare, moves);
        }

        while (capturePromotionB != 0)
        {
            int targetSquare = BitBoardUtility.PopLSB(ref capturePromotionB);
            int startSquare = targetSquare - pushDir * 9;
            GeneratePromotions(startSquare, targetSquare, moves);
        }
    }

    void GeneratePromotions(int startSquare, int targetSquare, List<Move> moves)
    {
        moves.Add(new Move(startSquare, targetSquare, Move.PromoteToQueenFlag));

        if (promotionsToGenerate == PromotionMode.All)
        {
            moves.Add(new Move(startSquare, targetSquare, Move.PromoteToBishopFlag));
            moves.Add(new Move(startSquare, targetSquare, Move.PromoteToRookFlag));
            moves.Add(new Move(startSquare, targetSquare, Move.PromoteToKnightFlag));
        }
        else if (promotionsToGenerate == PromotionMode.QueenAndKnight)
        {
            moves.Add(new Move(startSquare, targetSquare, Move.PromoteToKnightFlag));
        }
    }

    void GenerateKnightMoves(List<Move> moves)
    {
        PieceList knights = board.Knights[board.moveColorIndex];
        for (int index = 0; index < knights.Count; index++)
        {
            int startSquare = knights[index];
            foreach (int targetSquare in knightSquares[startSquare])
            {
                if (!Piece.IsColor(board.Square[targetSquare], friendlyColor))
                {
                    moves.Add(new Move(startSquare, targetSquare));
                    currentMoveIndex++;
                }
            }
        }
    }

    void Init()
    {
        currentMoveIndex = 0;

        isWhiteToMove = board.moveColor == Piece.White;
        friendlyColor = board.moveColor;
        opponentColor = board.opponentColor;
        friendlyColorIndex = board.moveColorIndex;
        opponentColorIndex = board.opponentColorIndex;

        opponentPieceBitboard = board.colorBitboards[opponentColorIndex];
        friendlyPieceBitboard = board.colorBitboards[friendlyColorIndex];
        allPieceBitboards = board.allPiecesBitboard;

        emptySquares = ~allPieceBitboards;
        enemyPieces = ~(emptySquares | friendlyPieceBitboard);
    }
}