using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//***************************************************************************
// This class is responsible for defining what a 'mob' is. A mob or 'mobile'
// is an artificial character that defines an enemy in the game.

namespace ZeldaCraft
{
    public class Mob : Entity
    {
        private Player PlayerToKill;
        private int AggroDistance;        

        private float meleeAttackCD;
        private float meleeAttackTimer;    


        public Mob(Vector2 initPos, Player inPlayer) : base(initPos)
        {
            Health = 5;
            Damage = 1;
            Speed = 2;

            PlayerToKill = inPlayer;
            AggroDistance = 200;           

            meleeAttackCD = 1;
            meleeAttackTimer = 0;
        }


        public override void Update(GameTime gameTime)
        {
            Movement();

            if (meleeAttackTimer < meleeAttackCD)
                meleeAttackTimer = meleeAttackTimer + (float)gameTime.ElapsedGameTime.TotalSeconds;

            // if we collided with the player and our attack is not on cd, then attack!
            // else check for mob to player collision
            if (HitBox.Intersects(PlayerToKill.HitBox) == true && meleeAttackTimer >= meleeAttackCD)
                meleeAttack();
            else
                EntityToEntityCollision(PlayerToKill);

            DirectionToPlayer();

            base.Update(gameTime);            
        }


        protected override void Movement()
        {
            // if the mob is on cooldown from previously attacking, don't moving.
            if (meleeAttackTimer < meleeAttackCD)
                return;

            //problem here when colliding with a PlayerToKill in the x axis, sometimes the down
            //  or up animation sticks, and the mob keeps animating, improper dir set!?
            if (distBetweenEntities(PlayerToKill) < AggroDistance)
            {       
                if (Position.Y < PlayerToKill.Position.Y)
                {
                    Position.Y = Position.Y + Speed;
                    HasMoved = true;
                }
                if (Position.Y > PlayerToKill.Position.Y)
                {
                    Position.Y = Position.Y - Speed;
                    HasMoved = true;
                }  

                if (Position.Y != HitBox.Y)   //check if Y actually changed values
                    EntityToLevelCollision();   //mobs rect will be updated here  


                if (Position.X > PlayerToKill.Position.X)
                {
                    Position.X = Position.X - Speed;
                    HasMoved = true;
                }
                if (Position.X < PlayerToKill.Position.X)
                {
                    Position.X = Position.X + Speed;
                    HasMoved = true;
                } 

                if (Position.X != HitBox.X)   //check if Y actually changed values
                    EntityToLevelCollision();   // mobs rect will be updated here                
            }            
        }
        

        // ----------------------------------------------------------------------------
        // Sets the appropriate direction for the mob to face while chasing the player.
        private void DirectionToPlayer()
        {
            float diffInX = Math.Abs(Position.X - PlayerToKill.Position.X);
            float diffInY = Math.Abs(Position.Y - PlayerToKill.Position.Y);

            if (Position.Y < PlayerToKill.Position.Y && diffInY > diffInX)
                Direction = "down";
            if (Position.Y > PlayerToKill.Position.Y && diffInY > diffInX)
                Direction = "up";

            if (Position.X > PlayerToKill.Position.X && diffInX > diffInY)
                Direction = "left";
            if (Position.X < PlayerToKill.Position.X && diffInX > diffInY)
                Direction = "right";
        }


        // ----------------------------------------------------------------------------
        // Performs the melee attack for the mob. Resets the timer, causes the player
        // to take damage and knocks the player back. The knockback direction is
        // determined by the current direction the mob is facing to the player.
        private void meleeAttack()
        {            
            meleeAttackTimer = 0;
            PlayerToKill.Health = PlayerToKill.Health - 1;
            
            Knockback(PlayerToKill, Direction);

            HasMoved = false;
        }        
    }
}
