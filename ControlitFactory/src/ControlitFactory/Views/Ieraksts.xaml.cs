using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ControlitFactory.ViewModels;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace ControlitFactory.Views
{
    public partial class Ieraksts : ContentPage
    {
        private IerakstsViewModel vm;
        public Ieraksts()
        {
            InitializeComponent();
            vm = (IerakstsViewModel)this.BindingContext;
            vm.GetImageStreamAsync = signaturePadView.GetImageStreamAsync;
            var mapImage = new TapGestureRecognizer();
            mapImage.Tapped += tapImage_Tapped;
            MapButton.GestureRecognizers.Add(mapImage);

            var getAddress = new TapGestureRecognizer();
            getAddress.Tapped += GetAddress_Tapped;
            GetAddress.GestureRecognizers.Add(getAddress);

            var getStartTime = new TapGestureRecognizer();
            getStartTime.Tapped += getStartTime_Tapped;
            GetDateAndTime.GestureRecognizers.Add(getStartTime);

            var getEndTime = new TapGestureRecognizer();
            getEndTime.Tapped += getEndTime_Tapped;
            GetDateAndTimePabeigta.GestureRecognizers.Add(getEndTime);

            var _getIzmaksasKopa = new TapGestureRecognizer();
            _getIzmaksasKopa.Tapped += GetIzmaksasKopa_Tapped;
            GetIzmaksasKopa.GestureRecognizers.Add(_getIzmaksasKopa);

            var _getLaiksKopa = new TapGestureRecognizer();
            _getLaiksKopa.Tapped += GetLaiksKopa_Tapped;
            GetLaiksKopa.GestureRecognizers.Add(_getLaiksKopa);


            var _lowWoltageEquipment = new TapGestureRecognizer();
            _lowWoltageEquipment.Tapped += LowWoltageEquipment_Tapped;
            LowWoltageEquipment.GestureRecognizers.Add(_lowWoltageEquipment);

            var _highWoltageEquipment = new TapGestureRecognizer();
            _highWoltageEquipment.Tapped += HighWoltageEquipment_Tapped;
            ClearHighWoltageEquipment.GestureRecognizers.Add(_highWoltageEquipment);
        }

        private void LowWoltageEquipment_Tapped(object sender, EventArgs e)
        {
            vm.LowWoltageEquipment = null;
        }
        private void HighWoltageEquipment_Tapped(object sender, EventArgs e)
        {
            vm.HighWoltageEquipment = null;
        }

        private void GetIzmaksasKopa_Tapped(object sender, EventArgs e)
        {
            vm?.DefektacijasAkts?.AprekinatKopa();
        }
        private void GetLaiksKopa_Tapped(object sender, EventArgs e)
        {
            vm?.DefektacijasAkts?.AprekinatLaiku();
        }


        private void tapImage_Tapped(object sender, EventArgs e)
        {
            vm.OpenMapCommand.Execute();
        }

        private void getStartTime_Tapped(object sender, EventArgs e)
        {
            vm.DefektacijasAkts.ParbaudeUzsakta = DateTime.Now;
            vm.Izmainits();
            ParbaudeUzsaktaDate.Date = vm.DefektacijasAkts.ParbaudeUzsakta;
            ParbaudeUzsaktaTime.Time = vm.DefektacijasAkts.ParbaudeUzsakta.TimeOfDay;
        }
        private void getEndTime_Tapped(object sender, EventArgs e)
        {
            vm.DefektacijasAkts.ParbaudePabeigta = DateTime.Now;
            vm.Izmainits();
            ParbaudePabeigtaDate.Date = vm.DefektacijasAkts.ParbaudePabeigta;
            ParbaudePabeigtaTime.Time = vm.DefektacijasAkts.ParbaudePabeigta.TimeOfDay;
        }

        private void GetAddress_Tapped(object sender, EventArgs e)
        {
            Loadings.IsRunning = true;
            GetAddress.IsVisible = false;
            GetDadress().ContinueWith((x) =>
            {
                if (x.Result != null)
                {
                    vm.DefektacijasAkts.Adrese = $"{x.Result.Thoroughfare} {x.Result.FeatureName}, {x.Result.Locality}, {x.Result.CountryName}, {x.Result.PostalCode} ";
                    vm.DefektacijasAkts.Latitude = x.Result.Latitude;
                    vm.DefektacijasAkts.Longitude = x.Result.Longitude;
                    Adrese.Text = vm.DefektacijasAkts.Adrese;
                }
                Loadings.IsRunning = false;
                GetAddress.IsVisible = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private async Task<Plugin.Geolocator.Abstractions.Address> GetDadress()
        {
            //check permissions
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                //if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                //{
                //    var tr = new TranslateExtension();

                //    await DisplayAlert(tr.GetTranslation("QuestionLabel"), "Gunna need that location", "OK");
                //}

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];
            }
            if (status == PermissionStatus.Granted)
            {
                try
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 20;

                    Plugin.Geolocator.Abstractions.Position position = null;

                    try
                    {
                        var token = new CancellationTokenSource(10000);
                        position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(10000), token.Token);
                    }
                    catch
                    {
                        position = null;
                    }
                    if (position == null)
                    {
                        position = await locator.GetLastKnownLocationAsync();
                    }
                    if (position != null)
                    {
                        var adreses = await locator.GetAddressesForPositionAsync(position);
                        if (adreses.Any())
                        {
                            var adrese = adreses.ToList()[0];
                            return adrese;
                        }
                    }
                }
                catch (System.Exception)
                {

                }
            }
            return null;
        }
    }
}
