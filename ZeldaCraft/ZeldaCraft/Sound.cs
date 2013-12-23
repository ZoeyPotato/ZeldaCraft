using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//***************************************************************************
// Static sound class for the game. All game soundeffects and other noises
// can be accessed and played globally through this class.

namespace ZeldaCraft
{
    static class Sound
    {
        public static SoundEffect PlayerHurt { get; set; }
        public static SoundEffect MobHurt { get; set; }

        public static SoundEffect NormalSwordAttack { get; set; }


        public static void LoadSounds(ContentManager content)
        {
            PlayerHurt = content.Load<SoundEffect>("Sounds/playerHurt");
            MobHurt = content.Load<SoundEffect>("Sounds/mobHurt");
            NormalSwordAttack = content.Load<SoundEffect>("Sounds/normalSwordAttack");
        }
    }
}
