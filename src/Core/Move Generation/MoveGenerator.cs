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
        this.board = board; // Ensure board field is set
        moves = new List<Move>();
        Init();

        for (int squareIndex = 0; squareIndex < 64; squareIndex++)
        {
            int piece = board.Square[squareIndex];
            if (piece == Piece.None) continue;
            if (Piece.PieceColor(piece) == board.moveColor)
            {
                if (Piece.IsSlidingPiece(piece))
                {
                    GenerateSlidingMoves(squareIndex, moves);
                }
            }
        }

        return moves;
    }

    void GenerateSlidingMoves(int startSquare, List<Move> moves)
    {
        int startDirectionIndex = Piece.PieceType(board.Square[startSquare]) == Piece.Bishop ? 4 : 0;
        int endDirectionIndex = Piece.PieceType(board.Square[startSquare]) == Piece.Rook ? 4 : 8;

        for (int directionIndex = startDirectionIndex; directionIndex < endDirectionIndex; directionIndex++)
        {
            for (int n = 0; n < PrecomputedMoveData.numSquaresToEdge[startSquare][directionIndex]; n++)
            {
                int targetSquare = startSquare + PrecomputedMoveData.directionOffsets[directionIndex] * (n + 1);
                int targetedPiece = board.Square[targetSquare];

                if (targetedPiece == Piece.None)
                {
                    moves.Add(new Move(startSquare, targetSquare));
                    continue;
                }

                // If the piece on the square is the same color, go to the next direction
                if (Piece.PieceColor(targetedPiece) == board.moveColor)
                {
                    break;
                }

                moves.Add(new Move(startSquare, targetSquare));

                // If landing on a opponent piece, you cant go further
                if (Piece.PieceColor(targetedPiece) != board.moveColor)
                {
                    break;
                }
            }
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