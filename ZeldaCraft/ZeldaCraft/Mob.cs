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


        public Mob(Vector2 initPos) : base(initPos)
        {
            EntityHealth = 5;   // setting defaults for a mob
            EntityDamage = 1;
            EntitySpeed = 2;
        }


        public override void Update(GameTime gameTime, Entity player)
        {
            mobMovement(player);

            base.Update(gameTime);
        }
        
        private void mobMovement(Entity player)
        {
            if (distBetweenEntities(player) < 200)
            {
                if ((int)player.EntityPos.X < (int)EntityPos.X)
                {
                    EntityPos.X = EntityPos.X - EntitySpeed;
                    EntityDir = "left"; EntityMoved = true;
                }
                if ((int)player.EntityPos.X > (int)EntityPos.X)
                {
                    EntityPos.X = EntityPos.X + EntitySpeed;
                    EntityDir = "right"; EntityMoved = true;
                }

                EntityToEntityCollision(player);   //check for mob to player collisions

                if (EntityPos.X != EntityRect.X)   //check if x actually changed values
                    EntityToLevelCollision();   //mobs rect will be updated here                  
                   
                 
                if ((int)player.EntityPos.Y < (int)EntityPos.Y)
                {
                    EntityPos.Y = EntityPos.Y - EntitySpeed;
                    EntityDir = "up"; EntityMoved = true;
                }
                if ((int)player.EntityPos.Y > (int)EntityPos.Y)
                {
                    EntityPos.Y = EntityPos.Y + EntitySpeed;
                    EntityDir = "down"; EntityMoved = true;
                }

                EntityToEntityCollision(player);  //next three lines same as above, for Y

                if (EntityPos.Y != EntityRect.Y)
                    EntityToLevelCollision();                                
            }                            
        }        
    }
}
