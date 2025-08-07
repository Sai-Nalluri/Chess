using System;
using Chess.Core;
using Chess.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Chess.Players;

public class HumanPlayer : Player
{
    public enum InputState
    {
        None,
        PieceSelected,
        DraggingPiece
    }
    InputState currentState;

    BoardUI boardUI;
    Coord selectedPieceSquare;
    Board board;

    public HumanPlayer(Board board)
    {
        boardUI = new BoardUI();
        this.board = board;
    }

    public override void NotifyTurnToMove()
    {

    }

    public override void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        MouseState mouseState = new MouseState();

        if (currentState == InputState.None)
        {
            HandlePieceSelection(mouseState);
        }
        else if (currentState == InputState.PieceSelected)
        {
            HandlePointAndClickMovement(mouseState);
        }
        else if (currentState == InputState.DraggingPiece)
        {
            HandleDragMovement(mouseState);
        }

        if (mouseState.RightButton == ButtonState.Pressed)
        {
            CancelPieceSelection();
        }
    }

    void HandlePointAndClickMovement(MouseState mouseState)
    {
        if (mouseState.RightButton == ButtonState.Pressed)
        {
            HandlePiecePlacement(mouseState);
        }
    }

    void HandleDragMovement(MouseState mouseState)
    {
        throw new NotImplementedException();
    }

    void HandlePiecePlacement(MouseState mouseState)
    {
        throw new NotImplementedException();
    }

    void CancelPieceSelection()
    {
        throw new NotImplementedException();
    }

    void TryMakeMove(Coord startSquare, Coord targetSquare)
    {
        throw new NotImplementedException();
    }

    void HandlePieceSelection(MouseState mouseState)
    {
        throw new NotImplementedException();
    }
}