using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using ControlitFactory.Models;
using ControlitFactory.Support;

namespace ControlitFactory.ViewModels
{
    public class EquipmentEditViewModel : ViewModelBase
    {
        private bool noIvadformasHW;
        private bool noIvadformasLW;

        public EquipmentEditViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
        {
            SaveCommand = new DelegateCommand(Save);
            DzestCommand = new DelegateCommand(Dzest);
            CancelCommand = new DelegateCommand(Cancel);
        }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DzestCommand { get; set; }

        public DelegateCommand CancelCommand { get; set; }

        public Classifiers Equipment { get; set; }

        public void Save()
        {
            App.Database.InsertClassifier(Equipment);
            if (noIvadformasHW)
            {
                var param = new NavigationParameters();
                param.Add("jhw", Equipment.Id);
                _navigationService.GoBackAsync(param);
            }
            else if (noIvadformasLW)
            {
                var param = new NavigationParameters();
                param.Add("jlw", Equipment.Id);
                _navigationService.GoBackAsync(param);
            }
            else
            {
                _navigationService.NavigateAsync("Equipment");
            }
        }
        public async void Dzest()
        {
            if (VarDzest)
            {
                var tr = new TranslateExtension();

                var r = await _pageDialogService.DisplayActionSheetAsync(tr.GetTranslation("DeleteConfirmationLabel"), tr.GetTranslation("QuestionLabel"), tr.GetTranslation("YesLabel"), tr.GetTranslation("NoLabel"));

                if (r == tr.GetTranslation("YesLabel"))
                {
                    await App.Database.DeleteClassifier(Equipment);
                    await _navigationService.NavigateAsync("Equipment");
                }
            }
            else
            {
                _navigationService.GoBackAsync();
            }
        }
        private void Cancel()
        {
            if (noIvadformasHW || noIvadformasLW)
                _navigationService.GoBackAsync();
            else
                _navigationService.NavigateAsync("Equipment");
        }
        public bool VarDzest => !noIvadformasHW && !noIvadformasLW;

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            if (parameters.ContainsKey("Id"))
            {
                var id = (int)parameters["Id"];
                if (id == 0)
                {
                    Equipment = new Classifiers();
                    if (parameters.ContainsKey("j"))
                    {
                        noIvadformasHW = (int)parameters["j"] == 1;
                        noIvadformasLW = (int)parameters["j"] == 2;
                    }
                }
                else
                {
                    Equipment = App.Database.GetClassifier(id);
                    RaisePropertyChanged(nameof(Equipment));
                }
            }
        }
    }
}
