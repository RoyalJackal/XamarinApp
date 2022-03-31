using Data.Context;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using XamarinApp.ViewModels.Base;
using XamarinApp.Views.Fodders;

namespace XamarinApp.ViewModels.Fodders
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class FodderDetailViewModel : BaseViewModel
    {
        public AppDbContext Db => DependencyService.Get<AppDbContext>();

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
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

        public FodderDetailViewModel()
        {
            EditCommand = new Command(OnEdit);
        }

        public async void LoadItemId(int itemId)
        {
            var item = await Db.Fodders.FindAsync(itemId);

            if (item == null)
                Debug.WriteLine("Failed to Load Item");
            else
            {
                Name = item.Name;
            }
        }

        public Command EditCommand { get; }

        public async void OnEdit() =>
            await Shell.Current.GoToAsync($"{nameof(NewFodderPage)}?{nameof(NewFodderViewModel.ItemId)}={ItemId}");
    }
}
