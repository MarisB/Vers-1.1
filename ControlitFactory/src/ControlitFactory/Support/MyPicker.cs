using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ControlitFactory.Support
{
    public class MyPicker : Picker
    {
        public MyPicker()
        {
        }

        public static readonly BindableProperty PickItemsProperty =
            BindableProperty.Create("PickItems", typeof(bool), typeof(MyPicker), false);

        public bool PickItems
        {
            get
            {
                return (bool)GetValue(PickItemsProperty);
            }
            set
            {
                this.SetValue(PickItemsProperty, value);
            }
        }
    }
}
