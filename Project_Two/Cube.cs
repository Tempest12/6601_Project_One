using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//My Imports

//3rd Part Imports
//using OpenTK.Graphics;
using OpenTK.Math;
using OpenTK.Graphics.OpenGL;


namespace Restart
{
    public class Cube
    {
        //Board States. Every state on the board will be one of the following:
        public const byte EMPTY = 0;
        public const byte PLAYERONE = 1;
        public const byte PLAYERTWO = 2;
        public const byte SHADOW = 3;

        //Colours
        public static OpenTK.Graphics.Color4 playerOneColour;
        public static OpenTK.Graphics.Color4 playerTwoColour;

        public static OpenTK.Graphics.Color4 shadowColour;
        public static OpenTK.Graphics.Color4 traceColour;

        public static OpenTK.Graphics.Color4 cubeColour;

        public static OpenTK.Graphics.Color4 highlightColour;

        public static int cubeDimension;
        public static float cubeSize;
        public static float maxCoordinate;

        public static int selectedCol = 0;
        public static int selectedRow = 0;

        public static bool focus = false;
        public static int focusRow = 0;
        public static int focusCol = 0;
        public static int focusDistance = 0;

        public static bool highlightRow = false;
        public static bool highlightCol = false;

        public static bool drawCubeWire = true;

        //Math Stuff
        public static float startCoordinate = 0.0f;

        //Game Board
        public byte[,,] cube;
        
        public Cube()
        {
            cube = new byte[cubeDimension, cubeDimension, cubeDimension];
        }

        public void draw()
        {
            GL.Begin(BeginMode.Triangles);
            {
                //Player One
                GL.Color4(playerOneColour);
                if (Core.playerOne.currentPosition != null)
                {
                    fillSmallCube(Core.playerOne.currentPosition);
                }

                //Player Two
                GL.Color4(playerTwoColour);
                if (Core.playerTwo.currentPosition != null)
                {
                    fillSmallCube(Core.playerTwo.currentPosition);
                }
            }
            GL.End();

            if (focus)
            {
                drawFocused();
            }
            else
            {
                drawCube();
            }
        }

        public void drawSmallCube(int row, int col, int distance)
        {
            for (int x = col; x < col + 2; x++)
            {
                for (int z = distance; z < distance + 2; z++)
                {
                    GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + row * cubeSize, startCoordinate + z * cubeSize);
                    GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate + z * cubeSize);
                }
            }

            for (int y = row; y < row + 2; y++)
            {
                for (int z = distance; z < distance + 2; z++)
                {
                    GL.Vertex3(startCoordinate + col * cubeSize, startCoordinate + y * cubeSize, startCoordinate + z * cubeSize);
                    GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate + y * cubeSize, startCoordinate + z * cubeSize);
                }
            }

            for (int x = col; x < col + 2; x++)
            {
                for (int y = row; y < row + 2; y++)
                {
                    GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + y * cubeSize, startCoordinate + distance * cubeSize);
                    GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + y * cubeSize, startCoordinate + (distance + 1) * cubeSize);
                }
            }
        }

        public void drawFocused()
        {
            GL.Begin(BeginMode.Lines);
            {
                GL.Color4(highlightColour);
                drawSmallCube(focusRow, focusCol, focusDistance);

                //Draw the trace of possible moves
                GL.Color4(traceColour);
                foreach (Move move in Core.currentPlayer.possibleMoves)
                {
                    if (highlightCol && selectedCol == move.col)
                    {
                        drawSmallCube(move.row, move.col, move.distance);
                    }
                    if (highlightRow && selectedRow == move.row)
                    {
                        drawSmallCube(move.row, move.col, move.distance);
                    }
                }

                //Draw the focused part of the cube
                GL.Color4(cubeColour);
                if (highlightCol)
                {
                    drawCol();
                }
                else if (highlightRow)
                {
                    drawRow();
                    GL.Color4(shadowColour);
                }
            }
            GL.End();

            
            //Now time to draw the state of the game
            GL.Begin(BeginMode.Triangles);
            {
                //Drawing the Shadow
                GL.Color4(shadowColour);
                if (highlightCol)
                {
                    for (int x = 0; x < cubeDimension; x++)
                    {
                        //for (int y = 0; y < cubeDimension; y++)
                        int y = selectedCol;
                        {
                            for (int z = 0; z < cubeDimension; z++)
                            {
                                if (cube[x, y, z] == SHADOW)
                                {
                                    fillSmallCube(x, y, z);
                                }
                            }
                        }
                    }           
                }
                else if(highlightRow)
                {
                    //for (int x = 0; x < cubeDimension; x++)
                    int x = selectedRow;
                    {
                        for (int y = 0; y < cubeDimension; y++)
                        {
                            for (int z = 0; z < cubeDimension; z++)
                            {
                                if (cube[x, y, z] == SHADOW)
                                {
                                    fillSmallCube(x, y, z);
                                }
                            }
                        }
                    }
                }
            }
            GL.End();
            
        }

        public void drawRow()
        {
            for (int z = 0; z < cubeDimension + 1; z++)
            {
                GL.Vertex3(startCoordinate, startCoordinate + selectedRow * cubeSize, startCoordinate + z * cubeSize);
                GL.Vertex3(maxCoordinate, startCoordinate + selectedRow * cubeSize, startCoordinate + z * cubeSize);

                GL.Vertex3(startCoordinate, startCoordinate + (selectedRow + 1) * cubeSize, startCoordinate + z * cubeSize);
                GL.Vertex3(maxCoordinate, startCoordinate + (selectedRow + 1) * cubeSize, startCoordinate + z * cubeSize);
            }

            for (int x = 0; x < cubeDimension + 1; x++)
            {
                GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + selectedRow * cubeSize, startCoordinate);
                GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + selectedRow * cubeSize, maxCoordinate);

                GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + (selectedRow + 1) * cubeSize, startCoordinate);
                GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + (selectedRow + 1) * cubeSize, maxCoordinate);
            }

            for (int x = 0; x < cubeDimension + 1; x++)
            {
                for (int z = 0; z < cubeDimension + 1; z++)
                {
                    GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + selectedRow * cubeSize, startCoordinate + z * cubeSize);
                    GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + (selectedRow + 1) * cubeSize, startCoordinate + z * cubeSize);
                }
            }
        }

        public void drawCol()
        {
            for (int z = 0; z < cubeDimension + 1; z++)
            {
                for (int y = 0; y < cubeDimension + 1; y++)
                {
                    GL.Vertex3(startCoordinate + selectedCol * cubeSize, startCoordinate + y * cubeSize, startCoordinate + z * cubeSize);
                    GL.Vertex3(startCoordinate + (selectedCol + 1) * cubeSize, startCoordinate + y * cubeSize, startCoordinate + z * cubeSize);
                }
            }

            for (int z = 0; z < cubeDimension + 1; z++)
            {
                GL.Vertex3(startCoordinate + selectedCol * cubeSize, startCoordinate, startCoordinate + z * cubeSize);
                GL.Vertex3(startCoordinate + selectedCol * cubeSize, maxCoordinate, startCoordinate + z * cubeSize);

                GL.Vertex3(startCoordinate + (selectedCol + 1) * cubeSize, startCoordinate, startCoordinate + z * cubeSize);
                GL.Vertex3(startCoordinate + (selectedCol + 1) * cubeSize, maxCoordinate, startCoordinate + z * cubeSize);
            }

            for (int y = 0; y < cubeDimension + 1; y++)
            {
                GL.Vertex3(startCoordinate + selectedCol * cubeSize, startCoordinate + y * cubeSize, startCoordinate);
                GL.Vertex3(startCoordinate + selectedCol * cubeSize, startCoordinate + y * cubeSize, maxCoordinate);

                GL.Vertex3(startCoordinate + (selectedCol + 1) * cubeSize, startCoordinate + y * cubeSize, startCoordinate);
                GL.Vertex3(startCoordinate + (selectedCol + 1) * cubeSize, startCoordinate + y * cubeSize, maxCoordinate);
            }

            
        }

        public void drawCube()
        {
            GL.Begin(BeginMode.Lines);
            {
                //Draw Selected Row if any.
                if (highlightCol)
                {
                    GL.Color4(highlightColour);
                    drawCol();
                }
                if (highlightRow)
                {
                    GL.Color4(highlightColour);
                    drawRow();
                }

                //Draw the trace of possible moves
                GL.Color4(traceColour);
                foreach (Move move in Core.currentPlayer.possibleMoves)
                {
                    drawSmallCube(move.row, move.col, move.distance);
                }

                //Draw the Cube
                GL.Color4(cubeColour);

                if (drawCubeWire)
                {
                    for (int y = 0; y < cubeDimension + 1; y++)
                    {
                        for (int x = 0; x < cubeDimension + 1; x++)
                        {
                            GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + y * cubeSize, startCoordinate);
                            GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate + y * cubeSize, maxCoordinate);
                        }
                    }
                    for (int y = 0; y < cubeDimension + 1; y++)
                    {
                        for (int z = 0; z < cubeDimension + 1; z++)
                        {
                            for (int x = 0; x < cubeDimension; x++)
                            {
                                GL.Vertex3(startCoordinate, startCoordinate + y * cubeSize, startCoordinate + z * cubeSize);
                                GL.Vertex3(maxCoordinate, startCoordinate + y * cubeSize, startCoordinate + z * cubeSize);
                            }
                        }
                    }

                    for (int x = 0; x < cubeDimension + 1; x++)
                    {
                        for (int z = 0; z < cubeDimension + 1; z++)
                        {
                            GL.Vertex3(startCoordinate + x * cubeSize, startCoordinate, startCoordinate + z * cubeSize);
                            GL.Vertex3(startCoordinate + x * cubeSize, maxCoordinate, startCoordinate + z * cubeSize);
                        }
                    }
                }
            }
            GL.End();

            //Now time to draw the state of the game
            GL.Begin(BeginMode.Triangles);
            {
                //Drawing the Shadow
                GL.Color4(shadowColour);

                for(int x = 0; x < cubeDimension; x++)
                {
                    for(int y = 0; y < cubeDimension; y++)
                    {
                        for(int z = 0; z < cubeDimension; z++)
                        {
                            if (cube[x, y, z] == SHADOW)
                            {
                                fillSmallCube(x, y, z);
                            }
                        }
                    }
                }
            }
            GL.End();

        }

        public void fillSmallCube(float row, float col, float distance)
        {
            //Bottom Face
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate +  row * cubeSize, startCoordinate +  distance * cubeSize);
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate +  row * cubeSize, startCoordinate +  distance * cubeSize);
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate +  row * cubeSize, startCoordinate + (distance + 1) * cubeSize);

            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate +  row * cubeSize, startCoordinate + (distance + 1) * cubeSize);
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate +  row * cubeSize, startCoordinate +  distance * cubeSize);
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate +  row * cubeSize, startCoordinate + (distance + 1) * cubeSize);

            //Top Face
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate +  distance * cubeSize);
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate +  distance * cubeSize);
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate + (distance + 1) * cubeSize);

            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate + (distance + 1) * cubeSize);
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate +  distance * cubeSize);
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate + (distance + 1) * cubeSize);

            //Front Face
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate +  row * cubeSize, startCoordinate +  distance * cubeSize);//Origin
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate +  distance * cubeSize);//y++
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate +  distance * cubeSize);//x and y ++

            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate +  row * cubeSize, startCoordinate +  distance * cubeSize);//Origin
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate +  row * cubeSize, startCoordinate +  distance * cubeSize);//x++
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate +  distance * cubeSize);//x and y ++

            //Back Face
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate +  row * cubeSize, startCoordinate + (distance + 1) * cubeSize);//z++
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate + (distance + 1) * cubeSize);//y and z++
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate + (distance + 1) * cubeSize);//end

            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate +  row * cubeSize, startCoordinate + (distance + 1) * cubeSize);//z++
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate +  row * cubeSize, startCoordinate + (distance + 1) * cubeSize);//x and z++
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate + (distance + 1) * cubeSize);//end

            //Left Face
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate +  row * cubeSize, startCoordinate +  distance * cubeSize);//Origin
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate +  row * cubeSize, startCoordinate + (distance + 1) * cubeSize);//z++
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate + (distance + 1) * cubeSize);//y and z++

            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate +  row * cubeSize, startCoordinate +  distance * cubeSize);//Origin
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate +  distance * cubeSize);//y++
            GL.Vertex3(startCoordinate +  col * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate + (distance + 1) * cubeSize);//y and z++

            //Right Face
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate +  row * cubeSize, startCoordinate +  distance * cubeSize);//x++
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate +  row * cubeSize, startCoordinate + (distance + 1) * cubeSize);//x and z++
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate + (distance + 1) * cubeSize);//end

            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate +  row * cubeSize, startCoordinate +  distance * cubeSize);//x++
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate +  distance * cubeSize);//x and y++
            GL.Vertex3(startCoordinate + (col + 1) * cubeSize, startCoordinate + (row + 1) * cubeSize, startCoordinate + (distance + 1) * cubeSize);//end
        }

        public void fillSmallCube(Move location)
        {
            //Bottom Face
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + location.distance * cubeSize);
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + location.distance * cubeSize);
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);

            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + location.distance * cubeSize);
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);

            //Top Face
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + (location.row + 1 ) * cubeSize, startCoordinate + location.distance * cubeSize);
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + location.distance * cubeSize);
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);

            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + location.distance * cubeSize);
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);

            //Front Face
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + location.distance * cubeSize);//Origin
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + location.distance * cubeSize);//y++
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + location.distance * cubeSize);//x and y ++

            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + location.distance * cubeSize);//Origin
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + location.distance * cubeSize);//x++
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + location.distance * cubeSize);//x and y ++

            //Back Face
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//z++
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//y and z++
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//end

            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//z++
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//x and z++
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//end

            //Left Face
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + location.distance * cubeSize);//Origin
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//z++
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//y and z++

            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + location.distance * cubeSize);//Origin
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + location.distance * cubeSize);//y++
            GL.Vertex3(startCoordinate + location.col * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//y and z++

            //Right Face
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + location.distance * cubeSize);//x++
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//x and z++
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//end

            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + location.row * cubeSize, startCoordinate + location.distance * cubeSize);//x++
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + location.distance * cubeSize);//x and y++
            GL.Vertex3(startCoordinate + (location.col + 1) * cubeSize, startCoordinate + (location.row + 1) * cubeSize, startCoordinate + (location.distance + 1) * cubeSize);//end

        }

        public static void incrementValue(ref int value)
        {
            value++;
            value %= cubeDimension;
        }

        public static void decrementValue(ref int value)
        {
            value += cubeDimension - 1;
            value %= cubeDimension;
        }

        public static void init()
        {
            setColours();

            cubeDimension = Config.convertSettingToInt("game", "cube_dimension");
            cubeSize = Config.convertSettingToFloat("game", "cube_size");
            
            maxCoordinate = cubeSize * (float)cubeDimension / 2;
            startCoordinate = -maxCoordinate;
        }

        private static void setColours()
        {
            playerOneColour = new OpenTK.Graphics.Color4(Config.convertSettingToFloat("colours", "playerOne_R"), Config.convertSettingToFloat("colours", "playerOne_G"), Config.convertSettingToFloat("colours", "playerOne_B"), Config.convertSettingToFloat("colours", "playerOne_A"));
            playerTwoColour = new OpenTK.Graphics.Color4(Config.convertSettingToFloat("colours", "playerTwo_R"), Config.convertSettingToFloat("colours", "playerTwo_G"), Config.convertSettingToFloat("colours", "playerTwo_B"), Config.convertSettingToFloat("colours", "playerTwo_A"));

            shadowColour = new OpenTK.Graphics.Color4(Config.convertSettingToFloat("colours", "shadow_R"), Config.convertSettingToFloat("colours", "shadow_G"), Config.convertSettingToFloat("colours", "shadow_B"), Config.convertSettingToFloat("colours", "shadow_A"));
            traceColour = new OpenTK.Graphics.Color4(Config.convertSettingToFloat("colours", "trace_R"), Config.convertSettingToFloat("colours", "trace_G"), Config.convertSettingToFloat("colours", "trace_B"), Config.convertSettingToFloat("colours", "trace_A"));

            cubeColour = new OpenTK.Graphics.Color4(Config.convertSettingToFloat("colours", "cube_R"), Config.convertSettingToFloat("colours", "cube_G"), Config.convertSettingToFloat("colours", "cube_B"), Config.convertSettingToFloat("colours", "cube_A"));
            highlightColour = new OpenTK.Graphics.Color4(Config.convertSettingToFloat("colours", "highlight_R"), Config.convertSettingToFloat("colours", "highlight_G"), Config.convertSettingToFloat("colours", "highlight_B"), Config.convertSettingToFloat("colours", "highlight_A"));   
        }
    }
}
