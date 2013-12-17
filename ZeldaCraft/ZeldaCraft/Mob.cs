using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//***************************************************************************
// This class is responsible for defining what a 'mob' is. WIP

namespace ZeldaCraft
{
    public class Mob : Entity
    {
        private Player PlayerToKill;

        private float meleeAttackCD;
        private float meleeAttackTimer;        


        public Mob(Vector2 initPos, Player inPlayer) : base(initPos)
        {
            Health = 5;
            Damage = 1;
            Speed = 2;

            PlayerToKill = inPlayer;

            meleeAttackCD = 1;
            meleeAttackTimer = 0;
        }


        public override void Update(GameTime gameTime)
        {
            if (meleeAttackTimer < meleeAttackCD)
                meleeAttackTimer = meleeAttackTimer + (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);            
        }


        protected override void Movement()
        {//problem here when colliding with a PlayerToKill in the x axis, sometimes the down
            //  or up animation sticks, and the mob keeps animating, improper dir set!?
            if (distBetweenEntities(PlayerToKill) < 200)
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

                bool collideWithPlayer = EntityToEntityCollision(PlayerToKill);   //check for mob to PlayerToKill collisions

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

                collideWithPlayer = EntityToEntityCollision(PlayerToKill);   //next three lines same as above, for Y

                if (Position.X != HitBox.X)
                    EntityToLevelCollision();                             


                mobDirection(PlayerToKill);   //set the appropriate dir for animation

                if (collideWithPlayer == true && meleeAttackTimer >= meleeAttackCD)                
                    meleeAttack(PlayerToKill);                                                
            }                            
        }

        private void mobDirection(Player PlayerToKill)
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


        private void meleeAttack(Player PlayerToKill)
        {
            meleeAttackTimer = 0;
            PlayerToKill.Health = PlayerToKill.Health - 1;
            

        }
    }
}
