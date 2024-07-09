using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public partial class Form2 : Form
    {
        //Главное окно
        private Form1 mainForm;
        public Form1 MainForm
        {
            get { return mainForm; }
            set { mainForm = value; }
        }
        public Form2()
        {
            InitializeComponent();
        }
        //Да
        private void button1_Click(object sender, EventArgs e)
        {
            mainForm.Start();
            this.Close();
        }
        //Нет
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
