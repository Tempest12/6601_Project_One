using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restart
{
    public class Move
    {
        public int row;
        public int col;
        public int distance;

        /*public Move()
        {
            this.row = 0;
            this.col = 0;
            this.distance = 0;
        }*/

        public Move(int row, int col, int distance)
        {
            this.row = row;
            this.col = col;
            this.distance = distance;
        }

        public Move(Move that)
        {
            this.row = that.row;
            this.col = that.col;
            this.distance = that.distance;
        }

        public static void fakeMove(byte[, ,] board, Player movingPlayer, Move move)
        {
            if (movingPlayer.currentPosition == null)
            {
                board[move.row, move.col, move.distance] = (byte)movingPlayer.playerNumber;
                movingPlayer.currentPosition = move;
            }
            else
            {
                board[move.row, move.col, move.distance] = (byte)movingPlayer.playerNumber;
                addShadows(board, movingPlayer.currentPosition, move);
                movingPlayer.currentPosition = move;
            }
        }

        public static List<Move> getPossibleMoves(byte[, ,] board, Move currentPosition)
        {
            List<Move> possibleMoves = new List<Move>();

            //if we just started and are placing our piece we can go anywhere that isn't already taken
            if (currentPosition == null)
            {
                for (int z = 0; z < Cube.cubeDimension; z++)
                {
                    for (int y = 0; y < Cube.cubeDimension; y++)
                    {
                        for (int x = 0; x < Cube.cubeDimension; x++)
                        {
                            if (board[x, y, z] == Cube.EMPTY)
                            {
                                possibleMoves.Add(new Move(x, y, z));
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }

                return possibleMoves;
            }

            //Log.writeDebug("The move sent to get possible moves is: " + currentPosition.ToString()); 

            if (Config.convertSettingToBool("game", "rock_move"))
            {
                //Left
                for (int left = (currentPosition.col + 1); left < Cube.cubeDimension; left++)
                {
                    if (board[currentPosition.row, left, currentPosition.distance] == Cube.EMPTY)
                    {
                        possibleMoves.Add(new Move(currentPosition.row, left, currentPosition.distance));
                    }
                    else
                    {
                        break;
                    }
                }

                //Right
                for (int right = (currentPosition.col - 1); right >= 0; right--)
                {
                    if (board[currentPosition.row, right, currentPosition.distance] == Cube.EMPTY)
                    {
                        possibleMoves.Add(new Move(currentPosition.row, right, currentPosition.distance));
                    }
                    else
                    {
                        break;
                    }
                }


                //Front
                for (int front = (currentPosition.distance - 1); front >= 0; front--)
                {
                    if (board[currentPosition.row, currentPosition.col, front] == Cube.EMPTY)
                    {
                        possibleMoves.Add(new Move(currentPosition.row, currentPosition.col, front));
                    }
                    else
                    {
                        break;
                    }
                }

                //Back
                for (int back = (currentPosition.distance + 1); back < Cube.cubeDimension; back++)
                {
                    if (board[currentPosition.row, currentPosition.col, back] == Cube.EMPTY)
                    {
                        possibleMoves.Add(new Move(currentPosition.row, currentPosition.col, back));
                    }
                    else
                    {
                        break;
                    }
                }


                //Up
                for (int up = (currentPosition.row + 1); up < Cube.cubeDimension; up++)
                {
                    if (board[up, currentPosition.col, currentPosition.distance] == Cube.EMPTY)
                    {
                        possibleMoves.Add(new Move(up, currentPosition.col, currentPosition.distance));
                    }
                    else
                    {
                        break;
                    }
                }

                //Down
                for(int down = (currentPosition.row - 1); down >= 0 ; down--)
                {
                    if (board[down, currentPosition.col, currentPosition.distance] == Cube.EMPTY)
                    {
                        possibleMoves.Add(new Move(down, currentPosition.col, currentPosition.distance));
                    }
                    else
                    {
                        break;
                    }
                }

                /*if (possibleMoves.Count < 25)
                {
                    Log.writeDebug("The possible moves are:");

                    foreach (Move move in possibleMoves)
                    {
                        Log.writeDebug("    " + move.ToString());
                    }
                }*/
            }
            if(Config.convertSettingToBool("game", "horizontal_queen_move"))
            {

            }
            if(Config.convertSettingToBool("game", "vertical_queen_move"))
            {

            }

            return possibleMoves;
        }

        public static void addShadows(byte[, ,] board, Move start, Move end)
        {
            if (start == null)
            {
                return;
            }

            if (Config.convertSettingToBool("game", "shadow_line"))
            {

            }
            else
            {
                board[start.row, start.col, start.distance] = (byte)Cube.SHADOW;
            }
        }

        public override bool  Equals(object obj)
        {
            if (obj is Move)
            {
                Move that = (Move)obj;

                if (this.row == that.row && this.col == that.col && this.distance == that.distance)
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "Move row: " + row + " col: " + col + " distance: " + distance;
        }
    }
}
