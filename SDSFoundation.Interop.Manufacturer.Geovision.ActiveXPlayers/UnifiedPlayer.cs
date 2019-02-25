using SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.LiveX;
using SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.SinglePlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers
{
    public partial class UnifiedPlayer : Form
    {
        public UnifiedPlayer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var uri = new Uri("https://47.206.13.147");
            var singlePlayerUriWithCred = new UriBuilder(uri) { UserName = "video", Password = "southerndigital", Port = 5552 }.Uri;
            singlePlayer.Player.Connect(singlePlayerUriWithCred);

            var livePlayerUriWithCredPrimary = new UriBuilder(uri) { UserName = "video", Password = "southerndigital", Port = 4550 }.Uri;
            var livePlayerUriWithCredSecondary = new UriBuilder(uri) { UserName = "video", Password = "southerndigital", Port = 5550 }.Uri;
            liveXPlayer.Player.Connect(livePlayerUriWithCredPrimary, livePlayerUriWithCredSecondary);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            singlePlayer.Player.Play();
            liveXPlayer.Play();
        }

        DateTime startDate;
        private void timePicker_ValueChanged(object sender, EventArgs e)
        {
            var startTimeVal = timePicker.Value;

            var startTime = startDate.Add(startTimeVal.TimeOfDay);
            var stopTime = startTime.AddMinutes(5).ToIPDateTime();

            var didSearch = singlePlayer.Search2(0, 0, 1, startTime.ToIPDateTime(), stopTime);

        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            startDate = datePicker.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            singlePlayer.Player.Pause();
            liveXPlayer.Pause();
        }



    }
}
