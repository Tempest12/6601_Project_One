using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restart
{
    public delegate float evalFunc(State state);

    public class State
    {
        public float value;
        public State highestParent;

        public byte[, ,] gameBoard;

        public bool ourMove;
        public Move generatorMove;

        public Player AIPlayer;
        public Player opponent;

        private static evalFunc playerOneEvalFunc;
        private static evalFunc playerTwoEvalFunc;

        public static void setEvalFunc()
        {
            string nameOne = Config.getValue("ai", "alphabeta_eval_one");
            string nameTwo = Config.getValue("ai", "alphabeta_eval_two");

            //Set One
            if(nameOne.Equals("stayAlive"))
            {
                playerOneEvalFunc += stayAlive;
            }
            else if (nameOne.Equals("ratio"))
            {
                playerOneEvalFunc += ratio;
            }
            else
            {
                MainMethod.die("State.setEvalFunc : Function named: \"" + nameOne + "\" not recognized for player One.");
            }

            //Set Two
           if(nameTwo.Equals("stayAlive"))
            {
                playerTwoEvalFunc += stayAlive;
            }
           else if (nameTwo.Equals("ratio"))
           {
               playerTwoEvalFunc += ratio;
           }
            else
            {
                MainMethod.die("State.setEvalFunc : Function named: \"" + nameOne + "\" not recognized for player One.");
            }
            
        }


        //For now to start the "tree" pass in the player's last move and our as false. Then it will generate the first level of all possible AI moves
        public State(Player ai, Player opponent, byte[, ,] board, Move move, bool ourMove)
        {
            this.AIPlayer = new Player(ai);
            this.opponent = new Player(opponent);

            this.gameBoard = new byte[Cube.cubeDimension, Cube.cubeDimension, Cube.cubeDimension];
            for (int x = 0; x < Cube.cubeDimension; x++)
            {
                for (int y = 0; y < Cube.cubeDimension; y++)
                {
                    for (int z = 0; z < Cube.cubeDimension; z++)
                    {
                        this.gameBoard[x, y, z] = board[x, y, z];
                    }
                }
            }

            this.ourMove = ourMove;
            this.generatorMove = move;

            if(ourMove)
            {
                if (move != null)
                {
                    Move.fakeMove(this.gameBoard, this.opponent, move);
                } 
                this.AIPlayer.getNewMoves(this.gameBoard);
                this.opponent.getNewMoves(this.gameBoard);
            }
            else
            {
                if (move != null)
                {
                    Move.fakeMove(this.gameBoard, this.AIPlayer, move);
                } 
                this.opponent.getNewMoves(this.gameBoard);
                this.AIPlayer.getNewMoves(this.gameBoard);
            }

            switch (AIPlayer.playerNumber)
            {
                case 1:
                    this.value = playerOneEvalFunc(this);
                    break;

                case 2:
                    this.value = playerTwoEvalFunc(this);
                    break;
            }
        }

        public bool isMax()
        {
            return ourMove;
        }

        public static float juan(State state)
        {
            return 7;
        }

        public static float ratio(State state)
        {
            if (state.opponent.possibleMoves.Count == 0)
            {
                return float.MaxValue;
            }

            return (float)state.AIPlayer.possibleMoves.Count / (float)state.opponent.possibleMoves.Count;
        }

        public static float stayAlive(State state)
        {
            if (state.ourMove)
            {
                if (state.isTerminal())
                {
                    return float.MinValue;
                }
                else
                {
                    return state.AIPlayer.possibleMoves.Count;
                }
            }
            else
            {
                if (state.isTerminal())
                {
                    return float.MaxValue;
                }
                else
                {
                    return state.AIPlayer.possibleMoves.Count;
                }
            }
        } 

        public List<State> generateChildren()
        {
            List<State> children = new List<State>();

            if(ourMove)
            {
                foreach (Move move in AIPlayer.possibleMoves)
                {
                    children.Add(new State(AIPlayer, opponent, gameBoard, move, !ourMove));
                }
            }
            else
            {
                foreach (Move move in opponent.possibleMoves)
                {
                    children.Add(new State(AIPlayer, opponent, gameBoard, move, !ourMove));
                }
            }

            return children;
        }

        public bool isTerminal()
        {           
            if (ourMove)
            {
                return AIPlayer.possibleMoves.Count == 0;
            }
            else
            {
                return opponent.possibleMoves.Count == 0;
            }
        }
    }
}
