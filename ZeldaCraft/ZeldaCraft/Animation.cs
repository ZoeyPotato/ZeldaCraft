using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//***************************************************************************
// This class is responsible for animating things in the game. Things like
// entities (both static and dynamic) or even something like a loading bar,
// should all be able to use this class to animate themselves. 

// Assuming that any texture(s) you want to animate is a sprite sheet, where
// the rows in the sprite sheet correspond to a series of frames to animate
// linerally. The sheet should be pixel perfect, otherwise there will be issues.

namespace ZeldaCraft
{
    public class Animation
    {
        public Texture2D AnimationSheet { get; private set; }
        public Dictionary<string, Rectangle[]> Animations { get; private set; }        

        private int largestRowSize;
        private int totalRows;
        private int frameWidth;
        private int frameHeight;
        public int CurFrame;

        private float timeToWait;  
        private float timeElapsed;             
        public bool IsLooping = false;


        public Animation(Texture2D newSpriteSheet, int totalRowsInSheet, int largestRowLength)
        {
            AnimationSheet = newSpriteSheet;
            Animations = new Dictionary<string, Rectangle[]>();

            largestRowSize = largestRowLength;
            totalRows = totalRowsInSheet;

            frameWidth = AnimationSheet.Width / largestRowSize;
            frameHeight = AnimationSheet.Height / totalRows;          

            timeToWait = 1 / 10;   // default animations to .1 time per frame            
            IsLooping = true;              
        }

        // need a simple add animation function, to add an animation one at a time.

        // ----------------------------------------------------------------------------
        // This function will be used to create the movement animations for entities.
        // 
        // Builds rectangles around frames in the sprite sheet, one single row at a time.
        // Populates these row of rectangles into a dictionary for easy animation finding.
        // 
        // Assuming that the entity sheets first four rows are directions, with the
        // following order: down, up, left, right.              
        public void AddDefaultEntityAnimations()
        {
            timeToWait = 25;   // default entity movement to animate every 25 pixels

            int i = 0;
            while (i < totalRows)
            {                
                Rectangle[] AnimationRect = new Rectangle[largestRowSize];

                // this loop will need some checking for when the current sheets 
                // row has less frames than largestRowSize (otherwise we will 
                // fill the that rows current rect array with 'blank' frames)
                for (int j = 0; j < largestRowSize; j++)            
                    AnimationRect[j] = new Rectangle(j * frameWidth, i * frameHeight,
                                                     frameWidth, frameHeight);

                if (i == 0)
                    Animations.Add("down", AnimationRect);

                if (i == 1)
                    Animations.Add("up", AnimationRect);

                if (i == 2)
                    Animations.Add("left", AnimationRect);

                if (i == 3)
                    Animations.Add("right", AnimationRect);
                
                i++;
            }            
        }


        public void Update(GameTime gameTime, String animationName)
        {
            timeElapsed = timeElapsed + (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed >= timeToWait)
            {
                if (CurFrame < Animations[animationName].Length)
                    CurFrame++;
                if (CurFrame >= Animations[animationName].Length && IsLooping == true)
                    CurFrame = 0;

                timeElapsed = 0;
            }
        }

        public void EntityMovementUpdate(float speed, String direction)
        {
            timeElapsed = timeElapsed + speed;
            
            if (timeElapsed >= timeToWait)
            {
                if (CurFrame < Animations[direction].Length)
                    CurFrame++;
                if (CurFrame >= Animations[direction].Length && IsLooping == true)
                    CurFrame = 0;

                timeElapsed = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 curEntityPos, String direction)
        {
            spriteBatch.Draw(AnimationSheet, curEntityPos,
                             Animations[direction][CurFrame], Color.White);
        }


        public void ChangeAnimationFPS(int newAnimationFPS)
        {
            timeToWait = 1 / newAnimationFPS;
        }       
    }
}
