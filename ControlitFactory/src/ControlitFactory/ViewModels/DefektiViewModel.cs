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
    public class DefektiViewModel : ViewModelBase
    {
        public DefektiViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
        {
            PievienotDefektuCommand = new DelegateCommand(PievienotDefektu);
            AtvertDefektuCommand = new DelegateCommand<Defekts>(AtvertDefektu);
        }
        public DelegateCommand PievienotDefektuCommand { get; set; }
        public DelegateCommand<Defekts> AtvertDefektuCommand { get; set; }

        private void PievienotDefektu()
        {
            //ja ir jauns defektācijas akts tad padotais id ir 0
            if (DefektācijasAktaId == 0)
            {
                if (App.Akts.Id == 0)
                {
                    App.Database.InsertDefektacijasAkts(App.Akts);
                }
                DefektācijasAktaId = App.AktaId;
            }
            var param = new NavigationParameters();
            param.Add(nameof(Defekts.Id), 0);
            param.Add("skaits", Ieraksti.Count);
            param.Add("DefektacijasAktaId", DefektācijasAktaId);

            _navigationService.NavigateAsync("DefektaRegistracija", param);
        }
        private void AtvertDefektu(Defekts defekts)
        {
            var param = new NavigationParameters();
            param.Add(nameof(Defekts.Id), defekts.Id);

            _navigationService.NavigateAsync("DefektaRegistracija", param);
        }

        public int DefektācijasAktaId { get; set; }

        public bool VarPievienotDefektu { get; set; }

        public ObservableCollection<Defekts> Ieraksti
        {
            get => DefektācijasAktaId > 0 ? new ObservableCollection<Defekts>(App.Database.GetDefekti(DefektācijasAktaId).Result.ToList()) : new ObservableCollection<Defekts>();
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            if (parameters.ContainsKey(nameof(DefektacijasAkts.Id)))
            {
                DefektācijasAktaId = (int)parameters[nameof(DefektacijasAkts.Id)];
                if (DefektācijasAktaId == 0)
                {
                    VarPievienotDefektu = false;
                }
                else
                {
                    VarPievienotDefektu = true;
                    RaisePropertyChanged(nameof(Ieraksti));
                }
            }
            else
            {
                RaisePropertyChanged(nameof(Ieraksti));
            }
        }
        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}

