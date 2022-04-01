using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinApp.ViewModels.Feeds;
using XamarinApp.Views.Feeds;
using XamarinApp.Views.Pets;
using Data.Context;
using Data.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using XamarinApp.ViewModels.Base;

namespace XamarinApp.ViewModels.Pets
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class PetDetailViewModel : BaseViewModel
    {
        public AppDbContext Db => DependencyService.Get<AppDbContext>();

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
        
        private string image;
        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
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
            var item = await Db.Pets.FindAsync(itemId);

            if (item == null)
                Debug.WriteLine("Failed to Load Item");
            else
            {
                Name = item.Name;
                Breed = item.Breed;
                Image = item.Image;
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
                var items = await Db.Feeds
                    .Include(x => x.Pet)
                    .Include(x => x.Fodder)
                    .Where(x => x.Pet.Id == ItemId)
                    .ToListAsync();
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

        async void OnItemSelected(Feed item)
        {
            if (item == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(FeedDetailPage)}?{nameof(FeedDetailViewModel.ItemId)}={item.Id}");
        }

        #endregion
    }
}
