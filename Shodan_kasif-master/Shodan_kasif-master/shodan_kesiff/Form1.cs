using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShodanNET;
using ShodanNET.Objects;
using System.Configuration;

namespace shodan_kesiff
{
    public partial class Form1 : Form
    {


        Shodan shodan = new Shodan("API keyinizi girin");
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    ara();
                    break;
                case 1:
                    c_c_sunucu_bul();
                    break;
                case 2:
                    oracle_msf();
                    break;
                case 4:
                    ara();
                    break;


            }
            }


        public void ara()
        {
           
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    listBox1.Items.Clear();
                    label2.Text = "Cisco Omurga ";
                    List<Host> cisco = shodan.Search("cisco-ios");
                    foreach (Host h in cisco)
                    {
                        listBox1.Items.Add(h.IP.ToString());


                    }
                    break;
                case 4:

                    listBox1.Items.Clear();
                    label2.Text = "Uzak Masaüstü";
                    List<Host> rdp = shodan.Search("Remote desktop");
                    foreach (Host h in rdp)
                    {
                        listBox1.Items.Add(h.IP.ToString());
                    }
                    break;
                    
            }

          
           


        }
        public void c_c_sunucu_bul()
        {
            label2.Text = "C&C Sunucu HOST IP";
            List<Host> host = shodan.Search("category:malware");
            foreach (Host h in host)
            {
                listBox1.Items.Add(h.IP.ToString());

            }

        }

        public void oracle_msf()
        {
            label2.Text = "Oracle MSF";
            List <MSFModule > oracle_msf = shodan.SearchMSFModules("Oracle");

            foreach (MSFModule ms in oracle_msf)
                listBox1.Items.Add(ms.Name);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
