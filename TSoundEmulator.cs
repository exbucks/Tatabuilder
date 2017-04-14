using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TataBuilder
{
    public class TSoundEmulator
    {
        delegate void PlaybackStopped(TSoundTask task);

        TLibraryManager libraryManager;
        string bgmFilePath = "";
        int bgmVolume = 100;
        TSoundTask bgmTask = null;
        List<TSoundTask> effectTasks = new List<TSoundTask>();
        List<TSoundTask> voiceTasks = new List<TSoundTask>();

        public TSoundEmulator(TLibraryManager libraryManager)
        {
            this.libraryManager = libraryManager;
        }

        public void playEffect(string fileName, int volume, bool loop)
        {
            lock (effectTasks) {
                string filePath = libraryManager.soundFilePath(libraryManager.soundIndex(fileName));
                if (filePath != "") {
                    try {
                        TSoundTask task = new TSoundTask(filePath, volume, loop, onEffectStopped);
                        task.play();
                        effectTasks.Add(task);
                    } catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public void stopEffect(string fileName)
        {
            lock (effectTasks) {
                for (int i = 0; i < effectTasks.Count; i++) {
                    TSoundTask task = effectTasks[i];
                    if (Path.GetFileName(task.filePath) == fileName) {
                        effectTasks.Remove(task);
                        task.stop();
                    }
                }
            }
        }

        public void stopAllEffects()
        {
            lock (effectTasks) {
                for (int i = 0; i < effectTasks.Count; i++) {
                    TSoundTask task = effectTasks[i];
                    task.stop();
                }

                effectTasks.Clear();
            }
        }

        private void onEffectStopped(TSoundTask task)
        {
            lock (effectTasks) {
                effectTasks.Remove(task);
            }
        }

        public void playVoice(string fileName, int volume, bool loop)
        {
            lock (voiceTasks) {
                string filePath = libraryManager.soundFilePath(libraryManager.soundIndex(fileName));
                if (filePath != "") {
                    try {
                        TSoundTask task = new TSoundTask(filePath, volume, loop, onVoiceStopped);
                        task.play();
                        voiceTasks.Add(task);
                    } catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public void stopVoice(string fileName)
        {
            lock (voiceTasks) {
                for (int i = 0; i < voiceTasks.Count; i++) {
                    TSoundTask task = voiceTasks[i];
                    if (Path.GetFileName(task.filePath) == fileName) {
                        voiceTasks.Remove(task);
                        task.stop();
                    }
                }
            }
        }

        public void stopAllVoices()
        {
            lock (voiceTasks) {
                for (int i = 0; i < voiceTasks.Count; i++) {
                    TSoundTask task = voiceTasks[i];
                    task.stop();
                }

                voiceTasks.Clear();
            }
        }

        private void onVoiceStopped(TSoundTask task)
        {
            lock (voiceTasks) {
                voiceTasks.Remove(task);
            }
        }

        public void playBGM(string fileName, int volume)
        {
            if (bgmTask != null) {
                if (Path.GetFileName(bgmTask.filePath) == fileName) {
                    // case when only volume was changed
                    if (bgmTask.volume != volume) {
                        bgmVolume = volume;
                        bgmTask.changeVolume(volume);
                    }

                    return;
                }

                bgmTask.stop();
                bgmTask = null;
            }

            bgmFilePath = libraryManager.soundFilePath(libraryManager.soundIndex(fileName));
            bgmVolume = volume;
            if (bgmFilePath != "") {
                try {
                    bgmTask = new TSoundTask(bgmFilePath, volume, true, onBGMStopped);
                    bgmTask.play();
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    bgmTask = null;
                }
            }
        }

        public void playBGM()
        {
            if (bgmTask != null)
                return;

            if (bgmFilePath != "") {
                try {
                    bgmTask = new TSoundTask(bgmFilePath, bgmVolume, true, onBGMStopped);
                    bgmTask.play();
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    bgmTask = null;
                }
            }
        }

        public void stopBGM()
        {
            if (bgmTask != null) {
                bgmTask.stop();
                bgmTask = null;
            }
        }

        private void onBGMStopped(TSoundTask task)
        {
            bgmTask = null;
        }

        public void stopAllSounds(bool bgm = true, bool effect = true, bool voice = true)
        {
            if (bgm)
                stopBGM();
            if (effect)
                stopAllEffects();
            if (voice)
                stopAllVoices();
        }

        class TSoundTask
        {
            public string filePath { get; set; }
            public int volume { get; set; }

            //Declarations required for audio out and the MP3 stream
            IWavePlayer waveOutDevice = null;
            AudioFileReader audioFileReader = null;
            PlaybackStopped playbackStoppedHandler = null;

            public TSoundTask(string filePath, int volume, bool loop, PlaybackStopped stoppedHandler)
            {
                this.filePath = filePath;
                this.volume = volume;
                this.audioFileReader = new LoopAudioFileReader(filePath, loop);
                this.audioFileReader.Volume = volume / 100f;
                this.waveOutDevice = new WaveOut();
                this.waveOutDevice.Init(this.audioFileReader);
                this.waveOutDevice.PlaybackStopped += onPlaybackStopped;
                this.playbackStoppedHandler = stoppedHandler;
            }

            public void play()
            {
                this.waveOutDevice.Play();
            }

            public void stop()
            {
                if (this.waveOutDevice != null) {
                    this.waveOutDevice.Stop();
                    this.waveOutDevice = null;
                }

                if (this.audioFileReader != null) {
                    this.audioFileReader.Close();
                    this.audioFileReader = null;
                }
            }

            public void changeVolume(int volume)
            {
                this.volume = volume;
                this.audioFileReader.Volume = volume / 100f;
            }

            public void onPlaybackStopped(object sender, StoppedEventArgs e)
            {
                if (this.waveOutDevice != null) {
                    this.waveOutDevice.Stop();
                    this.waveOutDevice = null;
                }

                if (this.audioFileReader != null) {
                    this.audioFileReader.Close();
                    this.audioFileReader = null;
                }

                if (this.playbackStoppedHandler != null) {
                    this.playbackStoppedHandler(this);
                }
            }
        }
    }
}
