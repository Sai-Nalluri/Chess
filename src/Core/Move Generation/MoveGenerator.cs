using System;
using System.Collections.Generic;
using System.Numerics;

namespace Chess.Core;


public class MoveGenerator
{
    List<Move> moves = new List<Move>();

    bool isWhiteToMove;
    int friendlyColor;
    int opponentColor;
    int friendlyColorIndex;
    int opponentColorIndex;
    ulong friendlyPieceBitboard;
    ulong opponentPieceBitboard;
    ulong emptySquareBitboards;
    ulong allPieceBitboards;

    Board board = new Board();

    // ref means I can change the value of the argument instead or reading from it
    public List<Move> GenerateMoves(Board board)
    {
        moves = new List<Move>();
        Init();

        for (int squareIndex = 0; squareIndex < 64; squareIndex++)
        {
            int piece = board.Square[squareIndex];
            if (piece == 0) continue;
            if (Piece.PieceColor(piece) == board.moveColor)
            {
                if (Piece.IsSlidingPiece(piece))
                {
                    GenerateSlidingMoves(squareIndex, piece);
                }
            }
        }

        return moves;
    }

    void GenerateSlidingMoves(int startSquare, int piece)
    {
        for (int directionIndex = 0; directionIndex < 8; directionIndex++)
        {

        }
    }

    void Init()
    {
        moves = new List<Move>();

        isWhiteToMove = board.moveColor == Piece.White;
        friendlyColor = board.moveColor;
        opponentColor = board.opponentColor;
        friendlyColorIndex = board.moveColorIndex;
        opponentColorIndex = board.opponentColorIndex;

        opponentPieceBitboard = board.colorBitboards[opponentColorIndex];
        friendlyPieceBitboard = board.colorBitboards[friendlyColorIndex];
        allPieceBitboards = board.allPiecesBitboard;

        emptySquareBitboards = ~allPieceBitboards;
    }
}