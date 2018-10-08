using Prism.Mvvm;
using System;
using SQLite;
using System.IO;
using Xamarin.Forms;

namespace ControlitFactory.Models
{
    public class Defekts : BindableBase
    {
        public Defekts()
        {

        }
        private int _id;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _defektacijasAktaId;
        public int DefektacijasAktaId
        {
            get { return _defektacijasAktaId; }
            set { SetProperty(ref _defektacijasAktaId, value); }
        }

        private int _defektaNr;
        public int DefektaNr
        {
            get { return _defektaNr; }
            set { SetProperty(ref _defektaNr, value); }
        }

        private int _novietojums;
        public int Novietojums
        {
            get { return _novietojums; }
            set { SetProperty(ref _novietojums, value); }
        }        
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                SetProperty(ref _filePath, value);
                if(value != null)
                {
                    BildeAttelosanai = ImageSource.FromFile(value);
                }
            }
        }

        private ImageSource _bilde;
        [Ignore]
        public ImageSource BildeAttelosanai
        {
            get
            {                
                return _bilde;
            }
            set
            {
                SetProperty(ref _bilde, value);
            }
        }

        private string _piezimes;

        public string Piezimes
        {
            get { return _piezimes; }
            set { SetProperty(ref _piezimes, value); }
        }


        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Ignore]
        public string GPSLabel => (Latitude > 0 && Longitude > 0) ? $"GPS {Latitude}, {Longitude}" : "";

        public override string ToString()
        {
            return "Defect number " + DefektaNr;
        }

    }
}
