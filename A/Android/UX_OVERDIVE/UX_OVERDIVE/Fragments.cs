using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace UX_OVERDIVE
{

    /// <summary>
    /// These classes are used for the various views between the tabs.
    /// </summary>
    class Sliders : Fragment
    {

        //here go possible variables
        private Switch switchArduinoConnect;
        private Switch switchDevice1;
        private Switch switchDevice2;
        private Switch switchDevice3;
        private Switch switchAllDevices;

        private TextView textViewConnectArduino;
        private TextView textViewDevice1;
        private TextView textViewDevice2;
        private TextView textViewDevice3;
        private TextView textViewAllDevices;

        private ImageButton settingButton;

        Timer timerClock, timerSockets;                
        Socket socket = null;                          

        List<Tuple<string, TextView>> commandList = new List<Tuple<string, TextView>>();  // List for commands and response places on UI
        int listIndex = 0;

        //new var
        private string IPADDRESS;
        private string PORT;

        //here goes the code for each "Tab"
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            //Select the View you want to use for this tab
            var view = inflater.Inflate(Resource.Layout.Sliders, container, false);

            switchDevice1 = view.FindViewById<Switch>(Resource.Id.switch_dv1);
            switchArduinoConnect = view.FindViewById<Switch>(Resource.Id.switch_Connect);

            textViewDevice1 = view.FindViewById<TextView>(Resource.Id.text_dv1);
            textViewConnectArduino = view.FindViewById<TextView>(Resource.Id.textConnectToArduino);

            settingButton = view.FindViewById<ImageButton>(Resource.Id.set_B);


            //loading settings/PREFERENCES!!!
            ISharedPreferences pref = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
            IPADDRESS = pref.GetString("IP", "192.168.100");
            PORT = pref.GetString("PORT", "53");


            settingButton.Click += settingButton_Click;
            //code
            switchDevice1.CheckedChange += (obj, args) =>
            {
                //DOES NOTHING

                //if (switchDevice1.Checked)
                //    textViewDevice1.Text = IPADDRESS;
                //if (!switchDevice1.Checked)
                //    textViewDevice1.Text = PORT;
            };

            //commandList.Add(new Tuple<string, TextView>("s", textViewChangePinStateValue));
            //commandList.Add(new Tuple<string, TextView>("a", textViewSensorValue));

            timerSockets = new System.Timers.Timer() { Interval = 1000, Enabled = false }; // Interval >= 750
            timerSockets.Elapsed += (obj, args) =>
            {
                //RunOnUiThread(() =>
                //{
                if (socket != null) // only if socket exists
                {
                    // Send a command to the Arduino server on every tick (loop though list)
                    UpdateGUI(executeCommand(commandList[listIndex].Item1), commandList[listIndex].Item2);  //e.g. UpdateGUI(executeCommand("s"), textViewChangePinStateValue);
                    if (++listIndex >= commandList.Count) listIndex = 0;
                }
                else timerSockets.Enabled = false;  // If socket broken -> disable timer
                //});
            };

            switchArduinoConnect.CheckedChange += (obj, args) =>
            {
                if (switchArduinoConnect.Checked)
                {
                    //Validate the user input (IP address and port)
                    if (CheckValidIpAddress(IPADDRESS) && CheckValidPort(PORT))
                    {
                        ConnectSocket(IPADDRESS, PORT);
                    }
                    else UpdateConnectionState(3, "Please check IP");
                }
            };

            return view;
        }


        private void settingButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(PreferencesActivity));
            this.StartActivity(intent);
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
            string butConText = "Connecting";  // default text
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
            Activity.RunOnUiThread(() =>
            {
                //textViewServerConnect.Text = text;
                if (butConText != null)  // text existst
                {
                    textViewConnectArduino.Text = butConText;
                    textViewConnectArduino.SetTextColor(color);
                    switchArduinoConnect.Enabled = butConEnabled;
                }
                //buttonChangePinState.Enabled = butPinEnabled;
            });
        }

        //Update GUI based on Arduino response
        public void UpdateGUI(string result, TextView textview)
        {
            Activity.RunOnUiThread(() =>
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
            Activity.RunOnUiThread(() =>
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

        ////Close the connection (stop the threads) if the application stops.
        //protected override void this.OnStop()
        //{
        //    base.OnStop();
        //}

        ////Close the connection (stop the threads) if the application is destroyed.
        //protected override void OnDestroy()
        //{
        //    base.OnDestroy();
        //}

        ////Prepare the Screen's standard options menu to be displayed.
        //public override bool OnPrepareOptionsMenu(IMenu menu)
        //{
        //    //Prevent menu items from being duplicated.
        //    menu.Clear();

        //    View.MenuInflater.Inflate(Resource.Menu.menu, menu);
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
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        //Check if the entered IP address is valid.
        private bool CheckValidIpAddress(string ip)
        {
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








    //next fragment
    class Clock : Fragment
    {
        //variables
        private int clickCount = 0;
        private Button clicker;
        private ImageButton settingButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Timed, container, false);

            clicker = view.FindViewById<Button>(Resource.Id.button1);
            settingButton = view.FindViewById<ImageButton>(Resource.Id.settingbutton2);

            //sets the text of the button
            clicker.Text = "You clicked the button " + clickCount++ + " times.";


            clicker.Click += clicker_Click;
            settingButton.Click += settingButton_Click;

            return view;
        }

        private void clicker_Click(object sender, EventArgs e)
        {
            clicker.Text = "You clicked the button " + clickCount++ + " times.";
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(Settings));
            this.StartActivity(intent);
        }
    }
}