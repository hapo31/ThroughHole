using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamPreview.Model
{
    public class WasapiAudioDevice
    {
        public WasapiAudioDevice(int deviceNumber)
        {
            var capabilities = WaveIn.GetCapabilities(deviceNumber);
            Name = WaveCapabilitiesHelpers.GetNameFromGuid(capabilities.NameGuid) ?? capabilities.ProductName;
            Capabilities = capabilities;
            DeviceNumber = deviceNumber;
            var i = (int)SupportedWaveFormat.WAVE_FORMAT_44M16;
            SupportedWaveFormat supportedFormat = 0;
            for (; i <= (int)SupportedWaveFormat.WAVE_FORMAT_48S16; i <<= 1)
            {
                if (capabilities.SupportsWaveFormat((SupportedWaveFormat)i))
                {
                    supportedFormat = (SupportedWaveFormat)i;
                }
            }
            WaveFormat = supportedWaveFormatEnumToWaveFormat(supportedFormat);
            Debug.WriteLine($"{WaveFormat}");
        }

        public string Name { private set; get; }
        public WaveInCapabilities Capabilities { private set; get; }

        public int DeviceNumber { private set; get; }

        public WaveFormat WaveFormat { private set; get; }

        public WaveIn ToSourceStream()
        {
            var stream = new WaveIn();
            stream.DeviceNumber = DeviceNumber;
            stream.WaveFormat = WaveFormat;
            return stream;
        }

        private WaveFormat supportedWaveFormatEnumToWaveFormat(SupportedWaveFormat supportedFormat)
        {
            switch (supportedFormat)
            {
                case SupportedWaveFormat.WAVE_FORMAT_1M08:
                    return new WaveFormat(11025, 8, 1);
                case SupportedWaveFormat.WAVE_FORMAT_1S08:
                    return new WaveFormat(11025, 8, 2);
                case SupportedWaveFormat.WAVE_FORMAT_1M16:
                    return new WaveFormat(11025, 16, 1);
                case SupportedWaveFormat.WAVE_FORMAT_1S16:
                    return new WaveFormat(11025, 16, 2);
                case SupportedWaveFormat.WAVE_FORMAT_2M08:
                    return new WaveFormat(22050, 8, 1);
                case SupportedWaveFormat.WAVE_FORMAT_2S08:
                    return new WaveFormat(22050, 8, 2);
                case SupportedWaveFormat.WAVE_FORMAT_2M16:
                    return new WaveFormat(22050, 16, 1);
                case SupportedWaveFormat.WAVE_FORMAT_2S16:
                    return new WaveFormat(22050, 16, 2);
                case SupportedWaveFormat.WAVE_FORMAT_4M08:
                    return new WaveFormat(44100, 8, 1);
                case SupportedWaveFormat.WAVE_FORMAT_4S08:
                    return new WaveFormat(44100, 8, 2);
                case SupportedWaveFormat.WAVE_FORMAT_4M16:
                    return new WaveFormat(44100, 16, 1);
                case SupportedWaveFormat.WAVE_FORMAT_4S16:
                    return new WaveFormat(44100, 16, 2);
                case SupportedWaveFormat.WAVE_FORMAT_48M08:
                    return new WaveFormat(48000, 8, 1);
                case SupportedWaveFormat.WAVE_FORMAT_48S08:
                    return new WaveFormat(48000, 8, 2);
                case SupportedWaveFormat.WAVE_FORMAT_48M16:
                    return new WaveFormat(48000, 16, 1);
                case SupportedWaveFormat.WAVE_FORMAT_48S16:
                    return new WaveFormat(48000, 16, 2);
                case SupportedWaveFormat.WAVE_FORMAT_96M08:
                    return new WaveFormat(96000, 8, 1);
                case SupportedWaveFormat.WAVE_FORMAT_96S08:
                    return new WaveFormat(96000, 8, 2);
                case SupportedWaveFormat.WAVE_FORMAT_96M16:
                    return new WaveFormat(96000, 16, 1);
                case SupportedWaveFormat.WAVE_FORMAT_96S16:
                    return new WaveFormat(96000, 16, 2);

                default:
                    throw new ArgumentException($"Not find SupportedWaveFormat enum value: 0x{supportedFormat:x}");
            }
        }
    }
}
