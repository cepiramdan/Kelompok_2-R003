using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectBakery
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            // Memeriksa apakah kotak teks UNameTb atau PasswordTb kosong
            if (UNameTb.Text == "" || PasswordTb.Text == "")
            {
                MessageBox.Show("Terjadi Kesalahan");
            }
            else
            {
                // Memeriksa apakah UNameTb dan PasswordTb memiliki nilai yang sesuai
                if (UNameTb.Text == "Admin" && PasswordTb.Text == "123456")
                {
                    // Membuat objek dari kelas Bakery dan menampilkannya
                    Bakery Obj = new Bakery();
                    Obj.Show();
                    this.Hide();
                }
                else
                {
                    // Menampilkan pesan kesalahan jika UNameTb dan PasswordTb tidak sesuai
                    MessageBox.Show("Masukan Data Yang Benar");
                    UNameTb.Text = "";
                    PasswordTb.Text = "";
                }
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            // Keluar dari aplikasi saat tombol PictureBox1 diklik
            Application.Exit();
        }
    }
}
