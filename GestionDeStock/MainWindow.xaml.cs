using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GestionDeStock.Data;
using GestionDeStock.Entities;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace GestionDeStock
{
    public partial class MainWindow : Window
    {
        public static int LangIndex { get; set; }

        /// <summary>
        /// ObservableCollection; utilisé afin de pouvoir rafraichir automatiquement la listview.
        /// </summary>
        private ObservableCollection<Products>? listProducts;
        private ProductWindow? pw;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                DataContext? context = new();
                List<Products>? products = context.Products.ToList();
                listProducts = new ObservableCollection<Products>(products);
                lvProducts.ItemsSource = listProducts;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur: " + ex.Message);
            }

            _ = tbReference.Focus();
        }

        /// <summary>
        /// Met à jour la listview après fermeture de la fenêtre d'édition.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PW_Closed(object? sender, EventArgs e)
        {
            if (pw == null)
            {
                return;
            }
            else if (pw.Changes == null)
            {
                return;
            }
            else
            {
                if (pw.WindowMode == 0) // insert
                {
                    if (LvProductsNumberOfItems(pw.Changes.Id) == 0)
                    {
                        listProducts.Add(pw.Changes);
                    }
                }
                else if (pw.WindowMode == 1) // update
                {
                    if (lvProducts.SelectedItems.Count > 0)
                    {
                        (lvProducts.SelectedItems[0] as Products).Name = pw.Changes.Name;
                        (lvProducts.SelectedItems[0] as Products).Price = pw.Changes.Price;
                        (lvProducts.SelectedItems[0] as Products).Stock = pw.Changes.Stock;
                    }
                }
            }
        }

        /// <summary>
        /// Test le nombre d'items dans la listview portant l'id passée en paramètre.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int LvProductsNumberOfItems(string id)
        {
            int nombre = 0;

            for (int i = 0; i < lvProducts.Items.Count; i++)
            {
                if (((Products)lvProducts.Items[i]).Id == id)
                {
                    nombre++;
                }
            }
            return nombre;
        }

        /// <summary>
        /// Fonction utilisée afin de pouvoir sortir les colonnes de la listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvProducts_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader)
            {
                GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;

                string bindingProperty = null!;
                if (clickedColumn != null)
                {
                    if (clickedColumn.Header.ToString() is "Référence" or "Referenz")
                    {
                        bindingProperty = "Id";
                    }
                    if (clickedColumn.Header.ToString() is "Nom article" or "Artikel Name")
                    {
                        bindingProperty = "Name";
                    }
                    if (clickedColumn.Header.ToString() is "Prix [CHF]" or "Preis [CHF]")
                    {
                        bindingProperty = "Price";
                    }
                    if (clickedColumn.Header.ToString() is "Stock" or "Stock")
                    {
                        bindingProperty = "Stock";
                    }
                    SortDescriptionCollection sdc = lvProducts.Items.SortDescriptions;
                    ListSortDirection sortDirection = ListSortDirection.Ascending;
                    if (sdc.Count > 0)
                    {
                        SortDescription sd = sdc[0];
                        sortDirection = (ListSortDirection)((((int)sd.Direction) + 1) % 2);
                        sdc.Clear();
                    }
                    sdc.Add(new SortDescription(bindingProperty, sortDirection));
                }
            }
        }

        private void LvProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Edit();
        }

        private void CmEdit_Click(object sender, RoutedEventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            if (lvProducts.SelectedItems.Count == 0)
            {
                return;
            }

            Products? p = (Products)lvProducts.SelectedItems[0];
            pw = new ProductWindow(p.Id);
            pw.Closed += PW_Closed;
            pw.Show();
        }

        /// <summary>
        /// Met à jour la bdd et la listProducts de la listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmDelete_Click(object sender, RoutedEventArgs e)
        {
            string msgDelete0 = Translations.Get["msgDelete0"][LangIndex];
            string msgDelete1 = Translations.Get["msgDelete1"][LangIndex];

            MessageBoxResult result = MessageBox.Show(msgDelete1, msgDelete0, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DataContext? context = new();
                    _ = context.Products.Remove((Products)lvProducts.SelectedItems[0]);
                    _ = context.SaveChanges();

                    _ = listProducts.Remove((Products)lvProducts.SelectedItems[0]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        private void btSearch_Click(object sender, RoutedEventArgs e)
        {
            string reference = tbReference.Text.ToLower();
            string nom = tbName.Text.ToLower();
            double priceMin = tbPriceMin.Text == "" ? 0 : Convert.ToDouble(tbPriceMin.Text.Replace('.', ','));
            double priceMax = tbPriceMax.Text == "" ? 0 : Convert.ToDouble(tbPriceMax.Text.Replace('.', ','));

            try
            {
                DataContext? context = new();
                List<Products>? products = context.Products.ToList();

                if (reference.Length > 0)
                {
                    products = products.Where(p => p.Id.ToLower().Contains(reference)).ToList();
                }
                if (nom.Length > 0)
                {
                    products = products.Where(p => p.Name.ToLower().Contains(nom)).ToList();
                }
                if (priceMin > 0)
                {
                    products = products.Where(p => p.Price >= priceMin).ToList();
                }
                if (priceMax > 0)
                {
                    products = products.Where(p => p.Price <= priceMax).ToList();
                }

                listProducts = new ObservableCollection<Products>(products);
                lvProducts.ItemsSource = listProducts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void BtNew_Click(object sender, RoutedEventArgs e)
        {
            New();
        }

        private void CmNew_Click(object sender, RoutedEventArgs e)
        {
            New();
        }

        private void New()
        {
            pw = new ProductWindow();
            pw.Closed += PW_Closed;
            pw.Show();
        }

        private void BtFR_Click(object sender, RoutedEventArgs e)
        {
            SetLanguage(0);
        }

        private void BtDE_Click(object sender, RoutedEventArgs e)
        {
            SetLanguage(1);
        }

        private void SetLanguage(int langIndex)
        {
            LangIndex = langIndex; // 0 = FR, 1 = DE

            mWindow.Title = Translations.Get["mWTitle"][langIndex];
            lblSubtitle.Content = Translations.Get["lblSubtitle"][langIndex];
            lblReference.Content = Translations.Get["lblReference"][langIndex];
            lblItem.Content = Translations.Get["lblItem"][langIndex];
            lblPriceMinMax.Content = Translations.Get["lblPriceMinMax"][langIndex];
            btNew.Content = Translations.Get["btNew"][langIndex];
            btSearch.Content = Translations.Get["btSearch"][langIndex];
            gvReference.Header = Translations.Get["gvReference"][langIndex];
            gvItem.Header = Translations.Get["gvItem"][langIndex];
            gvPrice.Header = Translations.Get["gvPrice"][langIndex];
            gvStock.Header = Translations.Get["gvStock"][langIndex];
            cmNew.Header = Translations.Get["cmNew"][langIndex];
            cmEdit.Header = Translations.Get["cmEdit"][langIndex];
            cmDelete.Header = Translations.Get["cmDelete"][langIndex];
        }
    }
}
