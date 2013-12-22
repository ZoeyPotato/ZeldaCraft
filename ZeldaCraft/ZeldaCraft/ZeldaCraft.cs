#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using FuncWorks.XNA.XTiled;
#endregion

//***************************************************************************
// This is the game loop for ZeldaCraft. This is where all the magic happens.

namespace ZeldaCraft
{
    public class ZeldaCraft : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Rectangle mapView;
        private float frameRate;
        
        private SpriteFont defaultFont;

        private Player player;
        private List<Mob> mobs;


        public ZeldaCraft() : base()
        {
            graphics = new GraphicsDeviceManager(this);            
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;            
        }

        
        protected override void Initialize()
        {          
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mapView = graphics.GraphicsDevice.Viewport.Bounds;

            player = new Player(new Vector2(640, 360));

            // creating mobs will be an algorithm later, instead single instance
            Mob mob = new Mob(new Vector2(2100, 2100), player);
            mobs = new List<Mob>();
            mobs.Add(mob);

            player.Mobs = mobs;

            Camera.Initialize(player, mapView);

            base.Initialize(); 
        }
        
        protected override void LoadContent()
        {                                
            Content.RootDirectory = "Content";

            Map defaultMap = Content.Load<Map>("Levels/defaultLevel");
            Level.SetLevel(defaultMap);
            player.SetSpawn();

            Texture2D linkNormalSheet = Content.Load<Texture2D>("Sprites/link/linkNormalSheet");
            player.ChangeCharacterSheet(linkNormalSheet);
            Texture2D linkNormalMeleeAttack = Content.Load<Texture2D>("Sprites/link/linkNormalMeleeAttack");
            player.ChangeMeleeAttackSheet(linkNormalMeleeAttack);

            Texture2D moblinSheet = Content.Load<Texture2D>("Sprites/mobs/moblin/moblinSheet");
            mobs[0].ChangeCharacterSheet(moblinSheet);

            defaultFont = Content.Load<SpriteFont>("defaultFont");
        }                

        
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);
            mobs[0].Update(gameTime);

            Camera.Update(player);                                  

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            spriteBatch.Begin();
            Level.Draw(spriteBatch, Camera.CamRect);                                
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                              null, null, null, null, Camera.TranslateMatrix());            
            player.Draw(spriteBatch);
            mobs[0].Draw(spriteBatch);
            DrawText();   // for debugging
            spriteBatch.End();

            base.Draw(gameTime);
        }


        #region DebugMethods

        // ----------------------------------------------------------------------------
        // Draws useful information to the screen.
        private void DrawText()
        {
            spriteBatch.DrawString(defaultFont, "FrameRate: " + frameRate.ToString(),
                                   new Vector2(Camera.CamPos.X + 20, Camera.CamPos.Y + 0),
                                   Color.White);

            spriteBatch.DrawString(defaultFont, "PlayerPos: " + player.Position.ToString(),
                                   new Vector2(Camera.CamPos.X + 20, Camera.CamPos.Y + 20),
                                   Color.White);
            spriteBatch.DrawString(defaultFont, "PlayerBox: " + player.HitBox.ToString(),
                                   new Vector2(Camera.CamPos.X + 20, Camera.CamPos.Y + 40),
                                   Color.White);
            spriteBatch.DrawString(defaultFont, "PlayerHealth: " + player.Health.ToString(),
                                   new Vector2(Camera.CamPos.X + 20, Camera.CamPos.Y + 60),
                                   Color.White);

            //spriteBatch.DrawString(defaultFont, "CamPos: " + Camera.CamPos.ToString(),
            //                       new Vector2(Camera.CamPos.X + 20, Camera.CamPos.Y + 90),
            //                       Color.White);
            //spriteBatch.DrawString(defaultFont, "CamRect: " + Camera.CamRect.ToString(),
            //                       new Vector2(Camera.CamPos.X + 20, Camera.CamPos.Y + 110),
            //                       Color.White);

            spriteBatch.DrawString(defaultFont, "MobPos: " + mobs[0].Position.ToString(),
                                   new Vector2(Camera.CamPos.X + 20, Camera.CamPos.Y + 90),
                                   Color.White);
            spriteBatch.DrawString(defaultFont, "MobBox: " + mobs[0].HitBox.ToString(),
                                   new Vector2(Camera.CamPos.X + 20, Camera.CamPos.Y + 110),
                                   Color.White);
            spriteBatch.DrawString(defaultFont, "MobHealth: " + mobs[0].Health.ToString(),
                                   new Vector2(Camera.CamPos.X + 20, Camera.CamPos.Y + 130),
                                   Color.White);
        }

        #endregion
    }
}
