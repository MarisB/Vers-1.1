using Prism.Mvvm;
using System;
using SQLite;


namespace ControlitFactory.Models
{
    public class DefektacijasAkts : BindableBase
    {
        public DefektacijasAkts()
        {

        }

        private int _id;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _aktaNr;
        public string AktaNr
        {
            get { return _aktaNr; }
            set { SetProperty(ref _aktaNr, value); }
        }

        private string _adrese;
        public string Adrese
        {
            get {
                return _adrese;
            }
            set {
                SetProperty(ref _adrese, value);
                RaisePropertyChanged(nameof(GPSLabel));
            }
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string GPSLabel => (Latitude > 0 && Longitude > 0) ? $"GPS {Latitude}, {Longitude}" : "";

        private string _talrunis;
        public string Talrunis
        {
            get { return _talrunis; }
            set { SetProperty(ref _talrunis, value); }
        }

        private string _epasts;
        public string Epasts
        {
            get { return _epasts; }
            set { SetProperty(ref _epasts, value); }
        }

        private decimal? _parbaudamaPlatiba;
        public decimal? ParbaudamaPlatiba
        {
            get { return _parbaudamaPlatiba; }
            set { SetProperty(ref _parbaudamaPlatiba, value); }
        }

        private decimal? _transportaIzdevumi;
        public decimal? TransportaIzdevumi
        {
            get { return _transportaIzdevumi; }
            set { SetProperty(ref _transportaIzdevumi, value);}
        }

        private decimal? _diagnostika;
        public decimal? Diagnostika
        {
            get { return _diagnostika; }
            set { SetProperty(ref _diagnostika, value);}
        }
        private decimal? _kopa;
        public decimal? Kopa
        {
            get { return _kopa; }
            set { SetProperty(ref _kopa, value); }
        }

        private string _lowVoltageEquipmentName;
        public string LowVoltageEquipmentName
        {
            get { return _lowVoltageEquipmentName; }
            set { SetProperty(ref _lowVoltageEquipmentName, value); }
        }

        private string _lowVoltageEquipmentSerial;
        public string LowVoltageEquipmentSerial
        {
            get { return _lowVoltageEquipmentSerial; }
            set { SetProperty(ref _lowVoltageEquipmentSerial, value); }
        }

        private string _highVoltageEquipmentName;
        public string HighVoltageEquipmentName
        {
            get { return _highVoltageEquipmentName; }
            set { SetProperty(ref _highVoltageEquipmentName, value); }
        }

        private string _highVoltageEquipmentSerial;
        public string HighVoltageEquipmentSerial
        {
            get { return _highVoltageEquipmentSerial; }
            set { SetProperty(ref _highVoltageEquipmentSerial, value); }
        }




        public void AprekinatKopa()
        {
            decimal temp = 0;
            if (Diagnostika.HasValue && TransportaIzdevumi.HasValue)
            {
                temp = Diagnostika.Value + TransportaIzdevumi.Value;
            }
            else if (Diagnostika.HasValue)
            {
                temp = Diagnostika.Value;
            }
            else if (TransportaIzdevumi.HasValue)
            {
                temp = TransportaIzdevumi.Value;
            }
            if(temp > 0)
            {
                if (Vat.HasValue)
                {
                    temp = temp * (Vat.Value / 100 + 1);
                }
                Kopa = temp;
            }
        }

        private DateTime _parbaudeUzsakta;
        public DateTime ParbaudeUzsakta
        {
            get { return _parbaudeUzsakta; }
            set
            {
                SetProperty(ref _parbaudeUzsakta, value);
            }
        }
        
        private DateTime _parbaudePabeigta;
        public DateTime ParbaudePabeigta
        {
            get { return _parbaudePabeigta; }
            set {
                SetProperty(ref _parbaudePabeigta, value);
            }
        }
        public void AprekinatLaiku()
        {
            if (ParbaudeUzsakta < ParbaudePabeigta && ParbaudeUzsakta > new DateTime(2017,1,1) && ParbaudePabeigta > new DateTime(2017, 1, 1))
            {
                var laiks = ParbaudePabeigta - ParbaudeUzsakta;
                LaiksKopa = decimal.Round(Convert.ToDecimal(laiks.TotalHours),2);
            }
        }
        private decimal? _laiksKopa;
        public decimal? LaiksKopa
        {
            get { return _laiksKopa; }
            set { SetProperty(ref _laiksKopa, value); }
        }

        private string _parbaudiVeica;
        public string ParbaudiVeica
        {
            get { return _parbaudiVeica; }
            set { SetProperty(ref _parbaudiVeica, value); }
        }


        private string _iekartasKalibracija;
        public string IekartasKalibracija
        {
            get { return _iekartasKalibracija; }
            set { SetProperty(ref _iekartasKalibracija, value); }
        }


        private string _membranasVeids;
        public string MembranasVeids
        {
            get { return _membranasVeids; }
            set { SetProperty(ref _membranasVeids, value); }
        }

        private string _membranasNosaukums;
        public string MembranasNosaukums
        {
            get { return _membranasNosaukums; }
            set { SetProperty(ref _membranasNosaukums, value); }
        }


        private string _membranasBiezums;
        public string MembranasBiezums
        {
            get { return _membranasBiezums; }
            set { SetProperty(ref _membranasBiezums, value); }
        }

        public decimal? Vat { get; set; }

        private string _pasutitajaParstavis;
        public string PasutitajaParstavis
        {
            get { return _pasutitajaParstavis; }
            set { SetProperty(ref _pasutitajaParstavis, value); }
        }

        private string _imageFolder;
        private string _paraksts;

        public string ImageFolder
        {
            get { return _imageFolder; }
            set { SetProperty(ref _imageFolder, value); }
        }

        public string Paraksts
        {
            get { return _paraksts; }
            set { SetProperty(ref _paraksts, value); }
        }

        public override string ToString()
        {
            return Adrese ?? "";
        }

    }
}
