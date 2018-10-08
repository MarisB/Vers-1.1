using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace ControlitFactory.Support
{
    public class GeocodePage
    {

        public static async Task<string> GetCurrentPosition()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;
            var lastPos = await locator.GetLastKnownLocationAsync();
            CancellationTokenSource ctsrc = new CancellationTokenSource(2000);
            Plugin.Geolocator.Abstractions.Position position = null;
            try
            {
                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), ctsrc.Token, true);
            }
            catch (Exception ex)
            {
                if (ex != null)
                {

                }
            }
            if (position != null)
                {
                    var adresses = await locator.GetAddressesForPositionAsync(position, "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l");
                    var temp = adresses.FirstOrDefault();
                    if (temp != null)
                        return temp.ToString();
                }
                else if (lastPos != null)
                {

                    var adresses = await locator.GetAddressesForPositionAsync(lastPos, "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l");
                    var temp = adresses.FirstOrDefault();
                    if (temp != null)
                        return temp.ToString();
                }
            return "";
        }

    }
}
