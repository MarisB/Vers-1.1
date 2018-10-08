using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using ControlitFactory.Models;
using Xamarin.Forms;
using System.IO;

namespace ControlitFactory.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
        {

            SaveProfileCommand = new DelegateCommand(SaveProfile);
            CancelCommand = new DelegateCommand(Cancel);
            Valodas = new List<string> { "LV", "EN", "SE", "LT" };
        }

        private void Cancel()
        {
            _navigationService.NavigateAsync("MainPage");
        }

        private void SaveProfile()
        {
            App.Database.InsertProfile(Profile);
            App.Profils = Profile;
            _navigationService.NavigateAsync("MainPage");
        }

        public bool IsVisible => App.Profils != null;

        private Settings _profile;
        public Settings Profile
        {
            get
            {
                if (_profile == null)
                {
                    if (App.Profils != null)
                    {
                        _profile = App.Profils;
                    }
                    else
                    {
                        _profile = new Settings() { Valoda = "EN" };
                    }
                }
                return _profile;
            }
        }
        public DelegateCommand SaveProfileCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public List<string> Valodas { get; set; }


        public ImageSource Logo
        {
            get
            {
                if (Profile == null || string.IsNullOrWhiteSpace(Profile.Logo))
                    return null;
                var mas = Convert.FromBase64String(Profile.Logo);
                var img = ImageSource.FromStream(() => new MemoryStream(mas));

                return img;
            }
        }

        public void UpadteLogo()
        {
            RaisePropertyChanged(nameof(Logo));

        }
    }
}
