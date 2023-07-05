using AForge.Video.DirectShow;
using System.Collections.Generic;
using System.Linq;

namespace CamPreview
{
    public class DeviceFilters
    {
        public string? Name { set; get; }
        public string? MonikerString { set; get; }

        public IEnumerable<DeviceFilters> Get()
        {
            return from FilterInfo info in new FilterInfoCollection(FilterCategory.VideoInputDevice)
                   select new DeviceFilters { Name = info.Name, MonikerString = info.MonikerString };
        }
    }
}
