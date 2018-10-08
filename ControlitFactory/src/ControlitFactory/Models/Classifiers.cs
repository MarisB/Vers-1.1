using Prism.Mvvm;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControlitFactory.Models
{
    public class Classifiers : BindableBase
    {

        private int _id;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private string _code;
        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        public override string ToString()
        {
            return (Code ?? "") + " " + (Name ?? "");
        }
    }
}
