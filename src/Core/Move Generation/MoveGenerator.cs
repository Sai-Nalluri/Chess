using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Tmds.DBus.Protocol;

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

    public List<Move> GenerateMoves(Board board)
    {
        moves = new List<Move>();
        GenerateMoves(board, ref moves);
        return moves;
    }

    // ref means I can change the value of the argument instead or reading from it
    public void GenerateMoves(Board board, ref List<Move> moves)
    {
        this.board = board;
        Init();

        GeneratePawnMoves(moves);
        moves.Add(new Move(8, 16));
        moves.Add(new Move(48, 40));
    }

    private void GeneratePawnMoves(List<Move> moves)
    {

    }

    void Init()
    {
        moves = new List<Move>();

        // currentMoveIndex = 0;

        isWhiteToMove = board.moveColor == Piece.White;
        friendlyColor = board.moveColor;
        opponentColor = board.opponentColor;
        friendlyColorIndex = board.moveColorIndex;
        opponentColorIndex = 1 - friendlyColorIndex;

        opponentPieceBitboard = board.colorBitboards[opponentColorIndex];
        friendlyPieceBitboard = board.colorBitboards[friendlyColorIndex];
        allPieceBitboards = board.allPiecesBitboard;
        emptySquareBitboards = ~allPieceBitboards;
    }
}