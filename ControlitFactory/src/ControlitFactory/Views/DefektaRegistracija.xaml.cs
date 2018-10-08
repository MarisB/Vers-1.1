using ControlitFactory.Support;
using ControlitFactory.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ControlitFactory.Views
{
    public partial class DefektaRegistracija : ContentPage
    {
        private DefektaRegistracijaViewModel vm;

        public DefektaRegistracija()
        {
            InitializeComponent();
            vm = (DefektaRegistracijaViewModel)this.BindingContext;
            vm.PropertyChanged += Vm_PropertyChanged;
            var takeImage = new TapGestureRecognizer();
            takeImage.Tapped += takeImage_Tapped;
            CameraButton.GestureRecognizers.Add(takeImage);
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null && e.PropertyName == nameof(DefektaRegistracijaViewModel.TakePicture))
            {
                if (vm.TakePicture)
                    takeImage_Tapped(this, new EventArgs());
            }
        }

        private async void takeImage_Tapped(object sender, EventArgs e)
        {//check permissions
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (status != PermissionStatus.Granted)
            {
                //if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                //{
                //    var tr = new TranslateExtension();

                //    await DisplayAlert(tr.GetTranslation("QuestionLabel"), "Gunna need that location", "OK");
                //}

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Camera))
                    status = results[Permission.Camera];
            }
            if (status == PermissionStatus.Granted)
            {
                try
                {
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "ControlitFactory",
                        SaveToAlbum = true,
                        CompressionQuality = 75,
                        CustomPhotoSize = 50,
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 2000,
                        DefaultCamera = CameraDevice.Rear
                    });
                    if (file != null)
                    {
                        if (!string.IsNullOrEmpty(vm.Defekts.FilePath))
                            DependencyService.Get<ISaveAndLoad>().DeleteFile(vm.Defekts.FilePath);
                        vm.Defekts.FilePath = file.Path;
                    }
                    if (vm.TakePicture)
                    {
                        Piezimes.Focus();
                    }

                }
                catch (Exception ex)
                {

                }
            }

        }
    }
}
