using OpenTK.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKPong {
    public class AudioManager {
        private const string Boop_1Path = "Assets/Sound/boop_1.wav";
        private const string Boop_2Path = "Assets/Sound/boop_2.wav";
        private const string BgmPath = "Assets/Sound/MainTheme.wav";

        private Dictionary<SoundEnums, AudioTask> m_cachedSounds;
        private AudioContext context;
        private int[] m_buffers;
        private int[] m_sources;

        public AudioManager() {
            this.context = new AudioContext();
            this.m_cachedSounds = new Dictionary<SoundEnums, AudioTask>();
            this.m_buffers = AL.GenBuffers(Enum.GetValues(typeof(SoundEnums)).Length);
            //var buffer = AL.GenBuffer();
            this.m_sources = AL.GenSources(Enum.GetValues(typeof(SoundEnums)).Length);
           
            //boop_1
            this.m_cachedSounds[SoundEnums.Boop_1] = new AudioTask(Boop_1Path, this.m_buffers[(int)SoundEnums.Boop_1], this.m_sources[(int)SoundEnums.Boop_1]);
            this.m_cachedSounds[SoundEnums.Boop_2] = new AudioTask(Boop_2Path, this.m_buffers[(int)SoundEnums.Boop_2], this.m_sources[(int)SoundEnums.Boop_2]);
            this.m_cachedSounds[SoundEnums.Bgm] = new AudioTask(BgmPath, this.m_buffers[(int)SoundEnums.Bgm], this.m_sources[(int)SoundEnums.Bgm]);
            //this.m_cachedSounds[SoundEnums.Bgm] = new AudioTask(BgmPath, AL.GenBuffer(), AL.GenSource());
        }

        public void PlaySound(SoundEnums sounds, bool loop) {
            if(this.m_cachedSounds.ContainsKey(sounds)){
                this.m_cachedSounds[sounds].Start(loop);
            }
        }

        public void KillSounds() {
            this.m_cachedSounds[SoundEnums.Bgm].StopLoop();
        }
    }
}
