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


        public override void Update(GameTime gameTime)
        {
            /*
            foreach (mob in mobList)
            {
                mob.mobMovement();
            }
            */

            base.Update(gameTime);
        }

        
        private void mobMovement(Entity passedEntity)
        {
            if (distBetweenEntities(passedEntity) < 200)
            {
                if ((int)passedEntity.EntityPos.X < (int)EntityPos.X)                
                    EntityPos.X = EntityPos.X - EntitySpeed;
                if ((int)passedEntity.EntityPos.X > (int)EntityPos.X)                
                    EntityPos.X = EntityPos.X + EntitySpeed;
                                    
                if ((int)passedEntity.EntityPos.Y < (int)EntityPos.Y)
                    EntityPos.Y = EntityPos.Y - EntitySpeed;
                if ((int)passedEntity.EntityPos.Y > (int)EntityPos.Y)                
                    EntityPos.Y = EntityPos.Y + EntitySpeed;                
            }
        }        
    }
}
