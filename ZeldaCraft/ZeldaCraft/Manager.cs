//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

////***************************************************************************
//// Logistical and utility class for the game. Holds important data structs
//// and utlity functionality, incuding debugging tools, are defined here.

//// Should probably be a singleton. Will come back to this later.

//namespace ZeldaCraft
//{
//    class Manager
//    {
//        public Player Player { get; private set; }
//        public List<Mob> Mobs { get; private set; }

//        public Manager()
//        {
//            Mobs = new List<Mob>();
//        }


//        public void InitializeEntities()
//        {
//            Player = new Player(new Vector2(640, 360));

//            // creating mobs will be an algorithm later, instead single instance
//            Mob mob = new Mob(new Vector2(2100, 2100), Player);
//            Mobs = new List<Mob>();
//            Mobs.Add(mob);

//            Player.Mobs = Mobs;
//        }
//    }
//}
