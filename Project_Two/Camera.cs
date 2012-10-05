using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//My Imports

//3rd Party Imports
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Restart
{
    public class Camera
    {
        public OpenTK.Vector3 position;
        public OpenTK.Vector3 focusPoint;
        public OpenTK.Vector3 upDirection;

        public float xRotation = 0.0f;
        public float yRotation = 0.0f;
        public float zRotation = 0.0f;

        public float rotateSpeed = 0.0f;
        public bool invertY = false;

        public Camera()
        {
            this.position = new Vector3(Config.convertSettingToFloat("camera", "position_x"), Config.convertSettingToFloat("camera", "position_y"), Config.convertSettingToFloat("camera", "position_z"));

            this.focusPoint = new Vector3(Config.convertSettingToFloat("camera", "focus_x"), Config.convertSettingToFloat("camera", "focus_y"), Config.convertSettingToFloat("camera", "focus_z"));

            this.upDirection = new Vector3(Config.convertSettingToFloat("camera", "up_x"), Config.convertSettingToFloat("camera", "up_y"), Config.convertSettingToFloat("camera", "up_z"));

            this.rotateSpeed = Config.convertSettingToFloat("camera", "rotate_speed");
            this.invertY = Config.convertSettingToBool("camera", "invert_y");
        }


        public Camera(OpenTK.Vector3 position, OpenTK.Vector3 focusPoint, OpenTK.Vector3 upDirection)
        {
            this.position = position;
            this.focusPoint = focusPoint;
            this.upDirection = upDirection;
        }

        public void zoomIn()
        {
            position.Z -= Config.convertSettingToFloat("camera", "delta_zoom");
        }

        public void zoomOut()
        {
            position.Z += Config.convertSettingToFloat("camera", "delta_zoom");
        }

        public void rotate(float xRotationChange, float yRotationChange, float zRotationChange)
        {
            xRotation += yRotationChange * rotateSpeed;

            if (invertY)
            {
                yRotation -= xRotationChange * rotateSpeed;
            }
            else
            {
                yRotation += xRotationChange * rotateSpeed;
            }
                
            zRotation += zRotationChange * rotateSpeed;

            xRotation %= 360.0f;
            yRotation %= 360.0f;
            zRotation %= 360.0f;
        }

        public void applyRotation()
        {
            GL.Rotate(xRotation, Vector3.UnitX);
            GL.Rotate(yRotation, Vector3.UnitY);
            GL.Rotate(zRotation, Vector3.UnitZ);
        }
    }
}
