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
using System.Diagnostics;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

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

        public new event PropertyChangedEventHandler PropertyChanged;

        protected new virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
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
            App.Current.Properties["activeScreen"] = 0;
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

        protected override void OnAppearing()
        {
            App.Current.Properties["activeScreen"] = 0;


            base.OnAppearing();
            var myTheme = Preferences.Get("IsDarkMode", true);
            var myCustom = Preferences.Get("IsCustom", true);
            //var myFontSize = Preferences.Get("FontSize", );

            CheckFontSize();

            //if (!App.Current.Resources.ContainsKey("FontSize"))
            //{
            //    App.Current.Resources["FontSize"] = "Default";
            //    Debug.WriteLine(App.Current.Resources["FontSize"]);
            //}
            //else
            //{
            //    var FontSize = App.Current.Resources["FontSize"];
            //    App.Current.Resources["FontSize"] = FontSize;
            //    Debug.WriteLine(App.Current.Resources["FontSize"]);
            //}



            if (myTheme && !myCustom)
            {
                Debug.WriteLine("on appearing DARK");

                SetThemeDark();
            }
            else if (!myTheme && !myCustom)
            {
                Debug.WriteLine("on appearing LIGHT");
                SetThemeLight();
            }
            else
            {
                Debug.WriteLine("main myTheme: " + myTheme);
                Debug.WriteLine("main myCustom: " + myCustom);
                Debug.WriteLine("main on appearing CUSTOM");

                SetThemeCustom();
            }
        }

        private void SetThemeLight()
        {
            //OUTER FRAME COLOURS
            App.Current.Resources["MainBackground"] = "#ebf1f5"; //ghost white /grey tinge
            App.Current.Resources["OuterBorder"] = "Black";

            //HEADER COLOURS
            App.Current.Resources["HeaderText"] = "#2196F3"; //lt blue

            //DISPLAYS
            App.Current.Resources["DisplayBackground"] = "#ffffff"; //wht subtle
            App.Current.Resources["DisplayBorder"] = "Black";
            App.Current.Resources["BlackText"] = "Black";
            App.Current.Resources["Buttons"] = "White";
            App.Current.Resources["ButtonBorder"] = "#769cbc"; //med blue

            App.Current.Resources["TipFrameLabelText"] = "#2196F3";
            App.Current.Resources["SliderBackground"] = "#ffffff";
            App.Current.Resources["SliderColour"] = "#2196F3";
            App.Current.Resources["TipLabelText"] = "#2196F3";
            App.Current.Resources["PercentageLabelText"] = "#2196F3";
            App.Current.Resources["SplitCostLabelText"] = "#2196F3";

            App.Current.Resources["StepperBackground"] = "White";
            App.Current.Resources["StepperColour"] = "#2196F3";

            App.Current.Resources["PayeeNumberLabelText"] = "#2196F3";

            App.Current.Resources["PayeeTitleLabelText"] = "#2196F3";

            App.Current.Resources["TotalSplitAmountLabelText"] = "#2196F3";

            App.Current.Resources["PayeeEachLabelText"] = "#2196F3";

        }
        private void SetThemeDark()
        {
            //OUTER FRAME COLOURS
            App.Current.Resources["MainBackground"] = "#333"; //dk grey
            App.Current.Resources["OuterBorder"] = "#2196F3"; //lt blue

            //HEADER COLOURS
            App.Current.Resources["HeaderText"] = "#2196F9"; //lt blue

            //DISPLAYS
            App.Current.Resources["DisplayBackground"] = "#4A4A4A"; // med grey
            App.Current.Resources["DisplayBorder"] = "#2196F3"; //lt blue
            App.Current.Resources["BlackText"] = "White"; //white
            App.Current.Resources["Buttons"] = "#222222"; //vry dk grey
            App.Current.Resources["ButtonBorder"] = "#769cbc";

            App.Current.Resources["TipFrameLabelText"] = "#2196F3"; //Lte blue
            App.Current.Resources["SliderBackground"] = "#769cbc"; //med blue
            App.Current.Resources["SliderColour"] = "White";
            App.Current.Resources["TipLabelText"] = "White";
            App.Current.Resources["PercentageLabelText"] = "White";
            App.Current.Resources["SplitCostLabelText"] = "White";

            App.Current.Resources["StepperBackground"] = "#769cbc"; //med blue
            App.Current.Resources["StepperColour"] = "White";
            App.Current.Resources["PayeeNumberLabelText"] = "White";
            App.Current.Resources["PayeeTitleLabelText"] = "White";
            App.Current.Resources["TotalSplitAmountLabelText"] = "White";
            App.Current.Resources["PayeeEachLabelText"] = "White";

        }

        private void SetThemeCustom()
        {
            //OUTER FRAME COLOURS
            //if (Application.Current.Properties.ContainsKey("MainBackground"))
            //{
            //    String mainBackgroundHex = App.Current.Properties["MainBackground"].ToString();
            //    Debug.WriteLine(mainBackgroundHex + "MAIN BACKGROUND");
            //    App.Current.Resources["MainBackground"] = mainBackgroundHex;//"#" + hex;
            //    App.Current.Resources["OuterBorder"] = "BLACK";
            //}

            if (Application.Current.Properties.ContainsKey("DisplayBackground"))
            {
                String displayBackgroundHex = Application.Current.Properties["CustomMainBackground"].ToString();
                App.Current.Resources["DisplayBackground"] = displayBackgroundHex;
                Debug.WriteLine(displayBackgroundHex);
            }

            //HEADER COLOURS
            if (Application.Current.Properties.ContainsKey("CustomHeaderText"))
            {
                String headerTextHex = Application.Current.Properties["CustomHeaderText"].ToString();
                App.Current.Resources["HeaderText"] = headerTextHex;
            }

            //DISPLAYS
            //App.Current.Resources["DisplayBackground"] = "#ffffff";
            App.Current.Resources["DisplayBorder"] = "Black";
            App.Current.Resources["BlackText"] = "Black";
            App.Current.Resources["Buttons"] = "White";
            App.Current.Resources["ButtonBorder"] = "#769cbc";

            App.Current.Resources["TipFrameLabelText"] = "#2196F3";
            App.Current.Resources["SliderBackground"] = "#ffffff";
            App.Current.Resources["SliderColour"] = "#2196F3";
            App.Current.Resources["TipLabelText"] = "#2196F3";
            App.Current.Resources["PercentageLabelText"] = "#2196F3";
            App.Current.Resources["SplitCostLabelText"] = "#2196F3";

            App.Current.Resources["StepperBackground"] = "White";
            App.Current.Resources["StepperColour"] = "#2196F3";

            App.Current.Resources["PayeeNumberLabelText"] = "#2196F3";

            App.Current.Resources["PayeeTitleLabelText"] = "#2196F3";

            App.Current.Resources["TotalSplitAmountLabelText"] = "#2196F3";

            App.Current.Resources["PayeeEachLabelText"] = "#2196F3";
        }

        private void CheckFontSize()
        {
            //var myFontSize = Preferences.Get("FontSize", );

            if (!App.Current.Resources.ContainsKey("FontSize"))
            {
                //var FontSize = App.Current.Resources["FontSize"];
                App.Current.Resources["FontSize"] = "Default";
                //Debug.WriteLine(App.Current.Resources[$"Main FontSize {FontSize}"]);
            }
            else
            {
                var FontSize = App.Current.Resources["FontSize"];
                App.Current.Resources["FontSize"] = FontSize;
                //Debug.WriteLine(App.Current.Resources[$"Main FontSize {FontSize}"]);
            }
        }
    }
}