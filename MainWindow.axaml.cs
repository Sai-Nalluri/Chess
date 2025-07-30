using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Chess.Core;

namespace ChessApp;

public partial class ChessWindow : Window
{
    // Square colors
    private SolidColorBrush lightColor = new SolidColorBrush(Avalonia.Media.Color.Parse("#F1D9C0"));
    private SolidColorBrush darkColor = new SolidColorBrush(Avalonia.Media.Color.Parse("#A97A65"));
    private SolidColorBrush selectionOverlayColor = new SolidColorBrush(Avalonia.Media.Color.FromArgb(180, 255, 255, 0));
    private SolidColorBrush legalMoveOverlayColor = new SolidColorBrush(Avalonia.Media.Color.FromArgb(180, 255, 0, 0));

    private int selectedRank = -1;
    private int selectedFile = -1;
    private int selectedSquareIndex = -1;
    private int selectedPiece = -1;

    private List<Move> legalMoves = new List<Move>(64);

    private List<int> legalRanks = new List<int>();
    private List<int> legalFiles = new List<int>();

    // Dictionary to refer to when loading in piece images
    private Dictionary<int, Bitmap> pieceImages = new Dictionary<int, Bitmap>();
    // Dictionary to refer to when trying to change overlays
    private Dictionary<(int rank, int file), Rectangle> overlays = new();

    // Instances
    MoveGenerator moveGenerator = new MoveGenerator();
    Board board = new Board();

    public ChessWindow()
    {
        InitializeComponent();
        LoadPieceImages();
        board.LoadStartPosition();
        // Test position
        // board.LoadPosition("rqb5/7p/8/8/8/8/P7/5BQR w - - 0 1");

        Window window = new Window();
        window.WindowState = WindowState.Maximized;

        DrawChessBoard();
        DrawUIElements();
        GenerateLegalMoves();
    }

    private void DrawChessBoard()
    {
        GameCanvas.Children.Clear();
        overlays.Clear();

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                // --DRAW SQUARE--
                bool isWhite = (rank + file) % 2 == 0 ? true : false;
                SolidColorBrush squareColor = isWhite ? lightColor : darkColor;

                Rectangle rectangle = new Rectangle
                {
                    Width = 80,
                    Height = 80,
                    Fill = squareColor
                };

                Canvas.SetLeft(rectangle, file * 80);
                Canvas.SetTop(rectangle, (7 - rank) * 80);

                GameCanvas.Children.Add(rectangle);

                // DEBUG
                TextBlock squareIndexText = new TextBlock
                {
                    Text = $"{rank * 8 + file}"
                };
                Canvas.SetLeft(squareIndexText, file * 80);
                Canvas.SetTop(squareIndexText, (7 - rank) * 80);
                GameCanvas.Children.Add(squareIndexText);
                // DEBUG

                // --GET OVERLAY READY--
                Rectangle overlay = new Rectangle
                {
                    Width = 80,
                    Height = 80,
                    Fill = Brushes.Transparent
                };
                Canvas.SetLeft(overlay, file * 80);
                Canvas.SetTop(overlay, (7 - rank) * 80);
                GameCanvas.Children.Add(overlay);

                overlays[(rank, file)] = overlay;

                // --DRAW PIECES--
                int squareIndex;
                if (board.playerColor == Piece.Black)
                {
                    squareIndex = (7 - rank) * 8 + file;
                }
                else
                {
                    squareIndex = rank * 8 + file;
                }
                int piece = board.Square[squareIndex];

                if (piece != Piece.None)
                {
                    Image pieceImage = new Image
                    {
                        Source = pieceImages[piece],
                        Width = 80,
                        Height = 80
                    };

                    Canvas.SetTop(pieceImage, (7 - rank) * 80);
                    Canvas.SetLeft(pieceImage, file * 80);

                    GameCanvas.Children.Add(pieceImage);
                }
            }
        }
    }

    private void DrawUIElements()
    {
        Button playAsWhite = new Button
        {
            Content = "Play White",
            Width = 175,
            Height = 50,
            FontSize = 24,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
        };
        Canvas.SetTop(playAsWhite, 445);
        Canvas.SetLeft(playAsWhite, -275);
        GameCanvas.Children.Add(playAsWhite);
        playAsWhite.Click += PlayAsWhite;

        Button playAsBlack = new Button
        {
            Content = "Play Black",
            Width = 175,
            Height = 50,
            FontSize = 24,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
        };
        Canvas.SetTop(playAsBlack, 500);
        Canvas.SetLeft(playAsBlack, -275);
        GameCanvas.Children.Add(playAsBlack);
        playAsBlack.Click += PlayAsBlack;

        Button quitButton = new Button
        {
            Content = "Quit",
            Width = 175,
            Height = 50,
            FontSize = 24,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
        };
        Canvas.SetTop(quitButton, 580);
        Canvas.SetLeft(quitButton, -275);
        GameCanvas.Children.Add(quitButton);
        quitButton.Click += Quit;
    }

    private void PlayAsWhite(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Trying to play as white");
    }

    private void PlayAsBlack(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Trying to play as black");
    }

    private void Quit(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private (int rank, int file) GetBoardPosition(double x, double y)
    {
        int rank = 7 - (int)(y / 80);
        int file = (int)x / 80;
        return (rank, file);
    }

    private bool IsLegal(Move move)
    {
        foreach (Move legalMove in legalMoves)
        {
            if (legalMove.startSquare == move.startSquare)
            {
                if (legalMove.targetSquare == move.targetSquare)
                {
                    return true;
                }
            }
            else
            {
                continue;
            }
        }
        return false;
    }

    private void GenerateLegalMoves()
    {
        legalMoves = moveGenerator.GenerateMoves(board);
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var position = e.GetPosition(sender as Control);
        var (rank, file) = GetBoardPosition(position.X, position.Y);

        int squareIndex = rank * 8 + file;
        int piece = board.Square[squareIndex];

        // Case 1: A piece is already selected
        if (selectedRank != -1 && selectedFile != -1)
        {
            // If the same square is clicked again, deselect immediately
            if (selectedRank == rank && selectedFile == file)
            {
                overlays[(selectedRank, selectedFile)].Fill = Brushes.Transparent;
                foreach (int legalRank in legalRanks)
                {
                    foreach (int legalFile in legalFiles)
                    {
                        overlays[(legalRank, legalFile)].Fill = Brushes.Transparent;
                    }
                }

                selectedRank = selectedFile = selectedSquareIndex = selectedPiece = -1;
                legalRanks.Clear();
                legalFiles.Clear();
                return;
            }

            int fromIndex = selectedSquareIndex;
            int toIndex = squareIndex;

            // Clear previous selection highlight
            overlays[(selectedRank, selectedFile)].Fill = Brushes.Transparent;
            foreach (int legalRank in legalRanks)
            {
                foreach (int legalFile in legalFiles)
                {
                    overlays[(legalRank, legalFile)].Fill = Brushes.Transparent;
                }
            }
            legalRanks.Clear();
            legalFiles.Clear();

            // If the clicked square contains a piece of the same color, change selection
            if (piece != Piece.None && Piece.PieceColor(piece) == board.moveColor)
            {
                selectedRank = rank;
                selectedFile = file;
                selectedSquareIndex = squareIndex;
                selectedPiece = piece;

                overlays[(rank, file)].Fill = selectionOverlayColor;

                // Highlight new legal moves
                foreach (Move move in legalMoves)
                {
                    if (move.startSquare == selectedSquareIndex)
                    {
                        int targetRank = move.targetSquare / 8;
                        int targetFile = move.targetSquare % 8;

                        legalRanks.Add(targetRank);
                        legalFiles.Add(targetFile);
                        overlays[(targetRank, targetFile)].Fill = legalMoveOverlayColor;
                    }
                }
            }
            // Otherwise, try to move the selected piece
            else
            {
                Move move = new Move(fromIndex, toIndex);
                if (IsLegal(move))
                {
                    board.MakeMove(move);
                    DrawChessBoard();
                    DrawUIElements();
                    GenerateLegalMoves();
                    legalRanks.Clear();
                    legalFiles.Clear();
                }

                selectedRank = selectedFile = selectedSquareIndex = selectedPiece = -1;
            }
        }
        // Case 2: No piece is currently selected
        else
        {
            if (piece != Piece.None && Piece.PieceColor(piece) == board.moveColor)
            {
                selectedRank = rank;
                selectedFile = file;
                selectedSquareIndex = squareIndex;
                selectedPiece = piece;

                overlays[(rank, file)].Fill = selectionOverlayColor;

                // Highlight legal moves for this piece
                foreach (Move move in legalMoves)
                {
                    if (move.startSquare == selectedSquareIndex)
                    {
                        int targetRank = move.targetSquare / 8;
                        int targetFile = move.targetSquare % 8;

                        legalRanks.Add(targetRank);
                        legalFiles.Add(targetFile);
                        overlays[(targetRank, targetFile)].Fill = legalMoveOverlayColor;
                    }
                }
            }
        }
    }

    private void LoadPieceImages()
    {
        string absolutePathReference = "C:/Users/mail2/OneDrive/Documents/My Stuff/Coding Projects/Chess Engine/ChessApp/resources/chess pieces/";

        pieceImages[Piece.WhitePawn] = new Bitmap(absolutePathReference + "white_pawn.png");
        pieceImages[Piece.WhiteKnight] = new Bitmap(absolutePathReference + "white_knight.png");
        pieceImages[Piece.WhiteBishop] = new Bitmap(absolutePathReference + "white_bishop.png");
        pieceImages[Piece.WhiteRook] = new Bitmap(absolutePathReference + "white_rook.png");
        pieceImages[Piece.WhiteQueen] = new Bitmap(absolutePathReference + "white_queen.png");
        pieceImages[Piece.WhiteKing] = new Bitmap(absolutePathReference + "white_king.png");

        pieceImages[Piece.BlackPawn] = new Bitmap(absolutePathReference + "black_pawn.png");
        pieceImages[Piece.BlackKnight] = new Bitmap(absolutePathReference + "black_knight.png");
        pieceImages[Piece.BlackBishop] = new Bitmap(absolutePathReference + "black_bishop.png");
        pieceImages[Piece.BlackRook] = new Bitmap(absolutePathReference + "black_rook.png");
        pieceImages[Piece.BlackQueen] = new Bitmap(absolutePathReference + "black_queen.png");
        pieceImages[Piece.BlackKing] = new Bitmap(absolutePathReference + "black_king.png");
    }
}