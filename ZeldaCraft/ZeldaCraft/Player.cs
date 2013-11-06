using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//***************************************************************************
// This class is responsible for defining what a 'player' is. A player has 
// attributes such as, health, damage, speed, etc. WIP

namespace ZeldaCraft
{
    public class Player : Entity
    {                  
        public Player(Vector2 initPos) : base(initPos)
        {                        
            EntityHealth = 10;   // setting defaults for a player
            EntityDamage = 1;
            EntitySpeed = 3;                 
        }


        public override void Update(GameTime gameTime)
        {
            playerMovement();                        
            
            base.Update(gameTime);      
        }

        // ----------------------------------------------------------------------------
        // Handles movement for player: 
        // Moves each axis seperately and checks for collision one axis at a time.
        private void playerMovement()
        {            
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                EntityPos.X = EntityPos.X - EntitySpeed;
                EntityDir = "left"; EntityMoved = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                EntityPos.X = EntityPos.X + EntitySpeed;
                EntityDir = "right"; EntityMoved = true;
            }
            
            if (EntityPos.X != EntityRect.X)   //check if x actually changed values
                EntityToLevelCollision();


            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                EntityPos.Y = EntityPos.Y - EntitySpeed;
                EntityDir = "up"; EntityMoved = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                EntityPos.Y = EntityPos.Y + EntitySpeed;
                EntityDir = "down"; EntityMoved = true;
            }

            if (EntityPos.Y != EntityRect.Y)   //check if y actually changed values
                EntityToLevelCollision();
        }        


        // ----------------------------------------------------------------------------
        // Sets the players spawn position to the 'diamondBlock' in the map. If the 
        // block does not exist, will spawn the player in the middle of the map.
        public void SetSpawn()
        {            
            Map curMap = Level.LevelMap;

            // need to decrement the bound's width/height, otherwise a fucking 
            // out of range exception is thrown by GetTilesInRegion (bullshit)
            int boundsWidth = curMap.Bounds.Width - 1;
            int boundsHeight = curMap.Bounds.Height - 1;
            Rectangle curBounds = new Rectangle(0, 0, boundsWidth, boundsHeight);

            // go through each 'placed' tile in the current Map
            foreach (TileData curTileData in curMap.GetTilesInRegion(curBounds))
            {
                // if we find a tile with property 'spawn', set player's Pos there
                if (curMap.SourceTiles[curTileData.SourceID].Properties.ContainsKey("spawn"))
                {
                    EntityPos.X = curTileData.Target.X - 16;   // 'target' is set to the middle
                    EntityPos.Y = curTileData.Target.Y - 16;   //   of the tile, not top left
                }
                else
                {
                    EntityPos.X = curMap.Bounds.Width / 2;
                    EntityPos.Y = curMap.Bounds.Height / 2;
                }
            }                
        }
    }
}
