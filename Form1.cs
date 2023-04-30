using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DbFirst
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnlistele_Click(object sender, EventArgs e)
        {
            using (ETrade12Entities db = new ETrade12Entities())
            {
                var query = from a in db.Tbl_Urunler
                            join b in db.Tbl_Categoriler on a.CategoriId equals b.Id
                            select new { a.Id, a.Name, a.Fiyat, a.Stok, b.Aciklama };

                dataGridView1.DataSource = query.ToList();
            }
        }

        private void btnekle_Click(object sender, EventArgs e)
        {
            using (ETrade12Entities db = new ETrade12Entities())
            {
                Tbl_Urunler yeniUrun = new Tbl_Urunler();
                yeniUrun.Name = txtName.Text;
                yeniUrun.Fiyat = Convert.ToDecimal(txtFiyat.Text);
                yeniUrun.Stok = Convert.ToInt32(txtStok.Text);
                yeniUrun.CategoriId = Convert.ToInt32(cmbKategori.SelectedValue);

                var urun = (from a in db.Tbl_Urunler
                            join b in db.Tbl_Categoriler on a.CategoriId equals b.Id
                            where a.Id == yeniUrun.Id
                            select new { a.Id, a.Name, a.Fiyat, a.Stok, b.Aciklama }).FirstOrDefault();

                db.Tbl_Urunler.Add(yeniUrun);
                db.SaveChanges();
                MessageBox.Show("Ürün başarıyla eklendi.");
                txtName.Clear();
                txtFiyat.Clear();
                txtStok.Clear();
                cmbKategori.SelectedIndex = -1;

                dataGridView1.DataSource = db.Tbl_Urunler.Select(u => new { u.Id, u.Name, u.Fiyat, u.Stok, u.Tbl_Categoriler.Aciklama }).ToList();
            }

        }

        private void btnsil_Click(object sender, EventArgs e)
        {
            using (ETrade12Entities db = new ETrade12Entities())
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                var silinecekUrun = db.Tbl_Urunler.Find(id);
                db.Tbl_Urunler.Remove(silinecekUrun);
                db.SaveChanges();
                MessageBox.Show("Ürün başarıyla silindi.");
                var query = from a in db.Tbl_Urunler
                            join b in db.Tbl_Categoriler on a.CategoriId equals b.Id
                            select new { a.Id, a.Name, a.Fiyat, a.Stok, b.Aciklama };
                dataGridView1.DataSource = query.ToList();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (ETrade12Entities db = new ETrade12Entities())
            {
                cmbKategori.DataSource = db.Tbl_Categoriler.ToList();
                cmbKategori.DisplayMember = "Aciklama";
                cmbKategori.ValueMember = "Id";
            }
        }
    }
}
