using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//***************************************************************************
// This class serves as a base class for all 'entities' in the game.
// From wikipedia, an 'entity' is something that exists by itself, although it 
// need not be of material existence.
// So, things like a player and a mob are considered entities. 
// This class defines attributes, characterisitics, and functionalities that all
// entities must have.

namespace ZeldaCraft
{
    public abstract class Entity
    {
        public Vector2 Position;         
        
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Rectangle HitBox { get; private set; }

        protected String Direction { get; set; }
        protected bool HasMoved { get; set; }
        protected float Speed { get; set; }
        protected Animation movingAni;
        
        public int Health { get; set; }
        protected int Damage { get; set; }
       
        protected int meleeAttackRange { get; set; }
        protected Animation meleeAttackAni;

        protected String CombatState;
        private bool isAlive;


        public Entity(Vector2 initPos)
        {
            Position = initPos;

            Direction = "down"; 
            HasMoved = false;            

            CombatState = "default";
            isAlive = true;
        }


        public virtual void Update(GameTime gameTime)
        {
            if (HasMoved == true)
                movingAni.AnimateMovement(Speed, Direction);

            HasMoved = false;   // reset back to false for next update
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            movingAni.DrawAnimation(spriteBatch, Position, Direction);            
        }


        // ----------------------------------------------------------------------------
        // Movement interface for all entities. All entities must define how to move.
        protected abstract void Movement();        


        // ----------------------------------------------------------------------------
        // Wrapper for passing only a character texture: 
        // Defaults totalRowsInSheet to 4, defaults imagesPerRowInSheet to 2. 
        public void ChangeCharacterSheet(Texture2D characterSheet)
        {
            ChangeCharacterSheet(characterSheet, 4, 2);
        }

        // ----------------------------------------------------------------------------
        // This method sets the moving animation, width, height, and hitbox of the 
        // entity from the given spritesheet.
        public void ChangeCharacterSheet(Texture2D characterSheet, int totalRowsInSheet, int imagesPerRowInSheet)
        {
            movingAni = new Animation(characterSheet, totalRowsInSheet, imagesPerRowInSheet);
            movingAni.CreateDirectionalAnimations(25);   // default entity movement to animate every 25 pixels

            Width = characterSheet.Width / imagesPerRowInSheet;
            Height = characterSheet.Height / totalRowsInSheet;

            // if w or h are equal to tile size, decrement by one. This will
            // allow the entity to fit into tile gaps equal to its w or h.
            if (Width == Level.LevelMap.TileWidth)
                Width = Width - 1;
            if (Height == Level.LevelMap.TileHeight)
                Height = Height - 1;

            HitBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height); 
        }

        // ----------------------------------------------------------------------------
        // Wrapper for passing only a melee attack texture: 
        // Defaults totalRowsInSheet to 4, defaults imagesPerRowInSheet to 1. 
        public void ChangeMeleeAttackSheet(Texture2D meleeAttackSheet)
        {
            ChangeMeleeAttackSheet(meleeAttackSheet, 4, 1);
        }

        // ----------------------------------------------------------------------------
        // This method sets the attack animation of the entity from the given spritesheet.
        // Changes the meleeAttackRange based on the difference in sprite widths.
        public void ChangeMeleeAttackSheet(Texture2D meleeAttackSheet, int totalRowsInSheet, int imagesPerRowInSheet)
        {
            meleeAttackAni = new Animation(meleeAttackSheet, totalRowsInSheet, imagesPerRowInSheet);
            meleeAttackAni.CreateDirectionalAnimations(0);

            // use the height of a sprite in this attack sheet to calculate attack range
            // should be able to use w or h, since attack sprites should be squares.
            int heightOfSprite = meleeAttackSheet.Height / totalRowsInSheet;
            meleeAttackRange = heightOfSprite - Height;
        }


        // ----------------------------------------------------------------------------
        // Handles level collisions for entities. Makes a new box from current Position, 
        // sees if that box is colliding. If so set Position back to the old Pos (using old box).
        protected void EntityToLevelCollision()
        {
            Rectangle deltaBox = new Rectangle((int)Position.X, (int)Position.Y, 
                                               Width, Height);

            if (Level.CollisionCheck(deltaBox) == true)
            {
                Position.X = HitBox.X;   //set Pos back to old Pos
                Position.Y = HitBox.Y;   
            }

            HitBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        // ----------------------------------------------------------------------------
        // Handles collisions for entities to entities. Makes a new rect from current 
        // Position, sees if that rect is colliding with the other entity. If so, set 
        // Position back to the old pos (using old rect) 
        protected void EntityToEntityCollision(Entity entityToCheck)
        {
            // build a rectangle from the current entity's pos. This rect should be
            // overlapping the entityToCheck's current hitbox.
            Rectangle deltaBox = new Rectangle((int)Position.X, (int)Position.Y,
                                               Width, Height);

            if (deltaBox.Intersects(entityToCheck.HitBox) == true)
            {
                Position.X = HitBox.X;   //set Pos back to old Pos
                Position.Y = HitBox.Y;

                HasMoved = false;            
            }
        }


        #region HelperMethods

        // ----------------------------------------------------------------------------
        // Returns the distance between the entity and passed entity.        
        protected double distBetweenEntities(Entity passedEntity)
        {
            double distance;

            distance = Math.Sqrt(Math.Pow((passedEntity.Position.X - Position.X), 2) +
                Math.Pow((passedEntity.Position.Y - Position.Y), 2));

            return distance;
        }

        // ----------------------------------------------------------------------------
        // 'Knocks' the passed entity back a few pixels, determined by the direction.
        // defaults the distance to knock back by 15 pixels.
        protected void Knockback(Entity entityToKnockback, string direction, int distance)
        {
            if (direction == "up")
                entityToKnockback.Position.Y = entityToKnockback.Position.Y - distance;

            if (direction == "down")
                entityToKnockback.Position.Y = entityToKnockback.Position.Y + distance;

            if (direction == "left")
                entityToKnockback.Position.X = entityToKnockback.Position.X - distance;

            if (direction == "right")
                entityToKnockback.Position.X = entityToKnockback.Position.X + distance;
        }

        #endregion
    }
}
