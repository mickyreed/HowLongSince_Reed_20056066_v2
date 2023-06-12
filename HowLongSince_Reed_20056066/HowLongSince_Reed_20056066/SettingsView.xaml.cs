using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Switch = System.Diagnostics.Switch;
using System.Reflection;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using System.Threading;
using ColorHelper;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Application = Xamarin.Forms.Application;

namespace HowLongSince_Reed_20056066
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentPage
    {
        public bool IsDarkTheme { get; set; }
        public bool IsSoundOn { get; set; }
        public string FontSize { get; set; }

        public SettingsView()
        {
            InitializeComponent();
            
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            App.Current.Properties["activeScreen"] = 2;


            //reload the slider value
            RSlider.Value = RedRGBValue;
            GSlider.Value = GreenRGBValue;
            BSlider.Value = BlueRGBValue;

            base.OnAppearing();
            var myTheme = Preferences.Get("IsDarkMode", true);
            var myCustom = Preferences.Get("IsCustom", true);
            Debug.WriteLine("MyTheme: " + myTheme);
            Debug.WriteLine("myCustom: " + myCustom);

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

                //OUTER FRAME COLOURS
                if (App.Current.Properties.ContainsKey("MainBackground"))
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
                if (App.Current.Properties.ContainsKey("HeaderText"))
                {
                    String headerTextHex = App.Current.Properties["HeaderText"].ToString();
                    Debug.WriteLine(headerTextHex + "HEADER");
                    App.Current.Resources["HeaderText"] = headerTextHex; //#2196F9


                }
            }

        }

        private void DarkThemeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("DarkTheme", DarkThemeSwitch.IsToggled);

            IsDarkTheme = e.Value;
            UpdateAppTheme();
        }

        private void SoundSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("SoundSwitch", SoundSwitch.IsToggled);
            IsSoundOn = e.Value;
        }

        private async void BackToMainButton_Clicked(object sender, EventArgs e)
        {
            PlaySound();
            Application.Current.Properties["activeScreen"] = 0;
            await Navigation.PopModalAsync();
        }

        private void UpdateAppTheme()
        {
            if (IsDarkTheme)
            {
               App.Current.Resources["BackgroundColor"] = Color.Black;
               App.Current.Resources["TextColor"] = Color.White;
            }
            else
            {
                App.Current.Resources["BackgroundColor"] = Color.White;
                App.Current.Resources["TextColor"] = Color.Black;
            }
        }

        public bool IsDarkMode
        {
            get => Preferences.Get(nameof(IsDarkMode), false);

            set
            {
                Preferences.Set(nameof(IsDarkMode), value);
                OnPropertyChanged(nameof(IsDarkMode));
                //ChangeTheme();
                Debug.WriteLine(" theme " + nameof(IsDarkMode), value.ToString());
            }
        }

        public bool IsMuted
        {
            get => Preferences.Get(nameof(IsMuted), false);

            set
            {
                Preferences.Set(nameof(IsMuted), value);
                OnPropertyChanged(nameof(IsMuted));
                Debug.WriteLine(" volume " + nameof(IsMuted), value.ToString());
            }
        }

        public bool IsCustom
        {
            get => Preferences.Get(nameof(IsCustom), false);

            set
            {
                Debug.WriteLine("public bool set");
                Preferences.Set(nameof(IsCustom), value);
                OnPropertyChanged(nameof(IsCustom));
                ChangeValueRGB();
                //ChangeTheme();
                Debug.WriteLine(" Custom Colour " + nameof(IsCustom), value.ToString());
            }
        }

        public double RedRGBValue
        {
            get => Preferences.Get(nameof(RedRGBValue), 0d);

            set
            {
                Debug.WriteLine("public dbl RedRGBValue set");
                Preferences.Set(nameof(RedRGBValue), value);
                OnPropertyChanged(nameof(RedRGBValue));
                Debug.WriteLine(" Custom Colour " + nameof(RedRGBValue), value.ToString());
            }
        }

        public double GreenRGBValue
        {
            get => Preferences.Get(nameof(GreenRGBValue), 0d);

            set
            {
                Debug.WriteLine("public dbl RedRGBValue set");
                Preferences.Set(nameof(GreenRGBValue), value);
                OnPropertyChanged(nameof(GreenRGBValue));
                //GSlider.Value.= GreenRGBValue;
                Debug.WriteLine(" Custom Colour " + nameof(GreenRGBValue), value.ToString());
            }
        }

        public double BlueRGBValue
        {
            get => Preferences.Get(nameof(BlueRGBValue), 0d);

            set
            {
                Debug.WriteLine("public dbl RedRGBValue set");
                Preferences.Set(nameof(BlueRGBValue), value);
                OnPropertyChanged(nameof(BlueRGBValue));
                Debug.WriteLine(" Custom Colour " + nameof(BlueRGBValue), value.ToString());
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

        private void ToggleSound_Toggled(object sender, ToggledEventArgs e)
        {
            PlaySound();
        }

        private void ToggleTheme_Toggled(object sender, ToggledEventArgs e)
        {
            PlaySound();
            if (IsDarkMode)
            {
                SetThemeDark();
            }
            else if (!IsDarkMode)
            {
                SetThemeLight();
            }
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
            //DISPLAYS
            //App.Current.Resources["MainBackground"] = "#ebf1f5";
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

        private void RSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double value = Math.Round(e.NewValue, 0);
            RedRGBValue = value;
            RValueLabel.Text = value.ToString();
            Debug.WriteLine("Red RGB Value Changed to: " + value);
            ChangeValueRGB();

        }

        private void GSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double value = Math.Round(e.NewValue, 0);
            GreenRGBValue = value;
            GValueLabel.Text = value.ToString();
            Debug.WriteLine("Green RGB Value Changed to: " + value);
            ChangeValueRGB();
        }

        private void BSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double value = Math.Round(e.NewValue, 0);
            BlueRGBValue = value;
            BValueLabel.Text = value.ToString();
            Debug.WriteLine("Blue RGB Value Changed to: " + value);
            ChangeValueRGB();
        }

        private void ChangeValueRGB()
        {

            RGB rgb = new RGB((byte)RedRGBValue, (byte)GreenRGBValue, (byte)BlueRGBValue);
            HEX hex = ColorConverter.RgbToHex(rgb);
            App.Current.Resources["MainBackground"] = "#" + hex;
            App.Current.Properties["MainBackground"] = "#" + hex;
            Preferences.Set("CustomMainBackground", "#" + hex);

            App.Current.Resources["DisplayBackground"] = "#" + hex;
            App.Current.Properties["DisplayBackground"] = "#" + hex;
            Preferences.Set("CustomDisplayBackground", "#" + hex);

            App.Current.Resources["OuterBorder"] = "BLACK";
            App.Current.Properties["OuterBorder"] = "BLACK";
            Preferences.Set("CustomOuterBorder", "BLACK");

            //Set current 
            App.Current.Properties["CustomMainBackground"] = "#" + hex;
            App.Current.Properties["CustomOuterBorder"] = "BLACK";
            App.Current.Properties["CustomDisplayBackground"] = "#" + hex;

            //HEADER COLOURS
            // check if RGB values of background adjust with header text and adjust
            if (RedRGBValue < 70 && BlueRGBValue < 70 && GreenRGBValue < 70)
            {
                App.Current.Resources["HeaderText"] = "#2196F9";
                Preferences.Set("CustomHeaderText", "#2196F9");

                App.Current.Properties["CustomHeaderText"] = "#2196F9";
            }

            else if (RedRGBValue > 200 && BlueRGBValue < 200 && GreenRGBValue < 200)
            {
                App.Current.Resources["HeaderText"] = "BLACK";
                App.Current.Properties["HeaderText"] = "BLACK";
                Preferences.Set("CustomHeaderText", "BLACK");
                App.Current.Properties["CustomHeaderText"] = "BLACK";
            }
            else
            {
                RGB rgb2 = new RGB((byte)GreenRGBValue, (byte)BlueRGBValue, (byte)RedRGBValue);
                HEX hex2 = ColorConverter.RgbToHex(rgb2);
                App.Current.Resources["HeaderText"] = "#" + hex2;
                App.Current.Resources["HeaderText"] = "BLACK";
                App.Current.Properties["HeaderText"] = "BLACK";

                Preferences.Set("CustomHeaderText", "BLACK");
                App.Current.Properties["CustomHeaderText"] = "BLACK";
            }
            //DISPLAYS
            App.Current.Resources["DisplayBackground"] = "#ffffff";
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

        private void ToggleCustom_Toggled(object sender, ToggledEventArgs e)
        {
            PlaySound();
            if (IsCustom)
            {
                DarkThemeSwitch.IsEnabled = false;
                RSlider.IsEnabled = true;
                GSlider.IsEnabled = true;
                BSlider.IsEnabled = true;
                Debug.WriteLine(" theme " + nameof(IsCustom).ToString());
                ChangeValueRGB();
            }
            else
            {
                DarkThemeSwitch.IsEnabled = true;
                RSlider.IsEnabled = false;
                GSlider.IsEnabled = false;
                BSlider.IsEnabled = false;
                Debug.WriteLine(" theme " + nameof(IsCustom).ToString());

                if (IsDarkMode)
                {
                    SetThemeDark();
                }
                else
                {
                    SetThemeLight();
                }
            }

        }

        private void FontPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Xamarin.Forms.Picker FontPicker = (Xamarin.Forms.Picker)sender;
            if (FontPicker.SelectedItem != null)
            {
                FontSize = FontPicker.SelectedItem.ToString();
                Preferences.Set("FontSize", FontSize);
                App.Current.Resources["FontSize"] = FontSize;
                App.Current.Properties["FontSize"] = FontSize;

                int SelectedIndex = FontPicker.SelectedIndex;

                CheckFontSize(FontSize);

            }
        }

        public void CheckFontSize(string FontSize)
        {
            if (FontSize != null)
            {
                if (FontSize == "Default")
                {
                    //Resources["FontSize"] = Resources["DefaultFontSize"];
                    Resources["FontSize"] = 16;
                    Resources["HeaderFontSize"] = 24;
                }
                else if (FontSize == "Small")
                {
                    //Resources["FontSize"] = Resources["SmallFontSize"];
                    Resources["FontSize"] = 12;
                    Resources["HeaderFontSize"] = 20;
                }
                else if (FontSize == "Medium")
                {
                    //Resources["FontSize"] = Resources["MediumFontSize"];
                    Resources["FontSize"] = 20;
                    Resources["HeaderFontSize"] = 28;
                }
                else if (FontSize == "Large")
                {
                    //Resources["FontSize"] = Resources["LargeFontSize"];
                    Resources["FontSize"] = 22;
                    Resources["HeaderFontSize"] = 30;
                }
            }
            else
            {
                Resources["FontSize"] = 16;
            }
        }

    }
}