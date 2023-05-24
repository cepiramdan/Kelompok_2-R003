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
            DisplayElements("ProductTbl", ProductsDGV);

        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Cepi Ramdan\Documents\BakeryDb.mdf"";Integrated Security=True;Connect Timeout=30");


        private void Label12_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(1);
            DisplayElements("CustomerTbl", CustomersDGV);

        }

        private void Label11_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(0);
            DisplayElements("ProductTbl", ProductsDGV);
        }

        private void Label13_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(2);
            DisplayElements("CategoryTbl", CategoryDGV);
        }

        private void Label14_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(3);
            DisplayElements("ProductTbl", BProductDGV);
            DisplayElements("SalesTbl", BillingListGDV);
            GetCustomer();
        }

        private void Label15_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(4);
        }
        private void DisplayElements(String TName, Bunifu.UI.WinForms.BunifuDataGridView DGV)
        {
            Con.Open();
            string Query = "select * from " + TName + "";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (ProdNameTb.Text == "" || QuantityTb.Text == "" || PriceTb.Text == "" || CatCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into ProductTbl(ProdName,ProdCat,ProdPrice,ProdQty)values(@PN,@PC,@PP,@PQ)", Con);
                    cmd.Parameters.AddWithValue("@PN", ProdNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", CatCb.SelectedIndex.ToString());
                    cmd.Parameters.AddWithValue("@PP", PriceTb.Text);
                    cmd.Parameters.AddWithValue("@PQ", QuantityTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("product added!!!");
                    Con.Close();
                    DisplayElements("ProductTbl", ProductsDGV);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }


        int key = 0;
        private void ProductsDGV_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            ProdNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
            QuantityTb.Text = ProductsDGV.SelectedRows[0].Cells[2].Value.ToString();
            PriceTb.Text = ProductsDGV.SelectedRows[0].Cells[3].Value.ToString();
            CatCb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();

            if (ProdNameTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (ProdNameTb.Text == "" || QuantityTb.Text == "" || PriceTb.Text == "" || CatCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update ProductTbl set ProdName=@PN,ProdCat=@PC,ProdPrice=@PP,ProdQty=@PQ where ProdId = @Pkey", Con);
                    cmd.Parameters.AddWithValue("@PN", ProdNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", CatCb.SelectedIndex.ToString());
                    cmd.Parameters.AddWithValue("@PP", PriceTb.Text);
                    cmd.Parameters.AddWithValue("@PQ", QuantityTb.Text);
                    cmd.Parameters.AddWithValue("@Pkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("product Update!!!");
                    Con.Close();
                    DisplayElements("ProductTbl", ProductsDGV);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Missing Information!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from ProductTbl where ProdId = @Pkey", Con);
                    cmd.Parameters.AddWithValue("@Pkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Deleted!!");
                    Con.Close();
                    DisplayElements("ProductTbl", ProductsDGV);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void AddCustBtn_Click(object sender, EventArgs e)
        {
            if (CNameTb.Text == "" || CAddressTb.Text == "" || CPhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into CustomerTbl(CustName,CustPhone,CustAddress)values(@CN,@CP,@CA)", Con);
                    cmd.Parameters.AddWithValue("@CN", CNameTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CAddressTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer added!!!");
                    Con.Close();
                    DisplayElements("CustomerTbl", CustomersDGV);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        int CKey = 0;
        private void CustomersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CNameTb.Text = CustomersDGV.SelectedRows[0].Cells[1].Value.ToString();
            CPhoneTb.Text = CustomersDGV.SelectedRows[0].Cells[2].Value.ToString();
            CAddressTb.Text = CustomersDGV.SelectedRows[0].Cells[3].Value.ToString();
            if (CNameTb.Text == "")
            {
                CKey = 0;
            }
            else
            {
                CKey = Convert.ToInt32(CustomersDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DelCustBtn_Click(object sender, EventArgs e)
        {
            if (CKey == 0)
            {
                MessageBox.Show("Missing Information!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CustomerTbl where CustId = @CKey", Con);
                    cmd.Parameters.AddWithValue("@CKey", CKey);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Deleted!!");
                    Con.Close();
                    DisplayElements("CustomerTbl", CustomersDGV);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void EditCustBtn_Click(object sender, EventArgs e)
        {
            if (CNameTb.Text == "" || CAddressTb.Text == "" || CPhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update CustomerTbl set CustName=@CN,CustPhone=@CP,CustAddress=@CA where CustId=@CKey", Con);
                    cmd.Parameters.AddWithValue("@CN", CNameTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CAddressTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", CKey);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer update!!!");
                    Con.Close();
                    DisplayElements("CustomerTbl", CustomersDGV);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void AddCatCtn_Click(object sender, EventArgs e)
        {
            if (CatNameTb.Text == "")
            {
                MessageBox.Show("Missing Information!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into CategoryTbl(CatName)values(@CN)", Con);
                    cmd.Parameters.AddWithValue("@CN", CatNameTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category added!!!");
                    Con.Close();
                    DisplayElements("CategoryTbl", CategoryDGV);
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
                MessageBox.Show("Missing Information!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CategoryTbl where CatId = @Ckey", Con);
                    cmd.Parameters.AddWithValue("@Ckey", CatKey);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category Deleted!!");
                    Con.Close();
                    DisplayElements("CategoryTbl", CategoryDGV);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        int CatKey = 0;
        private void CategoryDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CatNameTb.Text = CategoryDGV.SelectedRows[0].Cells[1].Value.ToString();
            if (CatNameTb.Text == "")
            {
                CatKey = 0;
            }
            else
            {
                CatKey = Convert.ToInt32(CategoryDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditCatBtn_Click(object sender, EventArgs e)
        {
            if (CatNameTb.Text == "")
            {
                MessageBox.Show("Missing Information!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update CategoryTbl set CatName=@CN where CatId=@CKey", Con);
                    cmd.Parameters.AddWithValue("@CN", CatNameTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", CatKey);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category update!!!");
                    Con.Close();
                    DisplayElements("CategoryTbl", CategoryDGV);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        int BPKey = 0;
        int Stock = 0;
        private void BProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            BProdNameTb.Text = BProductDGV.SelectedRows[0].Cells[1].Value.ToString();

            BPriceTb.Text = BProductDGV.SelectedRows[0].Cells[3].Value.ToString();

            if (BProdNameTb.Text == "")
            {
                key = 0;
                Stock = 0;
            }
            else
            {
                key = Convert.ToInt32(BProductDGV.SelectedRows[0].Cells[0].Value.ToString());
                Stock = Convert.ToInt32(BProductDGV.SelectedRows[0].Cells[2].Value.ToString());
            }
        }
        int n = 0;
        int GrdTotal = 0;
        private void AddBillBtn_Click(object sender, EventArgs e)
        {
            if (BQtyTb.Text == "")
            {
                MessageBox.Show("Enter the Quantity!!!");
            }
            else if (Convert.ToInt32(BQtyTb.Text) > Stock)
            {
                MessageBox.Show("No Enough Stock!!");
            }
            else
            {
                int total = Convert.ToInt32(BQtyTb.Text) * Convert.ToInt32(BPriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(YourBillDGV);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = BProdNameTb.Text;
                newRow.Cells[2].Value = BQtyTb.Text;
                newRow.Cells[3].Value = BPriceTb.Text;
                newRow.Cells[4].Value = total;
                YourBillDGV.Rows.Add(newRow);
                n++;
                GrdTotal = GrdTotal + total;
                GrdTotalbl.Text = "Rs" + GrdTotal;
            }
        }
        private void GetCustomer()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select CustId from CustomerTbl", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustId", typeof(int));
            dt.Load(Rdr);
            CustomerCb.ValueMember = "CustId";
            CustomerCb.DataSource = dt;
            Con.Close();
        }

        private void bunifuButton10_Click(object sender, EventArgs e)
        {
            BPriceTb.Text = "";
            BQtyTb.Text = "";
            BProdNameTb.Text = "";
        }

        private void SaveBill_Click(object sender, EventArgs e)
        {
            if (CustomerCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into SalesTbl(Customer,SAmount,SDate)values(@CN,@SA,@SD)", Con);
                    cmd.Parameters.AddWithValue("@CN", CustomerCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@SA", GrdTotal);
                    cmd.Parameters.AddWithValue("@SD", DateTime.Today.Date);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("sales added!!!");
                    Con.Close();
                    DisplayElements("SalesTbl", BillingListGDV);
                    BPriceTb.Text = "";
                    BQtyTb.Text = "";
                    BProdNameTb.Text = "";
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
    }
}

