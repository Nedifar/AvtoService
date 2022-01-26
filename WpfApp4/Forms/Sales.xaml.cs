using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp4.Forms
{
    /// <summary>
    /// Логика взаимодействия для Sales.xaml
    /// </summary>
    public partial class Sales : Window
    {
        ProductSale sale = new ProductSale();
        List<Filter> filters = new List<Filter>();
        public Sales(Product _product)
        {
            InitializeComponent();
            List<ProductSale> productSales = context.aGetContext().ProductSale.ToList();
            foreach (var c in context.aGetContext().Product.ToList())
            {
                if (c == _product)
                    filters.Add(new Filter { product = c, check = true });
                else
                    filters.Add(new Filter { product = c, check = false });
            }

            dgSale.ItemsSource = context.aGetContext().ProductSale.Where(p=>p.ProductID == _product.ID).ToList();
            filtr.ItemsSource = filters;
        }

        CollectionViewSource viewSource;
        class Filter
        {
            public Product product { get; set; }
            public bool check { get; set; }
        }

        private void filtr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var product = filtr.SelectedItem as Filter;
            dgSale.ItemsSource = context.aGetContext().ProductSale.Where(p=>p.ProductID == product.product.ID).ToList();
        }
    }
}
