using System;
using Plugin.AudioRecorder;
using Xamarin.Essentials;

namespace XamarinApp.Services
{
    public class AudioService
    {
        public AudioRecorderService Recorder { get; }
        public AudioPlayer Player { get; }

        public AudioService()
        {
            Recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = false, //will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
                TotalAudioTimeout = TimeSpan.FromSeconds(5), //audio will stop recording after 15 seconds
            };
            Player = new AudioPlayer();
        }
    }
}