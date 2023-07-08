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

        private VideoCaptureDevice? videoCapture;
        private AudioPassthrough? audioCapture;

        // audioCapture は、オーディオデバイスが見つからない、あえて再生しないなどが考えられるので考慮しない
        public bool IsConnected { get => videoCapture != null; }

        public CapturePreviewViewModel() { }

        public void StartAudioCapture(MMDevice device)
        {
            CloseAudioDevice();
            audioCapture = new AudioPassthrough(device);
        }

        public void StartVideoCapture(string monikerString)
        {
            CloseVideoDevice();
            videoCapture = new VideoCaptureDevice(monikerString);
            videoCapture.NewFrame += NewFrameGot;
            videoCapture.Start();
        }

        public void Disconnect()
        {
            CloseVideoDevice();
        }

        private void CloseVideoDevice()
        {
            if (videoCapture == null)
            {
                return;
            }
            videoCapture.NewFrame -= NewFrameGot;
            videoCapture.SignalToStop();
            videoCapture = null;
        }

        private void CloseAudioDevice()
        {
            if (audioCapture == null)
            {
                return;
            }
            audioCapture.Dispose();
            audioCapture = null;
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            CloseVideoDevice();
        }
    }
}