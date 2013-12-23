using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//***************************************************************************
// This class serves as the players camera for the game. By default, the camera
// will always keep the player in the center of the screen, as the player moves.

// There could be more work done here to add features like zooming, distortion, 
// and whatever.

// Should probably be a singleton and not static.

namespace ZeldaCraft
{
    static class Camera
    {
        public static Vector2 CamPos;
        public static Rectangle CamRect { get; private set; }
                                                  
        private static Matrix translation = Matrix.Identity;
        private static float zoom;
        private static bool updateMatrix;


        public static void Initialize(Player player, Rectangle mapView)
        {
            // first, set x/y to dead center of the player
            float adjustedX = player.Position.X + player.Width / 2;
            float adjustedY = player.Position.Y + player.Height / 2;
            
            // then adjust x/y to the top left of the screen (relative to player)
            adjustedX = adjustedX - mapView.Width / 2;            
            adjustedY = adjustedY - mapView.Height / 2;

            CamPos = new Vector2(adjustedX, adjustedY);            
            CamRect = new Rectangle((int)adjustedX, (int)adjustedY,
                                    mapView.Width, mapView.Height);

            zoom = (float)1;

            updateMatrix = false;        
        }


        public static void Update(Player player)
        {
            // move camera relative to the players movement
            CamPos.X = player.Position.X + player.Width / 2;
            CamPos.Y = player.Position.Y + player.Height / 2;

            // adjust camera to top left of screen (relative to player)
            CamPos.X = CamPos.X - CamRect.Width / 2;            
            CamPos.Y = CamPos.Y - CamRect.Height / 2;
            
            if ((CamPos.X != CamRect.X) || (CamPos.Y != CamRect.Y))   // if the camera moved
            {
                updateMatrix = true;
                
                int newRectX = (int)CamPos.X;
                int newRectY = (int)CamPos.Y;                                             
                
                CamRect = new Rectangle(newRectX, newRectY, CamRect.Width, CamRect.Height);
            }
        }

        
        public static Matrix TranslateMatrix()
        {
            if (updateMatrix == true)
                translation = Matrix.CreateTranslation(new Vector3(-CamPos.X, -CamPos.Y, 0)) *
                              Matrix.CreateScale(new Vector3(zoom, zoom, 1));

            updateMatrix = false;

            return translation;
        }
    }
}
