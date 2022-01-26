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
            viewSource = new CollectionViewSource();
            viewSource.Source = productSales;
            viewSource.Filter += ViewSource_Filter;
            
            dgSale.ItemsSource = viewSource.View;
            filtr.ItemsSource = filters;
        }

        private void ViewSource_Filter(object sender, FilterEventArgs e)
        {
            foreach(var fil in filters.Where(p=>p.check == true))
            {
                if ((e.Item as ProductSale).Product.Title == fil.product.Title)
                {
                    //e.Accepted;
                }
            }
        }

        CollectionViewSource viewSource;
        class Filter
        {
            public Product product { get; set; }
            public bool check { get; set; }
        }
    }
}
