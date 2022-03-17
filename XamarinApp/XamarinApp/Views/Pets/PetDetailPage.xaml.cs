﻿using Xamarin.Forms;
using XamarinApp.ViewModels.Pets;

namespace XamarinApp.Views.Pets
{
    public partial class PetDetailPage : ContentPage
    {
        PetDetailViewModel _viewModel;

        public PetDetailPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new PetDetailViewModel();
        }

        protected override void OnAppearing()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}