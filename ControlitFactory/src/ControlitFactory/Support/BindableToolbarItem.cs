using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ControlitFactory.Support
{
    public class BindableToolbarItem : ToolbarItem
    {

        public BindableToolbarItem()
        {
            InitVisibility();
        }

        private async void InitVisibility()
        {
            OnIsVisibleChanged(this, false, IsVisible);
        }

        public new ContentPage Parent { set; get; }

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static BindableProperty IsVisibleProperty =
            BindableProperty.Create<BindableToolbarItem, bool>(o => o.IsVisible, false, propertyChanged: OnIsVisibleChanged);

        private static void OnIsVisibleChanged(BindableObject bindable, bool oldvalue, bool newvalue)
        {
            var item = bindable as BindableToolbarItem;

            if (item.Parent == null) return;
            var items = ((ContentPage)item.Parent).ToolbarItems;

            if (newvalue && !items.Contains(item))
            {
                items.Add(item);
            }
            else if (!newvalue && items.Contains(item))
            {
                items.Remove(item);
            }
        }
    }
}
