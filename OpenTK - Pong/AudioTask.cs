using OpenTK.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenTKPong {
    public class AudioTask {

        public AudioContext Context { get; set; }

        public int Buffer { get; set; }
        public int Source { get; set; }

        private bool m_stop;
        private bool m_looping;
        private bool m_shouldLoop;
        private string m_path;

        private BackgroundWorker m_bgw;

        public AudioTask(string path, int buffer, int source) {
            this.Buffer = buffer;
            this.Source = source;            
            int state;
            this.m_path = path;

            int channels, bits_per_sample, sample_rate;

            byte[] sound_data = LoadWave(File.Open(this.m_path, FileMode.Open), out channels, out bits_per_sample, out sample_rate);

            AL.BufferData(this.Buffer, GetSoundFormat(channels, bits_per_sample), sound_data, sound_data.Length, sample_rate);
            AL.SourceQueueBuffer(this.Source, this.Buffer);
        }


        public void Start(bool loop) {
            this.m_shouldLoop = loop;

            this.m_bgw = new BackgroundWorker();
            this.m_bgw.DoWork += ( (s, e) => this.Play() );
            this.m_bgw.RunWorkerCompleted += ( (s, e) => this.Stop() );            
            this.m_bgw.RunWorkerAsync();
        }

        //TODO: Looping and non looping should be handled the same, rather than two seperate loops
        public void Play() {           
            AL.Source(this.Source, ALSourceb.Looping, this.m_shouldLoop);
            AL.SourcePlay(this.Source);
            int state;
            if ( this.m_shouldLoop ) {
                this.m_looping = true;
                do {
                    Thread.Sleep(100);
                } while ( this.m_looping == true );
            } else {
                do {
                    Thread.Sleep(10);
                    AL.GetSource(this.Source, ALGetSourcei.SourceState, out state);
                } while ( (ALSourceState)state == ALSourceState.Playing );
            }
        }

        public void StopLoop() {
            this.m_looping = false;
        }

        public void Stop() {
            AL.SourceRewind(this.Source);            
        }


        private static ALFormat GetSoundFormat(int channels, int bits) {
            switch ( channels ) {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate) {
            if ( stream == null )
                throw new ArgumentNullException("stream");

            using ( BinaryReader reader = new BinaryReader(stream) ) {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if ( signature != "RIFF" )
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if ( format != "WAVE" )
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if ( format_signature != "fmt " )
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                //string data_signature = new string(reader.ReadChars(4));
                //if ( data_signature != "data" )
                //    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }
    }
}
