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

            playerOne = new Player(1, PlayerType.HUMAN);
            playerTwo = new Player(2, PlayerType.HUMAN);
            switch (Config.convertSettingToInt("game", "starting_player"))
            {
                case 1:
                    currentPlayer = playerOne;
                    break;

                case 2:
                    currentPlayer = playerTwo;
                    break;
            }

            
        }

        /**
         * Initializes anything we need to Render
         */
        private static void initGL()
        {
            window = new RenderWindow();
        }

        public static void restartGame()
        {

        }

        public static void switchPlayers()
        {
            if(currentPlayer.Equals(playerOne))
            {
                currentPlayer = playerTwo;
                playerTwo.getNewMoves(drawCube.cube);
                if (playerTwo.possibleMoves.Count == 0)
                {
                    endGame(playerOne.playerNumber);
                }
            }
            else if(currentPlayer.Equals(playerTwo))
            {
                currentPlayer = playerOne;
                playerOne.getNewMoves(drawCube.cube);
                if (playerOne.possibleMoves.Count == 0)
                {
                    endGame(playerTwo.playerNumber);
                }
            }

            Console.WriteLine("");
        }

        public static void endGame(int winningPlayerNumber)
        {
            DialogResult result = MessageBox.Show("Player " + winningPlayerNumber, "Game Over", MessageBoxButtons.YesNo);

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
        public static void update()
        {
            //Console.WriteLine("Update");
            //window.camera.applyRotation();
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
