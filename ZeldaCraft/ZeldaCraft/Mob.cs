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
        public List<Mob> MobList { get; set; }

        private float meleeAttackCD;
        private float meleeAttackTimer;  


        public Mob(Vector2 initPos) : base(initPos)
        {
            EntityHealth = 5;   // setting defaults for a mob
            EntityDamage = 1;
            EntitySpeed = 2;

            meleeAttackCD = 2;
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
                if (EntityPos.Y < player.EntityPos.Y)
                {
                    EntityPos.Y = EntityPos.Y + EntitySpeed;
                    EntityMoved = true;
                }
                if (EntityPos.Y > player.EntityPos.Y)
                {
                    EntityPos.Y = EntityPos.Y - EntitySpeed;
                    EntityMoved = true;
                }

                bool collideWithPlayer = EntityToEntityCollision(player);   //check for mob to player collisions

                if (EntityPos.Y != EntityRect.Y)   //check if Y actually changed values
                    EntityToLevelCollision();   //mobs rect will be updated here  


                if (EntityPos.X > player.EntityPos.X)
                {
                    EntityPos.X = EntityPos.X - EntitySpeed;
                    EntityMoved = true;
                }
                if (EntityPos.X < player.EntityPos.X)
                {
                    EntityPos.X = EntityPos.X + EntitySpeed;
                    EntityMoved = true;
                }

                collideWithPlayer = EntityToEntityCollision(player);   //next three lines same as above, for Y

                if (EntityPos.X != EntityRect.X)
                    EntityToLevelCollision();                             


                mobDirection(player);   //set the appropriate dir for animation

                if (collideWithPlayer == true && meleeAttackTimer >= meleeAttackCD)                
                    meleeAttack(player);                                                
            }                            
        }

        private void mobDirection(Player player)
        {
            float diffInX = Math.Abs(EntityPos.X - player.EntityPos.X);
            float diffInY = Math.Abs(EntityPos.Y - player.EntityPos.Y);

            if (EntityPos.Y < player.EntityPos.Y && diffInY > diffInX)
                EntityDir = "down";
            if (EntityPos.Y > player.EntityPos.Y && diffInY > diffInX)
                EntityDir = "up"; 

            if (EntityPos.X > player.EntityPos.X && diffInX > diffInY)            
                EntityDir = "left";
            if (EntityPos.X < player.EntityPos.X && diffInX > diffInY)
                EntityDir = "right";                        
        }


        private void meleeAttack(Player player)
        {
            player.EntityHealth = player.EntityHealth - 1;

            // determine which side of the player we hit, then move player back in opposite direction about 20px?
        }
    }
}
