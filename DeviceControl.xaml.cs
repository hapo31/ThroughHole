using AForge.Video.DirectShow;
using AForge.Video;
using System.Windows;
using System.Windows.Controls;
using System;

namespace CamPreview
{
    /// <summary>
    /// DeviceControl.xaml の相互作用ロジック
    /// </summary>
    public partial class DeviceControl : UserControl, IDisposable
    {
        public DeviceControl()
        {
            InitializeComponent();
        }

        public event NewFrameEventHandler NewFrameGot = delegate { };
        private VideoCaptureDevice? device;

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            device = new VideoCaptureDevice((string)deviceListCombo.SelectedValue);
            device.NewFrame += NewFrameGot;
            device.Start();
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            CloseDevice();
        }

        public void Dispose()
        {
            CloseDevice();
        }


        private void CloseDevice()
        {
            if (device == null)
            {
                return;
            }
            device.NewFrame -= NewFrameGot;
            device.SignalToStop();
            device = null;
        }
    }
}
