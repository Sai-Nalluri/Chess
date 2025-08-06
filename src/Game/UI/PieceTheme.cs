using Chess.Core;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.UI;

public class PieceTheme
{
    public PieceSprites whitePieces;
    public PieceSprites blackPieces;

    public Texture2D GetPieceImage(int piece)
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
            case Piece.Rook:
                return pieceSprites.rook;
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
        public Texture2D pawn, knight, bishop, rook, queen, king;

        public Texture2D this[int i]
        {
            get
            {
                return new Texture2D[] { pawn, knight, bishop, rook, queen, king }[i];
            }
        }
    }
}