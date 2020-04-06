using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Net;
using System.Windows.Controls;

namespace KotoriPannel
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            string ip = IP.Text;
            string port = Port.Text;
            string time = Time.Text;
            string method = Methods.Text;

            WebClient wc = new WebClient { }; wc.DownloadString($"API");
          MessageBox.Show($"Attack Sent to {ip} using {method} for {time} sec");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            var myForm = new Form2 ();
            myForm.Show();
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            var myForm = new Form4 ();
            myForm.Show();
        }

        private void materialRaisedButton3_Click(object sender, EventArgs e)
        {
            var myForm = new Form5();
            myForm.Show();
        }

        private void materialRaisedButton4_Click(object sender, EventArgs e)
        {
            var myForm = new Form6();
            myForm.Show();
        }
    }
}