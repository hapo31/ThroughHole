using AForge.Video.DirectShow;
using System.Collections.Generic;
using System.Linq;

namespace CamPreview.Model
{
    public class DeviceFilters
    {
        public string? Name { set; get; }
        public string? MonikerString { set; get; }

        public string? VendorId { set; get; }

        public static IEnumerable<DeviceFilters> Enumurate()
        {
            return from FilterInfo info in new FilterInfoCollection(FilterCategory.VideoInputDevice)
                   select new DeviceFilters { Name = info.Name, MonikerString = info.MonikerString };
        }
    }
}
