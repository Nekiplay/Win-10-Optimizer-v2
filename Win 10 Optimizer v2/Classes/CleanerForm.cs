using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_10_Optimizer_v2.Classes
{
    public partial class CleanerForm : Form
    {
        public CleanerForm()
        {
            InitializeComponent();
        }
        Cleaner cleaner = null;
        private void CleanerForm_Load(object sender, EventArgs e)
        {
            Update();
        }
        private void Update()
        {
            if (cleaner == null)
            {
                cleaner = new Cleaner();
            }
            cleaner.UpdateDataBase();
            this.label1.Text = "DataBase: " + cleaner.Logs.Count();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            foreach (Cleaner.ClearSettings setting in cleaner.Logs)
            {
                setting.Clear();
            }
        }
    }
}
