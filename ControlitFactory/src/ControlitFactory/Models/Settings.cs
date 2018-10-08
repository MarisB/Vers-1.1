using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControlitFactory.Models
{
    public class Settings: BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _surname;
        public string Surname
        {
            get { return _surname; }
            set { SetProperty(ref _surname, value); }
        }

        private string _currency;

        public string Currency
        {
            get { return _currency; }
            set { SetProperty(ref _currency, value); }
        }
        public string FullName => ((Name ?? "") + " " + (Surname ?? "") + " " + (SertificateNumber ?? "")).Trim();

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        private string _mail;
        public string Mail
        {
            get { return _mail; }
            set { SetProperty(ref _mail, value); }
        }

        private string _sertificateNumber;
        public string SertificateNumber
        {
            get { return _sertificateNumber; }
            set { SetProperty(ref _sertificateNumber, value); }
        }

        private decimal? _vat;
        private string valoda;
        private string logo;
        private string uznemumaDati;

        public decimal? Vat
        {
            get { return _vat; }
            set { SetProperty(ref _vat, value); }
        }

        public string Valoda
        {
            get { return valoda; }
            set { SetProperty(ref valoda, value); }
        }

        public string Logo
        {
            get { return logo; }
            set { SetProperty(ref logo, value); }
        }

        public string UznemumaDati
        {
            get { return uznemumaDati; }
            set { SetProperty(ref uznemumaDati, value); }
        }

    }
}
