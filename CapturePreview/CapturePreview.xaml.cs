using AForge.Video;
using CamPreview.Model;
using CamPreview.ViewModel;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CamPreview
{
    /// <summary>
    /// CapturePreview.xaml の相互作用ロジック
    /// </summary>
    public partial class CapturePreview : UserControl, IDisposable
    {
        private CapturePreviewViewModel capturePreviewViewModel;
        public static readonly DependencyProperty VideoCaptureMonikerStringProperty = DependencyProperty.Register(
            "VideoCaptureMonikerString",
            typeof(string),
            typeof(CapturePreview),
            new PropertyMetadata(new PropertyChangedCallback(OnVideoCaptureMonikerStringPropertyChanged)));

        public static readonly DependencyProperty AudioCaptureDeviceProperty = DependencyProperty.Register(
            "AudioCaptureDevice",
            typeof(MMDevice),
            typeof(CapturePreview),
            new PropertyMetadata(new PropertyChangedCallback(OnAudioDevicePropertyChanged)));

        public CapturePreview()
        {
            capturePreviewViewModel = new CapturePreviewViewModel();
            capturePreviewViewModel.NewFrameGot += onNewFrameGot;
            InitializeComponent();
        }

        public string VideoCaptureMonikerString
        {
            get => (string)GetValue(VideoCaptureMonikerStringProperty);
            set { SetValue(VideoCaptureMonikerStringProperty, value); }
        }

        public string AudioCaptureDevice
        {
            get => (string)GetValue(AudioCaptureDeviceProperty);
            set { SetValue(AudioCaptureDeviceProperty, value); }
        }

        private void onNewFrameGot(object sender, NewFrameEventArgs eventArgs)
        {
            Preview.Dispatcher.Invoke(new Action<Bitmap>(bmp => Preview.Source = BitmapToFrame.Convert(bmp)), eventArgs.Frame);
        }

        private static void OnVideoCaptureMonikerStringPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var newMonikerString = (string)e.NewValue;
            var ctrl = (CapturePreview)obj;
            if (newMonikerString == null || ctrl == null)
            {
                return;
            }
            ctrl.capturePreviewViewModel.StartVideoCapture(newMonikerString);
            Debug.WriteLine($"connected:{newMonikerString}");
        }
        private static void OnAudioDevicePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

            var ctrl = (CapturePreview)obj;
            Debug.WriteLine($"changed:{e.NewValue}");
            Debug.WriteLine($"changed:{e.NewValue.GetType().FullName}");
            if (!(e.NewValue is MMDevice) || ctrl == null)
            {
                return;
            }
            var newDevice = (MMDevice)e.NewValue;
            ctrl.capturePreviewViewModel.StartAudioCapture(newDevice);
            Debug.WriteLine($"connected:{newDevice.DeviceFriendlyName}");
        }

        public void Dispose()
        {
            capturePreviewViewModel.Dispose();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}