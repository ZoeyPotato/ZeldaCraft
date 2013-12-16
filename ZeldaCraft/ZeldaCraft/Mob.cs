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
        private float meleeAttackCD;
        private float meleeAttackTimer;  


        public Mob(Vector2 initPos) : base(initPos)
        {
            Health = 5;
            Damage = 1;
            Speed = 2;

            meleeAttackCD = 1;
            meleeAttackTimer = 0;
        }


        public override void Update(GameTime gameTime, Player player)
        {
            if (meleeAttackTimer < meleeAttackCD)
                meleeAttackTimer = meleeAttackTimer + (float)gameTime.ElapsedGameTime.TotalSeconds;

            mobMovement(player);

            base.Update(gameTime);            
        }
        

        private void mobMovement(Player player)
        {//problem here when colliding with a player in the x axis, sometimes the down
            //  or up animation sticks, and the mob keeps animating, improper dir set!?
            if (distBetweenEntities(player) < 200)
            {                
                if (Position.Y < player.Position.Y)
                {
                    Position.Y = Position.Y + Speed;
                    HasMoved = true;
                }
                if (Position.Y > player.Position.Y)
                {
                    Position.Y = Position.Y - Speed;
                    HasMoved = true;
                }

                bool collideWithPlayer = EntityToEntityCollision(player);   //check for mob to player collisions

                if (Position.Y != HitBox.Y)   //check if Y actually changed values
                    EntityToLevelCollision();   //mobs rect will be updated here  


                if (Position.X > player.Position.X)
                {
                    Position.X = Position.X - Speed;
                    HasMoved = true;
                }
                if (Position.X < player.Position.X)
                {
                    Position.X = Position.X + Speed;
                    HasMoved = true;
                }

                collideWithPlayer = EntityToEntityCollision(player);   //next three lines same as above, for Y

                if (Position.X != HitBox.X)
                    EntityToLevelCollision();                             


                mobDirection(player);   //set the appropriate dir for animation

                if (collideWithPlayer == true && meleeAttackTimer >= meleeAttackCD)                
                    meleeAttack(player);                                                
            }                            
        }

        private void mobDirection(Player player)
        {
            float diffInX = Math.Abs(Position.X - player.Position.X);
            float diffInY = Math.Abs(Position.Y - player.Position.Y);

            if (Position.Y < player.Position.Y && diffInY > diffInX)
                Direction = "down";
            if (Position.Y > player.Position.Y && diffInY > diffInX)
                Direction = "up"; 

            if (Position.X > player.Position.X && diffInX > diffInY)            
                Direction = "left";
            if (Position.X < player.Position.X && diffInX > diffInY)
                Direction = "right";                        
        }


        private void meleeAttack(Player player)
        {
            meleeAttackTimer = 0;
            player.Health = player.Health - 1;
            

        }
    }
}
