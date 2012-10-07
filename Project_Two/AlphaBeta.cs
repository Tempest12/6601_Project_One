using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Restart
{
    class AlphaBeta
    {
        /* Best action found thus far for Max */
        public static Game.Node best_move;

        /*
         * ----------------------------------------------------------------------------
         *                              ALPHA BETA
         * ----------------------------------------------------------------------------
         */

        /*
         * Entry point to alphabeta
         */
        public static Move maxNextMoveAB(State currState, int height)
        {
            /* Create initial node */
            Game.Node max_init = new Game.Node(currState.AIPlayer,
                                                currState,
                                                int.MinValue,
                                                int.MaxValue,
                                                true);
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
            if ((height <= 0 && curr_node.state.isMax()) || (curr_node.state.isTerminal()))
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
                    min_node = new Game.Node(curr_node.state.opponent,
                                                        child_state,
                                                        curr_node.alpha,
                                                        curr_node.beta,
                                                        false);
                    /* get child's alpha value */
                    int child_alpha = alphabeta(min_node, height - 1);

                    /* set new alpha value for this node - 
                     * the less than or equal ensures that when AI
                     * sees it will lose no matter what, then it will 
                     * still pick a move
                     */
                    if (curr_node.alpha <= child_alpha)
                    {
                        curr_node.alpha = child_alpha;
                        /* only update best move if we are root node */
                        if (curr_node.root)
                        {
                            best_move = min_node;
                        }
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
                                                        curr_node.beta,
                                                        false);

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




        /*
         * ----------------------------------------------------------------------------
         *                  ITERATIVE DEEPENING
         * ----------------------------------------------------------------------------
         */

        /* Var to keep track of timer expiration */
        public static bool timerOn;

        /*
         * Iterative deepening of AlphaBeta
         */
        public static Move maxNextMoveID(State currState, float timeout)
        {
            /* reset timerOn variable */
            timerOn = true;

            /* # plys changes on every iteration */
            int plys = 1;

            /* Timer that expires when the specified number of seconds is passed */
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(TimerExpiredEvent);
            timer.Interval = timeout * 1000;    // timeout = # seconds
            timer.Enabled = true;               // turn timer on!

            /* Create initial node */
            Game.Node max_init = new Game.Node(currState.AIPlayer,
                                                currState,
                                                int.MinValue,
                                                int.MaxValue,
                                                true);

            /* Loop executes until timer expires */
            while (timerOn)
            {
                /* calculate next best move given new depth */
                alphabetaID(max_init, plys);

                /* increase plys level */
                plys++;
            }

            /* turn timer off */
            timer.Enabled = false;

            /* this line will break of there was not enough time to find a state,
             * which should not happen given the speed of AI
             */
            return best_move.state.generatorMove;
        }

        private static void TimerExpiredEvent(object source, ElapsedEventArgs e)
        {
            timerOn = false;
        }

        /*
         * alphabeta - Recursive algorithm that searches for the 
         *             best possible move taking into account the 
         *             opponent's move
         * @param:
         * @return: alpha or beta value
         */
        public static int alphabetaID(Game.Node curr_node, int height)
        {
            /* TIMER has gone off... break ALL recursion! 
             * Note: There is a case where AI does not find a state, in which case
             * the best_move is null
             */
            if (!timerOn)
                return 0;

            /* base case - check if we have reached desired depth or if
             * current node.state is a terminal state */
            if ((height <= 0 && curr_node.state.isMax()) || (curr_node.state.isTerminal()))
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
                    min_node = new Game.Node(curr_node.state.opponent,
                                                        child_state,
                                                        curr_node.alpha,
                                                        curr_node.beta,
                                                        false);
                    /* get child's alpha value */
                    int child_alpha = alphabetaID(min_node, height - 1);

                    /* set new alpha value for this node - 
                     * the less than or equal ensures that when AI
                     * sees it will lose no matter what, then it will 
                     * still pick a move
                     */
                    if (curr_node.alpha <= child_alpha)
                    {
                        curr_node.alpha = child_alpha;
                        /* only update best move if we are root node */
                        if (curr_node.root)
                        {
                            best_move = min_node;
                        }
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
                                                        curr_node.beta,
                                                        false);

                    /* set new alpha value for this node */
                    curr_node.beta = Math.Min(curr_node.beta,
                                              alphabetaID(max_node, height - 1));

                    /* alpha cut-off */
                    if (curr_node.beta <= curr_node.alpha)
                        break;
                }
                return curr_node.beta;
            }
        }
    }
}
