using Microsoft.Xna.Framework.Audio;

using ShadowMadness;

namespace CGPLibrary.Sound
{
    /// <summary>
    /// SoundEffectInfo stores information on each sound effect used within the game
    /// </summary>
    public class SoundEffectInfo
    {
        protected string effectName;
        protected string effectFileName;
        protected SoundEffectInstance soundEffectInstance;
        protected SoundEffect soundEffect;
        protected float volume, pitch, pan;
        protected bool loop;

        #region PROPERTIES
        public string NAME
        {
            get
            {
                return effectName;
            }
            set
            {
                effectName = value;
            }
        }
        public float VOLUME
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
            }
        }
        public float PITCH
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = value;
            }
        }
        public float PAN
        {
            get
            {
                return pan;
            }
            set
            {
                pan = value;
            }
        }
        public bool LOOP
        {
            get
            {
                return loop;
            }
            set
            {
                loop = value;
            }
        }
        public SoundEffectInstance SOUNDEFFECTINSTANCE
        {
            get
            {
                return soundEffectInstance;
            }
        }
        public SoundEffect SOUNDEFFECT
        {
            get
            {
                return soundEffect;
            }
        }
        #endregion

        public SoundEffectInfo(Main game, string effectName,
            string effectFileName, float volume, float pitch, float pan, bool loop)
        {
            set(game, effectName, effectFileName, volume, pitch, pan, loop);
        }

        public void set(Main game, string effectName,
            string effectFileName, float volume, float pitch, float pan, bool loop)
        {
            this.soundEffect = game.Content.Load<SoundEffect>(@"" + effectFileName);
            this.effectFileName = effectFileName;
            this.effectName = effectName;
            this.volume = volume; this.pitch = pitch; this.pan = pan;
            this.loop = loop;
        }
    }

}
