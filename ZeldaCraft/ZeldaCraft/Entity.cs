using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//***************************************************************************
// This class serves as a base class for all 'entities' in the game.
// From wikipedia, an 'entity' is something that exists by itself, although it 
//      need not be of material existence.
// So, things like a player and a mob are considered entities. 
// This class defines attributes, characterisitics, and functionalities that all
// entities must have.

namespace ZeldaCraft
{
    public class Entity
    {
        public Vector2 Position;               
        
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Rectangle HitBox { get; private set; }

        public Animation MovingAni { get; private set; }
        public String Direction { get; set; }
        public bool HasMoved { get; set; }

        public int Health { get; set; }
        public int Damage { get; set; }
        public float Speed { get; set; }        
        
        private String CurState;
        private bool IsAlive;


        public Entity(Vector2 initPos)
        {
            Position = initPos;

            Direction = "down"; HasMoved = false;
            
            CurState = "default";
            IsAlive = true;
        }


        public virtual void Update(GameTime gameTime)
        {
            if (HasMoved == true)
                MovingAni.EntityMovementUpdate(Speed, Direction);

            HasMoved = false;
        }

        public virtual void Update(GameTime gameTime, Player Player)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MovingAni.Draw(spriteBatch, Position, Direction);            
        }


        public void ChangeSheet(Texture2D newSpriteSheet, int totalRowsInSheet, int largestRowLength)
        {
            MovingAni = new Animation(newSpriteSheet, totalRowsInSheet, largestRowLength);
            MovingAni.AddDefaultEntityAnimations();

            Width = newSpriteSheet.Width / largestRowLength;
            Height = newSpriteSheet.Height / totalRowsInSheet;

            HitBox = new Rectangle((int)Position.X, (int)Position.Y,
                                       Width, Height); 
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
        // Makes a new rect from current Position, sees if that rect is colliding. 
        // If so set Position back to the old Pos (using old rect). 
        // Assuming this is the last check before finalizing an entities axis position
        // Updates the entities rect here and only here
        public void EntityToLevelCollision()
        {          
            int collisionWidth = Width;
            int collisionHeight = Height;

            // if entity w or h are equal to tile size, decrement by one. This will
            // allow the entity to fit into tile gaps equal to its w or h.
            if (Width == Level.LevelMap.TileWidth)
                collisionWidth = Width - 1;           
            if (Height == Level.LevelMap.TileHeight)
                collisionHeight = Height - 1;

            Rectangle entityMoved = new Rectangle((int)Position.X, (int)Position.Y, 
                                                  collisionWidth, collisionHeight);

            if (Level.CollisionCheck(entityMoved) == true)
            {
                Position.X = HitBox.X;   //set Pos back to old Pos
                Position.Y = HitBox.Y;            
            }

            HitBox = new Rectangle((int)Position.X, (int)Position.Y,
                                       Width, Height); 
        }

        // ----------------------------------------------------------------------------
        // Handles collisions for entities to entities: 
        // Makes a new rect from current Position, sees if that rect is colliding with
        //      the other entity. If so, set Position back to the old pos (using old rect) 
        public bool EntityToEntityCollision(Entity entityToCheck)
        {
            Rectangle entityMoved = new Rectangle((int)Position.X, (int)Position.Y,
                                                  Width, Height);        
        
            if (entityMoved.Intersects(entityToCheck.HitBox) == true)
            {
                Position.X = HitBox.X;   //set Pos back to old Pos
                Position.Y = HitBox.Y;

                HasMoved = false;
                return true;
            }

            return false;
        }


        #region HelperMethods

        // ----------------------------------------------------------------------------
        // Returns the distance between the entity and passed entity.        
        public double distBetweenEntities(Entity passedEntity)
        {
            double distance;

            distance = Math.Sqrt(Math.Pow((passedEntity.Position.X - Position.X), 2) +
                Math.Pow((passedEntity.Position.Y - Position.Y), 2));

            return distance;
        }

        #endregion
    }
}
