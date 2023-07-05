using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace CamPreview
{
    public class WasapiAudioDevices
    {
        public string? Name { set; get; }
        public MMDevice? Device { private set; get; }

        public IEnumerable<WasapiAudioDevices> Get()
        {
            return from device in new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
                   select new WasapiAudioDevices { Name = $"{device.FriendlyName}", Device = device };
        }
    }
}
