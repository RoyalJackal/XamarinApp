using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinApp.Models;

namespace XamarinApp.ViewModels.Feeds
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    [QueryProperty(nameof(PetId), nameof(PetId))]
    class NewFeedViewModel : BaseViewModel
    {
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
            var item = await App.Db.Table<Feed>()
                .Where(i => i.Id == itemId)
                .FirstOrDefaultAsync();

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
            var newItem = new Feed()
            {
                Id = ItemId,
                Time = Time,
                Fodder = Fodder,
                Amount = Amount,
                PetId = PetId
            };

            if (newItem.Id != 0)
                await App.Db.UpdateWithChildrenAsync(newItem);
            else
                await App.Db.InsertWithChildrenAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                PickerItems.Clear();
                var items = await App.Db.Table<Fodder>().ToListAsync();
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
