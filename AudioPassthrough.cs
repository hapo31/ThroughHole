using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;

namespace CamPreview
{
    internal class AudioPassthrough : IDisposable
    {
        private IWavePlayer player;
        private WasapiLoopbackCapture waveIn;

        private BufferedWaveProvider provider;

        public bool Disposed { get; private set; } = false;


        public AudioPassthrough(MMDevice device, MMDevice outputDevice)
        {
            waveIn = new WasapiLoopbackCapture(device);
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.RecordingStopped += WaveIn_RecordingStopped;
            waveIn.WaveFormat = new WaveFormat(44100, 16, 2);

            provider = new BufferedWaveProvider(waveIn.WaveFormat);
            player = new WasapiOut(outputDevice, AudioClientShareMode.Shared, false, 300);
            player.Init(new VolumeWaveProvider16(provider));
            player.Volume = 0.5f;

            player.Play();
            waveIn.StartRecording();
        }
        public AudioPassthrough(MMDevice device) : this(device, new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
        { }

        private void Stop()
        {
            if (player.PlaybackState != PlaybackState.Stopped)
            {
                player.Stop();
                player.Dispose();
            }
            if (waveIn != null)
            {
                waveIn.DataAvailable -= WaveIn_DataAvailable;
                waveIn.RecordingStopped -= WaveIn_RecordingStopped;
                waveIn.StopRecording();
                waveIn.Dispose();
            }
        }

        private void WaveIn_RecordingStopped(object? sender, StoppedEventArgs e)
        {
            Stop();
        }

        private void WaveIn_DataAvailable(object? sender, WaveInEventArgs e)
        {
            provider.AddSamples(e.Buffer, 0, e.BytesRecorded);
            Console.WriteLine("{0}", e.BytesRecorded);
        }

        public void Dispose()
        {
            Stop();
            Disposed = true;
        }
    }
}
