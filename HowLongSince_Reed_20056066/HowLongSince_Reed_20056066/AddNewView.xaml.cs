using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.SimpleAudioPlayer;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;

namespace HowLongSince_Reed_20056066
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewView : ContentPage
    {
        private DateTime startTime;
        public AddNewView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            App.Current.Properties["activeScreen"] = 1;

            base.OnAppearing();
            var myTheme = Preferences.Get("IsDarkMode", true);
            var myCustom = Preferences.Get("IsCustom", true);

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
                Debug.WriteLine("PrefMyTheme: " + myTheme);
                Debug.WriteLine("PrefmyCustom: " + myCustom);
                Debug.WriteLine("pref on appearing CUSTOM");

                SetThemeLight(); //set to light theme first
            }
        }
                
        private async void StartTimerButton_Clicked(object sender, EventArgs e)
        {
            PlaySound();
            // take input from user, checking it is not null and then trimming of any leading or trailing whitespace
            string eventName = ItemEntry.Text?.Trim();

            if (string.IsNullOrEmpty(eventName))
            {
                await DisplayAlert("Error", "Please enter a valid event name.", "OK");
            }
            else
            {
                Event newEvent = new Event
                {
                    EventName = eventName,
                    EventStart = DateTime.Now
                };

                MainPage mainPage = Application.Current.MainPage as MainPage;
                mainPage.eventList.Add(newEvent);
                mainPage.saveData();
                Application.Current.Properties["activeScreen"] = 0;
                await DisplayAlert("Success", "New event has been added.", "OK");
                await Navigation.PopModalAsync();
            }

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

        private async void BackToMainButton_Clicked(object sender, EventArgs e)
        {
            PlaySound();
            Application.Current.Properties["activeScreen"] = 0;
            await Navigation.PopModalAsync();
        }

        private void SetThemeLight()
        {
            //OUTER FRAME COLOURS
            App.Current.Resources["MainBackground"] = "#ebf1f5";
            App.Current.Resources["OuterBorder"] = "Black";

            //HEADER COLOURS
            App.Current.Resources["HeaderText"] = "#2196F3";

            //DISPLAYS
            App.Current.Resources["DisplayBackground"] = "#ffffff";
            App.Current.Resources["DisplayBorder"] = "Black";
            App.Current.Resources["BlackText"] = "Black";
            App.Current.Resources["Buttons"] = "White";
            App.Current.Resources["ButtonBorder"] = "#769cbc";
            App.Current.Resources["LabelText"] = "#2196F3";

        }

        private void SetThemeDark()
        {
            //OUTER FRAME COLOURS
            App.Current.Resources["MainBackground"] = "#333";
            App.Current.Resources["OuterBorder"] = "#2196F3";

            //HEADER COLOURS
            App.Current.Resources["HeaderText"] = "#2196F9";

            //DISPLAYS
            App.Current.Resources["DisplayBackground"] = "#4A4A4A";
            App.Current.Resources["DisplayBorder"] = "#2196F3";
            App.Current.Resources["BlackText"] = "White";
            App.Current.Resources["Buttons"] = "#222222";
            App.Current.Resources["ButtonBorder"] = "#769cbc";
            App.Current.Resources["LabelText"] = "White";

        }

        private void SetThemeCustom()
        {
            //OUTER FRAME COLOURS
            {
                String mainBackgroundHex = App.Current.Properties["MainBackground"].ToString();
                Debug.WriteLine(mainBackgroundHex + "MAIN BACKGROUND");
                App.Current.Resources["MainBackground"] = mainBackgroundHex;//"#" + hex;
                App.Current.Resources["OuterBorder"] = "BLACK";
            }

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
            //App.Current.Resources["MainBackground"] = "#ffffff";   // was DisplayBackground
            //App.Current.Resources["DisplayBorder"] = "Black";
            //App.Current.Resources["MainBackground"] = "#333";
            //App.Current.Resources["DisplayBackground"] = "#ffffff";
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

    }

}