using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using XamarinApp.ViewModels.Base;
using XamarinApp.Views.Feeds;

namespace XamarinApp.ViewModels.Feeds
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class FeedDetailViewModel : BaseViewModel
    {
        public AppDbContext Db => DependencyService.Get<AppDbContext>();

        private string fodderName;
        public string FodderName
        {
            get => fodderName;
            set => SetProperty(ref fodderName, value);
        }

        private int amount;
        public int Amount
        {
            get => amount;
            set => SetProperty(ref amount, value);
        }

        private TimeSpan time;
        public TimeSpan Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }

        private int petId;

        private int itemId;
        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public FeedDetailViewModel()
        {
            EditCommand = new Command(OnEdit);
        }

        public async void LoadItemId(int itemId)
        {
            var item = await Db.Feeds
                .Include(x => x.Fodder)
                .Include(x => x.Pet)
                .FirstOrDefaultAsync(x => x.Id == itemId);

            if (item == null)
                Debug.WriteLine("Failed to Load Item");
            else
            {
                FodderName = item.Fodder.Name;
                Amount = item.Amount;
                Time = item.Time;
                petId = item.Pet.Id;
            }
        }

        public Command EditCommand { get; }

        public async void OnEdit() =>
            await Shell.Current.GoToAsync($"{nameof(NewFeedPage)}?{nameof(NewFeedViewModel.ItemId)}={ItemId}&{nameof(NewFeedViewModel.PetId)}={petId}");
    }
}
