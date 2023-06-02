using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ProjectBakery
{
    public partial class Bakery : Form
    {
        public Bakery()
        {
            InitializeComponent();
            DisplayElements("ProductTbl", ProductsDGV); // Menampilkan elemen-elemen dari tabel "ProductTbl" ke dalam DataGridView "ProductsDGV"
            GetCategorie(); // Mendapatkan kategori
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Cepi Ramdan\Documents\BakeryDb.mdf"";Integrated Security=True;Connect Timeout=30");


        private void Label12_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(1); // Mengubah halaman yang ditampilkan pada bunifuPages1 menjadi halaman 1
            DisplayElements("CustomerTbl", CustomersDGV); // Menampilkan elemen-elemen dari tabel "CustomerTbl" ke dalam DataGridView "CustomersDGV"
        }

        private void Label11_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(0); // Mengubah halaman yang ditampilkan pada bunifuPages1 menjadi halaman 0
            DisplayElements("ProductTbl", ProductsDGV); // Menampilkan elemen-elemen dari tabel "ProductTbl" ke dalam DataGridView "ProductsDGV"
            GetCategorie(); // Mendapatkan kategori
        }

        private void GetCategorie()
        {
            Con.Open(); // Membuka koneksi database
            SqlCommand cmd = new SqlCommand("select CatId from CategoryTbl", Con); // Membuat perintah SQL untuk mendapatkan CatId dari tabel CategoryTbl
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader(); // Menjalankan perintah SQL dan mendapatkan hasilnya
            DataTable dt = new DataTable();
            dt.Columns.Add("CatId", typeof(int));
            dt.Load(Rdr); // Memuat hasil perintah SQL ke dalam DataTable
            CatCb.ValueMember = "CatId"; // Menentukan nilai yang akan digunakan sebagai ValueMember pada ComboBox CatCb
            CatCb.DataSource = dt; // Menghubungkan DataTable dt sebagai DataSource ComboBox CatCb
            Con.Close(); // Menutup koneksi database
        }

        private void Label13_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(2); // Mengubah halaman yang ditampilkan pada bunifuPages1 menjadi halaman 2
            DisplayElements("CategoryTbl", CategoryDGV); // Menampilkan elemen-elemen dari tabel "CategoryTbl" ke dalam DataGridView "CategoryDGV"
        }

        private void Label14_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(3); // Mengubah halaman yang ditampilkan pada bunifuPages1 menjadi halaman 3
            DisplayElements("ProductTbl", BProductDGV); // Menampilkan elemen-elemen dari tabel "ProductTbl" ke dalam DataGridView "BProductDGV"
            DisplayElements("SalesTbl", BillingListGDV); // Menampilkan elemen-elemen dari tabel "SalesTbl" ke dalam DataGridView "BillingListGDV"
            GetCustomer(); // Mendapatkan data pelanggan
        }

        private void Label15_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(4); // Mengubah halaman yang ditampilkan pada bunifuPages1 menjadi halaman 4
            CountCustomer(); // Menghitung jumlah pelanggan
            CountProduct(); // Menghitung jumlah produk
            SumAmount(); // Menghitung jumlah total penjualan
        }

        private void DisplayElements(String TName, Bunifu.UI.WinForms.BunifuDataGridView DGV)
        {
            Con.Open(); // Membuka koneksi database
            string Query = "select * from " + TName + ""; // Membuat query SQL untuk memilih semua data dari tabel yang ditentukan
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con); // Membuat SqlDataAdapter dengan query dan koneksi yang ditentukan
            SqlCommandBuilder builder = new SqlCommandBuilder(sda); // Membuat SqlCommandBuilder dari SqlDataAdapter
            var ds = new DataSet();
            sda.Fill(ds); // Mengisi DataSet dengan hasil query
            DGV.DataSource = ds.Tables[0]; // Menghubungkan DataTable pertama dari DataSet sebagai DataSource DataGridView yang ditentukan
            Con.Close(); // Menutup koneksi database
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (ProdNameTb.Text == "" || QuantityTb.Text == "" || PriceTb.Text == "" || CatCb.SelectedIndex == -1)
            {
                MessageBox.Show("Terjadi Kesalahan"); // Menampilkan pesan kesalahan jika ada field yang kosong atau kategori tidak dipilih
            }
            else
            {
                try
                {
                    Con.Open(); // Membuka koneksi database
                    SqlCommand cmd = new SqlCommand("Insert into ProductTbl(ProdName,ProdCat,ProdPrice,ProdQty)values(@PN,@PC,@PP,@PQ)", Con); // Membuat SqlCommand untuk melakukan operasi insert
                    cmd.Parameters.AddWithValue("@PN", ProdNameTb.Text); // Menambahkan parameter untuk nama produk
                    cmd.Parameters.AddWithValue("@PC", CatCb.SelectedValue.ToString()); // Menambahkan parameter untuk kategori produk
                    cmd.Parameters.AddWithValue("@PP", PriceTb.Text); // Menambahkan parameter untuk harga produk
                    cmd.Parameters.AddWithValue("@PQ", QuantityTb.Text); // Menambahkan parameter untuk jumlah produk
                    cmd.ExecuteNonQuery(); // Menjalankan perintah SQL untuk menambahkan data produk ke database
                    MessageBox.Show("Produk Ditambahkan"); // Menampilkan pesan bahwa produk telah ditambahkan
                    Con.Close(); // Menutup koneksi database
                    DisplayElements("ProductTbl", ProductsDGV); // Menampilkan elemen-elemen dari tabel "ProductTbl" ke dalam DataGridView "ProductsDGV"
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message); // Menampilkan pesan kesalahan jika terjadi exception saat menambahkan produk
                }
            }
        }


        int key = 0;
        private void ProductsDGV_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            ProdNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString(); // Mengisi TextBox "ProdNameTb" dengan nilai dari kolom 1 pada baris yang dipilih di DataGridView "ProductsDGV"
            QuantityTb.Text = ProductsDGV.SelectedRows[0].Cells[2].Value.ToString(); // Mengisi TextBox "QuantityTb" dengan nilai dari kolom 2 pada baris yang dipilih di DataGridView "ProductsDGV"
            PriceTb.Text = ProductsDGV.SelectedRows[0].Cells[3].Value.ToString(); // Mengisi TextBox "PriceTb" dengan nilai dari kolom 3 pada baris yang dipilih di DataGridView "ProductsDGV"
            CatCb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString(); // Mengisi ComboBox "CatCb" dengan nilai dari kolom 4 pada baris yang dipilih di DataGridView "ProductsDGV"

            if (ProdNameTb.Text == "")
            {
                key = 0; // Jika TextBox "ProdNameTb" kosong, mengatur nilai key menjadi 0
            }
            else
            {
                key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString()); // Jika TextBox "ProdNameTb" tidak kosong, mengubah nilai key menjadi integer dari kolom 0 pada baris yang dipilih di DataGridView "ProductsDGV"
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (ProdNameTb.Text == "" || QuantityTb.Text == "" || PriceTb.Text == "" || CatCb.SelectedIndex == -1)
            {
                MessageBox.Show("Terjadi Kesalahan"); // Menampilkan pesan kesalahan jika ada field yang kosong atau kategori tidak dipilih
            }
            else
            {
                try
                {
                    Con.Open(); // Membuka koneksi database
                    SqlCommand cmd = new SqlCommand("update ProductTbl set ProdName=@PN,ProdCat=@PC,ProdPrice=@PP,ProdQty=@PQ where ProdId = @Pkey", Con); // Membuat SqlCommand untuk melakukan operasi update
                    cmd.Parameters.AddWithValue("@PN", ProdNameTb.Text); // Menambahkan parameter untuk nama produk
                    cmd.Parameters.AddWithValue("@PC", CatCb.SelectedValue.ToString()); // Menambahkan parameter untuk kategori produk
                    cmd.Parameters.AddWithValue("@PP", PriceTb.Text); // Menambahkan parameter untuk harga produk
                    cmd.Parameters.AddWithValue("@PQ", QuantityTb.Text); // Menambahkan parameter untuk jumlah produk
                    cmd.Parameters.AddWithValue("@Pkey", key); // Menambahkan parameter untuk nilai kunci (ProdId)
                    cmd.ExecuteNonQuery(); // Menjalankan perintah SQL untuk memperbarui data produk di database
                    MessageBox.Show("Produk Diperbaharui"); // Menampilkan pesan bahwa produk telah diperbarui
                    Con.Close(); // Menutup koneksi database
                    DisplayElements("ProductTbl", ProductsDGV); // Menampilkan elemen-elemen dari tabel "ProductTbl" ke dalam DataGridView "ProductsDGV"
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message); // Menampilkan pesan kesalahan jika terjadi exception saat memperbarui produk
                }
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Terjadi Kesalahan"); // Menampilkan pesan kesalahan jika kunci (ProdId) bernilai 0
            }
            else
            {
                try
                {
                    Con.Open(); // Membuka koneksi database
                    SqlCommand cmd = new SqlCommand("delete from ProductTbl where ProdId = @Pkey", Con); // Membuat SqlCommand untuk melakukan operasi delete
                    cmd.Parameters.AddWithValue("@Pkey", key); // Menambahkan parameter untuk nilai kunci (ProdId)
                    cmd.ExecuteNonQuery(); // Menjalankan perintah SQL untuk menghapus data produk dari database
                    MessageBox.Show("Produk Dihapus"); // Menampilkan pesan bahwa produk telah dihapus
                    Con.Close(); // Menutup koneksi database
                    DisplayElements("ProductTbl", ProductsDGV); // Menampilkan elemen-elemen dari tabel "ProductTbl" ke dalam DataGridView "ProductsDGV"
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message); // Menampilkan pesan kesalahan jika terjadi exception saat menghapus produk
                }
            }
        }

        private void AddCustBtn_Click(object sender, EventArgs e)
        {
            if (CNameTb.Text == "" || CAddressTb.Text == "" || CPhoneTb.Text == "")
            {
                MessageBox.Show("Terjadi Kesalahan"); // Menampilkan pesan kesalahan jika ada kolom input yang kosong
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into CustomerTbl(CustName,CustPhone,CustAddress)values(@CN,@CP,@CA)", Con); // Membuat objek SqlCommand untuk melakukan penambahan data ke tabel CustomerTbl
                    cmd.Parameters.AddWithValue("@CN", CNameTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CAddressTb.Text);

                    cmd.ExecuteNonQuery(); // Menjalankan perintah SQL untuk menambahkan data ke dalam tabel
                    MessageBox.Show("Pelanggan Ditambahkan"); // Menampilkan pesan konfirmasi setelah penambahan data berhasil
                    Con.Close();
                    DisplayElements("CustomerTbl", CustomersDGV); // Menampilkan kembali elemen-elemen dalam tabel CustomerTbl pada DataGridView
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message); // Menampilkan pesan error jika terjadi exception saat penambahan data
                }
            }
        }

        int CKey = 0;
        private void CustomersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CNameTb.Text = CustomersDGV.SelectedRows[0].Cells[1].Value.ToString(); // Mengisi TextBox CNameTb dengan nilai dari kolom ke-1 pada baris yang dipilih dalam DataGridView
            CPhoneTb.Text = CustomersDGV.SelectedRows[0].Cells[2].Value.ToString(); // Mengisi TextBox CPhoneTb dengan nilai dari kolom ke-2 pada baris yang dipilih dalam DataGridView
            CAddressTb.Text = CustomersDGV.SelectedRows[0].Cells[3].Value.ToString(); // Mengisi TextBox CAddressTb dengan nilai dari kolom ke-3 pada baris yang dipilih dalam DataGridView
            if (CNameTb.Text == "")
            {
                CKey = 0; // Jika TextBox CNameTb kosong, mengatur nilai CKey menjadi 0
            }
            else
            {
                CKey = Convert.ToInt32(CustomersDGV.SelectedRows[0].Cells[0].Value.ToString()); // Jika TextBox CNameTb berisi nilai, mengkonversi nilai kolom ke-0 pada baris yang dipilih menjadi tipe data integer dan menetapkannya ke CKey
            }
        }


        private void DelCustBtn_Click(object sender, EventArgs e)
        {
            if (CKey == 0)
            {
                MessageBox.Show("Terjadi Kesalahan"); // Menampilkan pesan kesalahan jika CKey memiliki nilai 0
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CustomerTbl where CustId = @CKey", Con); // Membuat objek SqlCommand untuk menghapus data dari tabel CustomerTbl berdasarkan CustId
                    cmd.Parameters.AddWithValue("@CKey", CKey);
                    cmd.ExecuteNonQuery(); // Menjalankan perintah SQL untuk menghapus data dari tabel
                    MessageBox.Show("Pelanggang Dihapus"); // Menampilkan pesan konfirmasi setelah penghapusan data berhasil
                    Con.Close();
                    DisplayElements("CustomerTbl", CustomersDGV); // Menampilkan kembali elemen-elemen dalam tabel CustomerTbl pada DataGridView
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message); // Menampilkan pesan error jika terjadi exception saat menghapus data
                }
            }
        }

        private void EditCustBtn_Click(object sender, EventArgs e)
        {
            if (CNameTb.Text == "" || CAddressTb.Text == "" || CPhoneTb.Text == "")
            {
                MessageBox.Show("Terjadi Kesalahan"); // Menampilkan pesan kesalahan jika ada kolom input yang kosong
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update CustomerTbl set CustName=@CN,CustPhone=@CP,CustAddress=@CA where CustId=@CKey", Con); // Membuat objek SqlCommand untuk memperbarui data dalam tabel CustomerTbl berdasarkan CustId
                    cmd.Parameters.AddWithValue("@CN", CNameTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CAddressTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", CKey);
                    cmd.ExecuteNonQuery(); // Menjalankan perintah SQL untuk memperbarui data dalam tabel
                    MessageBox.Show("Berhasil Diperbahaui"); // Menampilkan pesan konfirmasi setelah pembaruan data berhasil
                    Con.Close();
                    DisplayElements("CustomerTbl", CustomersDGV); // Menampilkan kembali elemen-elemen dalam tabel CustomerTbl pada DataGridView
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message); // Menampilkan pesan error jika terjadi exception saat memperbarui data
                }
            }
        }

        private void AddCatCtn_Click(object sender, EventArgs e)
        {
            if (CatNameTb.Text == "")
            {
                MessageBox.Show("Terjadi Kesalahan"); // Menampilkan pesan kesalahan jika CatNameTb kosong
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into CategoryTbl(CatName)values(@CN)", Con); // Membuat objek SqlCommand untuk menambahkan data ke dalam tabel CategoryTbl
                    cmd.Parameters.AddWithValue("@CN", CatNameTb.Text);
                    cmd.ExecuteNonQuery(); // Menjalankan perintah SQL untuk menambahkan data ke dalam tabel
                    MessageBox.Show("Categori Ditambah"); // Menampilkan pesan konfirmasi setelah penambahan data berhasil
                    Con.Close();
                    DisplayElements("CategoryTbl", CategoryDGV); // Menampilkan kembali elemen-elemen dalam tabel CategoryTbl pada DataGridView
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void DeleteCatBtn_Click(object sender, EventArgs e)
        {
            if (CatKey == 0)
            {
                MessageBox.Show("Terjadi Kesalahan"); // Menampilkan pesan kesalahan jika CatKey memiliki nilai 0
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CategoryTbl where CatId = @Ckey", Con); // Membuat objek SqlCommand untuk menghapus data dari tabel CategoryTbl berdasarkan CatId
                    cmd.Parameters.AddWithValue("@Ckey", CatKey);

                    cmd.ExecuteNonQuery(); // Menjalankan perintah SQL untuk menghapus data dari tabel
                    MessageBox.Show("Berhasil Dihapus"); // Menampilkan pesan konfirmasi setelah penghapusan data berhasil
                    Con.Close();
                    DisplayElements("CategoryTbl", CategoryDGV); // Menampilkan kembali elemen-elemen dalam tabel CategoryTbl pada DataGridView

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message); // Menampilkan pesan error jika terjadi exception saat menghapus data
                }
            }
        }

        int CatKey = 0;
        private void CategoryDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CatNameTb.Text = CategoryDGV.SelectedRows[0].Cells[1].Value.ToString(); // Menetapkan nilai teks pada CatNameTb dengan nilai dari sel yang dipilih pada kolom indeks 1 (CatName) dari DataGridView CategoryDGV
            if (CatNameTb.Text == "") // Memeriksa apakah nilai teks pada CatNameTb kosong
            {
                CatKey = 0; // Jika kosong, mengatur nilai CatKey menjadi 0
            }
            else
            {
                CatKey = Convert.ToInt32(CategoryDGV.SelectedRows[0].Cells[0].Value.ToString()); // Jika tidak kosong, mengonversi nilai pada sel yang dipilih pada kolom indeks 0 (CatId) dari DataGridView CategoryDGV menjadi integer dan menetapkan nilainya ke CatKey
            }
        }


        private void EditCatBtn_Click(object sender, EventArgs e)
        {
            if (CatNameTb.Text == "")
            {
                MessageBox.Show("Terjadi Kesalahan"); // Menampilkan pesan kesalahan jika CatNameTb kosong
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update CategoryTbl set CatName=@CN where CatId=@CKey", Con); // Membuat objek SqlCommand untuk memperbarui data dalam tabel CategoryTbl berdasarkan CatId
                    cmd.Parameters.AddWithValue("@CN", CatNameTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", CatKey);
                    cmd.ExecuteNonQuery(); // Menjalankan perintah SQL untuk memperbarui data dalam tabel
                    MessageBox.Show("Berhasil Diperbaharui"); // Menampilkan pesan konfirmasi setelah pembaruan data berhasil
                    Con.Close();
                    DisplayElements("CategoryTbl", CategoryDGV); // Menampilkan kembali elemen-elemen dalam tabel CategoryTbl pada DataGridView
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message); // Menampilkan pesan error jika terjadi exception saat memperbarui data
                }
            }
        }

        int Stock = 0;
        private void BProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            BProdNameTb.Text = BProductDGV.SelectedRows[0].Cells[1].Value.ToString(); // Menetapkan nilai teks pada BProdNameTb dengan nilai dari sel yang dipilih pada kolom indeks 1 dari DataGridView BProductDGV
            BPriceTb.Text = BProductDGV.SelectedRows[0].Cells[3].Value.ToString(); // Menetapkan nilai teks pada BPriceTb dengan nilai dari sel yang dipilih pada kolom indeks 3 dari DataGridView BProductDGV

            if (BProdNameTb.Text == "") // Memeriksa apakah nilai teks pada BProdNameTb kosong
            {
                key = 0; // Jika kosong, mengatur nilai key menjadi 0
                Stock = 0; // Jika kosong, mengatur nilai Stock menjadi 0
            }
            else
            {
                key = Convert.ToInt32(BProductDGV.SelectedRows[0].Cells[0].Value.ToString()); // Jika tidak kosong, mengonversi nilai pada sel yang dipilih pada kolom indeks 0 dari DataGridView BProductDGV menjadi integer dan menetapkan nilainya ke key
                Stock = Convert.ToInt32(BProductDGV.SelectedRows[0].Cells[2].Value.ToString()); // Jika tidak kosong, mengonversi nilai pada sel yang dipilih pada kolom indeks 2 dari DataGridView BProductDGV menjadi integer dan menetapkan nilainya ke Stock
            }
        }

        int n = 0;
        int GrdTotal = 0;
        private void AddBillBtn_Click(object sender, EventArgs e)
        {
            if (BQtyTb.Text == "")
            {
                MessageBox.Show("Masukan Jumlah"); // Menampilkan pesan kesalahan jika BQtyTb kosong
            }
            else if (Convert.ToInt32(BQtyTb.Text) > Stock)
            {
                MessageBox.Show("Stok Tidak Cukup"); // Menampilkan pesan kesalahan jika jumlah yang dimasukkan melebihi stok
            }
            else
            {
                int total = Convert.ToInt32(BQtyTb.Text) * Convert.ToInt32(BPriceTb.Text); // Menghitung total dengan mengalikan nilai yang diambil dari BQtyTb dan BPriceTb
                DataGridViewRow newRow = new DataGridViewRow(); // Membuat baris baru untuk ditambahkan ke DataGridView YourBillDGV
                newRow.CreateCells(YourBillDGV); // Membuat sel-sel pada baris baru sesuai dengan jumlah kolom pada DataGridView YourBillDGV
                newRow.Cells[0].Value = n + 1; // Menetapkan nilai pada sel kolom indeks 0 dengan nilai n + 1
                newRow.Cells[1].Value = BProdNameTb.Text; // Menetapkan nilai pada sel kolom indeks 1 dengan nilai dari BProdNameTb
                newRow.Cells[2].Value = BQtyTb.Text;
                newRow.Cells[3].Value = BPriceTb.Text; 
                newRow.Cells[4].Value = total; // Menetapkan nilai pada sel kolom indeks 4 dengan nilai total yang dihitung sebelumnya
                YourBillDGV.Rows.Add(newRow); // Menambahkan baris baru ke DataGridView YourBillDGV
                n++; 
                GrdTotal = GrdTotal + total; // Menambahkan total ke GrdTotal
                GrdTotalbl.Text = "Rp. " + GrdTotal; // Menampilkan nilai GrdTotal pada GrdTotalbl dengan format "Rp. {nilai}"
            }

        }

        private void GetCustomer()
        {
            Con.Open(); // Membuka koneksi ke database
            SqlCommand cmd = new SqlCommand("Select CustId from CustomerTbl", Con); // Membuat objek SqlCommand untuk mengambil data CustId dari tabel CustomerTbl
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader(); // Menjalankan perintah SqlCommand dan mendapatkan hasil pembacaan data
            DataTable dt = new DataTable(); // Membuat objek DataTable untuk menyimpan hasil data
            dt.Columns.Add("CustId", typeof(int)); // Menambahkan kolom "CustId" dengan tipe data int ke DataTable
            dt.Load(Rdr); // Memuat data pembacaan ke dalam DataTable
            CustomerCb.ValueMember = "CustId"; // Mengatur properti ValueMember dari ComboBox CustomerCb ke "CustId"
            CustomerCb.DataSource = dt; // Mengatur sumber data ComboBox CustomerCb menjadi DataTable dt
            Con.Close(); // Menutup koneksi ke database
        }

        private void BunifuButton10_Click(object sender, EventArgs e)
        {
            BPriceTb.Text = ""; // Mengosongkan teks pada TextBox BPriceTb
            BQtyTb.Text = ""; // Mengosongkan teks pada TextBox BQtyTb
            BProdNameTb.Text = ""; // Mengosongkan teks pada TextBox BProdNameTb
        }

        private void SaveBill_Click(object sender, EventArgs e)
        {
            if (CustomerCb.SelectedIndex == -1)
            {
                MessageBox.Show("Terjadi Kesalahan"); // Menampilkan pesan kesalahan jika CustomerCb belum dipilih
            }
            else
            {
                try
                {
                    Con.Open(); // Membuka koneksi ke database
                    SqlCommand cmd = new SqlCommand("Insert into SalesTbl(Customer,SAmount,SDate)values(@CN,@SA,@SD)", Con); // Membuat objek SqlCommand untuk menyimpan data penjualan ke dalam tabel SalesTbl
                    cmd.Parameters.AddWithValue("@CN", CustomerCb.SelectedValue.ToString()); // Menambahkan parameter @CN dengan nilai CustomerCb.SelectedValue.ToString()
                    cmd.Parameters.AddWithValue("@SA", GrdTotal); // Menambahkan parameter @SA dengan nilai GrdTotal
                    cmd.Parameters.AddWithValue("@SD", DateTime.Today.Date); // Menambahkan parameter @SD dengan tanggal saat ini
                    cmd.ExecuteNonQuery(); // Menjalankan perintah SqlCommand untuk menyimpan data ke dalam tabel SalesTbl
                    MessageBox.Show("Berhasil Ditambah"); // Menampilkan pesan bahwa data berhasil ditambahkan
                    Con.Close(); // Menutup koneksi ke database
                    DisplayElements("SalesTbl", BillingListGDV); // Menampilkan elemen-elemen dari tabel SalesTbl ke dalam DataGridView BillingListGDV
                    BPriceTb.Text = ""; // Mengosongkan teks pada TextBox BPriceTb
                    BQtyTb.Text = ""; // Mengosongkan teks pada TextBox BQtyTb
                    BProdNameTb.Text = ""; // Mengosongkan teks pada TextBox BProdNameTb
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message); // Menampilkan pesan kesalahan jika terjadi exception
                }
            }
        }

        private void CountCustomer()
        {
            Con.Open(); // Membuka koneksi ke database
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from CustomerTBL", Con); // Membuat objek SqlDataAdapter untuk mengambil jumlah pelanggan dari tabel CustomerTBL
            DataTable dt = new DataTable(); // Membuat objek DataTable untuk menyimpan hasil data
            sda.Fill(dt); // Mengisi DataTable dengan data hasil eksekusi SqlDataAdapter
            CustLbl.Text = dt.Rows[0][0].ToString() + " Pelanggan"; // Menampilkan jumlah pelanggan ke dalam label CustLbl
            Con.Close(); // Menutup koneksi ke database
        }

        private void CountProduct()
        {
            Con.Open(); // Membuka koneksi ke database
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from ProductTBL", Con); // Membuat objek SqlDataAdapter untuk mengambil jumlah produk dari tabel ProductTBL
            DataTable dt = new DataTable(); // Membuat objek DataTable untuk menyimpan hasil data
            sda.Fill(dt); // Mengisi DataTable dengan data hasil eksekusi SqlDataAdapter
            ProductLbl.Text = dt.Rows[0][0].ToString() + " Items"; // Menampilkan jumlah produk ke dalam label ProductLbl
            Con.Close(); // Menutup koneksi ke database
        }

        private void SumAmount()
        {
            Con.Open(); // Membuka koneksi ke database
            SqlDataAdapter sda = new SqlDataAdapter("Select Sum(SAmount) from SalesTbl", Con); // Membuat objek SqlDataAdapter untuk mengambil total jumlah penjualan dari tabel SalesTbl
            DataTable dt = new DataTable(); // Membuat objek DataTable untuk menyimpan hasil data
            sda.Fill(dt); // Mengisi DataTable dengan data hasil eksekusi SqlDataAdapter
            SalesLbl.Text = "Rp. " + dt.Rows[0][0].ToString(); // Menampilkan total jumlah penjualan ke dalam label SalesLbl dengan menambahkan "Rs" sebagai awalan
            Con.Close(); // Menutup koneksi ke database
        }

        private void Label16_Click(object sender, EventArgs e)
        {
            Login Obj = new Login(); // Membuat objek Login
            Obj.Show(); // Menampilkan form Login
            this.Hide(); // Menyembunyikan form saat ini
        }
    }
}