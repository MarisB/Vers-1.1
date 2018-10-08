using System;
using System.IO;
using ControlitFactory.Helpers;
using ControlitFactory.Support;
using ControlitFactory.ViewModels;
using CoreGraphics;
using Plugin.Media;
using UIKit;
using Xamarin.Forms;

namespace ControlitFactory.Views
{
    public partial class Settings : ContentPage
    {
        private SettingsViewModel vm;
        public Settings()
        {
            InitializeComponent();
            vm = (SettingsViewModel)this.BindingContext;
            if (!vm.IsVisible)
            {
                this.ToolbarItems.Remove(backIcon);
            }
            PievienotLabel.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("no upload", "picking a photo is not supported", "ok");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync();
                if (file == null)
                    return;
                var str = file.GetStream();
                var temp = ImageHelper.ReadFully(str);
                var uiimage = ImageHelper.ToImage(temp);
                uiimage = ImageHelper.MaxResizeImage(uiimage, 300, 200);
                temp = ImageHelper.ToArray(uiimage);
                vm.Profile.Logo = System.Convert.ToBase64String(temp);
                vm.UpadteLogo();
            };
            languagePicker.SelectedIndexChanged += LanguagePicker_SelectedIndexChanged;
        }

        private void LanguagePicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var tr = new TranslateExtension(languagePicker.SelectedItem.ToString());
            LanguageLabel.Text = tr.GetTranslation(nameof(LanguageLabel));
            NameLabel.Text = tr.GetTranslation(nameof(NameLabel));
            SurnameLabel.Text = tr.GetTranslation(nameof(SurnameLabel));
            PhoneLabel.Text = tr.GetTranslation(nameof(PhoneLabel));
            MailLabel.Text = tr.GetTranslation(nameof(MailLabel));
            SertificateNumberLabel.Text = tr.GetTranslation(nameof(SertificateNumberLabel));
            VatLabel.Text = tr.GetTranslation(nameof(VatLabel));
            LogoLabel.Text = tr.GetTranslation(nameof(LogoLabel));
            PievienotLabel.Text = tr.GetTranslation("PievienotLbel");
            UznemumaDatiLabel.Text = tr.GetTranslation(nameof(UznemumaDatiLabel));
            CurrencyLabel.Text = tr.GetTranslation(nameof(CurrencyLabel));
        }
    }
}
