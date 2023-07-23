using AForge.Video;
using AForge.Video.DirectShow;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CamPreview.Model
{
    internal class VideoCapture : IDisposable
    {
        public event NewFrameEventHandler NewFrameGot = delegate { };
        public string ConnectedVideoDeviceMonikerString { private set; get; }
        private VideoCaptureDevice device;

        public Bitmap? PrevFrame { private set; get; }

        public VideoCapture(string connectedVideoDeviceMonikerString)
        {
            ConnectedVideoDeviceMonikerString = connectedVideoDeviceMonikerString;
            device = new VideoCaptureDevice(connectedVideoDeviceMonikerString);
        }

        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            device.NewFrame += onNewFrameGot;
            device.Start();
        }

        public void Stop()
        {
            device.NewFrame -= onNewFrameGot;
            PrevFrame = null;
            device.SignalToStop();
        }

        private void onNewFrameGot(object sender, NewFrameEventArgs eventArgs)
        {
            PrevFrame = eventArgs.Frame;
            NewFrameGot?.Invoke(this, eventArgs);
        }
    }
}
