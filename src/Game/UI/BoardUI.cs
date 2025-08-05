using Chess.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.UI;

public class BoardUI
{
    public Color lightColor = new Color(241, 217, 192);
    public Color darkColor = new Color(169, 122, 101);

    public PieceTheme pieceTheme;
    public BoardTheme boardTheme = new BoardTheme();
    public bool showLegalMoves;
    public bool whiteIsBottom = true;

    Rectangle[,] graphicalSquares = new Rectangle[8, 8];
    Color[,] graphicalSquareColors = new Color[8, 8];
    // Move lastMoveMade;
    MoveGenerator moveGenerator;

    public BoardUI()
    {
        moveGenerator = new MoveGenerator();
        SetBoardThemes();
        InitializeSquareColors();
        InitializeBoard();
    }

    void SetBoardThemes()
    {
        boardTheme.lightSquares.normal = new Color(238, 216, 192);
        boardTheme.lightSquares.legal = new Color(221, 89, 89);
        boardTheme.lightSquares.selected = new Color(236, 197, 123);
        boardTheme.lightSquares.moveFromHighlight = new Color(221, 208, 124);
        boardTheme.lightSquares.moveToHighlight = new Color(221, 208, 124);

        boardTheme.darkSquares.normal = new Color(171, 122, 101);
        boardTheme.darkSquares.legal = new Color(197, 68, 79);
        boardTheme.darkSquares.selected = new Color(200, 158, 80);
        boardTheme.darkSquares.moveFromHighlight = new Color(197, 158, 94);
        boardTheme.darkSquares.moveToHighlight = new Color(197, 158, 94);
    }

    void InitializeSquareColors()
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                bool isLight = (rank + file) % 2 == 0;
                graphicalSquareColors[rank, file] = isLight ? boardTheme.lightSquares.normal : boardTheme.darkSquares.normal;
            }
        }
    }

    void InitializeBoard()
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                Rectangle square = new Rectangle
                {
                    X = file * 80,
                    Y = rank * 80,
                    Width = 80,
                    Height = 80
                };
                graphicalSquares[rank, file] = square;
            }
        }
        ResetSquareColors();
    }

    void ResetSquareColors()
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                SetSquareColors(new Coord(rank, file));
            }
        }
    }

    void SetSquareColors(Coord square)
    {
        graphicalSquareColors[square.rankIndex, square.fileIndex] = square.IsLightSquare() ? boardTheme.lightSquares.normal : boardTheme.darkSquares.normal;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                Rectangle square = graphicalSquares[rank, file];
                Color color = graphicalSquareColors[rank, file];

                UIHelper.DrawSquare(spriteBatch, square, color);
            }
        }
    }
}