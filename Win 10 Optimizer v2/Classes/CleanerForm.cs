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
        Cleaner cleaner;
        private void CleanerForm_Load(object sender, EventArgs e)
        {
            cleaner = new Cleaner();
            cleaner.UpdateDataBase();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
