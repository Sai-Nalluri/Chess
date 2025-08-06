using System;
using Chess.Core;
using Chess.Players;
using Chess.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Chess.GameCore;

public class GameManager
{
    public event Action onPositionLoaded;
    public event Action<Move> onMoveMade;

    public enum PlayerType { Human, AI };

    public bool loadCustomPosition;
    public string customPosition = "1rbq1r1k/2pp2pp/p1n3p1/2b1p3/R3P3/1BP2N2/1P3PPP/1NBQ1RK1 w - - 0 1";

    public PlayerType whitePlayerType;
    public PlayerType blackPlayerType;

    public bool useClocks;

    Player whitePlayer;
    Player blackPlayer;
    Player playerToMove;
    BoardUI boardUI;

    public Board board { get; private set; }
    Board searchBoard;

    public void Start()
    {
        boardUI = new BoardUI();
        board = new Board();
        searchBoard = new Board();

        NewGame(whitePlayerType, blackPlayerType);
    }

    public void Update(GameTime gameTime)
    {
        HandleInput();
        UpdateGame();
    }

    void UpdateGame()
    {

    }

    void HandleInput()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.U))
        {
            Console.WriteLine("Undo a move");
        }
    }

    void NewGame(PlayerType whitePlayerType, PlayerType blackPlayerType)
    {
        if (loadCustomPosition)
        {
            board.LoadPosition(customPosition);
            searchBoard.LoadPosition(customPosition);
        }
        else
        {
            board.LoadStartPosition();
            searchBoard.LoadStartPosition();
        }
        onPositionLoaded?.Invoke();
        boardUI.UpdatePosition(board);
    }
}