using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code
{
    class Sound
    {
        private SoundEffectInstance effect;

        public float volume = 1.0f;
        public float pitch = 0.0f;
        public float pan = 0.0f;

        public Sound(Game game, String soundFileToLoad)
        {
            SoundEffect sfx = game.Content.Load<SoundEffect>(soundFileToLoad);
            effect = sfx.CreateInstance();
        }

        public void play(bool looped)
        {
            if (effect.State != SoundState.Playing)
            {
                effect.Volume = volume;
                effect.Pitch = pitch;
                effect.Pan = pan;
                if (looped)
                    effect.IsLooped = true;
                effect.Play();
            }
        }

        public void pause()
        {
            if (effect.State == SoundState.Playing)
                effect.Pause();
        }

        public void stop()
        {
            if (effect.State != SoundState.Stopped)
                effect.Stop();
        }
    }
}
