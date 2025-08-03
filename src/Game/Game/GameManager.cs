using System;
using Chess.Core;
using Chess.UI;

namespace Chess.Game;

public class GameManager
{
    public event System.Action onPositionLoaded;
    public event System.Action<Move> onMoveMade;

    public enum PlayerType { Human, AI };

    public bool loadCustomPosition;
    public string customPosition = "1rbq1r1k/2pp2pp/p1n3p1/2b1p3/R3P3/1BP2N2/1P3PPP/1NBQ1RK1 w - - 0 1";

    BoardUI boardUI;

    // This board can be read from, but only changed in this class
    public Board board { get; private set; }
    public Board searchBoard; // board for ai search

    public void Start()
    {
        boardUI = new BoardUI();
        board = new Board();
        searchBoard = new Board();

        NewGame();
    }

    void NewGame()
    {
        throw new NotImplementedException();
    }
}