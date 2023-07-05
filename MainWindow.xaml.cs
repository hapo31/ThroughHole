using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
            camDeviceCtrl.NewFrameGot += CamDeviceCtrlNewFrameGot;

            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            camDeviceCtrl.NewFrameGot -= CamDeviceCtrlNewFrameGot;
            camDeviceCtrl.Dispose();
            audioDeviceCtrl.Dispose();
        }

        private void CamDeviceCtrlNewFrameGot(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            picture.Dispatcher.Invoke(new Action<Bitmap>(bmp => picture.Source = BitmapToFrame.Convert(bmp)), eventArgs.Frame);
        }



    }
}