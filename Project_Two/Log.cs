//Windwos Auto Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//My Imports
using System.IO;

//3rd Party Imports


namespace Restart
{
    /**
     * Loging Utility. Supports mutlilevel logging.
     */

    public class Log
    {
        private static TextWriter logWriter = null;

        private const int debugLevel = 0;
        private const int infoLevel = 1;//1 = Info and above
	    private const int errorLevel = 2;//3 = Fatal and above
	    private const int fatalLevel = 3;//2 = Error and above
	    private const int specialOnly = 4;//4 = Special only BTW things marked special always log.
	    private static int logLevel = 0;


        /**
         * Initializes the Logger.
         * 
         * @param startingLevel - int - The level to log.
         */
        public static void init(int startingLevel)
        {
            if (logWriter == null)
            {
                logWriter = new StreamWriter("log.txt");
                printTimeStamp();
                logWriter.WriteLine(" Log Opened.");

                logLevel = startingLevel;
            }
            else
            {
                Console.WriteLine("Error: Log.init: The log has already been initialized.");
                writeError("Log.init(): The log has already been initlialized.");
            }
        }

        /**
         * Write a message to the log at Debug level.
         * 
         * @param message - string - the message to write
         */
        public static void writeDebug(string message)
        {
            if (message == null)
            {
                writeError("writeDebug: Message was null");
                return;
            }
            else if(logWriter == null)
            {
                Console.WriteLine("Error: writeDebug: Log has not been initialized yet.");
            }

            if (logLevel <= debugLevel)
            {
                printTimeStamp();
                logWriter.WriteLine(" DEBUG   : " + message);
            }
        }

        /**
         * Write a message to the log at Info level.
         * 
         * @param message - string - the message to write
         */
        public static void writeInfo(string message)
        {
            if (message == null)
            {
                writeError("writeInfo: Message was null");
                return;
            }
            else if (logWriter == null)
            {
                Console.WriteLine("Error: writeInfo: Log has not been initialized yet.");
            }

            if (logLevel <= infoLevel)
            {
                printTimeStamp();
                logWriter.WriteLine(" INFO    : " + message);
            }
        }

        /**
         * Write a message to the log at Error level.
         * 
         * @param message - string - the message to write
         */
        public static void writeError(string message)
        {
            if (message == null)
            {
                writeError("writeError: Message was null");
                return;
            }
            else if (logWriter == null)
            {
                Console.WriteLine("Error: writeError: Log has not been initialized yet.");
            }

            if (logLevel <= errorLevel)
            {
                printTimeStamp();
                logWriter.WriteLine(" ERROR   : " + message);
            }
        }

        /**
         * Write a message to the log at Fatal level.
         * 
         * @param message - string - the message to write
         */
        public static void writeFatal(string message)
        {
            if (message == null)
            {
                writeError("writeFatal: Message was null");
                return;
            }
            else if (logWriter == null)
            {
                Console.WriteLine("Error: writeFatal: Log has not been initialized yet.");
            }

            if (logLevel <= fatalLevel)
            {
                printTimeStamp();
                logWriter.WriteLine(" FATAL   : " + message);
            }
        }

        /**
         * Write a message to the log at Special level.
         * 
         * @param message - string - the message to write
         */
        public static void writeSpecial(string message)
        {
            if (message == null)
            {
                writeError("writeSpecial: Message was null");
                return;
            }
            else if (logWriter == null)
            {
                Console.WriteLine("Error: writeSpecial: Log has not been initialized yet.");
            }

            
            printTimeStamp();
            logWriter.WriteLine(" SPECIAL : " + message);
            
        }

        /**
         * Uninitializes the Logger. 
         */
        public static void uninit()
        {
            printTimeStamp();
            logWriter.WriteLine(" Log Closed.");
            logWriter.Close();
        }

        /**
         * Prints a time stamp in the logger
         */
        private static void printTimeStamp()
        {
            logWriter.Write(DateTime.Now);
        }
    }
}
