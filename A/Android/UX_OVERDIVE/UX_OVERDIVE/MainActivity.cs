using System;
using Black_magic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using Android.Content.PM;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Android.Graphics;
using Android.Util;
using System.Threading.Tasks;
using Android;
using Android.Preferences;
using Android.Speech;
using Domotica;

namespace UX_OVERDIVE
{
    [Activity(Label = "Ech0", MainLauncher = false, Icon = "@drawable/LOGO", Theme = "@style/MyCustomTheme",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        /* Welcome to Hell */


        //variables
        public static Fragment[] fragments; //makes array of fragments, what did you expect?
        static readonly string Tag = "UX-OVERDRIVE";

        Timer timerClock, timerSockets; // Timers   
        Socket socket = null; // Socket   
        Connector connector = null; // Connector (simple-mode or threaded-mode)

        List<Tuple<string, TextView>> commandList = new List<Tuple<string, TextView>>();
        // List for commands and response places on UI

        int listIndex = 0;

        bool device1, device2, device3, alldevices;
        //needed for speech
        private string textSpeech = String.Empty;
        public string textSpeechInput = String.Empty;
        private bool isRecording;
        private readonly int VOICE = 10;

        private bool allOn = false;

        protected override void OnCreate(Bundle bundle)
        {
            //http://www.cheaprope.co.uk/
            base.OnCreate(bundle);

            connector = new Connector(this);

            UpdateConnectionState(4, "Disconnected");

            // Set our view from the "main" layout resource
            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs; //ACTIONBAR is in Tabbed Mode
            SetContentView(Resource.Layout.Main); //sets view to main

            //removes title and icon from actionbar
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);

            //fragments used
            fragments = new Fragment[]
            {
                new Sliders(this),
                new Clock(),
                new Home(),
                new SpeechFragment(this),
            };

            AddTabToActionBar(Resource.String.empty, Resource.Drawable.Sliders);
            AddTabToActionBar(Resource.String.empty, Resource.Drawable.Clock);
            AddTabToActionBar(Resource.String.empty, Resource.Drawable.Home);
            AddTabToActionBar(Resource.String.empty, Resource.Drawable.Microphone);

            //this.Title = (connector == null) ? this.Title + " (simple sockets)" : this.Title + " (thread sockets)";

            ISharedPreferences pref = Application.Context.GetSharedPreferences("Time", FileCreationMode.Private);

            Timer clockTimer = new Timer() { Interval = 2000, Enabled = true };
            clockTimer.Elapsed += (obj, args) =>
            {
                string savedHour = pref.GetString("Hour", DateTime.Now.Hour.ToString());
                string savedMinute = pref.GetString("Minute", DateTime.Now.Hour.ToString());

                if (Convert.ToInt32(savedHour) == DateTime.Now.Hour)
                {
                    if (Convert.ToInt32(savedMinute) == DateTime.Now.Minute)
                        connector.SendMessage("k");
                }
            };
        }

        public void SwitchConnect(string ip, string prt)
        {
            if (CheckValidIpAddress(ip) && CheckValidPort(prt))
            {
                Console.WriteLine("Trying to Connect...");
                if (connector.CheckStarted())
                {
                    connector.StopConnector();
                    Console.WriteLine("Connector Thread has been stopped");
                }
                connector.StartConnector(ip, prt);
            }
            else
                UpdateConnectionState(3, "Please check IP");
        }

        public void SwitchDevice(int device)
        {
            switch (device)
            {
                case 1:
                    if (connector.CheckStarted())
                    {
                        if (device1 == false)
                            connector.SendMessage("t");
                        else
                            connector.SendMessage("c");
                        device1 = !device1;
                    }
                    break;
                case 2:
                    if (connector.CheckStarted())
                    {
                        if (device2 == false)
                            connector.SendMessage("h");
                        else
                            connector.SendMessage("d");
                        device2 = !device2;
                    }
                    break;
                case 3:
                    if (connector.CheckStarted())
                    {
                        if (device3 == false)
                            connector.SendMessage("j");
                        else
                            connector.SendMessage("e");
                        device3 = !device3;
                    }
                    break;
                case 4:
                    if (allOn == false)
                    {
                        connector.SendMessage("k");
                        device1 = true;
                        device2 = true;
                        device3 = true;
                        allOn = true;
                    }
                    else
                        connector.SendMessage("e");
                        device1 = false;
                        device2 = false;
                        device3 = false;
                        allOn = false;
                    break;
            }
        }

        /// <summary>
        /// Adds Tabs to Actionbar
        /// </summary>
        /// <param name="labelResourceId">label string of tab.
        /// Resource.String.empty returns an empty string/nameless</param>
        /// <param name="iconResourceId">icon of tab</param>
        public void AddTabToActionBar(int labelResourceId, int iconResourceId)
        {
            ActionBar.Tab tab = ActionBar.NewTab()
                .SetText(labelResourceId)
                .SetIcon(iconResourceId);
            tab.TabSelected += TabOnTabSelected;
            ActionBar.AddTab(tab);
        }

        //connects the right fragment to the right tab
        public void TabOnTabSelected(object sender, ActionBar.TabEventArgs tabEventArgs)
        {
            ActionBar.Tab tab = (ActionBar.Tab) sender;

            //checks if the tab has a fragment connected to it
            if (tab.Position == fragments.Length && !Dreams.IWantToLive())
            {
                Exception up = new Exception("Tab does not have a fragment");
                throw up; // hehe
            }

            Log.Debug(Tag, "The tab {0} has been selected.", tab.Text);
            Fragment frag = MainActivity.fragments[tab.Position];
            tabEventArgs.FragmentTransaction.Replace(Resource.Id.frameLayout1, frag);
        }


        //Send command to server and wait for response (blocking)
        //Method should only be called when socket existst
        public string executeCommand(string cmd)
        {
            byte[] buffer = new byte[4]; // response is always 4 bytes
            int bytesRead = 0;
            string result = "---";

            if (socket != null)
            {
                //Send command to server
                socket.Send(Encoding.ASCII.GetBytes(cmd));

                try //Get response from server
                {
                    //Store received bytes (always 4 bytes, ends with \n)
                    bytesRead = socket.Receive(buffer);
                    // If no data is available for reading, the Receive method will block until data is available,
                    //Read available bytes.              // socket.Available gets the amount of data that has been received from the network and is available to be read
                    while (socket.Available > 0) bytesRead = socket.Receive(buffer);
                    if (bytesRead == 4)
                        result = Encoding.ASCII.GetString(buffer, 0, bytesRead - 1); // skip \n
                    else result = "err";
                }
                catch (Exception exception)
                {
                    result = exception.ToString();
                    if (socket != null)
                    {
                        socket.Close();
                        socket = null;
                    }
                    UpdateConnectionState(3, result);
                }
            }
            return result;
        }

        //Update connection state label (GUI).
        public void UpdateConnectionState(int state, string text)
        {
            // connectButton
            string butConText = "Connect"; // default text
            bool butConEnabled = true; // default state
            Color color = Color.Red; // default color
            // pinButton
            bool butPinEnabled = false; // default state 

            //Set "Connect" button label according to connection state.
            if (state == 1)
            {
                butConText = "Please wait";
                color = Color.Orange;
                butConEnabled = false;
            }
            else if (state == 2)
            {
                butConText = "Disconnect";
                color = Color.Green;
                butPinEnabled = true;
            }
            //Edit the control's properties on the UI thread
            /*RunOnUiThread(() =>
            {
                textViewServerConnect.Text = text;
                if (butConText != null)  // text existst
                {
                    buttonConnect.Text = butConText;
                    textViewServerConnect.SetTextColor(color);
                    buttonConnect.Enabled = butConEnabled;
                }
                buttonChangePinState.Enabled = butPinEnabled;
            });*/
        }

        //Update GUI based on Arduino response
        public void UpdateGUI(string result, TextView textview)
        {
            RunOnUiThread(() =>
            {
                if (result == "OFF") textview.SetTextColor(Color.Red);
                else if (result == " ON") textview.SetTextColor(Color.Green);
                else textview.SetTextColor(Color.White);
                textview.Text = result;
            });
        }

        // Connect to socket ip/prt (simple sockets)
        public void ConnectSocket(string ip, string prt)
        {
            RunOnUiThread(() =>
            {
                if (socket == null) // create new socket
                {
                    UpdateConnectionState(1, "Connecting...");
                    try // to connect to the server (Arduino).
                    {
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(prt)));
                        if (socket.Connected)
                        {
                            UpdateConnectionState(2, "Connected");
                            timerSockets.Enabled = true; //Activate timer for communication with Arduino     
                        }
                    }
                    catch (Exception exception)
                    {
                        timerSockets.Enabled = false;
                        if (socket != null)
                        {
                            socket.Close();
                            socket = null;
                        }
                        UpdateConnectionState(4, exception.Message);
                    }
                }
                else // disconnect socket
                {
                    socket.Close();
                    socket = null;
                    timerSockets.Enabled = false;
                    UpdateConnectionState(4, "Disconnected");
                }
            });
        }

        //Close the connection (stop the threads) if the application stops.
        protected override void OnStop()
        {
            base.OnStop();

            if (connector != null)
            {
                if (connector.CheckStarted())
                {
                    connector.StopConnector();
                }
            }
        }

        //Close the connection (stop the threads) if the application is destroyed.
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (connector != null)
            {
                if (connector.CheckStarted())
                {
                    connector.StopConnector();
                }
            }
        }

        ////Prepare the Screen's standard options menu to be displayed.
        //public override bool OnPrepareOptionsMenu(IMenu menu)
        //{
        //    //Prevent menu items from being duplicated.
        //    menu.Clear();

        //    MenuInflater.Inflate(Resource.Menu.menu, menu);
        //    return base.OnPrepareOptionsMenu(menu);
        //}

        //Executes an action when a menu button is pressed.
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.exit:
                    //Force quit the application.
                    System.Environment.Exit(0);
                    return true;
                case Resource.Id.abort:

                    //Stop threads forcibly (for debugging only).
                    if (connector != null)
                    {
                        if (connector.CheckStarted()) connector.Abort();
                    }
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        //Check if the entered IP address is valid.
        private bool CheckValidIpAddress(string ip)
        {
            Console.WriteLine("Checking IP: {0}", ip);
            if (ip != "")
            {
                //Check user input against regex (check if IP address is not empty).
                Regex regex = new Regex("\\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\\.|$)){4}\\b");
                Match match = regex.Match(ip);
                return match.Success;
            }
            else return false;
        }

        //Check if the entered port is valid.
        private bool CheckValidPort(string port)
        {
            Console.WriteLine("Check port...");
            //Check if a value is entered.
            if (port != "")
            {
                Regex regex = new Regex("[0-9]+");
                Match match = regex.Match(port);

                if (match.Success)
                {
                    int portAsInteger = Int32.Parse(port);
                    //Check if port is in range.
                    return ((portAsInteger >= 0) && (portAsInteger <= 65535));
                }
                else return false;
            }
            else return false;
        }
        public void xxXTouwSlayerXxx()
        {
            // change the text on the button or maybe not
            if (true)
                {
                    // create the intent and start the activity
                    Intent voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                    // put a message on the modal dialog
                    voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt,
                        Application.Context.GetString(Resource.String.messageSpeakNow));

                    // if there is more then 1.5s of silence, consider the speech over
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                    // you can specify other languages recognised here, for example
                    // voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);
                    // if you wish it to recognise the default Locale language and German
                    // if you do use another locale, regional dialects may not be recognised very well

                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                    StartActivityForResult(voiceIntent, VOICE);
    }

        }
        //Speech in 2017 lul
        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            if (requestCode == 10 && !Dreams.HoeLaatIsHet("tijd voor Speech"))
            {
                //throw new Java.Lang.Exception("error, stik er in");
                if (resultVal == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        string textInput = textSpeech + matches[0];

                        // limit the output to 500 characters
                        if (textInput.Length > 500)
                        {
                            textInput = textInput.Substring(0, 500);
                            textSpeechInput = textInput;
                        }
                        else
                        {
                            // change the text back on the button;
                            //textInput = "No speech was recognised";
                            textSpeechInput = textInput;
                        }


                    }
                }

                base.OnActivityResult(requestCode, resultVal, data);
            }
            else
            {
                textSpeechInput = "error";

            }

        }
    }
}

        
 