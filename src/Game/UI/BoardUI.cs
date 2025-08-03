using System;
using Avalonia.Controls;
using Avalonia.Media;
using Chess.Core;
using Chess.Game;

namespace Chess.UI;

public partial class BoardUI : Window
{
    public SolidColorBrush lightColor = new SolidColorBrush(Color.FromRgb(241, 217, 192));
    public SolidColorBrush darkColor = new SolidColorBrush(Color.FromRgb(169, 122, 101));

    public PieceTheme pieceTheme;
    public BoardTheme boardTheme = new BoardTheme();
    public bool showLegalMoves;
    public bool whiteIsBottom = true;

    Border[,] squareBorders = new Border[8, 8];
    Move lastMoveMade;
    MoveGenerator moveGenerator;
    GameManager gameManager;

    public BoardUI()
    {
        InitializeComponent();
        gameManager = new GameManager();
        gameManager.Start();

        moveGenerator = new MoveGenerator();
        SetBoardThemes();
        CreateBoardUI();
    }

    void SetBoardThemes()
    {
        boardTheme.lightSquares.normal = new SolidColorBrush(Color.FromRgb(238, 216, 192));
        boardTheme.lightSquares.legal = new SolidColorBrush(Color.FromRgb(221, 89, 89));
        boardTheme.lightSquares.selected = new SolidColorBrush(Color.FromRgb(236, 197, 123));
        boardTheme.lightSquares.moveFromHighlight = new SolidColorBrush(Color.FromRgb(221, 208, 124));
        boardTheme.lightSquares.moveToHighlight = new SolidColorBrush(Color.FromRgb(221, 208, 124));

        boardTheme.darkSquares.normal = new SolidColorBrush(Color.FromRgb(171, 122, 101));
        boardTheme.darkSquares.legal = new SolidColorBrush(Color.FromRgb(197, 68, 79));
        boardTheme.darkSquares.selected = new SolidColorBrush(Color.FromRgb(200, 158, 80));
        boardTheme.darkSquares.moveFromHighlight = new SolidColorBrush(Color.FromRgb(197, 158, 94));
        boardTheme.darkSquares.moveToHighlight = new SolidColorBrush(Color.FromRgb(197, 158, 94));
    }

    void CreateBoardUI()
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                Border square = new Border
                {
                    Width = 80,
                    Height = 80
                };
                squareBorders[rank, file] = square;
                Canvas.SetTop(square, file * 80);
                Canvas.SetLeft(square, rank * 80);
                GameCanvas.Children.Add(square);
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
                SetSquareColors(new Coord(rank, file), boardTheme.lightSquares.normal, boardTheme.darkSquares.normal);
            }
        }
    }

    void SetSquareColors(Coord square, SolidColorBrush lightColor, SolidColorBrush darkColor)
    {
        squareBorders[square.rankIndex, square.fileIndex].Background = square.IsLightSquare() ? lightColor : darkColor;
    }
}