using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;
using Xamarin.Forms.Internals;
using Newtonsoft.Json;
using PCLStorage;
using FileSystem = PCLStorage.FileSystem;
using Plugin.SimpleAudioPlayer;
using Application = Xamarin.Forms.Application;
using ColorHelper;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace HowLongSince_Reed_20056066
{
    public partial class MainPage : ContentPage
    {

        const string folderName = "eventsFolder";
        const string fileName = "events.txt";
        static ISimpleAudioPlayer explosion = CrossSimpleAudioPlayer.Current;

        public ObservableCollection<Event> eventList;

        public MainPage()
        {
            InitializeComponent();

            // Initialize eventList before setting ItemsListView.ItemsSource
            eventList = new ObservableCollection<Event>();

            // Load data from file or create default data
            loadData();

            //Check Preferences and load active screen
            CheckActiveScreen();

            // Calculate time differences and update EventListView
            UpdateEventListTimeDifference();

            // Start a timer or background task to periodically update the time differences
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                UpdateEventListTimeDifference();
                return true;
            });

        }

        protected override void OnAppearing()
        {
            
            //Application.Current.Properties["activeScreen"] = 0;


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
        }


        public void UpdateEventListTimeDifference()
        {
           
            if (eventList != null)
            {
                foreach (var evnt in eventList)
                {
                    if (evnt != null) // if null check
                    {
                        TimeSpan timeDifference = DateTime.Now - evnt.EventStart;

                        // Calculate the individual components
                        int days = timeDifference.Days;
                        int hours = timeDifference.Hours;
                        int minutes = timeDifference.Minutes;
                        int seconds = timeDifference.Seconds;

                        // Format the time since start
                        evnt.TimeSinceStart = string.Format("{0} days, {1} hrs, {2} min,  {3} sec.", days, hours, minutes, seconds);
                    }
                }
            }

        }

        public async void loadData()
        {
            Debug.WriteLine("loading...");
            try
            {
                IFolder folder = FileSystem.Current.LocalStorage;
                folder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
                IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

                string loadedContent = await file.ReadAllTextAsync();
                eventList = JsonConvert.DeserializeObject<ObservableCollection<Event>>(loadedContent);
                
                Debug.WriteLine(folder.Path);
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.WriteLine("There is no existing files");
                ResponseLoadDefaultData();
            }

            if (eventList != null && eventList.Count != 0)
            {
                Debug.WriteLine(eventList.Count.ToString());
                Debug.WriteLine("Loaded!!!");
                ItemsListView.ItemsSource = eventList;
            }

            else if (eventList == null || eventList.Count == 0)
            {
                Debug.WriteLine("The existing file is empty!");
                ResponseLoadDefaultData();
            }
        }
        public async void ResponseLoadDefaultData()
        {
            bool response = await DisplayAlert("Sorry, there is no existing events", "Would you like to Load some default events?", "Yes", "No");
            if (response)
            {
                LoadDefaultData();
                Debug.WriteLine("LoadDefaultData");
                saveData();
                Debug.WriteLine("saveData");
                _ = DisplayAlert("Success!","Default events now loaded", "OK"); // no await ... will run synchronously
            }
            else
            {
                return;
            }
        }

        public void LoadDefaultData()
        {
            if (eventList == null)
            {
                eventList = new ObservableCollection<Event>();
            }

            eventList = new ObservableCollection<Event>
            {
                new Event { EventName = "Workout", EventStart = DateTime.Parse("05/01/2023 08:30:38") },
                new Event { EventName = "Called Mum", EventStart = DateTime.Parse("05/21/2023 09:43:02") },
                new Event { EventName = "Change oil in Car", EventStart = DateTime.Parse("04/15/2023 14:21:43") },
            };
            ItemsListView.ItemsSource = eventList;;      
            Debug.WriteLine("Loaded default values!)");
            String jsonString = JsonConvert.SerializeObject(eventList);
        }

        public async void saveData()
        {
            Debug.WriteLine("Saving...");
            String jsonString = JsonConvert.SerializeObject(eventList);
            try
            {
                IFolder folder = FileSystem.Current.LocalStorage;
                folder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
                IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                await file.WriteAllTextAsync(jsonString);

                Debug.WriteLine(folder.Path);

            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.WriteLine("There is no existing files");
                loadData();
            }
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            PlaySound();
            await Navigation.PushModalAsync(new SettingsView());
        }
        private async void AddNewButton_Clicked(object sender, EventArgs e)
        {
            PlaySound();
            await Navigation.PushModalAsync(new AddNewView());
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            PlaySound();
            var item = e.Item as Event;
            DisplayAlert($"{item.EventName}", $"Start Date: {item.EventStart}\nElapsed Time: {item.TimeSinceStart}", "OK");
        }

        private void ImageButton_restart(object sender, EventArgs e)
        {
            PlaySound();
            var imageButton = sender as ImageButton;
            var item = imageButton.BindingContext as Event;

            // Reset the EventStart value to the current DateTime
            item.EventStart = DateTime.Now;
            saveData();

            // Display an alert with the updated EventStart value
            DisplayAlert($"{item.EventName}", $"Event Reset at: {item.EventStart.ToString()}", $" Duration:\n { item.TimeSinceStart }\n", "OK");
        }

        private void ImageButton_delete(object sender, EventArgs e)
        {
            PlaySound();
            var button = sender as ImageButton;
            var item = button?.BindingContext as Event;

            if (item != null)
            {
                // Remove the listView item
                eventList.Remove(item);
            }

            saveData();
            // Display an alert with notifying of the removed event
            DisplayAlert($"{item.EventName}", $"Event Deleted at: {item.EventStart.ToString()}", $" Duration:\n { item.TimeSinceStart }\n", "OK");

        }

        //Android and Ios Hold/swipe events to restart delete.. untested
        private void MenuItem_Restart(object sender, EventArgs e)
        {
            PlaySound();
            var menuItem = sender as MenuItem;
            var item = menuItem.CommandParameter as Event;

            // Reset the EventStart value to the current DateTime
            item.EventStart = DateTime.Now;


            // Display an alert with the updated EventStart value
            DisplayAlert(item.EventName, item.EventStart.ToString(), item.TimeSinceStart, "OK");
        }

        //Android and Ios Hold/swipe events to restart delete.. untested
        private void MenuItem_Delete(object sender, EventArgs e)
        {
            PlaySound();
            var menuItem = sender as MenuItem;
            var item = menuItem.CommandParameter as Event;

            if (item != null)
            {
                // Remove the listView item
                eventList.Remove(item);
            }

            // Display an alert with notifying of the removed event
            DisplayAlert($"{item.EventName}", $"Event Deleted at: {item.EventStart.ToString()}", $" Duration:\n { item.TimeSinceStart }\n", "OK");
        }

        private void PlaySound()
        {

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

        private void CheckActiveScreen()
        {
            if (App.Current.Properties.ContainsKey("activeScreen"))
            {
                int myPageStart = (int)Application.Current.Properties["activeScreen"];
                Debug.WriteLine("Page" + myPageStart);

                if (myPageStart == 0)
                {
                    Application.Current.Properties["activeScreen"] = 0;
                }
                else if (myPageStart == 1)
                {
                    NavigateToSettings();
                }
                else if (myPageStart == 2)
                {
                    NavigateToAddNew();
                }

            }
        }

        async private void NavigateToSettings()
        {
            Application.Current.Properties["activeScreen"] = 1;
            SettingsView settingsView = new SettingsView();
            await Navigation.PushModalAsync(settingsView);
        }

        async private void NavigateToAddNew()
        {
            Application.Current.Properties["activeScreen"] = 2;
            AddNewView addNewView = new AddNewView();
            await Navigation.PushModalAsync(addNewView);
        }
    }
}

