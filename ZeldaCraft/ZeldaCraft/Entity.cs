using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//***************************************************************************
// This class serves as a base class for all 'things' in the game.
// A 'thing' is considered an entity if it needs to have a position in the game,
// animations, and other misc data members.

// Players, mobs, items, can all be derived from entity.

namespace ZeldaCraft
{
    public class Entity
    {
        public Vector2 EntityPos;               
        
        public int EntityWidth { get; private set; }
        public int EntityHeight { get; private set; }
        public Rectangle EntityRect { get; private set; }

        public Animation EntityAnimation { get; private set; }
        public String EntityDir { get; set; }
        public bool EntityMoved { get; set; }

        public int EntityHealth { get; set; }
        public int EntityDamage { get; set; }
        public float EntitySpeed { get; set; }        
        
        private String entityState;
        private bool entityAlive;        


        public Entity(Vector2 initPos)
        {
            EntityPos = initPos;

            EntityDir = "down"; EntityMoved = false;
            
            entityState = "default";
            entityAlive = true;
        }


        public virtual void Update(GameTime gameTime)
        {
            if (EntityMoved == true)
                EntityAnimation.EntityMovementUpdate(EntitySpeed, EntityDir);

            EntityMoved = false;
        }

        public virtual void Update(GameTime gameTime, Player Player)
        {            
            if (EntityMoved == true)
                EntityAnimation.EntityMovementUpdate(EntitySpeed, EntityDir);

            EntityMoved = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            EntityAnimation.Draw(spriteBatch, EntityPos, EntityDir);            
        }


        public void ChangeSheet(Texture2D newSpriteSheet, int totalRowsInSheet, int largestRowLength)
        {
            EntityAnimation = new Animation(newSpriteSheet, totalRowsInSheet, largestRowLength);
            EntityAnimation.AddDefaultEntityAnimations();

            EntityWidth = newSpriteSheet.Width / largestRowLength;
            EntityHeight = newSpriteSheet.Height / totalRowsInSheet;

            EntityRect = new Rectangle((int)EntityPos.X, (int)EntityPos.Y,
                                       EntityWidth, EntityHeight); 
        }

        // ----------------------------------------------------------------------------
        // Wrapper for passing only a texture: 
        // Defaults totalRowsInSheet to '4', defaults largestRowLength to 2. 
        public void ChangeSheet(Texture2D newSpriteSheet)
        {
            ChangeSheet(newSpriteSheet, 4, 2);
        }


        // ----------------------------------------------------------------------------
        // Handles level collisions for entities: 
        // Makes a new rect from current EntityPos, sees if that rect is colliding. 
        // If so set EntityPos back to the old Pos (using old rect). 
        // Assuming this is the last check before finalizing an entities axis position
        // Updates the entities rect here and only here
        public void EntityToLevelCollision()
        {          
            int collisionWidth = EntityWidth;
            int collisionHeight = EntityHeight;

            // if entity w or h are equal to tile size, decrement by one. This will
            // allow the entity to fit into tile gaps equal to its w or h.
            if (EntityWidth == Level.LevelMap.TileWidth)
                collisionWidth = EntityWidth - 1;           
            if (EntityHeight == Level.LevelMap.TileHeight)
                collisionHeight = EntityHeight - 1;

            Rectangle entityMoved = new Rectangle((int)EntityPos.X, (int)EntityPos.Y, 
                                                  collisionWidth, collisionHeight);

            if (Level.CollisionCheck(entityMoved) == true)
            {
                EntityPos.X = EntityRect.X;   //set Pos back to old Pos
                EntityPos.Y = EntityRect.Y;            
            }

            EntityRect = new Rectangle((int)EntityPos.X, (int)EntityPos.Y,
                                       EntityWidth, EntityHeight); 
        }

        // ----------------------------------------------------------------------------
        // Handles collisions for entities to entities: 
        // Makes a new rect from current EntityPos, sees if that rect is colliding with
        //      the other entity. If so, set EntityPos back to the old pos (using old rect) 
        public void EntityToEntityCollision(Entity entityToCheck)
        {
            Rectangle entityMoved = new Rectangle((int)EntityPos.X, (int)EntityPos.Y,
                                                  EntityWidth, EntityHeight);        
        
            if (entityMoved.Intersects(entityToCheck.EntityRect) == true)
            {
                EntityPos.X = EntityRect.X;   //set Pos back to old Pos
                EntityPos.Y = EntityRect.Y;

                EntityMoved = false;
            }              
        }


        #region HelperMethods

        // ----------------------------------------------------------------------------
        // Returns the distance between the entity and passed entity.        
        public double distBetweenEntities(Entity passedEntity)
        {
            double distance;

            distance = Math.Sqrt(Math.Pow((passedEntity.EntityPos.X - EntityPos.X), 2) +
                Math.Pow((passedEntity.EntityPos.Y - EntityPos.Y), 2));

            return distance;
        }

        #endregion
    }
}
