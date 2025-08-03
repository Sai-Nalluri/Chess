using System;
using Avalonia.Controls;
using Avalonia.Media;
using Chess.Core;

namespace Chess.UI;

public partial class BoardUI : Window
{
    public SolidColorBrush lightColor = new SolidColorBrush(Color.FromArgb(255, 241, 217, 192));
    public SolidColorBrush darkColor = new SolidColorBrush(Color.FromArgb(255, 169, 122, 101));

    public PieceTheme pieceTheme;
    public BoardTheme boardTheme;
    public bool showLegalMoves;
    public bool whiteIsBottom = true;

    Move lastMoveMade;
    MoveGenerator moveGenerator;

    public BoardUI()
    {
        InitializeComponent();

        moveGenerator = new MoveGenerator();
        CreateBoardUI();
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
                Canvas.SetTop(square, file * 80);
                Canvas.SetLeft(square, rank * 80);
                GameCanvas.Children.Add(square);
            }
        }
    }
}