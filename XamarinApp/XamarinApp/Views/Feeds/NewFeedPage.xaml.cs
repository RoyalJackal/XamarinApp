﻿using Xamarin.Forms;
using XamarinApp.ViewModels.Feeds;

namespace XamarinApp.Views.Feeds
{
    public partial class NewFeedPage : ContentPage
    {
        NewFeedViewModel _viewModel;

        public NewFeedPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new NewFeedViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}