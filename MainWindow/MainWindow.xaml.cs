using AForge.Video.DirectShow;
using CamPreview.Model;
using CamPreview.ViewModel;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Xps.Packaging;

namespace CamPreview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel mainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            mainWindowViewModel = new MainWindowViewModel();
            DataContext = mainWindowViewModel;

            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            CapturePreview.Dispose();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CapturePreview.Dispose();
            Close();
        }

        private void CaptureVideoDeviceMenuClick(object sender, RoutedEventArgs e)
        {
            var clicked = e.OriginalSource as MenuItem;
            if (clicked == null)
            {
                return;
            }
            var videoDeviceMonikerString = clicked.Tag as string;
            mainWindowViewModel.ConnectedVideoDeviceMonikerString = videoDeviceMonikerString;
        }

        private void CaptureAudioDeviceMenuClick(object sender, RoutedEventArgs e)
        {
            var clicked = e.OriginalSource as MenuItem;
            if (clicked == null)
            {
                return;
            }
            var captureTargetAudioDevice = clicked.Tag as WasapiAudioDevice;

            mainWindowViewModel.ConnectedAudioDevice = captureTargetAudioDevice;
        }

        private void MuteMenuItemClick(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.Muted = !mainWindowViewModel.Muted;
        }
    }
}
