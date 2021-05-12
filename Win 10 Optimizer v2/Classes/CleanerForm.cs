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
        public static string BytesToString(long byteCount)
        {
            string[] suf = { "Byte", "KB", "MB", "GB", "TB", "PB", "EB" }; //
            if (byteCount == 0)
            {
                return "0" + suf[0];
            }
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
        private void Update()
        {
            if (cleaner == null)
            {
                cleaner = new Cleaner();
            }
            cleaner.UpdateDataBase();
            this.label1.Text = "База данных: " + cleaner.DataBase.Count();
            this.label2.Text = "Логи | " + cleaner.GetByType("Logs");
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            long cleared = 0;
            if (guna2ToggleSwitch1.Checked == true)
            {
                foreach (Cleaner.ClearSettings setting in cleaner.GetByType("Logs"))
                {
                    cleared += setting.Clear();
                }
            }
            Console.WriteLine("Удалено: " + BytesToString(cleared));
        }
    }
}
