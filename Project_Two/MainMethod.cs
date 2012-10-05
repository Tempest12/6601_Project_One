//Windows Auto Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Our Language Imports


//3rd Party Libraries

namespace Restart
{
    public class MainMethod
    {
        /**
         * Starts the Program
         */
        public static void Main(string[] args)
        {
            Core.init();

            //test();
            Core.startGameLoop();

            Core.uninit();
        }

        /**
         * Runs any selected Tests
         */
        public static void test()
        {
            //Test the Config
            Config.DebugPrint();

            //Test the Logger
            //Log.writeDebug("Debuging out");
            //Log.writeInfo("Information is overrated.");
            //Log.writeError("Windows error remix is the best song ever!!");
            //Log.writeFatal("Houston we have a (fatal) problem");
            //Log.writeSpecial("Always here brah");
        }

        /**
         * Killes the program and prints a message to the console.
         * 
         * @param message - string - The Error message
         */
        public static void die(string message)
        {
            Console.WriteLine(message);

            if(Config.convertSettingToBool("die_options", "print_stack_trace"))
            {
                Console.WriteLine(new System.Diagnostics.StackTrace());
            }
            
            Core.uninit();
        }
    }
}
