using System.ComponentModel;

namespace GestionDeStock.Entities
{
    /// <summary>
    /// Implémentation de l'interface INotifyPropertyChanged pour le rechargement dynamique des données dans la listview.
    /// Class de base pour générer la base de données.
    /// </summary>
    public class Products : INotifyPropertyChanged
    {
        private string name;
        private double price;
        private int stock;

        public string Id { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }
        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                NotifyPropertyChanged("Price");
            }
        }
        public int Stock
        {
            get { return stock; }
            set
            {
                stock = value;
                NotifyPropertyChanged("Stock");
            }
        }

        public Products()
        {

        }
        public Products(string id, string name, double price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
