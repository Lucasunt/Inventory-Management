using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GestionDeStock.Data;
using GestionDeStock.Entities;

namespace GestionDeStock
{
    public partial class ProductWindow : Window
    {
        public int WindowMode { get; set; } // 0=insert, 1=update

        public Products Changes { get; set; }
       public ProductWindow() // insert
        {
            WindowMode = 0;

            InitializeComponent();
            SetWindowIcon("icon.new.big.png");
            PreviewKeyDown += new KeyEventHandler(HandleEsc);

            tbReference.Text = SetNewID();

            _ = tbName.Focus();
            SetLanguage();
        }

        public ProductWindow(string id) // edit
        {
            WindowMode = 1;

            InitializeComponent();
            SetWindowIcon("icon.edit.png");
            PreviewKeyDown += new KeyEventHandler(HandleEsc);

            tbReference.Text = id.ToString();

            DataContext? context = new();
            Products? product = context.Products.Where(p => p.Id == id).FirstOrDefault();

            tbName.Text = product.Name.ToString();
            tbPrice.Text = product.Price.ToString();
            tbStock.Text = product.Stock.ToString();

            _ = tbName.Focus();
            SetLanguage();
        }

        private void SetWindowIcon(string imgName)
        {
            Uri iconUri = new(imgName, UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
        }

        private void SetLanguage()
        {
            if (WindowMode == 0)
            {
                Title = Translations.Get["eWTitle0"][MainWindow.LangIndex];
                lblTitle.Content = Translations.Get["eWTitle0"][MainWindow.LangIndex];
            }
            if (WindowMode == 1)
            {
                Title = Translations.Get["eWTitle1"][MainWindow.LangIndex];
                lblTitle.Content = Translations.Get["eWTitle1"][MainWindow.LangIndex];
            }
            lblReference.Content = Translations.Get["lblReference"][MainWindow.LangIndex];
            lblItem.Content = Translations.Get["lblItem"][MainWindow.LangIndex];
            lblPrice.Content = Translations.Get["lblPrice"][MainWindow.LangIndex];
            lblStock.Content = Translations.Get["lblStock"][MainWindow.LangIndex];
            btCancel.Content = Translations.Get["btCancel"][MainWindow.LangIndex];
            btSave.Content = Translations.Get["btSave"][MainWindow.LangIndex];
        }

        /// <summary>
        /// Nouvelle ID en fonction de la dernière dans la bdd.
        /// </summary>
        /// <returns></returns>
        private static string SetNewID() // exemple: "sneakers-00012"
        {
            string newlId = "sneakers-00001";
            try
            {
                DataContext? context = new();
                Products? product = context.Products.OrderByDescending(p => p.Id).FirstOrDefault();
                int tmpId = Convert.ToInt32(product.Id.Substring(9, 5));
                tmpId++;
                newlId = "0000" + tmpId.ToString();
                int len = newlId.Length;
                int startIndex = len - 5;
                newlId = string.Concat("sneakers-", newlId.AsSpan(startIndex, 5));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return newlId;
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            DataContext? context = new();
            Products product;

            if (WindowMode == 0) // insert
            {
                int count = context.Products.Where(p => p.Id.ToLower() == tbReference.Text.ToLower()).Count();
                if (count != 0)
                {
                    string msgReference0 = Translations.Get["msgReference0"][MainWindow.LangIndex];
                    string msgReference1 = Translations.Get["msgReference1"][MainWindow.LangIndex];

                    _ = MessageBox.Show(msgReference1, msgReference0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                product = new Products
                {
                    Id = tbReference.Text.ToLower(),
                    Name = tbName.Text,
                    Price = tbPrice.Text == "" ? 0 : Convert.ToDouble(tbPrice.Text.Replace('.', ',')),
                    Stock = tbStock.Text == "" ? 0 : Convert.ToInt32(tbStock.Text)
                };
                _ = context.Products.Add(product);
            }
            else // update
            {
                product = context.Products.Where(p => p.Id == tbReference.Text).First();
                product.Name = tbName.Text;
                product.Price = tbPrice.Text == "" ? 0 : Convert.ToDouble(tbPrice.Text.Replace('.', ','));
                product.Stock = tbStock.Text == "" ? 0 : Convert.ToInt32(tbStock.Text);
            }
            _ = context.SaveChanges();
            Changes = product;

            Close();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
