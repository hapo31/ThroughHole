using AForge.Video.DirectShow;
using AForge.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CamPreview.Model;
using System.ComponentModel;
using NAudio.CoreAudioApi;
using System.Windows;

namespace CamPreview.ViewModel
{
    internal class CapturePreviewViewModel : INotifyPropertyChanged, IDisposable
    {

        public event NewFrameEventHandler NewFrameGot = delegate { };
        public event PropertyChangedEventHandler? PropertyChanged;

        public VideoCaptureDevice? VideoCapture { get; private set; }
        public AudioPassthrough? AudioCapture { get; private set; }

        // audioCapture は、オーディオデバイスが見つからない、あえて再生しないなどが考えられるので考慮しない
        public bool IsConnected { get => VideoCapture != null; }

        public CapturePreviewViewModel() { }

        public void StartAudioCapture(WasapiAudioDevice device)
        {
            StopAudioCapture();
            AudioCapture = new AudioPassthrough(device);
        }

        public void StartVideoCapture(string monikerString)
        {
            StopVideoCapture();
            VideoCapture = new VideoCaptureDevice(monikerString);
            VideoCapture.NewFrame += NewFrameGot;
            VideoCapture.Start();
        }

        public void Disconnect()
        {
            StopVideoCapture();
            StopAudioCapture();
        }

        public void StopVideoCapture()
        {
            if (VideoCapture == null)
            {
                return;
            }
            VideoCapture.NewFrame -= NewFrameGot;
            VideoCapture.SignalToStop();
            VideoCapture = null;
        }

        public void StopAudioCapture()
        {
            if (AudioCapture == null)
            {
                return;
            }
            AudioCapture.Dispose();
            AudioCapture = null;
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            StopVideoCapture();
            StopAudioCapture();
        }
    }
}
