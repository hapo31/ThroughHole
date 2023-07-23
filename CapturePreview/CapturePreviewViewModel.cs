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
using CamPreview.Lib;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace CamPreview.ViewModel
{
    internal class CapturePreviewViewModel : ViewModelBase, IDisposable
    {

        public event NewFrameEventHandler NewFrameGot = delegate { };

        public VideoCapture? VideoCapture { get; private set; }
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
            VideoCapture = new VideoCapture(monikerString);
            VideoCapture.NewFrameGot += NewFrameGot;
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
            VideoCapture.NewFrameGot -= NewFrameGot;
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

        public void CopyPrevVideoFrameToClipboard()
        {
            if (VideoCapture?.PrevFrame == null)
            {
                return;
            }

            // TODO: VideoCapture.PrevFrame から System.ArgumentException Parameter is not valid. が出る

            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                VideoCapture.PrevFrame.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
            Clipboard.SetImage(bitmapSource);
        }

        public void Dispose()
        {
            StopVideoCapture();
            StopAudioCapture();
        }
    }
}
