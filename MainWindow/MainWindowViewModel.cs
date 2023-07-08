using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AForge.Video.DirectShow;
using CamPreview.Model;
using NAudio.CoreAudioApi;

namespace CamPreview.ViewModel
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool muted = false;
        private List<MenuItem> videoDevicesMenu;
        private List<MenuItem> audioDevicesMenu;
        private string? connectedVideoDeviceMonikerString;
        private MMDevice? connectedAudioDevice;

        public string? ConnectedVideoDeviceMonikerString
        {
            get => connectedVideoDeviceMonikerString;
            set
            {
                if (connectedVideoDeviceMonikerString == value)
                {
                    return;
                }
                connectedVideoDeviceMonikerString = value;
                foreach (var menuItem in videoDevicesMenu)
                {
                    menuItem.IsChecked = (string)menuItem.Tag == value;
                }
                RaisePropertyChanged("ConnectedVideoDeviceMonikerString");
            }
        }

        public MMDevice? ConnectedAudioDevice
        {
            get => connectedAudioDevice;
            set
            {
                if (connectedAudioDevice == value)
                {
                    return;
                }
                connectedAudioDevice = value;
                foreach (var menuItem in audioDevicesMenu)
                {
                    menuItem.IsChecked = menuItem.Tag is MMDevice && ((MMDevice)menuItem.Tag).ID == value?.ID;
                }
                RaisePropertyChanged("ConnectedAudioDevice");
            }
        }

        public bool Muted
        {
            get => muted;
            set
            {
                muted = value;
                RaisePropertyChanged("Muted");
            }
        }

        public List<MenuItem> VideoDevicesMenu
        {
            get => videoDevicesMenu;
            set
            {
                videoDevicesMenu = value;
                RaisePropertyChanged("VideoDevicesMenu");
            }
        }

        public List<MenuItem> AudioDevicesMenu
        {
            get => audioDevicesMenu;
            set
            {
                audioDevicesMenu = value;
                RaisePropertyChanged("AudioDevicesMenu");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public delegate void OnClickVideoCaptureDeviceMenuHandler(object sender, DeviceFilters clickedDevice);
        public event OnClickVideoCaptureDeviceMenuHandler? onClickVideoCaptureDeviceMenu;

        public MainWindowViewModel()
        {
            videoDevicesMenu = (from device in DeviceFilters.Enumurate() select new MenuItem() { Header = device.Name, Tag = device.MonikerString }).ToList();
            audioDevicesMenu = (from device in WasapiAudioDevices.Enumurate() select new MenuItem() { Header = device.Name, Tag = device.Device }).ToList();
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
