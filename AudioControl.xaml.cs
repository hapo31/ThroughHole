using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
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
    /// AudioControl.xaml の相互作用ロジック
    /// </summary>
    public partial class AudioControl : UserControl, IDisposable
    {
        private AudioPassthrough? sound;

        public bool Connected
        {
            get
            {
                return sound != null && !sound.Disposed;
            }
        }

        public AudioControl()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            sound?.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Button_Click");
            StartButton.IsEnabled = false;
            try
            {
                if (!Connected)
                {
                    if (deviceListCombo.SelectedValue is MMDevice)
                    {
                        var device = (MMDevice)deviceListCombo.SelectedValue;
                        sound = new AudioPassthrough(device);
                        Debug.WriteLine($"connected:{device.FriendlyName}");
                    }
                }
                else
                {
                    Debug.WriteLine("disconnected");
                    sound?.Dispose();
                    sound = null;
                }
            }
            finally
            {
                StartButton.IsEnabled = true;
            }
        }

        private void deviceListCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (deviceListCombo.SelectedValue is MMDevice)
            {
                StartButton.IsEnabled = true;
            }
        }
    }
}
