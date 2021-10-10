using System.Collections.Generic;

namespace GestionDeStock
{
    internal class Translations
    {
        public static Dictionary<string, string[]> Get = new()
        {
            { "mWTitle", new string[] { "Logiciel de gestion de stock", "Lagerverwaltung Applikation" } },
            { "lblSubtitle", new string[] { "Produits en stock", "Produkte am Lager" } },
            { "lblReference", new string[] { "Référence:", "Referenz:" } },
            { "lblItem", new string[] { "Nom article:", "Artikel Name:" } },
            { "lblPrice", new string[] { "Prix [CHF]:", "Preis [CHF]:" } },
            { "lblStock", new string[] { "Stock:", "Lager:" } },
            { "lblPriceMinMax", new string[] { "Prix min./max.:", "Preis min./max.:" } },

            { "gvReference", new string[] { "Référence", "Referenz" } },
            { "gvItem", new string[] { "Nom article", "Artikel Name" } },
            { "gvPrice", new string[] { "Prix [CHF]", "Preis [CHF]" } },
            { "gvStock", new string[] { "Stock", "Stock" } },

            { "btCancel", new string[] { "Annuler", "Annulieren" } },
            { "btSave", new string[] { "Enregistrer", "Speichern" } },
            { "btNew", new string[] { "Nouveau produit", "Neues Produkt" } },
            { "btSearch", new string[] { "Rechercher", "Suchen" } },

            { "cmNew", new string[] { "Nouveau", "Neu" } },
            { "cmEdit", new string[] { "Editer", "Ändern" } },
            { "cmDelete", new string[] { "Effacer", "Löschen" } },

            { "eWTitle0", new string[] { "Nouveau produit", "Neues Produkt" } },
            { "eWTitle1", new string[] { "Edition produit", "Produkt ändern" } },

            { "msgDelete0", new string[] { "Effacer", "Löschen" } },
            { "msgDelete1", new string[] { "Êtes-vous sûre de vouloir effacer l'article?", "Sind Sie sicher, dass Sie den Artikel löschen möchten?" } },
            { "msgReference0", new string[] { "Référence", "Referenz" } },
            { "msgReference1", new string[] { "Erreur: La référence produit existe déjà, veuillez la changer!", "Fehler: Die Artikelreferenz existiert schon, bitte ändern!" } }
        };
    }
}
