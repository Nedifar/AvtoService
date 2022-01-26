using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;

namespace WpfApp4.Forms
{
    /// <summary>
    /// Логика взаимодействия для AddEdit.xaml
    /// </summary>
    public partial class AddEdit : Window
    {
        Product product = new Product();
        public AddEdit(Product _product)
        {
            InitializeComponent();
            if (_product != null)
                product = _product;
            cbmanaf.ItemsSource = context.aGetContext().Manufacturer.ToList();
            cbdopProducts.ItemsSource = context.aGetContext().Product.Where(p => p.IsActive == true && p.ID != product.ID).ToList();
            DataContext = product;
        }

        private void clPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files(*.PNG;*.JPG;*.JPEG)|*.PNG;*.JPG;*.JPEG";
            openFile.InitialDirectory = @"Y:\4пк2\!!!ТоварыАвтосервиса\Сессия 1\products_a_import\Товары автосервиса";
            if (openFile.ShowDialog() == true)
            {
                if (new FileInfo(openFile.FileName).Length / 1024 <= 2048)
                {
                    product.MainImagePath = openFile.FileName.Replace(@"Y:\4пк2\!!!ТоварыАвтосервиса\Сессия 1\products_a_import\", "");
                    image.Source = new BitmapImage(new Uri(openFile.FileName));
                }
                else
                {
                    MessageBox.Show("Рамер изображения превышает допустимый");
                }
            }
        }

        private void clSave(object sender, RoutedEventArgs e)
        {
            if (product.Cost >= 0 && product.Manufacturer != null && !String.IsNullOrWhiteSpace(product.Title))
            {
                if (product.ID == 0)
                    context.aGetContext().Product.Add(product);
                context.aGetContext().SaveChanges();
                MessageBox.Show("Продкут успешно сохранен.");
                Close();
            }
            else
            {
                MessageBox.Show("Проверьте правильность введенных данных");
            }
        }

        private void clDelete(object sender, RoutedEventArgs e)
        {
            if(lvDop.SelectedItems.Count != 0)
            {
                product.Product1.Remove(lvDop.SelectedItem as Product);
            }
        }

        private void clAdd(object sender, RoutedEventArgs e)
        {
            AddEdit addEdit = new AddEdit((sender as Button).DataContext as Product);
            addEdit.Show();

        }

        private void clAddNewGroup(object sender, RoutedEventArgs e)
        {
            if (cbdopProducts.SelectedIndex != -1)
            {
                product.Product1.Add(cbdopProducts.SelectedItem as Product);
                MessageBox.Show("Added succesfully");
            }
            else
                MessageBox.Show("Выберите продукт для добавления");
        }
    }
}
