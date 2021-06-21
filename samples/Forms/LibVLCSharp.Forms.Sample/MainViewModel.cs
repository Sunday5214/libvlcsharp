using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LibVLCSharp.Forms.Sample
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Initialize();
        }

        private LibVLC LibVLC { get; set; }

        private MediaPlayer _mediaPlayer;
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            private set => Set(nameof(MediaPlayer), ref _mediaPlayer, value);
        }

        private bool IsLoaded { get; set; }
        private bool IsVideoViewInitialized { get; set; }

        private void Set<T>(string propertyName, ref T field, T value)
        {
            if (field == null && value != null || field != null && !field.Equals(value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Initialize()
        {
            Core.Initialize();

            List<string> argList = new List<string> { "--rtsp-tcp" };
            //argList.Add($"--rtsp-user=bdcon");
            //argList.Add($"--rtsp-pwd=EmctCamera");
            argList.Add("--video-filter=transform");
            argList.Add("--transform-type=90");
            LibVLC = new LibVLC(argList.ToArray());

            var media = new Media(LibVLC, new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));

            MediaPlayer = new MediaPlayer(LibVLC)
            {
                Media = media,
            };

            
            media.Dispose();
        }

        public void OnAppearing()
        {
            MediaPlayer.AspectRatio = "Fit screen";
            MediaPlayer.Scale = 0;
            IsLoaded = true;
            Play();
        }

        internal void OnDisappearing()
        {
            MediaPlayer.Dispose();
            LibVLC.Dispose();
        }

        public void OnVideoViewInitialized()
        {
            IsVideoViewInitialized = true;
            Play();
        }

        private void Play()
        {
            if (IsLoaded && IsVideoViewInitialized)
            {
                MediaPlayer.Play();
            }
        }
    }
}
