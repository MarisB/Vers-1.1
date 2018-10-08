using System;
using ControlitFactory.ViewModels;
using Xamarin.Forms;

namespace ControlitFactory.Views
{
    public partial class MainPage : CustomToolbarContentPage
    {
        private ToolbarItem _add;
        private ToolbarItem _settings;
        private ToolbarItem _equipment;
        private ToolbarItem _help;
        public override event EventHandler ToolbarItemAdded;
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RefreshToolbarOptions();
        }

        private void RefreshToolbarOptions()
        {
            var viewModel = BindingContext as MainPageViewModel;

            ToolbarItems.Clear();

            if (viewModel != null)
            {
                _add = new ToolbarItem
                {
                    Text = "Pievienot",
                    Icon = "add.png",
                    Command = viewModel.PievienotDefektacijasAktuCommand,
                    Order = ToolbarItemOrder.Primary
                };

                ToolbarItems.Add(_add);

                _settings = new ToolbarItem
                {
                    Text = "Profils",
                    Icon = "add.png",
                    Command = viewModel.NavigateCommand,
                    CommandParameter = nameof(Settings),
                    Order = ToolbarItemOrder.Secondary
                };

                ToolbarItems.Add(_settings);
                _equipment = new ToolbarItem
                {
                    Text = "Iekārtas",
                    Icon = "add.png",
                    Command = viewModel.NavigateCommand,
                    CommandParameter = nameof(Equipment),
                    Order = ToolbarItemOrder.Secondary
                };

                ToolbarItems.Add(_equipment);
                _help = new ToolbarItem
                {
                    Text = "Palīdzība",
                    Icon = "add.png",
                    Command = viewModel.HelpCommand,
                    Order = ToolbarItemOrder.Secondary
                };

                ToolbarItems.Add(_help);
                OnToolbarItemAdded();
            }
        }

        protected void OnToolbarItemAdded()
        {
            var e = ToolbarItemAdded;
            e?.Invoke(this, new EventArgs());
        }
        public override Color CellBackgroundColor => Color.White;

        public override Color CellTextColor => Color.Black;

        public override Color MenuBackgroundColor => Color.White;

        public override float RowHeight => 56;

        public override Color ShadowColor => Color.Black;

        public override float ShadowOpacity => 0.3f;

        public override float ShadowRadius => 5.0f;

        public override float ShadowOffsetDimension => 5.0f;

        public override float TableWidth => 250;
    }
}

