using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restart
{
    public class State
    {
        public int value;
        public State highestParent;

        public byte[, ,] gameBoard;

        public bool ourMove;
        public Move generatorMove;

        public Player AIPlayer;
        public Player opponent;

        //public static delegate evaluateOne;
        //public static delegate evaluateTwo;

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
                Move.fakeMove(this.gameBoard, this.opponent, move);
                this.AIPlayer.getNewMoves(gameBoard);
                this.opponent.getNewMoves(gameBoard);
            }
            else
            {
                Move.fakeMove(this.gameBoard, this.AIPlayer, move);
                this.opponent.getNewMoves(gameBoard);
                this.AIPlayer.getNewMoves(gameBoard);
            }

            value = this.evaluate();

        }

        public bool isMax()
        {
            return ourMove;
        }

        public int evaluate()
        {
            if (ourMove)
            {
                if (isTerminal())
                {
                    return int.MinValue;
                }
                else
                {
                    return AIPlayer.possibleMoves.Count;
                }
            }
            else
            {
                if (isTerminal())
                {
                    return int.MaxValue;
                }
                else
                {
                    return AIPlayer.possibleMoves.Count;
                }
            }

            //return AIPlayer.possibleMoves.Count;
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
