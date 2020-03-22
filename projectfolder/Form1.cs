using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace k_ortalamalari_kumelemesi_deneme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void baslat_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog()!=DialogResult.OK)
            {
                return;
            }

            Bitmap girdi = new Bitmap(dialog.FileName);

            pictureBox1.Image = girdi;
            pictureBox1.Refresh();

            K_OrtalamaKumeleme.GorselSiniflandirma siniflandirma = new K_OrtalamaKumeleme.GorselSiniflandirma(new Bitmap(girdi, (int)(girdi.Width * ((float)numericUpDown2.Value / 100f)), (int)(girdi.Height * ((float)numericUpDown2.Value / 100f))), (int)numericUpDown1.Value);
            Bitmap cikti = siniflandirma.Siniflandir((int)numericUpDown3.Value);
            pictureBox2.Image = new Bitmap(cikti, girdi.Width, girdi.Height);            
        }
    }
}
