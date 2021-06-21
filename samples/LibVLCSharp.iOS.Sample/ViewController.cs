using LibVLCSharp.Platforms.iOS;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using UIKit;

namespace LibVLCSharp.iOS.Sample
{
    public class ViewController : UIViewController
    {
        VideoView _videoView;
        LibVLC _libVLC;
        Shared.MediaPlayer _mediaPlayer;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            List<string> argList = new List<string> { "--rtsp-tcp" };
            argList.Add("--video-filter=transform");
            argList.Add("--transform-type=90");
            _libVLC = new LibVLC(argList.ToArray());
            _mediaPlayer = new Shared.MediaPlayer(_libVLC);

            _videoView = new VideoView { MediaPlayer = _mediaPlayer };

            View = _videoView;

            using var media = new Media(_libVLC, new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));
            _videoView.MediaPlayer.Play(media);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            _mediaPlayer.Dispose();
            _libVLC.Dispose();
        }
    }
}
