using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace CamPreview.Model
{
    public class WasapiAudioDevices
    {
        public string? Name { set; get; }
        public MMDevice? Device { private set; get; }

        public static IEnumerable<WasapiAudioDevices> Enumurate()
        {
            return from device in new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.All)
                   select new WasapiAudioDevices { Name = $"{device.FriendlyName}", Device = device };
        }
    }
}
