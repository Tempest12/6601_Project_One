using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restart
{
    class Game
    {
        /*
         * Node: representation of a node that make up the tree
         */
        public class Node
        {
            /* Constructor */
            public Node(Player p, State s, int alpha_in, int beta_in, bool root_in)
            {
                player = p;
                state = s;
                alpha = alpha_in;
                beta = beta_in;
                root = root_in;
            }


            /* player playing this node */
            public Player player { get; set; }
            
            /* node's state of game */
            public State state { get; set; }

            /* alpha value for node */
            public int alpha { get; set; }

            /* beta value for node */
            public int beta { get; set; }

            public bool root { get; set; }
        }

        /*
         * State: representation of a game state
         */
        /*public class State
        {
            public State(int value)
            {
                Value = value;
            }
            // TODO - a simple state number for now
            public int Value { get ; set ;}
        }*/
 
        /*
         * Player: representation of a player in the game
         */
        //public enum Player { Max, Min };

        /*
         * terminal_test - tests if a state is a terminal state
         * @param: state
         * @return: true if state is terminal; false otherwise
         */
        /*public static bool terminal_test(State state)
        {
            // TODO - never terminate for now since game is ficticious 
            return false;
        }*/
        
        /*
         * eval_func - returns the value of a given state
         * @param: state
         * @return: integer value of a state
         */
        /*public static int eval_func(State state)
        {
            // TODO - return known evaluation numbers to test algorithm 
            return TermSvalues[term_index++];
        }*/
        
        /*
         * actions - calculates possible set of states given a state
         * @param: state
         * @return: new set of possible states from given one
         */
        /*public static List<State> actions(State parent)
        {
            /* list of children to return 
            List<State> children = new List<State>();

            /* hard-coded number of children for each node 
            int num_children = 3;
            for (int c = 1; c <= num_children; c++)
            {
                /* a child's state is simply the parents value increased
                 * by 1, 2, 3, ... num_children 
                State child = new State(parent.Value + c);
                children.Add(child);
            }
            return children;
        }*/

        /*
         * TermSvalues - hardcoded terminal state values to test algorithm
         *               against knwon trees.
         *               This is the same as book example on p.164
         */
        //public static int[] TermSvalues = { 3, 12, 8, 2, 14, 5, 2 };
        //public static int[] TermSvalues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        /* index to access terminal values above in left-to-right order */
        //static int term_index = 0;

        /*
         * printState - prints a given state
         */
        /*public static void printState(State state)
        {
            /* simple implementation just prints the value 
            Console.WriteLine("Best move = ");
            Console.WriteLine(state.Value);
        }*/
    }
}
