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
        public List<Mob> Mobs { private get; set; }

        private float meleeAttackCD;
        private float meleeAttackTimer;


        public Player(Vector2 initPos) : base(initPos)
        {
            Speed = 3;
            Health = 10;
            Damage = 1;

            meleeAttackCD = (float).5;
            meleeAttackTimer = (float).5;
        }


        public override void Update(GameTime gameTime)
        {
            Movement();

            if (meleeAttackTimer < meleeAttackCD)
                meleeAttackTimer = meleeAttackTimer + (float)gameTime.ElapsedGameTime.TotalSeconds; 

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                meleeAttack();

            base.Update(gameTime);      
        }


        // ----------------------------------------------------------------------------
        // Handles movement for player: 
        // Moves each axis seperately and checks for collision one axis at a time.
        protected override void Movement()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Position.Y = Position.Y + Speed;
                Direction = "down"; HasMoved = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position.Y = Position.Y - Speed;
                Direction = "up"; HasMoved = true;
            }

            EntityToEntityCollision(Mobs[0]);

            if (Position.Y != HitBox.Y)   //check if y actually changed values
                EntityToLevelCollision();


            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Position.X = Position.X - Speed;
                Direction = "left"; HasMoved = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Position.X = Position.X + Speed;
                Direction = "right"; HasMoved = true;
            }

            EntityToEntityCollision(Mobs[0]);

            if (Position.X != HitBox.X)   //check if x actually changed values
                EntityToLevelCollision();
        }


        private void meleeAttack()
        {
            if (meleeAttackTimer >= meleeAttackCD)
            {
                meleeAttackTimer = 0;

                //meleeAttackAni.Draw(Direction);   will finish...

                // currently do a simple iteration through all mobs
                for (int i = 0; i < Mobs.Count; i++)
                {
                    Mobs[i].Health = Mobs[i].Health - 1;
                    Knockback(Mobs[i], Direction);
                }
            }
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
                    Position.X = curTileData.Target.X - 16;   // 'target' is set to the middle
                    Position.Y = curTileData.Target.Y - 16;   //   of the tile, not top left
                }
                else
                {
                    Position.X = curMap.Bounds.Width / 2;
                    Position.Y = curMap.Bounds.Height / 2;
                }
            }                
        }
    }
}
