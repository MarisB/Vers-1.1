using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ControlitFactory.Support
{
    public class NumericTextBox : Entry
    {

        #region Bindables
        public static readonly BindableProperty NumericValueProperty = BindableProperty.Create(
            "NumericValue",
            typeof(decimal?),
            typeof(NumericTextBox),
            null,
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                SetDisplayFormat((NumericTextBox)bindable);
            }
        );

        public static readonly BindableProperty NumericValueFormatProperty = BindableProperty.Create(
            "NumericValueFormat",
            typeof(string),
            typeof(NumericTextBox),
            "N0",
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                SetDisplayFormat((NumericTextBox)bindable);
            }
        );
        #endregion

        #region Constructor
        public NumericTextBox()
        {
            Keyboard = Keyboard.Numeric;
            Focused += OnFocused;
            Unfocused += OnUnfocused;
        }

        private void NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Events
        private void OnFocused(object sender, FocusEventArgs e)
        {
            SetEditFormat(this);
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            var numberFormant = CultureInfo.InvariantCulture.NumberFormat;
            var _text = Text.Replace(".", numberFormant.NumberDecimalSeparator).Replace(",", numberFormant.NumberDecimalSeparator);

            if (decimal.TryParse(_text, NumberStyles.AllowDecimalPoint, numberFormant, out decimal numericValue))
            {
                int round = Convert.ToInt32(NumericValueFormat.Substring(1));
                NumericValue = Math.Round(numericValue, round);
            }
            else
            {
                NumericValue = null;
            }

            SetDisplayFormat(this);
        }
        
        #endregion

        #region Properties
        public decimal? NumericValue
        {
            get { return (decimal?)GetValue(NumericValueProperty); }
            set { SetValue(NumericValueProperty, value); }
        }

        public string NumericValueFormat
        {
            get { return (string)GetValue(NumericValueFormatProperty) ?? "N0"; }
            set
            {
                var _value = string.IsNullOrWhiteSpace(value) ? "N0" : value;
                SetValue(NumericValueFormatProperty, value);
            }
        }
        #endregion

        #region Methods
        private static void SetDisplayFormat(NumericTextBox textBox)
        {
            if (textBox.NumericValue.HasValue)
            {
                textBox.Text = textBox.NumericValue.Value.ToString(textBox.NumericValueFormat, CultureInfo.DefaultThreadCurrentCulture);
            }
            else
            {
                textBox.Text = string.Empty;
            }
        }

        private static void SetEditFormat(NumericTextBox textBox)
        {
            if (textBox.NumericValue.HasValue)
            {
                var numberFormant = CultureInfo.InvariantCulture.NumberFormat;
                textBox.Text = textBox.NumericValue.Value.ToString(textBox.NumericValueFormat, CultureInfo.InvariantCulture).Replace(numberFormant.NumberGroupSeparator, string.Empty);
            }
            else
            {
                textBox.Text = string.Empty;
            }
        }
        #endregion
    }
}
