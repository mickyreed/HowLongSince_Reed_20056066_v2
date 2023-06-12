using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLongSince_Reed_20056066
{
    public class SoundEffects
    {
        static ISimpleAudioPlayer tap = null;
        public static void PlayTap()
        {
            //-- Just in time initialisation
            if (tap == null)
            {
                tap = CrossSimpleAudioPlayer.Current;
                tap.Load("tap.wav");
            }
            //-- Play the sound
            tap.Play(); //explosion.Play();
        }
    }
}
