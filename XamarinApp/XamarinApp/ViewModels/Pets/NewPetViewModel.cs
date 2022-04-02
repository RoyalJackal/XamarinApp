using System;
using System.Diagnostics;
using Xamarin.Forms;
using Data.Context;
using Data.Models;
using XamarinApp.Services;
using XamarinApp.ViewModels.Base;

namespace XamarinApp.ViewModels.Pets
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class NewPetViewModel : BaseViewModel
    {
        public AppDbContext Db => DependencyService.Get<AppDbContext>();
        public AlertService Alerts => DependencyService.Get<AlertService>();
        public MediaService Media => DependencyService.Get<MediaService>();
        public AudioService AudioService => DependencyService.Get<AudioService>();



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
        
        private string audio;
        public string Audio
        {
            get => audio;
            set => SetProperty(ref audio, value);
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

        public NewPetViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            NewImageCommand = new Command(GetNewImage);
            RecordAudioCommand = new Command(OnRecordAudio);
            PlayAudioCommand = new Command(OnPlayAudio);
            StopAudioCommand = new Command(OnStopAudio);
            PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command NewImageCommand { get; }
        public Command RecordAudioCommand { get; }
        public Command PlayAudioCommand { get; }
        public Command StopAudioCommand { get; }


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
                Audio = item.Audio;
            }
        }

        private bool ValidateSave() =>
            !string.IsNullOrWhiteSpace(name);

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {            
            if (ItemId == default)
            {
                var pet = new Pet
                {
                    Name = Name,
                    Breed = Breed,
                    Image = Image,
                    Audio = Audio
                };
                await Db.AddAsync(pet);
            }
            else
            {
                var pet = await Db.Pets.FindAsync(ItemId);
                pet.Name = Name;
                pet.Breed = Breed;
                pet.Image = Image;
                pet.Audio = Audio;
            }
            await Db.SaveChangesAsync();

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        public async void GetNewImage()
        {
            Image = await Media.TakePhotoAsync();
        }

        public async void OnRecordAudio()
        {
            try
            {
                if (!AudioService.Recorder.IsRecording) //Record button clicked
                {
                    //start recording audio
                    var audioRecordTask = await AudioService.Recorder.StartRecording ();

                    await audioRecordTask;
                }
                else //Stop button clicked
                {
                    //stop the recording...
                    await AudioService.Recorder.StopRecording();
                    Audio = AudioService.Recorder.GetAudioFilePath();
                }
            }
            catch (Exception ex)
            {
                await Alerts.ShowErrorAsync(ex.Message);
            }
        }

        public async void OnPlayAudio()
        {
            try
            {
                if (Audio != null)
                {
                    AudioService.Player.Play(Audio);
                }
            }
            catch (Exception ex)
            {
                await Alerts.ShowErrorAsync(ex.Message);
            }
        }

        public void OnStopAudio() =>
            AudioService.Player.Pause();
    }
}
