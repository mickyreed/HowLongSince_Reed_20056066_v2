using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Plugin.SimpleAudioPlayer;
using Application = Xamarin.Forms.Application;

namespace HowLongSince_Reed_20056066
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsView : ContentPage, INotifyPropertyChanged
    {
        private MainPage mainPage;
        public Action UpdateEventListTimeDifference { get; set; }
        public Event selectedItem;
        //private YourItem selectedItem;
        //private MainPage viewModel;
        //private MainPage viewModel;


        public Event SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Event> EventList { get; }
        public SelectedItemChangedEventArgs SelectedItemChangedEventArgs { get; }
        public object SelectedItem1 { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public DetailsView()
        //{
        //    InitializeComponent();
        //    //this.viewModel = viewModel;
        //    BindingContext = this;
        //}

        //public DetailsView(Event event)
        //{
        //    InitializeComponent();
        //    varDetailsView
        //}


        //public DetailsView(MainPage viewModel, Event selectedItem)
        //{
        //    InitializeComponent();
        //    //viewModel = viewModel;
        //    mainPage = viewModel;
        //    SelectedItem = selectedItem;
        //    UpdateEventListTimeDifference = mainPage.UpdateEventListTimeDifference;
        //    BindingContext = this;

        //}

        //public DetailsView(ObservableCollection<Event> eventList, Event selectedItem)
        //{
        //    InitializeComponent();
        //    SelectedItem = selectedItem;
        //    BindingContext = this;
        //    this.eventList = eventList;
        //}


        //public DetailsView(ObservableCollection<Event> eventList, SelectedItemChangedEventArgs selectedItemChangedEventArgs)
        //{
        //    EventList = eventList;
        //    SelectedItemChangedEventArgs = selectedItemChangedEventArgs;
        //}

        public DetailsView(ObservableCollection<Event> eventList, object selectedItem, MainPage mainPage)
        {
            InitializeComponent();
            EventList = eventList;
            SelectedItem1 = selectedItem;

            if (mainPage != null)
            {
                UpdateEventListTimeDifference = mainPage.UpdateEventListTimeDifference;
            }
            //UpdateEventListTimeDifference = mainPage.UpdateEventListTimeDifference;
            BindingContext = this;
            //BindingContext = selectedItem1;
        }

        public DetailsView(ObservableCollection<Event> eventList, object selectedItem1)
        {
            EventList = eventList;
        }

        private void ResetTimeButton_Clicked(object sender, EventArgs e)
        {
            PlaySound();
            //selectedItem.StartTime = DateTime.Now;
            // Update the start time of the item in your data storage or collection
            SelectedItem.EventStart = DateTime.Now;
            mainPage.UpdateEventListTimeDifference();
            //UpdateEventListTimeDifference();
        }

        private void StopTimerButton_Clicked(object sender, EventArgs e)
        {
            PlaySound();
            // Implement logic to stop the timer without resetting it
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            PlaySound();
            // Implement logic to delete the item from your data storage or collection
            mainPage.eventList.Remove(SelectedItem);
            mainPage.UpdateEventListTimeDifference();
            //eventList.Remove(SelectedItem);
            //UpdateEventListTimeDifference();
            Navigation.PopModalAsync();
        }

        private async void BackToMainButton_Clicked(object sender, EventArgs e)
        {
            PlaySound();
            await Navigation.PopModalAsync();
        }

        private void PlaySound()
        {
            //if (App.Current.Properties.ContainsKey("IsMuted") && (bool)App.Current.Properties["IsMuted"])
            //{
            //    SoundEffects.PlayTap();
            //}

            var mySound = Preferences.Get("IsMuted", false);
            if (!mySound)
            //if (soundOn)
            {
                SoundEffects.PlayTap();
            }
            else
            {
                return;
            }
        }
    }
}