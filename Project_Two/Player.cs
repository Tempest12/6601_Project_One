using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restart
{
    public class Player
    {
        public PlayerType type;
        public List<Move> possibleMoves;
        public Move currentPosition;
        public int playerNumber;

        public Player(int playerNumber, PlayerType type)
        {
            this.playerNumber = playerNumber;

            this.currentPosition = null;
            this.possibleMoves = Move.getPossibleMoves(Core.drawCube.cube, currentPosition);

            this.type = type;
        }

        public Player(Player that)
        {
            this.type = that.type;
            this.playerNumber = that.playerNumber;

            if (that.currentPosition == null)
            {
                this.currentPosition = null;
            }
            else
            {
                this.currentPosition = new Move(that.currentPosition);
            }
        }

        public void getNewMoves(byte[,,] board)
        {
            if (possibleMoves == null)
            {
                possibleMoves = new List<Move>();
            }
            else
            {
                possibleMoves.Clear();
            }

            possibleMoves = Move.getPossibleMoves(board, currentPosition);
        }

        public void makeMove()
        {
            Move blah = AlphaBeta.maxNextMove(RenderWindow.lastState, int.MaxValue);
            Core.drawCube.cube[blah.row, blah.col, blah.distance] = (byte)playerNumber;
            Move.addShadows(Core.drawCube.cube, currentPosition, blah);
            currentPosition = blah;
        }

        public bool tryMove(Move move)
        {
            if (possibleMoves.Contains(move))
            {
                //Fill in all of the moves between where we are and where we are going.
                Core.drawCube.cube[move.row, move.col, move.distance] = (byte)playerNumber;
                if (currentPosition != null)
                {
                    //Core.drawCube.cube[currentPosition.row, currentPosition.col, currentPosition.distance] = (byte)Cube.SHADOW;
                    Move.addShadows(Core.drawCube.cube, currentPosition, move);
                }
                Core.moveCounter++;

                currentPosition = move;

                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Player)
            {
                Player that = (Player)obj;
                if (this.playerNumber == that.playerNumber)
                {
                    return true;
                }
            }

            return false;
        }

        //Just here to scare away the warnings
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum PlayerType
    {
        HUMAN = 0,
        AI,
        MAX,
        MIN
    }
}
