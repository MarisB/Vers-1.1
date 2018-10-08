using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using SignaturePad.Forms;
using System.IO;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Messaging;
using ControlitFactory.Models;
using System.Reflection;
using System.Text;
using ControlitFactory.Support;
using ControlitFactory.Helpers;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace ControlitFactory.ViewModels
{
    public class IerakstsViewModel : ViewModelBase
    {
        public IerakstsViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
        {

            SaveProfileCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
            DeleteCommand = new DelegateCommand(DeleteAsync);
            OpenMapCommand = new DelegateCommand(OpenMap);
            ShareCommand = new DelegateCommand(Share);
        }

        public Func<SignatureImageFormat, ImageConstructionSettings, Task<Stream>> GetImageStreamAsync { get; set; }
        private async void Share()
        {//check permissions
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                //if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                //{
                //    var tr = new TranslateExtension();

                //    await DisplayAlert(tr.GetTranslation("QuestionLabel"), "Gunna need that location", "OK");
                //}

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Storage))
                    status = results[Permission.Storage];
            }
            if (status == PermissionStatus.Granted)
            {
                IEmailTask emailMessenger = null;
                try
                {
                    emailMessenger = CrossMessaging.Current.EmailMessenger;
                }
                catch (Exception ex)
                {
                    await _pageDialogService.DisplayAlertAsync("", ex.ToString(), "OK");
                }
                if (emailMessenger.CanSendEmail || 1 == 1)
                {
                    try
                    {
                        var logo = @"   <img alt='' src='data: image / png; base64,iVBORw0KGgoAAAANSUhEUgAAASwAAABjCAYAAAAhBD14AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAAZdEVYdFNvZnR3YXJlAEFkb2JlIEltYWdlUmVhZHlxyWU8AAAqz0lEQVR4Xu2dB5wURfbH38wGYInLCkpWOEFPMIGHkSCCWUD0FO9Q8FDvRDxETICg / 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'>";
                        var defekti = App.Database.GetDefekti(DefektacijasAkts.Id).Result.ToList().OrderBy(x => x.DefektaNr).ToList();
                        var tmp = App.Database.GetProfile().Result;
                        if (tmp.Count > 0)
                        {
                            App.Profils = tmp[0];
                        }
                        else
                        {
                            App.Profils = new Settings();
                        }
                        if (!string.IsNullOrWhiteSpace(App.Profils.Logo))
                        {
                            logo = $"<img alt='' src='data: image/png;base64, {App.Profils.Logo}' />";
                        }
                        var kopa = defekti.Count;
                        var horiz = defekti.Where(x => x.Novietojums == 1).Count();
                        var vert = defekti.Where(x => x.Novietojums == 2).Count();
                        Stream stream;
                        var numurs = "";
                        var desc = "";
                        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(IerakstsViewModel)).Assembly;
                        switch (App.Profils.Valoda)
                        {
                            case "LV":
                                stream = assembly.GetManifestResourceStream("ControlitFactory.Reportlv.txt");
                                numurs = "Defekts numur";
                                desc = "Defekta attēls";
                                break;
                            default:
                                stream = assembly.GetManifestResourceStream("ControlitFactory.Reporten.txt");
                                numurs = "Defect number";
                                desc = "Defect picture";
                                break;
                        }
                        string text = "";
                        using (var reader = new System.IO.StreamReader(stream, Portable.Text.Encoding.UTF8))
                        {
                            text = reader.ReadToEnd();
                        }
                        var sb = new StringBuilder();
                        foreach (var item in defekti)
                        {
                            var name = System.IO.Path.GetFileName(item.FilePath);
                            sb.AppendLine($"&nbsp;&nbsp;&nbsp;&nbsp;{numurs}: {item.DefektaNr}, {desc} <a href='{name}'> {name} </a> {(string.IsNullOrWhiteSpace(item.Piezimes) ? "" : "(" + item.Piezimes + ")")}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</br>");
                        }

                        var email = new EmailMessageBuilder()
                              .To("")
                              .Cc(DefektacijasAkts.Epasts)
                              .Subject("Defektācijas akts")
                              .BodyAsHtml("");
                        var temp = text.Replace("$AN$", DefektacijasAkts.AktaNr)
                            .Replace("$AD$", DefektacijasAkts.Adrese)
                            .Replace("$NK$", App.Profils.UznemumaDati)
                            .Replace("$MT$", App.Profils.Currency)
                            .Replace("$TEL$", DefektacijasAkts.Talrunis)
                            .Replace("$MAIL$", DefektacijasAkts.Epasts)
                            .Replace("$HWIDN$", DefektacijasAkts.HighVoltageEquipmentName)
                            .Replace("$HWIDS$", DefektacijasAkts.HighVoltageEquipmentSerial)
                            .Replace("$LWIDN$", DefektacijasAkts.LowVoltageEquipmentName)
                            .Replace("$LWIDS$", DefektacijasAkts.LowVoltageEquipmentSerial)
                            .Replace("$CEAM$", DefektacijasAkts.IekartasKalibracija)
                            .Replace("$WMT$", DefektacijasAkts.MembranasVeids)
                            .Replace("$WMN$", DefektacijasAkts.MembranasNosaukums)
                            .Replace("$WMT$", DefektacijasAkts.MembranasBiezums)
                            .Replace("$PLA$", DefektacijasAkts.ParbaudamaPlatiba.ToString())
                            .Replace("$TIZD$", DefektacijasAkts.TransportaIzdevumi.ToString())
                            .Replace("$DIAG$", DefektacijasAkts.Diagnostika.ToString())
                            .Replace("$VAT$", DefektacijasAkts.Vat.ToString())
                            .Replace("$TOT$", DefektacijasAkts.Kopa.ToString())
                            .Replace("$UZS$", DefektacijasAkts.ParbaudeUzsakta.ToShortDateString() + " " + DefektacijasAkts.ParbaudeUzsakta.ToShortTimeString())
                            .Replace("$PAB$", DefektacijasAkts.ParbaudePabeigta.ToShortDateString() + " " + DefektacijasAkts.ParbaudePabeigta.ToShortTimeString())
                            .Replace("$KST$", DefektacijasAkts.LaiksKopa.ToString())
                            .Replace("$HO$", horiz.ToString())
                            .Replace("$VE$", vert.ToString())
                            .Replace("$KE$", kopa.ToString())
                            .Replace("$DE$", sb.ToString())
                            .Replace("$VUS$", DefektacijasAkts.ParbaudiVeica)
                            .Replace("$LOGO$", logo)
                            .Replace("$PVU$", DefektacijasAkts.PasutitajaParstavis + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img alt='' src='data:image/png;base64," + DefektacijasAkts.Paraksts + "' />");

                        var fi = Xamarin.Forms.DependencyService.Get<ISaveAndLoad>().SaveText("report.html", temp);
                        email.WithAttachment(fi, "report");
                        foreach (var item in defekti)
                        {
                            email.WithAttachment(item.FilePath, "image");
                        }
                        var m = email.Build();
                        emailMessenger.SendEmail(m);


                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
        }

        public DelegateCommand SaveProfileCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand OpenMapCommand { get; set; }
        public DelegateCommand ShareCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        private async void Save()
        {
            var settings = new ImageConstructionSettings
            {
                StrokeColor = Xamarin.Forms.Color.Black,
                BackgroundColor = Xamarin.Forms.Color.White,
                StrokeWidth = 1f
            };

            try
            {
                using (var bitmap = await GetImageStreamAsync(SignatureImageFormat.Png, settings))
                {
                    //var s = Convert.ToBase64String(ReadFully(bitmap));
                    if (bitmap == null)
                    {
                        var a = "";
                    }
                    else
                    {
                        var temp = ImageHelper.ReadFully(bitmap);
                        var uiimage = ImageHelper.ToImage(temp);
                        uiimage = ImageHelper.MaxResizeImage(uiimage, 200, 100);
                        temp = ImageHelper.ToArray(uiimage);
                        DefektacijasAkts.Paraksts = Convert.ToBase64String(temp);
                        RaisePropertyChanged(nameof(Paraksts));
                    }
                }
                App.Database.InsertDefektacijasAkts(DefektacijasAkts);
            }
            catch (Exception ex)
            {

            }
        }

        public ImageSource Paraksts
        {
            get
            {
                if (DefektacijasAkts == null || string.IsNullOrWhiteSpace(DefektacijasAkts.Paraksts))
                    return null;
                var mas = Convert.FromBase64String(DefektacijasAkts.Paraksts);
                var img = ImageSource.FromStream(() => new MemoryStream(mas));

                return img;
            }
        }

        private async void DeleteAsync()
        {

            var tr = new TranslateExtension();

            var r = await _pageDialogService.DisplayActionSheetAsync(tr.GetTranslation("DeleteConfirmationLabel"), tr.GetTranslation("QuestionLabel"), tr.GetTranslation("YesLabel"), tr.GetTranslation("NoLabel"));

            if (r == tr.GetTranslation("YesLabel"))
            {
                var defekti = App.Database.GetDefekti(DefektacijasAkts.Id).ContinueWith(t =>
                {
                    foreach (var def in t.Result)
                    {
                        Xamarin.Forms.DependencyService.Get<ISaveAndLoad>().DeleteFile(def.FilePath);
                        App.Database.DeleteDefekts(def);
                    }
                });
                await App.Database.DeleteDefektacijasAkts(DefektacijasAkts);
                await _navigationService.GoBackAsync();
            }
        }
        private void Cancel()
        {
            _navigationService.GoBackAsync();
        }

        private void OpenMap()
        {
            var param = new NavigationParameters();
            param.Add("lat", DefektacijasAkts.Latitude);
            param.Add("lon", DefektacijasAkts.Longitude);
            param.Add("adrese", DefektacijasAkts.Adrese);

            _navigationService.NavigateAsync("Map", param);
        }


        private DefektacijasAkts _defektacijasAkts;
        public DefektacijasAkts DefektacijasAkts
        {
            get
            {
                return _defektacijasAkts;
            }
            set
            {
                SetProperty(ref _defektacijasAkts, value);
            }
        }


        private Classifiers _highWoltageEquipment;
        public Classifiers HighWoltageEquipment
        {
            get { return _highWoltageEquipment; }
            set
            {
                SetProperty(ref _highWoltageEquipment, value);
                if (value == null)
                {
                    DefektacijasAkts.HighVoltageEquipmentName = null;
                    DefektacijasAkts.HighVoltageEquipmentSerial = null;
                }
                else
                {
                    DefektacijasAkts.HighVoltageEquipmentName = value.Name;
                    DefektacijasAkts.HighVoltageEquipmentSerial = value.Code;
                }
            }
        }

        private Classifiers _lowWoltageEquipment;
        public Classifiers LowWoltageEquipment
        {
            get { return _lowWoltageEquipment; }
            set
            {
                SetProperty(ref _lowWoltageEquipment, value);
                if (value == null)
                {
                    DefektacijasAkts.LowVoltageEquipmentName = null;
                    DefektacijasAkts.LowVoltageEquipmentSerial = null;
                }
                else
                {
                    DefektacijasAkts.LowVoltageEquipmentName = value.Name;
                    DefektacijasAkts.LowVoltageEquipmentSerial = value.Code;
                }
            }
        }
        private ObservableCollection<Classifiers> _iekartas;

        public ObservableCollection<Classifiers> Iekartas
        {
            get
            {
                if (_iekartas == null)
                { _iekartas = new ObservableCollection<Classifiers>(App.Database.GetEquipmentList().Result); }
                return _iekartas;
            }
        }


        private DateTime _uzsaksanasDatums;
        public DateTime UzsaksanasDatums
        {
            get
            {
                if (DefektacijasAkts != null)
                    return DefektacijasAkts.ParbaudeUzsakta.Date;
                else
                    return DateTime.Now;
                //return _uzsaksanasDatums;
            }
            set
            {
                SetProperty(ref _uzsaksanasDatums, value);
                if (DefektacijasAkts != null)
                {
                    var temp = value.Date.AddHours(DefektacijasAkts.ParbaudeUzsakta.Hour).AddMinutes(DefektacijasAkts.ParbaudeUzsakta.Minute);
                    DefektacijasAkts.ParbaudeUzsakta = temp;
                }

            }
        }
        private TimeSpan _uzsaksanasLaiks;
        public TimeSpan UzsaksanasLaiks
        {
            get
            {
                if (DefektacijasAkts != null)
                    return DefektacijasAkts.ParbaudeUzsakta.TimeOfDay;
                else
                    return new TimeSpan();

            }
            set
            {
                SetProperty(ref _uzsaksanasLaiks, value);
                if (DefektacijasAkts != null)
                    DefektacijasAkts.ParbaudeUzsakta = UzsaksanasDatums.Date.AddHours(value.Hours).AddMinutes(value.Minutes);

            }
        }

        private DateTime _pabeigsanasDatums;
        public DateTime PabeigsanasDatums
        {
            get
            {
                if (DefektacijasAkts != null)
                    return DefektacijasAkts.ParbaudePabeigta.Date;
                else
                    return DateTime.Now;
                //return _uzsaksanasDatums;
            }
            set
            {
                SetProperty(ref _pabeigsanasDatums, value);
                if (DefektacijasAkts != null)
                {
                    var temp = value.Date.AddHours(DefektacijasAkts.ParbaudePabeigta.Hour).AddMinutes(DefektacijasAkts.ParbaudePabeigta.Minute);
                    DefektacijasAkts.ParbaudePabeigta = temp;
                }

            }
        }
        private TimeSpan _pabeigsanasLaiks;
        public TimeSpan PabeigsanasLaiks
        {
            get
            {
                if (DefektacijasAkts != null)
                    return DefektacijasAkts.ParbaudePabeigta.TimeOfDay;
                else
                    return new TimeSpan();

            }
            set
            {
                SetProperty(ref _pabeigsanasLaiks, value);
                if (DefektacijasAkts != null)
                    DefektacijasAkts.ParbaudePabeigta = PabeigsanasDatums.Date.AddHours(value.Hours).AddMinutes(value.Minutes);

            }
        }

        public void Izmainits()
        {
            RaisePropertyChanged(nameof(DefektacijasAkts));
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            if (parameters.ContainsKey("lat") && parameters.ContainsKey("lon"))
            {
                DefektacijasAkts.Latitude = (double)parameters["lat"];
                DefektacijasAkts.Longitude = (double)parameters["lon"];
                DefektacijasAkts.Adrese = (string)parameters["adrese"];
                RaisePropertyChanged(nameof(DefektacijasAkts));
            }
            else if (parameters.ContainsKey(nameof(DefektacijasAkts.Id)))
            {
                var id = (int)parameters[nameof(DefektacijasAkts.Id)];
                if (id == 0)
                {
                    Settings _profile = new Settings();
                    var items = App.Database.GetProfile().Result;
                    if (items.Count > 0)
                    {
                        _profile = items[0];
                    }
                    DefektacijasAkts = new DefektacijasAkts()
                    {
                        Talrunis = _profile.Phone,
                        Epasts = _profile.Mail,
                        ParbaudiVeica = _profile.FullName,
                        Vat = _profile.Vat,
                        ParbaudeUzsakta = DateTime.Now.Date,
                        ParbaudePabeigta = DateTime.Now.Date
                    };

                }
                else
                {
                    Settings _profile = new Settings();
                    var items = App.Database.GetProfile().Result;
                    if (items.Count > 0)
                    {
                        _profile = items[0];
                    }
                    DefektacijasAkts = App.Database.GetDefektacijasAkts(id);
                    DefektacijasAkts.Vat = _profile.Vat;
                    if (!string.IsNullOrEmpty(DefektacijasAkts.HighVoltageEquipmentName) || !string.IsNullOrEmpty(DefektacijasAkts.HighVoltageEquipmentSerial))
                    {
                        var h = Iekartas.FirstOrDefault(x => x.Code == DefektacijasAkts.HighVoltageEquipmentSerial && x.Name == DefektacijasAkts.HighVoltageEquipmentName);
                        if (h != null)
                        {
                            HighWoltageEquipment = h;
                            RaisePropertyChanged(nameof(HighWoltageEquipment));
                        }
                        else
                        {
                            h = new Classifiers() { Code = DefektacijasAkts.HighVoltageEquipmentSerial, Name = DefektacijasAkts.HighVoltageEquipmentName };
                            Iekartas.Add(h);
                            HighWoltageEquipment = h;
                            RaisePropertyChanged(nameof(HighWoltageEquipment));
                        }
                    }
                    if (!string.IsNullOrEmpty(DefektacijasAkts.LowVoltageEquipmentName) || !string.IsNullOrEmpty(DefektacijasAkts.LowVoltageEquipmentSerial))
                    {
                        var h = Iekartas.FirstOrDefault(x => x.Code == DefektacijasAkts.LowVoltageEquipmentSerial && x.Name == DefektacijasAkts.LowVoltageEquipmentName);
                        if (h != null)
                        {
                            LowWoltageEquipment = h;
                            RaisePropertyChanged(nameof(LowWoltageEquipment));
                        }
                        else
                        {
                            h = new Classifiers() { Code = DefektacijasAkts.LowVoltageEquipmentSerial, Name = DefektacijasAkts.LowVoltageEquipmentName };
                            Iekartas.Add(h);
                            LowWoltageEquipment = h;
                            RaisePropertyChanged(nameof(LowWoltageEquipment));
                        }
                    }
                    RaisePropertyChanged(nameof(UzsaksanasDatums));
                    RaisePropertyChanged(nameof(UzsaksanasLaiks));
                    RaisePropertyChanged(nameof(PabeigsanasDatums));
                    RaisePropertyChanged(nameof(PabeigsanasLaiks));
                    RaisePropertyChanged(nameof(Paraksts));
                }
                App.Akts = DefektacijasAkts;
            }
        }
    }
}

