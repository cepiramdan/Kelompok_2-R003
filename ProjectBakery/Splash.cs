using Bunifu.Framework.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectBakery
{
    public partial class Splash : Form
    {
        // Inisialisasi variabel startPos dengan nilai awal 0
        int startPos = 0;

        // Event handler untuk timer
        private void Timer1_Tick(object sender, EventArgs e)
        {
            startPos += 1;
            MyProgress.Value = startPos;
            Pencentage.Text = startPos + "%";

            // Mengecek jika nilai progres mencapai 100
            if (MyProgress.Value == 100)
            {
                MyProgress.Value = 0;
                timer1.Stop();

                // Membuat instance form Login dan menampilkannya
                Login log = new Login();
                log.Show();

                // Menyembunyikan form Splash
                this.Hide();
            }
        }

        // Event handler saat form Splash dimuat
        private void Splash_Load_1(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
