using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

//***************************************************************************
// This class is responsible for creating levels for the game. Levels include
// information such as what tiles to draw and where, which tiles are collidable,
// and other useful information pertaining to a 'level'

namespace ZeldaCraft
{
    static class Level
    {
        public static Map LevelMap { get; set; }             

        
	    public static void SetLevel(Map curMap)
	    {
            LevelMap = curMap;   	        
        }       
        
            
        public static void Draw(SpriteBatch spriteBatch, Rectangle camRect)
        {
            LevelMap.Draw(spriteBatch, camRect);
        }


        // ----------------------------------------------------------------------------
        // Collision checking method:            
        // If the rect overlaps a tile with the property 'collide', return true.
        public static bool CollisionCheck(Rectangle comparingRect)
        {
            // go through each tiledata in the entities rect
            foreach (TileData curTileData in LevelMap.GetTilesInRegion(comparingRect))
            {
                // if any of the tiles have the property 'collide', return true
                if (LevelMap.SourceTiles[curTileData.SourceID].Properties.ContainsKey("collide"))
                    return true;
            }

            return false;
        }  
    }    
}
