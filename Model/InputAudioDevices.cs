using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace CamPreview.Model
{
    public class InputAudioDevices
    {
        public string? Name { set; get; }
        public int DeviceNumber { private set; get; }

        public IEnumerable<InputAudioDevices> Get()
        {
            return from deviceNumber in Enumerable.Range(0, WaveInEvent.DeviceCount)
                   select new InputAudioDevices { Name = $"{WaveInEvent.GetCapabilities(deviceNumber).ProductName}", DeviceNumber = deviceNumber };
        }
    }
}
