using System;
using Chess.Core;

namespace Chess.Players;

public abstract class Player
{
    public event Action<Move> onMoveChosen;

    public abstract void Update();

    public abstract void NotifyTurnToMove();

    public virtual void ChoseMove(Move move)
    {
        onMoveChosen?.Invoke(move);
    }
}