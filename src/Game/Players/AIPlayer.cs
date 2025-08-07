using Chess.Core;

namespace Chess.Players;

public class AIPlayer : Player
{
    bool moveFound;
    Move move;
    public Board board;

    public AIPlayer(Board board)
    {
        this.board = board;
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    public override void NotifyTurnToMove()
    {
        throw new System.NotImplementedException();
    }
}