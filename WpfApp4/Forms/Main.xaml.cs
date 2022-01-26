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
using System.Windows.Shapes;
using DevExpress.XtraPdfViewer;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

namespace WpfApp4.Forms
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        List<Product> Products = context.aGetContext().Product.ToList();
        List<Product> ItogProducts = new List<Product>();
        public Main()
        {
            InitializeComponent();
            all.IsChecked = true;
            lvProducts.ItemsSource = Products;
            List<Manufacturer> str = new List<Manufacturer>();
            str.Add(new Manufacturer { Name = "Все производители" });
            str.AddRange(context.aGetContext().Manufacturer);
            filtr.ItemsSource = str;
            cbSort.ItemsSource = new string[] { "Не выбрано", "По убыванию", "По возрастанию" };
            Counter(Products.Count());
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            lvProducts.ItemsSource = Products.Where(p=>lev(p.Title,search.Text) <=3).ToList();
            Counter(Products.Where(p => lev(p.Title, search.Text) <= 3).Count());
            if (search.Text.Trim() == "")
            {
                lvProducts.ItemsSource = Products.ToList();
                Counter(Products.Count());
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.CheckBox).Content.ToString() != "Все производители")
            {
                all.IsChecked = false;
                ItogProducts.AddRange(Products.Where(p => p.Manufacturer.Name == (sender as System.Windows.Controls.CheckBox).Content.ToString()).ToList());
                lvProducts.ItemsSource = ItogProducts;
                otherSearch();
            }
            else
            {
                search_TextChanged(null, null);
                all.IsChecked = true;
            }
        }
        private void otherSearch()
        {
            lvProducts.ItemsSource = ItogProducts.Where(p => p.Title.Contains(search.Text)).ToList();
            Counter(ItogProducts.Where(p => p.Title.Contains(search.Text)).Count());
            if (search.Text.Trim() == "")
            {
                lvProducts.ItemsSource = ItogProducts.ToList();
                Counter(ItogProducts.Count());

            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.CheckBox).Content.ToString() != "Все производители")
            {
                if (filtr.SelectedIndex != 0)
                {
                    ItogProducts = ItogProducts.Where(p => p.Manufacturer.Name != (sender as System.Windows.Controls.CheckBox).Content.ToString()).ToList();
                    lvProducts.ItemsSource = ItogProducts;
                    otherSearch();
                }
            }
            else
            {
                search_TextChanged(null, null);
                all.IsChecked = true;
            }
        }
        private void Counter(int count)
        {
            tbCount.Text = count + " из " + context.aGetContext().Product.Count();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbSort.SelectedIndex)
            {
                case 0:
                    if (ItogProducts.Count() != 0)
                    {
                        //I
                        
                    }
                    else
                    {

                    }
                    break;
                case 1:
                    if(ItogProducts.Count()!=0)
                    {

                    }
                    break;
                case 2:
                    break;
            }
        }

        private void clDelete(object sender, RoutedEventArgs e)
        {
            var rem = lvProducts.SelectedItem as Product;
            if(lvProducts.SelectedItems.Count !=0)
            {
                if (rem.ProductSale.Count == 0)
                {
                    context.aGetContext().Product.Remove(rem);
                    context.aGetContext().SaveChanges();
                    MessageBox.Show("Удаление завершено");
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите элемент для удаления, у данного товара есть история продаж");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для удаления");
            }
        }

        private void clAdd(object sender, RoutedEventArgs e)
        {
            AddEdit add = new AddEdit(null);
            add.Show();
            add.IsVisibleChanged += Add_IsVisibleChanged;
            btAdd.IsEnabled = false;
            btEdit.IsEnabled = false;
        }

        private void Add_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if((sender as Window).IsVisible == false)
            {
                btAdd.IsEnabled = true;
                btEdit.IsEnabled = true;
            }
        }

        private void clEdit(object sender, RoutedEventArgs e)
        {
            if (lvProducts.SelectedItems.Count != 0)
            {
                AddEdit add = new AddEdit(lvProducts.SelectedItem as Product);
                add.Show();
                add.IsVisibleChanged += Add_IsVisibleChanged;
                btAdd.IsEnabled = false;
                btEdit.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для редактирования");
            }
        }

        private void clSales(object sender, RoutedEventArgs e)
        {
            Sales sales = new Sales(lvProducts.SelectedItem as Product);
            sales.Show();
        }

        private int lev(string string1, string string2)
        {
            int def;
            int[,] m = new int[string1.Length + 1, string2.Length + 1];
            for (int i = 0; i <= string1.Length; i++)
                m[i, 0] = i;
            for (int j = 0; j <= string2.Length; j++)
                m[0, j] = j;
            for(int i = 1; i<=string1.Length; i++)
            {
                for(int j = 1; j<=string2.Length; j++)
                {
                    def = (string1[i - 1] == string2[j - 1]) ? 0 : 1;
                    m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1, m[i, j - 1] + 1), m[i - 1, j - 1] + def);

                }
            }
            return m[string1.Length, string2.Length];
        }

        private void clReport(object sender, RoutedEventArgs e)
        {
            var processor = new RichEditDocumentServer();
            processor.CreateNewDocument();
            var document = processor.Document;
            document.BeginUpdate();
            var paragraph = document.Paragraphs.Append();
            document.InsertText(paragraph.Range.Start, "New Report");
            var paragraph1 = document.Paragraphs.Append();
            var table = document.Tables.Create(paragraph1.Range.Start, context.aGetContext().Product.Count(), 2);
            for(int i =0; i<context.aGetContext().Product.Count(); i++)
            {
                document.InsertText(table[i, 0].Range.Start, Products[i].Title);
                document.InsertText(table[i, 1].Range.Start, Products[i].Cost.ToString());
            }
            var paragraph2 = document.Paragraphs.Append();
            var shape = document.Shapes.InsertPicture(paragraph2.Range.Start, DocumentImageSource.FromFile(@"C:\Users\gazimov.ii0794\Desktop\images\307880.jpg"));
            shape.RelativeVerticalPosition = ShapeRelativeVerticalPosition.Paragraph;
            shape.Height = 400;
            document.EndUpdate();
            processor.ExportToPdf($@"{AppDomain.CurrentDomain.BaseDirectory}new.pdf");
            PdfViewer viewer = new PdfViewer();
            viewer.LoadDocument($@"{AppDomain.CurrentDomain.BaseDirectory}new.pdf");
            viewer.Print();

            using(var csv = new StreamWriter($@"{AppDomain.CurrentDomain.BaseDirectory}new.csv", true, Encoding.UTF8))
            {
                foreach (var a in context.aGetContext().ProductSale)
                {
                    var line = string.Format("{0},{1},{2}", a.ID, a.Product.Title, a.SaleDate);
                    csv.WriteLine(line);
                }
                csv.Flush();
            }
        }
    }
}
