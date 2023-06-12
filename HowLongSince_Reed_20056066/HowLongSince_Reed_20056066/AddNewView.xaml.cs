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

            if (Preferences.ContainsKey("FontSize"))
            {
                //string savedFontSize = Preferences.Get("FontSize", "Default");
                //FontPicker.SelectedItem = savedFontSize;
                string savedFontSize = (string)Application.Current.Properties["FontSize"];
                Debug.WriteLine($"Font Size Check is {savedFontSize}");
                CheckFontSize(savedFontSize);
            }
            else
            {
                //FontPicker.SelectedItem = "Default";
                CheckFontSize("Default");
            }


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
            if (mySound)
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


            //Text COLOURS
            if (Application.Current.Properties.ContainsKey("CustomBlackText"))
            {
                String headerTextHex = Application.Current.Properties["CustomBlackText"].ToString();
                App.Current.Resources["BlackText"] = headerTextHex;
            }

            //DISPLAYS
            //App.Current.Resources["DisplayBackground"] = "#ffffff";
            App.Current.Resources["DisplayBorder"] = "Black";
            //App.Current.Resources["BlackText"] = "Black";
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

            //
            //
            //OUTER FRAME COLOURS
            //    {
            //    String mainBackgroundHex = App.Current.Properties["MainBackground"].ToString();
            //    Debug.WriteLine(mainBackgroundHex + "MAIN BACKGROUND");
            //    App.Current.Resources["MainBackground"] = mainBackgroundHex;//"#" + hex;
            //    App.Current.Resources["OuterBorder"] = "BLACK";
            //}

            //if (Application.Current.Properties.ContainsKey("DisplayBackground"))
            //{
            //    String displayBackgroundHex = Application.Current.Properties["CustomMainBackground"].ToString();
            //    App.Current.Resources["DisplayBackground"] = displayBackgroundHex;
            //    Debug.WriteLine(displayBackgroundHex);
            //}

            ////HEADER COLOURS
            //if (Application.Current.Properties.ContainsKey("CustomHeaderText"))
            //{
            //    String headerTextHex = Application.Current.Properties["CustomHeaderText"].ToString();
            //    App.Current.Resources["HeaderText"] = headerTextHex;
            //}

            ////DISPLAYS
            ////App.Current.Resources["MainBackground"] = "#ffffff";   // was DisplayBackground
            ////App.Current.Resources["DisplayBorder"] = "Black";
            ////App.Current.Resources["MainBackground"] = "#333";
            ////App.Current.Resources["DisplayBackground"] = "#ffffff";
            //App.Current.Resources["BlackText"] = "Black";
            //App.Current.Resources["Buttons"] = "White";
            //App.Current.Resources["ButtonBorder"] = "#769cbc";

            //App.Current.Resources["TipFrameLabelText"] = "#2196F3";
            //App.Current.Resources["SliderBackground"] = "#ffffff";
            //App.Current.Resources["SliderColour"] = "#2196F3";
            //App.Current.Resources["TipLabelText"] = "#2196F3";
            //App.Current.Resources["PercentageLabelText"] = "#2196F3";
            //App.Current.Resources["SplitCostLabelText"] = "#2196F3";

            //App.Current.Resources["StepperBackground"] = "White";
            //App.Current.Resources["StepperColour"] = "#2196F3";

            //App.Current.Resources["PayeeNumberLabelText"] = "#2196F3";

            //App.Current.Resources["PayeeTitleLabelText"] = "#2196F3";

            //App.Current.Resources["TotalSplitAmountLabelText"] = "#2196F3";

            //App.Current.Resources["PayeeEachLabelText"] = "#2196F3";
        }

        public void CheckFontSize(string FontSize)
        {
            if (FontSize != null)
            {
                //< x:Double x:Key = "LargeFontSize" > 24 </ x:Double >
                //< x:Double x:Key = "MediumFontSize" > 18 </ x:Double >
                //< x:Double x:Key = "SmallFontSize" > 14 </ x:Double >
                //< x:Double x:Key = "DefaultFontSize" > 16 </ x:Double >

                if (FontSize == "Default")
                {
                    //Resources["FontSize"] = Resources["DefaultFontSize"];
                    Resources["FontSize"] = 16;
                    App.Current.Resources["FontSize"] = FontSize;
                    App.Current.Properties["FontSize"] = FontSize;
                    Resources["HeaderFontSize"] = 24;
                    App.Current.Resources["HeaderFontSize"] = "Header" + FontSize;
                    App.Current.Properties["HeaderFontSize"] = "Header" + FontSize;
                }
                else if (FontSize == "Small")
                {
                    // Resources["FontSize"] = Resources["SmallFontSize"];
                    Resources["FontSize"] = 12;
                    App.Current.Resources["FontSize"] = FontSize;
                    App.Current.Properties["FontSize"] = FontSize;
                    Resources["HeaderFontSize"] = 20;
                    App.Current.Resources["HeaderFontSize"] = "Header" + FontSize;
                    //App.Current.Properties["HeaderFontSize"] = "Header" + FontSize;
                }
                else if (FontSize == "Medium")
                {
                    //Resources["FontSize"] = Resources["MediumFontSize"];
                    Resources["FontSize"] = 20;
                    App.Current.Resources["FontSize"] = FontSize;
                    App.Current.Properties["FontSize"] = FontSize;
                    Resources["HeaderFontSize"] = 28;
                    App.Current.Resources["HeaderFontSize"] = "Header" + FontSize;
                    App.Current.Properties["HeaderFontSize"] = "Header" + FontSize;
                }
                else if (FontSize == "Large")
                {
                    //Resources["FontSize"] = Resources["LargeFontSize"];
                    Resources["FontSize"] = 22;
                    App.Current.Resources["FontSize"] = FontSize;
                    App.Current.Properties["FontSize"] = FontSize;
                    Resources["HeaderFontSize"] = 30;
                    App.Current.Resources["HeaderFontSize"] = "Header" + FontSize;
                    App.Current.Properties["HeaderFontSize"] = "Header" + FontSize;
                }
                else
                {
                    Resources["FontSize"] = 16;
                    App.Current.Resources["FontSize"] = FontSize;
                    App.Current.Properties["FontSize"] = FontSize;
                    Resources["HeaderFontSize"] = 24;
                    App.Current.Resources["HeaderFontSize"] = "Header" + FontSize;
                    App.Current.Properties["HeaderFontSize"] = "Header" + FontSize;
                }
            }
        }

    }

}