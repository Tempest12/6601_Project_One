//Windows Auto Imports 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//My Imports
using System.Windows.Forms;

//3rd Party Imports
//using OpenTK;

//using GLU = OpenTK.Graphics.Glu;
using OpenTK.Graphics.OpenGL;
using OpenTK.Math;
using OpenTK.Input;

namespace Restart
{
    /**
     * The Core of the application.
     */
    public class Core
    {
        public static int moveCounter = 0;

        public static RenderWindow window;
        public static Cube drawCube;

        public static Player currentPlayer;

        public static Player playerOne;
        public static Player playerTwo;

        public static int run_code;

        public static Random numberGenerator;

        public static bool simulating;
        public static int simCount;
        public static int simCountMax;

        public static int playerOneVictories = 0;
        public static int playerTwoVictories = 0;

        public static State currentState;

        //sprivate static Color4 clearColour;

        //Used to draw spheres... Not sure why it's needed...
        //public static IntPtr intptr = GLU.NewQuadric();

        /**
         * Initializes the Core of the program by initliazling everything. Failures here cause program termination
         */
        public static void init()
        {
            Config.init();

            Log.init(Config.convertSettingToInt("log", "default_level"));

            initGL();

            Cube.init();
            drawCube = new Cube();

            playerOne = new Player(1, getPlayerType(1));
            playerTwo = new Player(2, getPlayerType(2));
            switch (Config.convertSettingToInt("game", "starting_player"))
            {
                case 1:
                    currentPlayer = playerOne;
                    break;

                case 2:
                    currentPlayer = playerTwo;
                    break;
            }

            numberGenerator = new Random();

            simulating = Config.convertSettingToBool("game", "simulating");
            simCount = 0;
            simCountMax = Config.convertSettingToInt("game", "sim_count");
        }

        /**
         * Initializes anything we need to Render
         */
        private static void initGL()
        {
            window = new RenderWindow();
        }

        public static PlayerType getPlayerType(int playerNumber)
        {
            switch (playerNumber)
            {
                case 1:
                    if (Config.getValue("players", "player_one") == "human")
                    {
                        return PlayerType.HUMAN;
                    }
                    else if(Config.getValue("players", "player_one") == "ai")
                    {
                        return PlayerType.AI;
                    }
                    else
                    {
                        MainMethod.die("Error : Core.getPlayerType : player type: \"" + Config.getValue("players", "player_one") + "\" is not supported. Typo?");
                    }
                    break;

                case 2:
                    if (Config.getValue("players", "player_two") == "human")
                    {
                        return PlayerType.HUMAN;
                    }
                    else if(Config.getValue("players", "player_two") == "ai")
                    {
                        return PlayerType.AI;
                    }
                    else
                    {
                        MainMethod.die("Error : Core.getPlayerType : player type: \"" + Config.getValue("players", "player_two") + "\" is not supported. Typo?");
                    }
                    break;
            }

            //should never reach here but:
            return PlayerType.HUMAN;
        }

        public static void restartGame()
        {
            drawCube = new Cube();

            playerOne.currentPosition = null;
            playerTwo.currentPosition = null;

            switch (Config.convertSettingToInt("game", "starting_player"))
            {
                case 1:
                    currentPlayer = playerOne;
                    break;

                case 2:
                    currentPlayer = playerTwo;
                    break;
            }

            currentPlayer.getNewMoves(drawCube.cube);

        }

        public static bool done = false;

        public static void switchPlayers()
        {
            if (done)
            {
                return;
            }


            if(currentPlayer.Equals(playerOne))
            {
                currentPlayer = playerTwo;
                playerTwo.getNewMoves(drawCube.cube);
                playerOne.getNewMoves(drawCube.cube);
                if (playerTwo.possibleMoves.Count == 0)
                {
                    endGame(playerOne.playerNumber);
                }
                else if (playerOne.possibleMoves.Count == 0)
                {
                    endGame(playerTwo.playerNumber);
                }
            }
            else if (currentPlayer.Equals(playerTwo))
            {
                currentPlayer = playerOne;
                playerOne.getNewMoves(drawCube.cube);
                playerTwo.getNewMoves(drawCube.cube);
                if (playerOne.possibleMoves.Count == 0)
                {
                    endGame(playerTwo.playerNumber);
                }
                else if (playerTwo.possibleMoves.Count == 0)
                {
                    endGame(playerOne.playerNumber);
                }
            }
        }

        public static void endGame(int winningPlayerNumber)
        {
            if (simulating)
            {
                Log.writeInfo("Game has ended Player: " + winningPlayerNumber + " has won.");
                switch (winningPlayerNumber)
                {
                    case 1:
                        playerOneVictories++;
                        break;

                    case 2:
                        playerTwoVictories++;
                        break;
                }

                simCount++;
                if (simCount < simCountMax)
                {
                    restartGame();
                }
                else
                {
                    Log.writeInfo("Final Results:");
                    Log.writeInfo("Player One: " + playerOneVictories + " victories.");
                    Log.writeInfo("Player Two: " + playerTwoVictories + " victories.");
                    uninit();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Player " + winningPlayerNumber + " has won!. Would you like to play another game?", "Game Over", MessageBoxButtons.YesNo);

                switch (result)
                {
                    case DialogResult.Yes:
                        restartGame();

                        break;

                    case DialogResult.No:

                        uninit();

                        break;
                }
            }
        }

        public static Player getNotCurrentPlayer()
        {
            if(currentPlayer.Equals(playerOne))
            {
                return playerTwo;
            }
            else if (currentPlayer.Equals(playerTwo))
            {
                return playerOne;
            }
            else
            {
                MainMethod.die("Error: Core.getNotCurrentPlayer : The current player is not player one or player two");
                return null;
            }
        }


        public static void startGameLoop()
        {
            window.Run(60);
        }

        /**
         * Updates the current state of the Program
         */
        public static int counter = 0;

        public static void update()
        {
            counter++;

            if (currentPlayer.type == PlayerType.AI)
            {
                //Make the current Game State
                currentState = new State(currentPlayer, Core.getNotCurrentPlayer(), drawCube.cube, Core.getNotCurrentPlayer().currentPosition, true);

                //Pass it on to the Computer
                currentPlayer.makeMove(currentState);

                //Console.ReadLine();

                switchPlayers();
            }
        }
        /**
         * Uninitializes my OpenGL Stuff
         */
        private static void uninitGL()
        {

        }

        /**
         * Uninitialies the Core of the program
         */
        public static void uninit()
        {
            Config.uninit();

            Log.uninit();

            uninitGL();

            Console.WriteLine("Exiting");

            Environment.Exit(0);
        }

        public static float degreeToRadian(float degree)
        {
            return degree * (180.0f / (float)Math.PI);
        }

        public static float radianToDegree(float radian)
        {
            return radian * ((float)Math.PI / 180.0f);
        }
    }
}
