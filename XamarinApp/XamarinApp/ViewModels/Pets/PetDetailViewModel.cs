using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.ViewModels.Feeds;
using XamarinApp.Views.Feeds;
using XamarinApp.Views.Pets;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensions;
using SQLiteNetExtensionsAsync.Extensions;
using SQLiteNetExtensionsAsync;


namespace XamarinApp.ViewModels.Pets
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class PetDetailViewModel : BaseViewModel
    {
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string breed;
        public string Breed
        {
            get => breed;
            set => SetProperty(ref breed, value);
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
                LoadItemId(value);
            }
        }

        public PetDetailViewModel()
        {
            Items = new ObservableCollection<Feed>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            EditCommand = new Command(OnEdit);
            ItemTapped = new Command<Feed>(OnItemSelected);
            AddItemCommand = new Command(OnAddItem);
        }

        public async void LoadItemId(int itemId)
        {
            var item = await App.Db.Table<Pet>()
                .Where(i => i.Id == itemId)
                .FirstOrDefaultAsync();

            if (item == null)
                Debug.WriteLine("Failed to Load Item");
            else
            {
                Name = item.Name;
                Breed = item.Breed;
            }
        }

        public Command EditCommand { get; }

        public async void OnEdit() =>
            await Shell.Current.GoToAsync($"{nameof(NewPetPage)}?{nameof(NewPetViewModel.ItemId)}={ItemId}");

        #region FeedList

        private Feed _selectedItem;

        public ObservableCollection<Feed> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Feed> ItemTapped { get; }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                //var items = await App.Db.GetAllWithChildrenAsync<Feed>();
                var items = await App.Db.Table<Feed>().Where(x => x.PetId == ItemId).ToListAsync();
                foreach (var item in items)
                    Items.Add(item);
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
            SelectedItem = null;
        }

        public Feed SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(NewFeedPage)}?{nameof(NewFeedViewModel.PetId)}={ItemId}");
        }

        void OnItemSelected(Feed item)
        {
            if (item == null)
                return;
            //await Shell.Current.GoToAsync($"{nameof(FeedDetailPage)}?{nameof(FeedDetailViewModel.ItemId)}={item.Id}");
        }

        #endregion
    }
}
