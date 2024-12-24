using System;
using System.ComponentModel;
using LibVLCSharp.Shared;

namespace MauiVlc.Controls
{
	public class MediaViewer : View
	{
        public event Action PauseRequested;
        public event Action PlayRequested;
        public event Action MuteRequested;
        public event Action SeekToRequested;

        public static readonly BindableProperty VideoUrlProperty = BindableProperty.Create(nameof(VideoUrl)
           , typeof(string)
           , typeof(MediaViewer)
           , ""
           , defaultBindingMode: BindingMode.TwoWay);

        /// <summary>
        /// Disables or enables scanning
        /// </summary>
        /// 

        public string VideoUrl
        {
            get => (string)GetValue(VideoUrlProperty);
            set => SetValue(VideoUrlProperty, value);
        }

        public bool MuteVal;
        public TimeSpan Time;


        public long Length { get; set; }

        public float Position { get; set; }


        public MediaViewer()
		{
		}

        public void Pause()
        {
            PauseRequested?.Invoke();
        }

        public void Play()
        {
            PlayRequested?.Invoke();
        }

        public void Mute(bool val)
        {
            MuteVal = val;
            MuteRequested?.Invoke();
        }


        public void SeekTo(double position)
        {
            Position = (float) position;
            SeekToRequested?.Invoke();
        }
    }
}

