using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Data.Context;
using Data.Models;
using XamarinApp.ViewModels.Base;

namespace XamarinApp.ViewModels.Fodders
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class NewFodderViewModel : BaseViewModel
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
                if (itemId != default)
                    LoadItemId(value);
            }
        }

        public async void LoadItemId(int itemId)
        {
            var item = await Db.Fodders
                .Where(i => i.Id == itemId)
                .FirstOrDefaultAsync();

            if (item == null)
                Debug.WriteLine("Failed to Load Item");
            else
            {
                Name = item.Name;
            }
        }

        public NewFodderViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave() =>
            !String.IsNullOrWhiteSpace(name);


        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {           
            if (ItemId == default)
            {
                var fodder = new Fodder()
                {
                    Name = Name
                };
                await Db.AddAsync(fodder);
            }
            else
            {
                var fodder = await Db.Fodders.FindAsync(ItemId);
                fodder.Name = Name;
            }
            await Db.SaveChangesAsync();

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
