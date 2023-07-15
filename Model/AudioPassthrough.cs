using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;

namespace CamPreview.Model
{
    internal class AudioPassthrough : IDisposable
    {
        private WaveIn waveIn;
        private BufferedWaveProvider waveOutProvider;

        private IWavePlayer player;

        private VolumeSampleProvider volumeSampleProvider;

        public bool Disposed { get; private set; } = false;

        public WasapiAudioDevice Device { get; private set; }

        public float Volume
        {
            get
            {
                return volumeSampleProvider.Volume;
            }
            set
            {
                volumeSampleProvider.Volume = value;
            }
        }

        public AudioPassthrough(WasapiAudioDevice device)
        {
            Device = device;
            waveIn = device.ToSourceStream();
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.RecordingStopped += WaveIn_RecordingStopped;

            waveOutProvider = new BufferedWaveProvider(waveIn.WaveFormat);
            player = new WasapiOut(AudioClientShareMode.Shared, false, 300);

            volumeSampleProvider = new VolumeSampleProvider(waveOutProvider.ToSampleProvider());
            volumeSampleProvider.Volume = 2.0f;
            player.Init(volumeSampleProvider);
            waveIn.StartRecording();
            player.Play();
        }

        private void WaveIn_RecordingStopped(object? sender, StoppedEventArgs e)
        {
            Dispose();
        }

        private void WaveIn_DataAvailable(object? sender, WaveInEventArgs e)
        {
            waveOutProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        public void Dispose()
        {
            if (player.PlaybackState != PlaybackState.Stopped)
            {
                player.Stop();
                player.Dispose();
            }

            waveIn.DataAvailable -= WaveIn_DataAvailable;
            waveIn.RecordingStopped -= WaveIn_RecordingStopped;
            waveIn.StopRecording();
            waveIn.Dispose();

            waveOutProvider.ClearBuffer();

            Disposed = true;
        }
    }
}
