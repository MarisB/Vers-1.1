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
    public class DefektaRegistracijaViewModel : ViewModelBase
    {
        private TranslateExtension tr = new TranslateExtension();
        public DefektaRegistracijaViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
            DeleteCommand = new DelegateCommand(DeleteAsync);
            PievienotDefektuCommand = new DelegateCommand(PievienotDefektu);

            RadioButtonSource = new List<RadioItem>()
            {
                new RadioItem(){ID = 1, Name = tr.GetTranslation("HorizantalaVirsmaLabel")},
                new RadioItem(){ID = 2, Name = tr.GetTranslation("VerikalaVirsmaLabel")}
            };
        }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand PievienotDefektuCommand { get; set; }

        private void PievienotDefektu()
        {
            var param = new NavigationParameters();
            param.Add(nameof(Defekts.Id), 0);
            param.Add("skaits", App.Database.GetDefekti(Defekts.DefektacijasAktaId).Result.Count);
            param.Add("DefektacijasAktaId", Defekts.DefektacijasAktaId);

            _navigationService.NavigateAsync("DefektaRegistracija", param);
        }
        private async void DeleteAsync()
        {

            var r = await _pageDialogService.DisplayActionSheetAsync(tr.GetTranslation("DeleteConfirmationLabel"), tr.GetTranslation("QuestionLabel"), tr.GetTranslation("YesLabel"), tr.GetTranslation("NoLabel"));

            if (r == tr.GetTranslation("YesLabel"))
            {

                //DependencyService.Get<ISaveAndLoad>().DeleteFile(Defekts.FilePath);
                await App.Database.DeleteDefekts(Defekts);
                await _navigationService.GoBackAsync();
            }
        }

        private bool _takePicture;
        public bool TakePicture
        {
            get { return _takePicture; }
            set { SetProperty(ref _takePicture, value); }
        }
        private void Save()
        {
            if (Defekts.DefektacijasAktaId == 0)
            {
                Defekts.DefektacijasAktaId = App.AktaId;
            }
            App.Database.InsertDefekts(Defekts);
            _navigationService.GoBackAsync();
        }
        private void Cancel()
        {
            _navigationService.GoBackAsync();
        }

        private Defekts _defekts;
        public Defekts Defekts
        {
            get { return _defekts; }
            set { SetProperty(ref _defekts, value); }
        }

        public List<RadioItem> RadioButtonSource { get; set; }
        private RadioItem _novietojums;
        public RadioItem Novietojums
        {
            get
            {
                if (Defekts != null)
                {
                    return RadioButtonSource.FirstOrDefault(x => x.ID == Defekts.Novietojums);
                }
                return null;
            }
            set
            {
                SetProperty(ref _novietojums, value);
                Defekts.Novietojums = value.ID;
            }
        }
        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            if (parameters.ContainsKey(nameof(Defekts.Id)))
            {
                var id = (int)parameters[nameof(Defekts.Id)];
                if (id == 0)
                {
                    var sk = (int)parameters["skaits"];
                    var DefektacijasAktaId = (int)parameters["DefektacijasAktaId"];
                    Defekts = new Defekts() { DefektacijasAktaId = DefektacijasAktaId, DefektaNr = sk + 1, Novietojums = 1 };
                    RaisePropertyChanged(nameof(Novietojums));
                    TakePicture = true;
                }
                else
                {
                    Defekts = App.Database.GetDefekts(id);
                    RaisePropertyChanged(nameof(Novietojums));
                }
            }
        }
    }
    public class RadioItem
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
}
