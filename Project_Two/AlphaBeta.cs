﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restart
{
    class AlphaBeta
    {
        /* Best action found thus far for Max */
        public static Game.Node best_move;

        /*
         * Entry point to alphabeta
         */
        public static Move maxNextMove(State currState, int height)
        {
            /* Create initial node */
            Game.Node max_init = new Game.Node(currState.AIPlayer,
                                                currState,
                                                int.MinValue,
                                                int.MaxValue);
            // find next move
            alphabeta(max_init, height);

            return best_move.state.generatorMove;
        }

        /*
         * alphabeta - Recursive algorithm that searches for the 
         *             best possible move taking into account the 
         *             opponent's move
         * @param:
         * @return: alpha or beta value
         */
        public static int alphabeta(Game.Node curr_node, int height)
        {
            /* base case - check if we have reached desired depth or if
             * current node.state is a terminal state */
            if ((height == 0) || (curr_node.state.isTerminal()))
            {
                return curr_node.state.value;
            }

            if (curr_node.state.isMax())
            {
                Game.Node min_node = null;
                foreach (State child_state in curr_node.state.generateChildren())
                {
                    /* construct node containing child's information
                     * Node: This new node will be played by Min!
                     */
                    min_node = new Game.Node( curr_node.state.opponent,
                                                        child_state,
                                                        curr_node.alpha,
                                                        curr_node.beta);
                    /* get child's alpha value */
                    int child_alpha = alphabeta(min_node, height - 1);

                    /* set new alpha value for this node */
                    if (curr_node.alpha < child_alpha)
                    {
                        curr_node.alpha = child_alpha;
                        best_move = min_node;
                    }
                    //curr_node.alpha = Math.Max(curr_node.alpha,
                    //                          alphabeta(min_node, height - 1));

                    /* beta cut-off */
                    if (curr_node.beta <= curr_node.alpha)
                        break;
                }
                return curr_node.alpha;
            }
            else
            {
                Game.Node max_node = null;
                foreach (State child_state in curr_node.state.generateChildren())
                {
                    /* construct node containing child's information
                     * Node: This new node will be played by Max!
                     */
                    max_node = new Game.Node(curr_node.state.AIPlayer,
                                                        child_state,
                                                        curr_node.alpha,
                                                        curr_node.beta);

                    /* set new alpha value for this node */
                    curr_node.beta = Math.Min(curr_node.beta,
                                              alphabeta(max_node, height - 1));

                    /* alpha cut-off */
                    if (curr_node.beta <= curr_node.alpha)
                        break;
                }
                return curr_node.beta;
            }
        }

    }
}