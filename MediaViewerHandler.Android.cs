using System;
using LibVLCSharp.Platforms.Android;
using LibVLCSharp.Shared;
using MauiVlc.Controls;
using Microsoft.Maui.Handlers;

namespace MauiVlc.Handlers
{
    public partial class MediaViewerHandler : ViewHandler<MediaViewer, VideoView>
    {
        VideoView _videoView;
        LibVLC _libVLC;
        LibVLCSharp.Shared.MediaPlayer _mediaPlayer;

        protected override VideoView CreatePlatformView() => new VideoView(Context);

        protected override void ConnectHandler(VideoView nativeView)
        {
            base.ConnectHandler(nativeView);

            PrepareControl(nativeView);
            HandleUrl(VirtualView.VideoUrl);
            //_mediaPlayer.Play();

            base.ConnectHandler(nativeView);
        }

        private void VirtualView_PlayRequested()
        {
            PrepareControl(_videoView);
            HandleUrl(VirtualView.VideoUrl);
            //_mediaPlayer.Play();
        }

        private void VirtualView_PauseRequested()
        {
            _mediaPlayer.Pause();
        }
        
        private void VirtualView_MuteRequested()
        {
            //HandleMute(!_mediaPlayer.Mute);
            HandleMute(VirtualView.MuteVal);
        }

        private void VirtualView_SeekToRequested()
        {
            _mediaPlayer.SeekTo(TimeSpan.FromMilliseconds(VirtualView.Position * _mediaPlayer.Length));
        }

        protected override void DisconnectHandler(VideoView nativeView)
        {
            VirtualView.PauseRequested -= VirtualView_PauseRequested;
            nativeView.Dispose();
            base.DisconnectHandler(nativeView);
        }

        private void PrepareControl(VideoView nativeView)
        {
            _libVLC = new LibVLC(enableDebugLogs: true);
            _mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC)
            {
                EnableHardwareDecoding = true
            };

            _videoView = nativeView ?? new VideoView(Context);
            _videoView.MediaPlayer = _mediaPlayer;

            VirtualView.Length = _mediaPlayer.Length;
            VirtualView.Position = _mediaPlayer.Position;

            VirtualView.PauseRequested += VirtualView_PauseRequested;
            VirtualView.PlayRequested += VirtualView_PlayRequested;
            VirtualView.MuteRequested += VirtualView_MuteRequested;
            VirtualView.SeekToRequested += VirtualView_SeekToRequested;
        }

        private void HandleUrl(string url)
        {
            try
            {

                if (url.EndsWith("/"))
                {
                    url = url.TrimEnd('/');
                }

                //url = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4";

                if (!string.IsNullOrEmpty(url))
                {
                    var media = new Media(_libVLC, url, FromType.FromLocation);

                    _mediaPlayer.NetworkCaching = 1500;

                    if (_mediaPlayer.Media != null)
                    {
                        _mediaPlayer.Stop();
                        _mediaPlayer.Media.Dispose();
                    }

                    _mediaPlayer.Media = media;
                    _mediaPlayer.Mute = false;
                    _mediaPlayer.Play();
                }
            }
            catch (Exception)
            {
            }
        }

        private void HandleMute(bool mute)
        {
            try
            {
                _mediaPlayer.Mute = mute;
            }
            catch (Exception)
            {
            }
        }
    }
}

