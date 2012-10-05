using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alpha_Beta
{
    class Program
    {
        /*static void Main(string[] args)
        {
            /* How deep in the tree we are willing to explore
             * Note: leaves nodes (cutoff) are at height 0).
             *       Thus, height = 2 means 2,1,0
             *       Max        @ 2
             *       Min        @ 1
             *       cutoff     @ 0
             *
            int height = 2;
            
            /* calculate the maximum alpha for Max at depth 2 *
            AlphaBeta.Max_alpha(height);

            /* print best move *
            Game.printState(AlphaBeta.best_move.state);

            /* prevent console from closing *
            Console.ReadLine();
        }**/
    }
}
