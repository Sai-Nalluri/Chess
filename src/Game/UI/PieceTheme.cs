using Avalonia.Controls;
using Chess.Core;

namespace Chess.UI;

public class PieceTheme
{
    public PieceSprites whitePieces;
    public PieceSprites blackPieces;

    public Image GetPieceImage(int piece)
    {
        PieceSprites pieceSprites = Piece.IsColor(piece, Piece.White) ? whitePieces : blackPieces;

        switch (Piece.PieceType(piece))
        {
            case Piece.Pawn:
                return pieceSprites.pawn;
            case Piece.Knight:
                return pieceSprites.knight;
            case Piece.Bishop:
                return pieceSprites.bishop;
            case Piece.Queen:
                return pieceSprites.queen;
            case Piece.King:
                return pieceSprites.king;
            default:
                return null;
        }
    }

    public class PieceSprites
    {
        public Image pawn, knight, bishop, queen, king;

        public Image this[int i]
        {
            get
            {
                return new Image[] { pawn, knight, bishop, queen, king }[i];
            }
        }
    }
}