using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using System.Collections.ObjectModel;
using ControlitFactory.Models;
using Xamarin.Forms;

namespace ControlitFactory.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
        {

                HelpCommand = new DelegateCommand(Help);
                PievienotDefektacijasAktuCommand = new DelegateCommand(PievienotDefektacijasAktu);
                AtvertDefektacijasAktuCommand = new DelegateCommand<DefektacijasAkts>(AtvertDefektacijasAktu);

            }

            private void Help()
            {
                if (App.Profils == null)
                {
                    Device.OpenUri(new Uri("http://controlit.lv/en/mobile-app/"));
                }
                else
                {
                    switch (App.Profils.Valoda)
                    {
                        case "LV":
                            Device.OpenUri(new Uri("http://controlit.lv/mob-aplikacija/"));
                            break;
                        case "SE":
                            Device.OpenUri(new Uri("http://controlit.lv/sv/mobilapplikation/"));
                            break;
                        case "LT":
                            Device.OpenUri(new Uri("http://controlit.lv/lt/1041-2/"));
                            break;
                        default:
                            Device.OpenUri(new Uri("http://controlit.lv/en/mobile-app/"));
                            break;
                    }
                }
            }

        public DelegateCommand PievienotDefektacijasAktuCommand { get; set; }

        public DelegateCommand HelpCommand { get; set; }
        public DelegateCommand<DefektacijasAkts> AtvertDefektacijasAktuCommand { get; set; }

        public void PievienotDefektacijasAktu()
        {
            var parametri = new NavigationParameters
            {
                { "Id", 0 }
            };

            _navigationService.NavigateAsync("EditTabPage", parametri);
        }
        public void AtvertDefektacijasAktu(DefektacijasAkts item)
        {
            var parametri = new NavigationParameters
            {
                { "Id", item.Id }
            };

            _navigationService.NavigateAsync("EditTabPage", parametri);
        }

        public ObservableCollection<DefektacijasAkts> Ieraksti
        {
            get => new ObservableCollection<DefektacijasAkts>(App.Database.GetDefektacijasAkti().Result);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            RaisePropertyChanged(nameof(Ieraksti));
        }
    }
}
