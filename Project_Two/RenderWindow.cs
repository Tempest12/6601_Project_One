//Windows Auto Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//My Imports

//3rd Party Imports
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Restart
{
    public class RenderWindow : GameWindow
    {
        //Camrea Stuff
        public Camera camera;
        
        //Mouse Stuff
        bool leftClickDown;

        int oldMouseX = 0;
        int oldMouseY = 0;
        public static State lastState;


        public RenderWindow() : base(Config.convertSettingToInt("window", "width"), Config.convertSettingToInt("window", "height"), OpenTK.Graphics.GraphicsMode.Default, Config.getValue("window", "title"))
        {
            this.VSync = VSyncMode.On;

            //GL.ClearDepth(1.0);
            
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearColor(Config.convertSettingToFloat("window", "bg_red"), Config.convertSettingToFloat("window", "bg_green"), Config.convertSettingToFloat("window", "bg_red"), Config.convertSettingToFloat("window", "bg_alpha"));

            //GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Viewport(0, 0, Width, Height);

            camera = new Camera();

            this.Keyboard.KeyDown += handleKeyboardDown;
            this.Keyboard.KeyUp += handleKeyboardUp;

            this.Mouse.ButtonDown += handleMouseButtonDown;
            this.Mouse.ButtonUp += handleMouseButtonUp;
            this.Mouse.Move += handleMouseMove;
        }

        public void handleMouseButtonUp(object sender, MouseButtonEventArgs mbea)
        {
            if (mbea.Button == MouseButton.Left)
            {
                leftClickDown = false;
            }
        }

        public void handleMouseButtonDown(object sender, MouseButtonEventArgs mbea)
        {
            if (mbea.Button == MouseButton.Left)
            {
                leftClickDown = true;

                oldMouseX = Mouse.X;
                oldMouseY = Mouse.Y;
            }
        }

        public void handleMouseMove(object sender, MouseMoveEventArgs mmea)
        {
            if (leftClickDown)
            {
                camera.rotate(Mouse.X - oldMouseX, Mouse.Y - oldMouseY, 0.0f);
            }
            else
            {
                return;
            }

            oldMouseX = Mouse.X;
            oldMouseY = Mouse.Y;
        }

        public void handleKeyboardDown(object sender, KeyboardKeyEventArgs kkea)
        {
            if(Core.currentPlayer.type != PlayerType.HUMAN && kkea.Key != Key.Escape)
            {
                return;
            }

            switch(kkea.Key)
            {
                case Key.Escape:
                    
                    Core.uninit();
                    break;

                case Key.A:
                case Key.Left:

                    if (Cube.focus)
                    {
                        if (Cube.highlightCol)
                        {
                            Cube.incrementValue(ref Cube.focusDistance);
                        }
                        else
                        {
                            Cube.incrementValue(ref Cube.focusCol);
                        }
                    }
                    else
                    {
                        if (Cube.highlightCol)
                        {
                            Cube.incrementValue(ref Cube.selectedCol);
                        }
                        else
                        {
                            Cube.highlightCol = true;
                            Cube.highlightRow = false;
                        }
                    }

                    break;

                case Key.W:
                case Key.Up:

                    if (Cube.focus)
                    {
                        if (Cube.highlightCol)
                        {
                            Cube.incrementValue(ref Cube.focusRow);
                        }
                        else
                        {
                            Cube.incrementValue(ref Cube.focusDistance);
                        }
                    }
                    else
                    {
                        if (Cube.highlightRow)
                        {
                            Cube.incrementValue(ref Cube.selectedRow);
                        }
                        else
                        {
                            Cube.highlightRow = true;
                            Cube.highlightCol = false;
                        }
                    }
                    break;

                case Key.S:
                case Key.Down:

                    if (Cube.focus)
                    {
                        if (Cube.highlightCol)
                        {
                            Cube.decrementValue(ref Cube.focusRow);
                        }
                        else
                        {
                            Cube.decrementValue(ref Cube.focusDistance);
                        }
                    }
                    else
                    {
                        if (Cube.highlightRow)
                        {
                            Cube.decrementValue(ref Cube.selectedRow);
                        }
                        else
                        {
                            Cube.highlightCol = false;
                            Cube.highlightRow = true;
                        }
                    }
                    break;

                case Key.D:
                case Key.Right:

                    if (Cube.focus)
                    {
                        if (Cube.highlightCol)
                        {
                            Cube.decrementValue(ref Cube.focusDistance);
                        }
                        else
                        {
                            Cube.decrementValue(ref Cube.focusCol);
                        }
                    }
                    else
                    {
                        if (Cube.highlightCol)
                        {
                            Cube.decrementValue(ref Cube.selectedCol);
                        }
                        else
                        {
                            Cube.highlightCol = true;
                            Cube.highlightRow = false;
                        }
                    }
                    break;

                case Key.Enter:
                case Key.Space:

                    if (Cube.focus)
                    {
                        //lastState = new State(Core.getNotCurrentPlayer(), Core.currentPlayer, Core.drawCube.cube, new Move(Cube.focusRow, Cube.focusCol, Cube.focusDistance), false);
                        Log.writeDebug("");

                        if (Core.currentPlayer.tryMove(new Move(Cube.focusRow, Cube.focusCol, Cube.focusDistance)))
                        {
                            Log.writeDebug("Moved to location " + Core.currentPlayer.currentPosition.ToString());

                            //Testing State Code
                            

                            Core.switchPlayers();
                            Cube.focus = false;

                        }
                        else
                        {
                            //Bad move. Respond by doing something
                            return;
                        }
                    }
                    else
                    {
                        Cube.focus = true;
                        Cube.focusCol = Cube.selectedCol;
                        Cube.focusRow = Cube.selectedRow;
                    }

                    break;

                case Key.Tab:
                case Key.BackSpace:

                    Cube.focus = false;

                    break;
            }

            /*if (!Cube.focus)
            {
                Console.WriteLine("Current Location, selectedCol : " + Cube.selectedCol + " and the selectedRow: " + Cube.selectedRow);
            }
            else
            {
                Console.WriteLine("Focused at locatoin col: " + Cube.focusCol + " row: " + Cube.focusRow + " distance: " + Cube.focusDistance);
            }*/
        }

        public void handleKeyboardUp(object sender, KeyboardKeyEventArgs kkea)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
        
        }
        //                      OnRenderFrame
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            

            Core.update();

            //GL.LoadIdentity();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 matrixStack = Matrix4.LookAt(camera.position, camera.focusPoint, camera.upDirection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref matrixStack);

            camera.applyRotation();
            Core.drawCube.draw();

            this.SwapBuffers();
            //GL.Finish();
        }

        public int getMouseX()
        {
            return this.Mouse.X;
        }

        public int getMouseY()
        {
            return this.Mouse.Y;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }
    }
}
