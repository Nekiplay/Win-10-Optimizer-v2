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
        List<string> buttons_texts = new List<string>();
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
            foreach (CstBut butrem in bts)
            {
                this.Controls.Remove(butrem.Switch);
                this.Controls.Remove(butrem.label);
            }
            bts.Clear();
            buttons_texts.Clear();
            cleaner.UpdateDataBase();
            this.label1.Text = "База данных: " + cleaner.DataBase.Count();
            foreach (Cleaner.ClearSettings st in cleaner.DataBase)  
            {
                if (!buttons_texts.Contains(st.Type))
                {
                    buttons_texts.Add(st.Type);
                }
            }
            int offset = 0;
            foreach (string cst2 in buttons_texts)
            {
                Guna.UI2.WinForms.Guna2ToggleSwitch guna2 = new Guna.UI2.WinForms.Guna2ToggleSwitch();
                guna2.Location = new Point(15, 27 + offset);
                Label text = new Label();
                text.AutoSize = true;
                text.Text = cst2 + " | " + cleaner.GetByType(cst2).Count();
                text.ForeColor = Color.Black;
                text.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                text.Location = new Point(55, 30 + offset);
                offset += 23;
                CstBut ct = new CstBut();
                ct.Type = cst2;
                ct.Switch = guna2;
                ct.label = text;
                bts.Add(ct);
                this.Controls.Add(text);
                this.Controls.Add(guna2);
            }
            this.guna2Button2.Location = new Point(15, 27 + offset);
            this.guna2Button1.Location = new Point(15, 27 + offset + 27);
        }
        public List<CstBut> bts = new List<CstBut>();
        public class CstBut
        {
            public string Type;
            public Guna.UI2.WinForms.Guna2ToggleSwitch Switch;
            public Label label;
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Update();
        }

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            long cleared = 0;
            Task cl = Task.Factory.StartNew(() =>
            {
                foreach (CstBut bt in bts)
                {
                    if (bt.Switch.Checked == true)
                    {
                        foreach (Cleaner.ClearSettings setting in cleaner.GetByType(bt.Type))
                        {
                            cleared += setting.Clear();
                        }
                        bt.Switch.Checked = false;
                    }
                }
            });
            await cl;
            NotificationManager.Manager manager = new NotificationManager.Manager();
            manager.Alert("Удалено: " + BytesToString(cleared), NotificationManager.NotificationType.Success);
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            Update();
        }
    }
}
