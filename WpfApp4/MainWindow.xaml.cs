using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //metod();
        }

        private void mouse(object sender, MouseEventArgs e)
        {
            int count = Directory.GetFiles(@"C:\Users\gazimov.ii0794\Desktop\images").Count();
            double widht = border.ActualWidth / count;
            var winPos = e.GetPosition(border);
            double a = Math.Truncate(winPos.X / widht);
            string[] mas = Directory.GetFiles(@"C:\Users\gazimov.ii0794\Desktop\images");
            gg.Source = new BitmapImage(new Uri(mas[Convert.ToInt32(a)]));
        }
        private void metod()
        {
            var file = File.ReadAllLines(@"C:\Users\gazimov.ii0794\Desktop\import\port.txt", Encoding.Default);
            foreach (var line in file)
            {
                string[] mas = line.Split('\t');
                string name = mas[0].Trim();
                ProductSale product = new ProductSale()
                {
                    Product = context.aGetContext().Product.Where(p=>p.Title == name).FirstOrDefault(),
                    Quantity = int.Parse(mas[1].Trim()),
                    SaleDate = Convert.ToDateTime(mas[2])
                };
                context.aGetContext().ProductSale.Add(product);
                context.aGetContext().SaveChanges();
            }
            //foreach (var line in file)
            //{
            //    string[] mas = line.Split(',');
            //    Manufacturer manufacturer= new Manufacturer()
            //    {
            //        Name = mas[0],
            //        StartDate = Convert.ToDateTime(mas[1]).Date
            //    };
            //    context.aGetContext().Manufacturer.Add(manufacturer);
            //    context.aGetContext().SaveChanges();
            //}
        }
    }
}
