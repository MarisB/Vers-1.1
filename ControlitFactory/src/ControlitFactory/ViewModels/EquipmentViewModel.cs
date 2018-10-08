using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using ControlitFactory.Models;
using System.Collections.ObjectModel;

namespace ControlitFactory.ViewModels
{
    public class EquipmentViewModel : ViewModelBase
    {
        public EquipmentViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
        {
            PievienotCommand = new DelegateCommand(Pievienot);
            AtvertCommand = new DelegateCommand<Classifiers>(Atvert);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public DelegateCommand PievienotCommand { get; set; }

        public DelegateCommand<Classifiers> AtvertCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public ObservableCollection<Classifiers> Ieraksti
        {
            get => new ObservableCollection<Classifiers>(App.Database.GetEquipmentList().Result);
        }

        public void Pievienot()
        {
            var parametri = new NavigationParameters
            {
                { "Id", 0 }
            };

            _navigationService.NavigateAsync("EquipmentEdit", parametri);
        }
        public void Atvert(Classifiers item)
        {
            var parametri = new NavigationParameters
            {
                { "Id", item.Id }
            };

            _navigationService.NavigateAsync("EquipmentEdit", parametri);
        }
        private void Cancel()
        {
            _navigationService.NavigateAsync("MainPage");
        }
        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            RaisePropertyChanged(nameof(Ieraksti));
        }
    }
}
