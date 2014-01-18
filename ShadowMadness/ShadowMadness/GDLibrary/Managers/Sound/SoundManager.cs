using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

using ShadowMadness;

namespace CGPLibrary.Sound
{
    public class SoundManager
    {
        protected Dictionary<string, SoundEffectInfo> soundEffectDictonary;

        public SoundManager()
        {
            soundEffectDictonary = new Dictionary<string, SoundEffectInfo>();
        }

        public bool add(SoundEffectInfo effectInfo)
        {
            if (!soundEffectDictonary.ContainsKey(effectInfo.NAME))
            {
                soundEffectDictonary.Add(effectInfo.NAME, effectInfo);
                return true;
            }

            return false;
        }

        public SoundEffectInfo getEffectInfo(string name)
        {
            return soundEffectDictonary[name];
        }

        public SoundEffectInstance getEffectInstance(string name)
        {
            SoundEffectInfo effectInfo = soundEffectDictonary[name];
            SoundEffectInstance soundEffectInstance = effectInfo.SOUNDEFFECT.CreateInstance();
            soundEffectInstance.Volume = effectInfo.VOLUME;
            soundEffectInstance.Pitch = effectInfo.PITCH;
            soundEffectInstance.Pan = effectInfo.PAN;
            soundEffectInstance.IsLooped = effectInfo.LOOP;
            return soundEffectInstance;
        }

        public bool remove(string name)
        {
            //find effect info so we can nullify for garbage collection
            SoundEffectInfo effectInfo = soundEffectDictonary[name];
            //remove the effect info from dictionary and store return value
            bool wasRemoved = soundEffectDictonary.Remove(name);
            //nullify for garbage collection
            effectInfo = null; 
            return wasRemoved;
        }

        public void clear()
        {
            soundEffectDictonary.Clear();
        }

        public int size()
        {
            return soundEffectDictonary.Count;
        }
    }

    
}
