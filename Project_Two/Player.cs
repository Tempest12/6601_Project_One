using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restart
{

    delegate Move aiMove(State currentState);

    public class Player
    {
        public PlayerType type;
        public List<Move> possibleMoves;
        public Move currentPosition;
        public int playerNumber;

        private aiMove aiFunc;

        public Player(int playerNumber, PlayerType type)
        {
            this.playerNumber = playerNumber;

            this.currentPosition = null;
            this.possibleMoves = Move.getPossibleMoves(Core.drawCube.cube, currentPosition);

            this.type = type;

            if (type == PlayerType.AI)
            {
                switch (playerNumber)
                {
                    case 1:

                        setAIFunc(Config.getValue("ai", "player_one"));
                        break;

                    case 2:

                        setAIFunc(Config.getValue("ai", "player_two"));
                        break;
                }   
            }
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

        private Move pickRandomMove(State currentState)
        {
            if (possibleMoves.Count == 0)
            {
                MainMethod.die(" Error : Player.pickRandomMove : the number of possible moves was zero. The game should have ended before this.");               
                return null;
            }

            return possibleMoves[Core.numberGenerator.Next(possibleMoves.Count)];
        }

        private Move alphaBeta(State currentState)
        {
            if (currentPosition == null)
            {
                return pickRandomMove(currentState);
            }
            else
            {
                return AlphaBeta.maxNextMove(currentState, 7);
            }
        }

        public void makeMove(State currentState)
        {
            if (type != PlayerType.AI)
            {
                MainMethod.die("Player.makeMove : makeMove called on a human player.");
            }

            Move move = aiFunc(currentState);

            if (move == null)
            {
                return;
            }

            Log.writeSpecial("Move sent back by AI is: " + move.ToString());

            Core.drawCube.cube[move.row, move.col, move.distance] = (byte)playerNumber;
            Move.addShadows(Core.drawCube.cube, currentPosition, move);
            currentPosition = move;
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

                Log.writeSpecial("Player has moved to: " + move.ToString());

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

        //Sets the AI function to use when making moves.
        public void setAIFunc(string funcName)
        {
            if (funcName == "random")
            {
                aiFunc = pickRandomMove;
            }
            else if (funcName == "alpha_beta")
            {
                aiFunc = alphaBeta;
            }
            else
            {
                MainMethod.die("Error : Player.setAIFunc : Function name: " + funcName + " is not supported");
            }
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
