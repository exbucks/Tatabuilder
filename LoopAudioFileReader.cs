using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TataBuilder
{
    public class LoopAudioFileReader : AudioFileReader
    {
        /// <summary>
        /// Use this to turn looping on or off
        /// </summary>
        public bool Loop { get; set; }

        public LoopAudioFileReader(string fileName, bool loop)
            : base(fileName)
        {
            this.Loop = loop;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count) {
                int bytesRead = base.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0) {
                    if (base.Position == 0 || !Loop) {
                        // something wrong with the source stream
                        break;
                    }
                    // loop
                    base.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}
