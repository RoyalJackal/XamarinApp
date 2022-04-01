using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamarinApp.Services
{
    public class MediaService
    {
        public async Task<string> TakePhotoAsync()
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            await SavePhotoAsync(photo);
            Console.WriteLine($"CapturePhotoAsync COMPLETED: {photo.FullPath}");
                
            return photo.FullPath;
        }
        
        private async Task SavePhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);
        }
    }
}