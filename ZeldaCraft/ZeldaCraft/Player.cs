using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//***************************************************************************
// This class is responsible for defining what a 'player' is. A player is an 
// entity controlled by a game player. The player controls a sprite through
// keyboard inputs. Any functionalities relating to the notion of 'player'
// are found here. This includes things like attacking, moving, etc. 

namespace ZeldaCraft
{
    public class Player : Entity
    {
        public List<Mob> Mobs { private get; set; }

        private float meleeAttackCD;
        private float meleeAttackTimer;
        private float durationToDrawAttack;


        public Player(Vector2 initPos) : base(initPos)
        {
            Speed = 3;
            Health = 10;
            Damage = 1;

            meleeAttackCD = (float).3;
            meleeAttackTimer = (float).3;
            durationToDrawAttack = (float).15;
        }


        public override void Update(GameTime gameTime)
        {
            Movement();

            if (meleeAttackTimer < meleeAttackCD)
                meleeAttackTimer = meleeAttackTimer + (float)gameTime.ElapsedGameTime.TotalSeconds;

            meleeAttack();            

            base.Update(gameTime);      
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (CombatState == "meleeAttack")
            {
                Vector2 adjustedAttackPos = Position;

                // these checks will adjust the up and left pos to draw the 
                // attack animation at the correct pos
                if (Direction == "up")
                    adjustedAttackPos.Y = Position.Y - meleeAttackRange;
                if (Direction == "left")
                    adjustedAttackPos.X = Position.X - meleeAttackRange;

                meleeAttackAni.DrawAnimation(spriteBatch, adjustedAttackPos, Direction);
            }
            else
                base.Draw(spriteBatch);
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


        // ----------------------------------------------------------------------------
        // Performs the melee attack for the player. If space is pressed and the attack
        // is not on cd, then a check occurs for every mob to see if a mob is in range
        // for the attack. If so, the mob takes damage and is knockbacked the direction
        // the player is facing/attacked.
        private void meleeAttack()
        {
            // this check will stop drawing the attack if it is over its time to draw
            if (meleeAttackTimer >= durationToDrawAttack)
                CombatState = "default";

            // if space is pressed and the meleeattack is not on cd, then attack!
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && meleeAttackTimer >= meleeAttackCD)
            {
                meleeAttackTimer = 0;
                CombatState = "meleeAttack";

                Rectangle meleeAttackBox = createMeleeAttackBox(Direction);

                // currently do a simple iteration through all mobs
                // this should later only check mobs in a close vincinity.
                for (int i = 0; i < Mobs.Count; i++)
                {
                    if (meleeAttackBox.Intersects(Mobs[i].HitBox) == true)
                    {
                        Mobs[i].Health = Mobs[i].Health - 1;
                        Knockback(Mobs[i], Direction, 15);

                        Sound.MobHurt.Play();
                    }    
                }

                Sound.NormalSwordAttack.Play();
            }
        }

        // ----------------------------------------------------------------------------
        // Helper method for meleeAttack, creates the appropriate attack box for hit
        // detection based on the direction attacked.
        private Rectangle createMeleeAttackBox(String curDirection)
        {
            Rectangle attackBox;

            if (curDirection == "up")
            {
                attackBox = new Rectangle((int)Position.X, (int)Position.Y - meleeAttackRange,
                                          Width, meleeAttackRange);

                return attackBox;
            }
            if (curDirection == "down")
            {
                attackBox = new Rectangle((int)Position.X, (int)Position.Y + Height,
                                          Width, meleeAttackRange);

                return attackBox;
            }
            if (curDirection == "left")
            {
                attackBox = new Rectangle((int)Position.X - meleeAttackRange, (int)Position.Y,
                                          meleeAttackRange, Height);

                return attackBox;
            }
            if (curDirection == "right")
            {
                attackBox = new Rectangle((int)Position.X + Width, (int)Position.Y,
                                          meleeAttackRange, Height);

                return attackBox;
            }

            return attackBox = new Rectangle();
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
