using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;

namespace LagoVista.Uas.BaseStation.ControlApp.Converters
{
    public class GeoLocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is LagoVista.Core.Models.Geo.GeoLocation local)
            {
                if (local.Latitude.HasValue && local.Longitude.HasValue && local.Altitude.HasValue)
                {
                    return new Geopoint(new BasicGeoposition()
                    {
                        Latitude = local.Latitude.Value,
                        Longitude = local.Longitude.Value,
                        Altitude = local.Altitude.Value,
                    });
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
