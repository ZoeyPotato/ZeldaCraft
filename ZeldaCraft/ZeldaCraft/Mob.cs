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
        private Player playerToKill;
        private int aggroDistance;        

        private float meleeAttackCD;
        private float meleeAttackTimer;    


        public Mob(Vector2 initPos, Player inPlayer) : base(initPos)
        {
            Health = 5;
            Damage = 1;
            Speed = 2;

            playerToKill = inPlayer;
            aggroDistance = 200;           

            meleeAttackCD = 1;
            meleeAttackTimer = 1;
        }


        public override void Update(GameTime gameTime)
        {
            Movement();

            if (meleeAttackTimer < meleeAttackCD)
                meleeAttackTimer = meleeAttackTimer + (float)gameTime.ElapsedGameTime.TotalSeconds;           

            base.Update(gameTime);            
        }


        protected override void Movement()
        {
            // CURRENT BUG: SOMETIMES THE ANIMATION FOR MOVING UP OR DOWN WILL CONTINUE
            // WHEN THE MOB COLLIDES WITH THE PLAYER FROM THE UP OR DOWN. PROBABLY DUE 
            // TO MOVING IN THE X SLIGHTLY (SINCE WE MOVE X AXIS AFTER Y AXIS) AND SETS 
            // HASMOVED TO TRUE.

            if (distBetweenEntities(playerToKill) < aggroDistance)
            {       
                if (Position.Y < playerToKill.Position.Y)
                {
                    Position.Y = Position.Y + Speed;
                    HasMoved = true;
                }
                if (Position.Y > playerToKill.Position.Y)
                {
                    Position.Y = Position.Y - Speed;
                    HasMoved = true;
                }

                meleeAttack();   // check for an attack before resetting pos from collision                                 

                EntityToEntityCollision(playerToKill);

                if (Position.Y != HitBox.Y)   //check if Y actually changed values
                    EntityToLevelCollision();   //mobs box will be updated here                


                // check here for the small bug.
                if (Position.X > playerToKill.Position.X)
                {
                    Position.X = Position.X - Speed;
                    HasMoved = true;
                }
                if (Position.X < playerToKill.Position.X)
                {
                    Position.X = Position.X + Speed;
                    HasMoved = true;
                }

                meleeAttack();   // check for an attack before resetting pos from collision     

                EntityToEntityCollision(playerToKill);

                if (Position.X != HitBox.X)   //check if X actually changed values
                    EntityToLevelCollision();   // mobs box will be updated here                 


                DirectionToPlayer();
            }
        }
        

        // ----------------------------------------------------------------------------
        // Sets the appropriate direction for the mob to face while chasing the player.
        private void DirectionToPlayer()
        {
            float diffInX = Math.Abs(Position.X - playerToKill.Position.X);
            float diffInY = Math.Abs(Position.Y - playerToKill.Position.Y);

            if (Position.Y < playerToKill.Position.Y && diffInY > diffInX)
                Direction = "down";
            if (Position.Y > playerToKill.Position.Y && diffInY > diffInX)
                Direction = "up";

            if (Position.X > playerToKill.Position.X && diffInX > diffInY)
                Direction = "left";
            if (Position.X < playerToKill.Position.X && diffInX > diffInY)
                Direction = "right";
        }


        // ----------------------------------------------------------------------------
        // Performs the melee attack for the mob. Resets the timer, causes the player
        // to take damage and knocks the player back. The knockback direction is
        // determined by the current direction the mob is facing to the player.
        private void meleeAttack()
        {
            // build a rectangle from the latest mobs's pos.
            Rectangle deltaBox = new Rectangle((int)Position.X, (int)Position.Y,
                                                Width, Height);

            // if we collided with the player and our attack is not on cd, then attack!
            if (deltaBox.Intersects(playerToKill.HitBox) == true && meleeAttackTimer >= meleeAttackCD)
            {
                meleeAttackTimer = 0;
                playerToKill.Health = playerToKill.Health - 1;

                Knockback(playerToKill, Direction);
            }          
        }
    }
}
