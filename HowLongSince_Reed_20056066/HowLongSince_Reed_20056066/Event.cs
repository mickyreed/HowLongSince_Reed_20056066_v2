using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace HowLongSince_Reed_20056066
{
    public class Event : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string eventName;
        public string EventName
        {
            get { return eventName; }
            set
            {
                if (eventName != value)
                {
                    eventName = value;
                    OnPropertyChanged();
                }
            }
        }
 
        private DateTime eventStart;
        public DateTime EventStart
        {
            get { return eventStart; }
            set
            {
                if (eventStart != value)
                {
                    eventStart = value;
                    OnPropertyChanged();
                }
            }
        }

        private string timeSinceStart;

        public string TimeSinceStart
        {
            get { return timeSinceStart; }
            set
            {
                if (timeSinceStart != value)
                {
                    timeSinceStart = value;
                    OnPropertyChanged();
                    //OnPropertyChanged(nameof(TimeSinceStart));
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
