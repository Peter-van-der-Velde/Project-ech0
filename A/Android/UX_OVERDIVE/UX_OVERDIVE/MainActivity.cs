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
using Domotica;

namespace UX_OVERDIVE
{
    [Activity(Label = "UX-OVERDIVE", MainLauncher = true, Icon = "@drawable/LOGO", Theme = "@style/MyCustomTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        /* Welcome to Hell */


        //variables
        public static Fragment[] fragments; //makes array of fragments, what did you expect?
        static readonly string Tag = "UX-OVERDRIVE";

        Timer timerClock, timerSockets;             // Timers   
        Socket socket = null;                       // Socket   
        Connector connector = null;                 // Connector (simple-mode or threaded-mode)
        List<Tuple<string, TextView>> commandList = new List<Tuple<string, TextView>>();  // List for commands and response places on UI
        int listIndex = 0;

        bool device1, device2, device3;

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
            };

            AddTabToActionBar(Resource.String.empty, Resource.Drawable.Sliders);
            AddTabToActionBar(Resource.String.empty, Resource.Drawable.Clock);
            AddTabToActionBar(Resource.String.empty, Resource.Drawable.Home);

            //this.Title = (connector == null) ? this.Title + " (simple sockets)" : this.Title + " (thread sockets)";
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
            switch(device)
            {
                case 0:
                    if (connector.CheckStarted())
                    {
                        if (device1 == false)
                            connector.SendMessage("t");
                        else
                            connector.SendMessage("c");
                        device1 = !device1;
                    }
                    break;
                case 1:
                    if (connector.CheckStarted())
                    {
                        if (device2 == false)
                            connector.SendMessage("h");
                        else
                            connector.SendMessage("d");
                        device2 = !device1;
                    }
                    break;
                case 2:
                    if (connector.CheckStarted())
                    {
                        if (device3 == false)
                            connector.SendMessage("j");
                        else
                            connector.SendMessage("e");
                        device3 = !device1;
                    }
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
                throw up;  // hehe
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
                    bytesRead = socket.Receive(buffer);  // If no data is available for reading, the Receive method will block until data is available,
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
            string butConText = "Connect";  // default text
            bool butConEnabled = true;      // default state
            Color color = Color.Red;        // default color
            // pinButton
            bool butPinEnabled = false;     // default state 

            //Set "Connect" button label according to connection state.
            if (state == 1)
            {
                butConText = "Please wait";
                color = Color.Orange;
                butConEnabled = false;
            }
            else
            if (state == 2)
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
                if (socket == null)                                       // create new socket
                {
                    UpdateConnectionState(1, "Connecting...");
                    try  // to connect to the server (Arduino).
                    {
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(prt)));
                        if (socket.Connected)
                        {
                            UpdateConnectionState(2, "Connected");
                            timerSockets.Enabled = true;                //Activate timer for communication with Arduino     
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
                    socket.Close(); socket = null;
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
    }
}