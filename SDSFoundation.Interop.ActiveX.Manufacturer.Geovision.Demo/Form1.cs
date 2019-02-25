using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers;

namespace SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            singlePlayer.PlayMode = 1;
            singlePlayer2.PlayMode = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var uri = new Uri("https://47.206.13.147");
            var singlePlayerUriWithCred = new UriBuilder(uri) { UserName = "video", Password = "southerndigital", Port = 5552 }.Uri;
            singlePlayer.Player.Connect(singlePlayerUriWithCred);
            singlePlayer2.Player.Connect(singlePlayerUriWithCred);

            var livePlayerUriWithCredPrimary = new UriBuilder(uri) { UserName = "video", Password = "southerndigital", Port = 4550 }.Uri;
            var livePlayerUriWithCredSecondary = new UriBuilder(uri) { UserName = "video", Password = "southerndigital", Port = 5550 }.Uri;
            liveXPlayer.Player.Connect(livePlayerUriWithCredPrimary, livePlayerUriWithCredSecondary);
        }

        private void button2_Click(object sender, EventArgs e)
        {
                singlePlayer.Player.Play();
                singlePlayer2.Play();
                liveXPlayer.Play();
        }

        DateTime startDate;
        private void timePicker_ValueChanged(object sender, EventArgs e)
        {
            var startTimeVal = timePicker.Value;

            var startTime = startDate.Add(startTimeVal.TimeOfDay);
            var stopTime = startTime.AddMinutes(5).ToIPDateTime();

            var didSearch = singlePlayer.Search2(0, 0, 1, startTime.ToIPDateTime(), stopTime);

            var didSearch2 = singlePlayer2.Search2(0, 0, 2, startTime.ToIPDateTime(), stopTime);
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            startDate = datePicker.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            singlePlayer.Player.Pause();
            singlePlayer2.Pause();
            liveXPlayer.Pause();
        }
    }
}
