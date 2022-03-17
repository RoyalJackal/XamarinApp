using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Data.Context;
using Data.Models;

namespace XamarinApp.ViewModels.Feeds
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    [QueryProperty(nameof(PetId), nameof(PetId))]
    class NewFeedViewModel : BaseViewModel
    {
        public AppDbContext Db => DependencyService.Get<AppDbContext>();

        private TimeSpan time;
        public TimeSpan Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }

        private int amount;
        public int Amount
        {
            get => amount;
            set => SetProperty(ref amount, value);
        }

        private Fodder fodder;
        public Fodder Fodder
        {
            get => fodder;
            set => SetProperty(ref fodder, value);
        }

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
                if (itemId != default)
                    LoadItemId(value);
            }
        }

        private int petId;
        public int PetId
        {
            get
            {
                return petId;
            }
            set
            {
                petId = value;
                if (petId == default)
                    throw new Exception("Can't create a feed without a pet.");
            }
        }

        public ObservableCollection<Fodder> PickerItems { get; }


        public NewFeedViewModel()
        {
            PickerItems = new ObservableCollection<Fodder>();
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command LoadItemsCommand { get; }


        public async void LoadItemId(int itemId)
        {
            var item = await Db.Feeds.Include(x => x.Fodder).FirstOrDefaultAsync(x => x.Id == itemId);

            if (item == null)
                Debug.WriteLine("Failed to Load Item");
            else
            {
                Time = item.Time;
                Amount = item.Amount;
                Fodder = item.Fodder;
            }
        }

        private bool ValidateSave() =>
            Amount != 0 && Fodder != null;

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            if (ItemId == default)
            {
                var pet = await Db.Pets.FindAsync(petId);
                var feed = new Feed()
                {
                    Time = Time,
                    Fodder = Fodder,
                    Amount = Amount,
                    Pet = pet
                };
                await Db.AddAsync(feed);
            }
            else
            {
                var feed = await Db.Feeds.FindAsync(ItemId);
                feed.Time = Time;
                feed.Fodder = Fodder;
                feed.Amount = Amount;
            }
            await Db.SaveChangesAsync();

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                PickerItems.Clear();
                var items = await Db.Fodders.ToListAsync();
                foreach (var item in items)
                    PickerItems.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
