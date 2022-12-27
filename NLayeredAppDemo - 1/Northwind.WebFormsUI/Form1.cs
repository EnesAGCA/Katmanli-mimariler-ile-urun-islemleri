using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.DataAccess.Abstract;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.DataAccess.Concrete.NHibernate;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WebFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _productService = new ProductManager(new EfProductDal());
            _categoryService = new CategoryManager(new EfCategoryDal());
        }

        private IProductService _productService;
        private ICategoryService _categoryService;
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadCategories();
        }

        private void LoadCategories()
        {
            cbxCategory.DataSource = _categoryService.GetAll();
            cbxCategory.DisplayMember = "CategoryName";    //görünen
            cbxCategory.ValueMember = "categoryId";        //seçtiğimizde bize id si lazım

            cbxCategoryId.DataSource = _categoryService.GetAll();
            cbxCategoryId.DisplayMember = "CategoryName";    //görünen
            cbxCategoryId.ValueMember = "categoryId";        //seçtiğimizde bize id si lazım

            cbxCategoryIdUpdate.DataSource = _categoryService.GetAll();
            cbxCategoryIdUpdate.DisplayMember = "CategoryName";    //görünen
            cbxCategoryIdUpdate.ValueMember = "categoryId";        //seçtiğimizde bize id si lazım


        }

        private void LoadProducts()
        {
            dgwProduct.DataSource = _productService.GetAll();
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try  //değerler dolarken hata alıyorduk hatırlarsan ikisini birden getirmeye çalışıyordu onun yerine ilk başta normal ürünleri getir biz category  tıklayınca dediğimizürünleri getirirsin 
            {                                                                               //selected value bir onject onu çevirmem lazım int olarak
                dgwProduct.DataSource = _productService.GetProductsByCategory(Convert.ToInt32(cbxCategory.SelectedValue)); //ben sana ürünleri geitrmen için bir category id verecem sen o category id ye göre bana ürünleri getir

            }
            catch
            {

            }
        }

        private void tbxProductName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxProductName.Text))      //textbox ta ürün varsa bunu  getir
            {
                dgwProduct.DataSource = _productService.GetProductsByProductName(tbxProductName.Text);       //productservice götürecek unutma

            }
            else
            {
                LoadProducts();       // yoksa tümünü getir
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _productService.Add(new Product
                {
                    CategoryId = Convert.ToInt32(cbxCategoryId.SelectedValue),
                    ProductName = tbxProductName2.Text,
                    QuantityPerUnit = tbxQuantityPerUnit.Text,
                    UnitPrice = Convert.ToDecimal(tbxUnitPrice.Text),
                    UnitsInStock = Convert.ToInt16(tbxStock.Text)
                });
                MessageBox.Show("Ürün Eklendi");
                LoadProducts();
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
            _productService.Update(new Product
            {
                ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value),
                ProductName = tbxUpdateProductName.Text,
                CategoryId = Convert.ToInt32(cbxCategoryIdUpdate.SelectedValue),
                UnitsInStock = Convert.ToInt16(tbxUnitsInStockUpdate.Text),
                QuantityPerUnit = tbxQuantityPerUnitUpdate.Text,
                UnitPrice = Convert.ToDecimal(tbxUnitPriceUpdate.Text),


            });
            MessageBox.Show("Ürün Güncellendi!");
            LoadProducts();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);

            }

        }

        private void dgwProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgwProduct.CurrentRow;
            tbxUpdateProductName.Text = row.Cells[1].Value.ToString();
            cbxCategoryIdUpdate.SelectedValue = row.Cells[2].ToString();
            tbxUnitPriceUpdate.Text = row.Cells[3].Value.ToString();
            tbxQuantityPerUnitUpdate.Text = row.Cells[4].Value.ToString();
            tbxUnitsInStockUpdate.Text = row.Cells[5].Value.ToString();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgwProduct.CurrentRow != null) // boş değilse aşağıdakini uygula   ,, bir şey seçmeden silme işlemi gerçekleştiremeyecek
            {
                try
                {
                    _productService.Delete(new Product                   //burada hatayı kullanıcıya nasısl göstereceğimle ilgileniyorum form çünküm burası
                    {
                        ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value)
                    });
                    MessageBox.Show("Ürün Silindi!");
                    LoadProducts();
                }
                catch (Exception exception)                                             // buranın amacı ilişkisel veritabanından dolayı hata gönderiyor bir id yi silince veritabanından (bizim eklediğimiz ürün için vermez) onu başka yerde yeri olduğu için lan noluyor oluyor
                {
                    MessageBox.Show(exception.Message);

                }

            }


        }
    }
}
