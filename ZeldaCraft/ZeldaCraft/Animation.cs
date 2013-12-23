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
        private Texture2D spriteSheet;
        private Dictionary<string, Rectangle[]> animations;

        private int imagesPerRow;
        private int totalRows;
        
        private int frameWidth;
        private int frameHeight;
              
        private float timeToWait;  
        private float timeElapsed;

        private int curFrame;
        public bool IsLooping = false;


        public Animation(Texture2D newSpriteSheet, int totalRowsInSheet, int imagesPerRowInSheet)
        {
            spriteSheet = newSpriteSheet;
            animations = new Dictionary<string, Rectangle[]>();

            imagesPerRow = imagesPerRowInSheet;
            totalRows = totalRowsInSheet;

            frameWidth = spriteSheet.Width / imagesPerRow;
            frameHeight = spriteSheet.Height / totalRows;

            timeToWait = 1 / 10;   // default animations to .1 time per frame 
            timeElapsed = 0;

            curFrame = 0;
            IsLooping = true;              
        }


        // need a simple add animation function, to add an animation one at a time.

        // ----------------------------------------------------------------------------
        // This function will be used to create the 4-directional animations for entities.        
        // Builds rectangles around frames in the sprite sheet, one single row at a time.
        // Populates these row of rectangles into a dictionary for easy animation finding.
        // Assuming that the entity sheets first four rows correspond to directions, with 
        // the following order: down, up, left, right.
        public void CreateDirectionalAnimations(float timeBetweenFrames)
        {
            timeToWait = timeBetweenFrames;

            int i = 0;
            while (i < totalRows)
            {                
                Rectangle[] AnimationRect = new Rectangle[imagesPerRow];

                // this loop will need checking for when the current row has less frames than 
                // expected imagesPerRow. Otherwise that rows rect array will have empty frames.
                for (int j = 0; j < imagesPerRow; j++)
                    AnimationRect[j] = new Rectangle(j * frameWidth, i * frameHeight,
                                                     frameWidth, frameHeight);                
                
                if (i == 0)
                    animations.Add("down", AnimationRect);
                if (i == 1)
                    animations.Add("up", AnimationRect);
                if (i == 2)
                    animations.Add("left", AnimationRect);
                if (i == 3)
                    animations.Add("right", AnimationRect);
                
                i++;
            }
        }


        public void AnimateStatic(GameTime gameTime, String animationName)
        {
            timeElapsed = timeElapsed + (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed >= timeToWait)
            {
                if (curFrame < animations[animationName].Length)
                    curFrame++;
                if (curFrame >= animations[animationName].Length && IsLooping == true)
                    curFrame = 0;

                timeElapsed = 0;
            }
        }

        public void AnimateMovement(float speed, String direction)
        {
            timeElapsed = timeElapsed + speed;
            
            if (timeElapsed >= timeToWait)
            {
                if (curFrame < animations[direction].Length)
                    curFrame++;
                if (curFrame >= animations[direction].Length && IsLooping == true)
                    curFrame = 0;

                timeElapsed = 0;
            }
        }


        public void DrawAnimation(SpriteBatch spriteBatch, Vector2 curEntityPos, String direction)
        {
            spriteBatch.Draw(spriteSheet, curEntityPos,
                             animations[direction][curFrame], Color.White);
        }       


        public void ChangeAnimationFPS(int newAnimationFPS)
        {
            timeToWait = 1 / newAnimationFPS;
        }       
    }
}
