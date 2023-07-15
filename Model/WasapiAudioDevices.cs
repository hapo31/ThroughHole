using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace CamPreview.Model
{
    public class WasapiAudioDevices
    {
        public static IEnumerable<WasapiAudioDevice> Enumurate()
        {
            var devices = new List<WasapiAudioDevice>();

            for (var i = 0; i < WaveIn.DeviceCount; i++)
            {
                devices.Add(new WasapiAudioDevice(i));

            }
            return devices;
        }
    }
}
