using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Common;
using MySqlX.XDevAPI.Relational;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Xml;

namespace Projet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=fleurs;UID=root;PASSWORD=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            Console.WriteLine("Bienvenue sur l'application de l'entreprise de fleurs !");
            Console.WriteLine();
            Console.WriteLine("Que voulez vous faire ? 1:connexion, 2:nouveau compte, 3:quitter");
            string action = Console.ReadLine();

            while (action != "1" && action != "2")
            {
                Console.Write("Veuillez réessayer : ");
                action = Console.ReadLine();
            }

            Console.WriteLine();

            string reponse;
            if (action == "2")
            {
                ajouterClient(connection);
                Console.Write("Souhaitez vous vous connecter ? ");
                reponse = Console.ReadLine();
                if (reponse == "oui") { action = "1"; }
                Console.WriteLine();
            }

            if (action == "1")
            {
                string co = connexion(connection);

                if (co != "admin" && co != "" && co !="exit") //client
                {
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT prenom FROM client WHERE courriel='{co}';";
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string prenom = reader.GetValue(0).ToString();
                    reader.Close();

                    List<string> actions = new List<string>();
                    actions.AddRange(new string[] { "exit", "nouvelle commande","actions", "afficher composition bouquet standard", "afficher mes commandes" });
                    Console.Clear();
                    Console.WriteLine($"Bienvenue {prenom} !");
                    Console.WriteLine();
                    Console.WriteLine("Que voulez vous faire aujourd'hui ?");
                    string actionDesiree = Console.ReadLine();
                    while (actions.Contains(actionDesiree.ToLower()) == false)
                    {
                        Console.Write("Action non reconnue, veuillez réessayer : ");
                        actionDesiree = Console.ReadLine();
                    }

                    while (actionDesiree != "exit")
                    {
                        if (actionDesiree == "nouvelle commande") { ajouterCommande(co, connection); }
                        else if (actionDesiree == "afficher composition bouquet standard") { afficherFormationBouquetStandard(connection); }
                        else if (actionDesiree == "afficher mes commandes") { afficherCommandesClient(co, connection); }
                        else if (actionDesiree == "actions") { foreach (string i in actions) { Console.Write(i + ", "); } }
                        Console.WriteLine();
                        Console.Write("Action terminée. Veuillez entrer une nouvelle action : ");
                        actionDesiree = Console.ReadLine();
                        while (actions.Contains(actionDesiree.ToLower()) == false)
                        {
                            Console.Write("Action non reconnue, veuillez réessayer : ");
                            actionDesiree = Console.ReadLine();
                        }
                    }

                }

                if (co == "admin") //admin
                {

                    Console.Clear();
                    Console.Write("Que désirez vous faire aujourd'hui ? Pour afficher la liste des actions possibles, tapez actions : ");

                    List<string> actions = new List<string>();
                    actions.AddRange(new string[] { "requete", "statistiques", "xml", "actions","supprimer item", "afficher commandes", "preparer commande", "exit", "arrivage", "former bouquet personnalise", "former bouquet standard", "afficher composition bouquet standard", "nouveau client", "nouveau bouquet standard", "nouvelle fleur", "nouvel accessoire", "infos client", "stock", "nouvelle commande" });

                    string actionDesiree = Console.ReadLine();
                    while (actions.Contains(actionDesiree.ToLower()) == false)
                    {
                        Console.Write("Action non reconnue, veuillez réessayer : ");
                        actionDesiree = Console.ReadLine();
                    }


                    while (actionDesiree.ToLower() != "exit")
                    {
                        Console.WriteLine();
                        if (actionDesiree.ToLower() == "arrivage" || actionDesiree == "arrivages") { arrivages(connection); }
                        else if (actionDesiree.ToLower() == "afficher commandes") { afficherCommandes(connection); }
                        else if (actionDesiree.ToLower() == "statistiques") { statistiques(connection); }
                        else if (actionDesiree.ToLower() == "supprimer item") { supprimerItem(connection); }
                        else if (actionDesiree.ToLower() == "preparer commande") { preparerCommande(connection); }
                        else if (actionDesiree.ToLower() == "nouveau bouquet standard") { nouveauBouquetStandard(connection); }
                        else if (actionDesiree.ToLower() == "nouveau client") { ajouterClient(connection); }
                        else if (actionDesiree.ToLower() == "nouvelle fleur") { nouvelleFleur(connection); }
                        else if (actionDesiree.ToLower() == "nouvel accessoire") { nouvelAccessoire(connection); }
                        else if (actionDesiree.ToLower() == "xml") { exportXMLClientsFideles(connection); }
                        else if (actionDesiree.ToLower() == "infos client") { ficheClient(connection); }
                        else if (actionDesiree.ToLower() == "stock") { affichageStockMagasin(connection); }
                        else if (actionDesiree.ToLower() == "requete") { requete(connection); }
                        else if (actionDesiree.ToLower() == "former bouquet standard") { formerBouquetStandard(connection); }
                        else if (actionDesiree.ToLower() == "afficher composition bouquet standard") { afficherFormationBouquetStandard(connection); }
                        else if (actionDesiree.ToLower() == "actions")
                        {
                            Console.Write("Voici la liste de toutes les actions disponibles : ");
                            foreach (string i in actions) { Console.Write(i + ", "); }
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                        Console.Write("Action terminée, veuillez en entrer une nouvelle : ");
                        actionDesiree = Console.ReadLine();
                        while (actions.Contains(actionDesiree.ToLower()) == false)
                        {
                            Console.Write("Action non reconnue, veuillez réessayer : ");
                            actionDesiree = Console.ReadLine();
                        }
                    }
                }

                else if (co == "") //pas de connection
                {
                    Console.WriteLine("Connexion refusée.");
                }

            }

            Console.WriteLine();
            Console.WriteLine("Fin des opérations !");
            connection.Close();
            Console.ReadLine();

        }

        //Fonctions utiles

        static void requete(MySqlConnection connection)
        {
            Console.Write("Entrer requête SQL : ");
            string requete = Console.ReadLine();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)    // parcours cellule par cellule
                {
                    string valueAsString = reader.GetValue(i).ToString();  // recuperation de la valeur de chaque cellule sous forme d'une string (voir cependant les differentes methodes disponibles !!)
                    currentRowAsString += valueAsString + ", ";
                }
                Console.WriteLine(currentRowAsString);
            }
            Console.WriteLine();
            reader.Close();
        }

        static void afficherTable(string table, MySqlConnection connection)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {table}";
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())                           // parcours ligne par ligne
            {
                string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)    // parcours cellule par cellule
                {
                    string valueAsString = reader.GetValue(i).ToString();  // recuperation de la valeur de chaque cellule sous forme d'une string (voir cependant les differentes methodes disponibles !!)
                    currentRowAsString += valueAsString + ", ";
                }
                Console.WriteLine(currentRowAsString);    // affichage de la ligne (sous forme d'une "grosse" string) sur la sortie standard
            }
            reader.Close();
        }

        static List<string> getPrimaryKeys(string table, string pk, MySqlConnection connection)
        {
            List<string> listeClients = new List<string>();
            string requete = $"SELECT {pk} FROM {table}";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    listeClients.Add(valueAsString);
                }
            }
            reader.Close();
            return listeClients;
        }

        static List<string> getPrimaryKeysLower(string table, string pk, MySqlConnection connection)
        {
            List<string> listeClients = new List<string>();
            string requete = $"SELECT {pk} FROM {table}";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString().ToLower();
                    listeClients.Add(valueAsString);
                }
            }
            reader.Close();
            return listeClients;
        }


        //Fonctions du projet

        static string connexion(MySqlConnection connection)
        {
            Console.Write("Veuillez entrer votre courriel : ");
            string nomU = Console.ReadLine();
            if (nomU == "admin") { return "admin"; }
            List<string> courrielsClient = getPrimaryKeysLower("client", "courriel", connection);
            int validateur = 0;
            if (courrielsClient.Contains(nomU))
            {
                validateur = 1;
            }


            if (validateur == 0)
            {
                while (validateur != 1)
                {
                    Console.Write("Courriel inconnu. Veuillez réessayer : ");
                    nomU = Console.ReadLine();
                    if (nomU == "admin") { return "admin"; }
                    if (nomU == "exit") { return "exit"; }
                    if (courrielsClient.Contains(nomU))
                    {
                        validateur = 1;
                    }
                }

            }

            string mdpcorrect;
            Console.Write("Veuillez entrer le mot de passe (ou taper changer mot de passe) : ");
            string password = "";
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                password += keyInfo.KeyChar;
                Console.Write("*");
            }

            if (password == "changer mot de passe")
            {
                changerMDP(connection, nomU);
            }

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT motdepasse FROM client WHERE courriel='{nomU}'";
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            mdpcorrect = reader.GetValue(0).ToString();
            reader.Close();

            string action;
            while (password != mdpcorrect)
            {
                Console.WriteLine();
                Console.Write("Mot de passe incorrect veuillez réessayer (ou taper exit pour quitter) : ");
                action = "";
                while (true)
                {
                    var keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    action += keyInfo.KeyChar;
                    Console.Write("*");
                }
                Console.WriteLine();
                if (action == "exit") { return ""; }
                else { password = action; }
            }

            return nomU;

        }

        static void ajouterClient(MySqlConnection connection)
        {
            Console.WriteLine();
            Console.WriteLine("Ajout d'un nouveau client :");
            Console.Write("Courriel : ");
            string courrielNew = Console.ReadLine();
            List<string> listeClients = getPrimaryKeys("client", "courriel", connection);
            int cpt = 0;
            foreach (string c in listeClients)
            {
                if (c == courrielNew) { cpt++; }
            }
            if (cpt != 0)
            {
                Console.WriteLine("Courriel déjà présent dans la base. Pas de modification apportée.");
            }

            //Ajout du client
            else
            {
                Console.Write("Veuillez entrer un mot de passe : ");
                string password = "";
                while (true)
                {
                    var keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                Console.WriteLine();
                Console.Write("Veuillez confirmer le mot de passe : ");
                string verification = "";
                while (true)
                {
                    var keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    verification += keyInfo.KeyChar;
                    Console.Write("*");
                }
                Console.WriteLine();
                while (password != verification)
                {
                    Console.Write("Les mots de passes ne sont pas identiques ! Veuillez réessayer : ");
                    password = "";
                    while (true)
                    {
                        var keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            break;
                        }
                        password += keyInfo.KeyChar;
                        Console.Write("*");
                    }
                    Console.WriteLine();
                    Console.Write("Veuillez confirmer le mot de passe : ");
                    verification = "";
                    while (true)
                    {
                        var keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            break;
                        }
                        verification += keyInfo.KeyChar;
                        Console.Write("*");
                    }
                    Console.WriteLine();
                }

                Console.Write("Nom : ");
                string NomNew = Console.ReadLine();
                Console.Write("Prénom : ");
                string PrenomNew = Console.ReadLine();
                Console.Write("Numéro de téléphone : ");
                string PhoneNew = Console.ReadLine();
                int phone;
                while (int.TryParse(PhoneNew, out phone) == false)
                {
                    Console.Write("Veuillez réessayer : ");
                    PhoneNew = Console.ReadLine();
                }
                phone = int.Parse(PhoneNew);
                Console.Write("Adresse : ");
                string addresseNew = Console.ReadLine();

                string requete = $"INSERT INTO client(Courriel,motdepasse, Nom, Prenom, Numero_de_telephone, Adresse, Statut_Fidelite) VALUES('{courrielNew}','{password}', '{NomNew}', '{PrenomNew}', {phone}, '{addresseNew}', 'BRONZE');";
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = requete;
                MySqlDataReader reader = command.ExecuteReader();
                Console.WriteLine($"Le client '{courrielNew}' à bien été ajouté"); ;
                reader.Close();
            }
        }

        static void ficheClient(MySqlConnection connection)
        {
            Console.WriteLine();
            Console.Write("Veuillez entrer le courriel pour la fiche client : ");
            string emailFiche = Console.ReadLine();

            List<string> listeClients = getPrimaryKeys("client", "courriel", connection);

            //Affichage de la fiche
            int cpt = 0;
            while (listeClients.Contains(emailFiche) == false)
            {
                Console.Write("Le client est introuvable. Veuillez réessayer : ");
                emailFiche = Console.ReadLine();
            }

            Console.WriteLine();
            string requete = $"SELECT * FROM client WHERE courriel = '{emailFiche}'";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> listeInfos = new List<string>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    listeInfos.Add(valueAsString);
                }
            }
            reader.Close();
            Console.WriteLine("Fiche client de : " + listeInfos[2] + " " + listeInfos[3]);
            Console.WriteLine("");
            Console.WriteLine("Courriel : " + listeInfos[0]);
            Console.WriteLine("Numéro de téléphone : " + listeInfos[4]);
            Console.WriteLine("Addresse : " + listeInfos[5]);
            Console.WriteLine("Statut de fidélité : " + listeInfos[6]);
        }

        static void nouvelleFleur(MySqlConnection connection)
        {
            Console.WriteLine();
            Console.WriteLine("Ajout d'une nouvelle fleur à la base de données !");
            Console.WriteLine();
            Console.Write("Quel est le nom de la nouvelle fleur ? ");
            string newFlower = Console.ReadLine();

            List<string> fleurs = getPrimaryKeys("fleur", "NomFleur", connection);
            int cpt = 0;
            foreach (string f in fleurs)
            {
                if (f.ToLower() == newFlower.ToLower()) { cpt++; }
            }
            if (cpt != 0) { Console.WriteLine("Cette fleur se trouve déjà dans la base de données"); }
            else
            {
                Console.Write("Quel est le prix unitaire réel de cette fleur ? ");
                string prixUnitaireFlower = Console.ReadLine();
                Console.Write("Quelle est la catégorie de cette fleur ? (Attention, pas de é ou d'apostrophe) ");
                string categorieNewFlower = Console.ReadLine();

                string requete = $"INSERT INTO fleur (NomFleur,Prix,Disponibilite) VALUES ('{newFlower}',{prixUnitaireFlower},'{categorieNewFlower}');";
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = requete;
                MySqlDataReader reader = command.ExecuteReader();
                Console.WriteLine("La nouvelle fleur à bien été ajoutée !");
                reader.Close();
            }



        }

        static void nouvelAccessoire(MySqlConnection connection)
        {
            Console.WriteLine();
            Console.WriteLine("Ajout d'un nouvel accessoire à la base de données !");
            Console.WriteLine();
            Console.Write("Quel est le nom de ce nouvel accessoire ? ");
            string newAccessoire = Console.ReadLine();

            List<string> accessoires = getPrimaryKeys("accessoire", "NomAccessoire", connection);
            int cpt = 0;
            foreach (string i in accessoires)
            {
                if (i.ToLower() == newAccessoire.ToLower())
                {
                    cpt++;
                }
            }
            if (cpt != 0) { Console.WriteLine("Cet accessoire est déjà dans la base de données"); }
            else
            {
                Console.Write("Quel est le prix unitaire réel de ce nouvel accessoire ? ");
                string prixUnitaireAccessoire = Console.ReadLine();

                string requete = $"INSERT INTO accessoire (NomAccessoire,Prix) VALUES ('{newAccessoire}',{prixUnitaireAccessoire});";
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = requete;
                MySqlDataReader reader = command.ExecuteReader();
                Console.WriteLine("Le nouvel accessoire à bien été ajouté !");
                reader.Close();
            }
        }

        static void affichageStockMagasin(MySqlConnection connection)
        {
            Console.WriteLine();
            Console.Write("Veuillez entrer l ID du magasin dont on veut afficher les caractéritiques : ");
            string id = Console.ReadLine();

            List<string> idMagasins = getPrimaryKeys("magasin", "idMagasin", connection);
            while (idMagasins.Contains(id) == false)
            {
                Console.Write("L ID de ce magasin est introuvable, veuillez réessayer : ");
                id = Console.ReadLine();
            }

            List<string> infosMagasin = new List<string>();
            string requete = $"SELECT NomMagasin,Localisation FROM magasin WHERE idMagasin = {id}";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    infosMagasin.Add(valueAsString);
                }
            }
            reader.Close();

            Console.WriteLine("Nom du magasin : " + infosMagasin[0]);
            Console.WriteLine("Localisation du magasin : " + infosMagasin[1]);
            Console.WriteLine();
            Console.WriteLine("Stock de fleurs : ");

            requete = $"SELECT NomFleur,stock FROM stock_fleurs WHERE idMagasin = {id}";
            command = connection.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(0) + " : " + reader.GetString(1));
            }
            reader.Close();

            Console.WriteLine();
            Console.WriteLine("Stock d accessoires : ");

            requete = $"SELECT NomAccessoire,stock FROM stock_accessoires WHERE idMagasin = {id}";
            command = connection.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(0) + " : " + reader.GetString(1));
            }
            reader.Close();

            Console.WriteLine();
            Console.WriteLine("Stock de bouquets standards : ");
            command = connection.CreateCommand();
            command.CommandText = $"SELECT NomBouquetStandard,Stock FROM Contient_Bouquet_Standard WHERE idMagasin='{id}';";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(0) + " : " + reader.GetString(1));
            }
            reader.Close();




        }

        static void arrivages(MySqlConnection connection)
        {
            Console.WriteLine();
            Console.WriteLine("Nouveaux arrivages ! ");
            Console.WriteLine();

            //Selection du magasin concerné (avec sécurité)
            Console.WriteLine("Quel est l'id du magasin concerné par cet arrivage ?");
            int id_magasin_arrivage = int.Parse(Console.ReadLine());
            List<string> idMagasins = getPrimaryKeys("magasin", "idMagasin", connection);
            while (!idMagasins.Contains(id_magasin_arrivage.ToString()))
            {
                Console.WriteLine("Mauvais id de magasin, veuillez réessayer.");
                id_magasin_arrivage = int.Parse(Console.ReadLine());
            }

            Console.WriteLine();

            //Choix du type d'arrivage (fleur ou accessoire)
            Console.WriteLine("Veuillez renseigner la nature des nouveaux arrivages (fleur ou accessoire) : ");
            string natureArrivage = Console.ReadLine();
            List<string> possibilities = new List<string>();
            possibilities.AddRange(new string[] { "fleur", "fleurs", "Fleurs", "Fleur", "accessoire", "Accessoire", "accessoires", "Accessoires" });
            while (possibilities.Contains(natureArrivage) == false)
            {
                Console.WriteLine("Erreur d'entrée, veuillez réessayer");
                natureArrivage = Console.ReadLine();
            }
            Console.WriteLine();

            //Cas où l'arrivage est des fleurs
            if (natureArrivage.ToLower() == "fleur" || natureArrivage.ToLower() == "fleurs")
            {
                Console.WriteLine("Quel est le nom des fleurs qui sont arrivées ?");
                string nomFleur = Console.ReadLine();

                //Liste du nom des fleurs du magasin
                List<string> listeFleursMagasin = new List<string>();
                string requete = $"SELECT NomFleur FROM stock_fleurs WHERE idMagasin = {id_magasin_arrivage}";
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = requete;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string valueAsString = reader.GetValue(i).ToString().ToLower();
                        listeFleursMagasin.Add(valueAsString);
                    }
                }
                reader.Close();

                //Vérification de l'existence de la fleur dans la base
                List<string> listeFleurs = getPrimaryKeysLower("fleur", "NomFleur", connection);

                while (listeFleurs.Contains(nomFleur.ToLower()) == false)
                {
                    Console.WriteLine("La fleur entrée n'existe pas dans la base de données. Veuillez recommencer.");
                    nomFleur = Console.ReadLine();
                }
                Console.WriteLine();

                //Cas où on a déjà un stock de ces fleurs dans le magasin
                if (listeFleursMagasin.Contains(nomFleur.ToLower()))
                {
                    requete = $"SELECT stock FROM stock_fleurs WHERE idMagasin = {id_magasin_arrivage} AND NomFleur = '{nomFleur}'";
                    command = connection.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Read();
                    int stock_arrivage_fleur = int.Parse(reader.GetValue(0).ToString());
                    reader.Close();
                    Console.WriteLine($"Combien de {nomFleur} sont arrivées ?");
                    int nb_arrivage = int.Parse(Console.ReadLine());
                    if (nb_arrivage < 0)
                    {
                        Console.Write("Vous avez entré un nombre incohérent, veuillez retaper : ");
                        nb_arrivage = int.Parse(Console.ReadLine());
                    }
                    Console.WriteLine();
                    int newStock = stock_arrivage_fleur + nb_arrivage;

                    requete = $"UPDATE stock_fleurs SET stock={newStock} WHERE NomFleur = '{nomFleur}' and idMagasin = {id_magasin_arrivage}";
                    command = connection.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    Console.WriteLine($"Stock de {nomFleur} mis à jour : initiallement {stock_arrivage_fleur}, maintenant {newStock}");
                    reader.Close();
                }

                //Cas où les fleurs sont nouvelles dans le magasin
                else
                {
                    Console.WriteLine($"Combien de {nomFleur} sont arrivées ?");
                    int nb_arrivage = int.Parse(Console.ReadLine());
                    if (nb_arrivage < 0)
                    {
                        Console.Write("Vous avez entré un nombre incohérent, veuillez retaper : ");
                        nb_arrivage = int.Parse(Console.ReadLine());
                    }
                    Console.WriteLine();
                    requete = $"INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES ({id_magasin_arrivage},'{nomFleur}',{nb_arrivage});";
                    command = connection.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    Console.WriteLine($"Stock de {nomFleur} crée ! maintenant {nb_arrivage} pièces.");
                    reader.Close();
                }

            }

            //Cas où l'arrivage est des accessoires
            if (natureArrivage.ToLower() == "accessoires" || natureArrivage.ToLower() == "accessoire")
            {
                Console.WriteLine("Quel est le nom des accessoires qui sont arrivés ?");
                string nomAccessoire = Console.ReadLine();

                //Liste du nom des accessoires du magasin
                List<string> listeAccessoiresMagasin = new List<string>();
                string requete = $"SELECT NomAccessoire FROM stock_accessoires WHERE idMagasin = {id_magasin_arrivage}";
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = requete;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string valueAsString = reader.GetValue(i).ToString().ToLower();
                        listeAccessoiresMagasin.Add(valueAsString);
                    }
                }
                reader.Close();

                //Vérification de l'existence de l'accessoire dans la base
                List<string> listeAccessoires = getPrimaryKeysLower("accessoire", "NomAccessoire", connection);

                while (listeAccessoires.Contains(nomAccessoire.ToLower()) == false)
                {
                    Console.WriteLine("L'accessoire entré n'existe pas dans la base de données. Veuillez recommencer.");
                    nomAccessoire = Console.ReadLine();
                }

                //Cas où on a déjà un stock de ces accessoires dans le magasin
                if (listeAccessoiresMagasin.Contains(nomAccessoire.ToLower()))
                {
                    requete = $"SELECT stock FROM stock_accessoires WHERE idMagasin = {id_magasin_arrivage} AND NomAccessoire = '{nomAccessoire}'";
                    command = connection.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Read();
                    int stock_arrivage_accessoire = int.Parse(reader.GetValue(0).ToString());
                    reader.Close();
                    Console.WriteLine($"Combien de {nomAccessoire} sont arrivés ?");
                    int nb_arrivage = int.Parse(Console.ReadLine());
                    int newStock = stock_arrivage_accessoire + nb_arrivage;

                    requete = $"UPDATE stock_accessoires SET stock={newStock} WHERE NomAccessoire = '{nomAccessoire}' and idMagasin = {id_magasin_arrivage}";
                    command = connection.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    Console.WriteLine($"Stock de {nomAccessoire} mis à jour : initiallement {stock_arrivage_accessoire}, maintenant {newStock}");
                    reader.Close();
                }

                //Cas où les accessoires sont nouveaux dans le magasin
                else
                {
                    Console.WriteLine($"Combien de {nomAccessoire} sont arrivés ?");
                    int nb_arrivage = int.Parse(Console.ReadLine());
                    requete = $"INSERT INTO stock_accessoires (idMagasin,NomAccessoire,Stock) VALUES ({id_magasin_arrivage},'{nomAccessoire}',{nb_arrivage});";
                    command = connection.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    Console.WriteLine($"Stock de {nomAccessoire} crée ! maintenant {nb_arrivage} pièces.");
                    reader.Close();
                }
            }
        }

        static void formerBouquetPersonnalise(string client, int code, string magasin, MySqlConnection connection)
        {
            Console.WriteLine();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT Statut_Fidelite FROM client WHERE courriel ='{client}'";
            MySqlDataReader reader = command.ExecuteReader();
            string fidelite;
            if (reader.Read())
            {
                fidelite = reader.GetValue(0).ToString().ToLower();
            }
            else { fidelite = ""; }
            reader.Close();



            //Création du bouquet
            List<int> listeIdBouquetsPersonnalise = new List<int>();             //Liste des idBouquets pour ne pas faire de doublons
            command = connection.CreateCommand();
            command.CommandText = "SELECT id_Bouquet_Personnalise FROM bouquet_personnalise";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    listeIdBouquetsPersonnalise.Add(int.Parse(valueAsString));
                }
            }
            reader.Close();

            List<string> listeIdMagasins = getPrimaryKeys("magasin", "idMagasin", connection);

            int idBP = code;
            string idMagasin = magasin;


            //Composition du bouquet
            List<string> listeFleurs = getPrimaryKeysLower("fleur", "NomFleur", connection);
            List<string> listeAccessoires = getPrimaryKeys("accessoire", "NomAccessoire", connection);
            List<int> quantitesFleurs = new List<int>();
            List<int> quantitesAccessoires = new List<int>();
            int quantiteVoulue, quantiteDisponible;

            float prixBouquet = 0;
            string quantitevouluestring;

            foreach (string i in listeFleurs)
            {
                Console.Write($"Combien de {i.ToLower()}s va contenir ce bouquet ? ");

                quantitevouluestring = Console.ReadLine();

                while (int.TryParse(quantitevouluestring, out quantiteVoulue) == false)
                {
                    Console.Write("Veuillez réessayer : ");
                    quantitevouluestring = Console.ReadLine();
                }

                command = connection.CreateCommand();
                command.CommandText = $"SELECT stock FROM stock_fleurs WHERE NomFleur='{i}' AND idMagasin = {idMagasin};";
                reader = command.ExecuteReader();
                if (reader.Read()) { quantiteDisponible = int.Parse(reader.GetValue(0).ToString()); }
                else { quantiteDisponible = 0; }
                reader.Close();

                while (quantiteVoulue > quantiteDisponible)
                {
                    Console.Write($"Pas assez de stock, veuillez baisser le nombre de {i.ToLower()}s souhaitées : ");
                    quantiteVoulue = int.Parse(Console.ReadLine());
                }

                quantitesFleurs.Add(quantiteVoulue);

                command = connection.CreateCommand();
                command.CommandText = $"SELECT prix FROM fleur WHERE NomFleur='{i}';";
                reader = command.ExecuteReader();
                if (reader.Read()) { prixBouquet += float.Parse(reader.GetValue(0).ToString()); }
                reader.Close();


            }



            foreach (string i in listeAccessoires)
            {
                Console.Write($"Combien de {i.ToLower()}s va contenir ce bouquet ? ");
                quantitevouluestring = Console.ReadLine();

                while (int.TryParse(quantitevouluestring, out quantiteVoulue) == false)
                {
                    Console.Write("Veuillez réessayer : ");
                    quantitevouluestring = Console.ReadLine();
                }

                int.TryParse(quantitevouluestring, out quantiteVoulue);

                command = connection.CreateCommand();
                command.CommandText = $"SELECT stock FROM stock_accessoires WHERE NomAccessoire='{i}' AND idMagasin = {idMagasin};";
                reader = command.ExecuteReader();
                if (reader.Read()) { quantiteDisponible = int.Parse(reader.GetValue(0).ToString()); }
                else { quantiteDisponible = 0; }
                reader.Close();

                while (quantiteVoulue > quantiteDisponible)
                {
                    Console.Write($"Pas assez de stock, veuillez baisser le nombre de {i.ToLower()}s souhaités : ");
                    quantiteVoulue = int.Parse(Console.ReadLine());
                }

                quantitesAccessoires.Add(quantiteVoulue);

                command = connection.CreateCommand();
                command.CommandText = $"SELECT prix FROM accessoire WHERE NomAccessoire='{i}';";
                reader = command.ExecuteReader();
                if (reader.Read()) { prixBouquet += float.Parse(reader.GetValue(0).ToString()); }
                reader.Close();
            }

            if (fidelite == "or") { prixBouquet *= 0.85f; }
            if (fidelite == "bronze") { prixBouquet *= 0.95f; }

            string prix = prixBouquet.ToString().Replace(",", ".");


            //Mise à jour des tables
            command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO bouquet_personnalise (id_Bouquet_Personnalise,Prix) VALUES ({idBP},{prix});"; //Table de bouquets personnalisés
            reader = command.ExecuteReader();
            reader.Close();

            string fleurActuelle, accessoireActuel;
            int quantiteAvant;
            int cpt = 0;
            foreach (int i in quantitesFleurs)   //Pour les fleurs
            {
                fleurActuelle = listeFleurs[cpt];

                command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO Composition_Fleurs_Personnalise (id_Bouquet_Personnalise,NomFleur,Quantite) VALUES ({idBP},'{fleurActuelle}',{i});";
                reader = command.ExecuteReader();
                reader.Close();

                command = connection.CreateCommand();
                command.CommandText = $"SELECT stock FROM stock_fleurs WHERE idMagasin={idMagasin} AND NomFleur = '{fleurActuelle}';";
                reader = command.ExecuteReader();
                if (reader.Read()) { quantiteAvant = int.Parse(reader.GetValue(0).ToString()); }
                else { quantiteAvant = 0; }
                reader.Close();

                command.CommandText = $"UPDATE stock_fleurs SET stock = {quantiteAvant - i} WHERE NomFleur = '{fleurActuelle}' and idMagasin = {idMagasin};";
                reader = command.ExecuteReader();
                reader.Close();

                cpt++;
            }

            cpt = 0;

            foreach (int i in quantitesAccessoires)   //Pour les accessoires
            {
                accessoireActuel = listeAccessoires[cpt];

                command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO composition_accessoires_personnalises (id_Bouquet_Personnalise,NomAccessoire,Quantite) VALUES ({idBP},'{accessoireActuel}',{i});";
                reader = command.ExecuteReader();
                reader.Close();

                command = connection.CreateCommand();
                command.CommandText = $"SELECT stock FROM stock_accessoires WHERE idMagasin={idMagasin} AND NomAccessoire = '{accessoireActuel}';";
                reader = command.ExecuteReader();
                reader.Read();
                quantiteAvant = int.Parse(reader.GetValue(0).ToString());
                reader.Close();

                command.CommandText = $"UPDATE stock_accessoires SET stock = {quantiteAvant - i} WHERE NomAccessoire = '{accessoireActuel}' and idMagasin = {idMagasin};";
                reader = command.ExecuteReader();
                reader.Close();

                cpt++;
            }

            Console.WriteLine("Le bouquet à bien été crée. Les stocks du magasin ont été mis à jour.");

        }

        static void formerBouquetStandard(MySqlConnection connection)
        {
            //Définition du bouquet dont il est question
            Console.WriteLine();
            Console.Write("Quel magasin forme le bouquet standard ? ");
            int magasin = int.Parse(Console.ReadLine());
            List<string> magasins = getPrimaryKeysLower("magasin", "idMagasin", connection);
            while (magasins.Contains(magasin.ToString()) == false)
            {
                Console.Write("Magasin non reconnu ! Veuillez réessayer : ");
                magasin = int.Parse(Console.ReadLine());
            }

            Console.Write("Quel bouquet à été formé : ");
            string bouquetForme = Console.ReadLine();
            List<string> listeBouquetStandard = getPrimaryKeysLower("bouquet_standard", "NomBouquetStandard", connection);
            while (listeBouquetStandard.Contains(bouquetForme.ToLower()) == false)
            {
                Console.Write("Bouquet inexistant, veuillez réessayer : ");
                bouquetForme = Console.ReadLine();
            }

            //Informations sur le bouquet
            List<string> nomFleurs = new List<string>();
            List<int> quantiteFleurs = new List<int>();
            float prixBouquet = 0;
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT NomFleur,quantite FROM composition_fleurs_standard WHERE Nom_Bouquet_Standard = '{bouquetForme}'";
            MySqlDataReader reader = command.ExecuteReader();
            int cpt = 0;
            int quantite;
            while (reader.Read())
            {
                nomFleurs.Add(reader.GetValue(0).ToString().ToLower());
                quantiteFleurs.Add(int.Parse(reader.GetValue(1).ToString()));
            }
            reader.Close();

            List<string> nomAccessoires = new List<string>();
            List<int> quantiteAccessoires = new List<int>();
            command = connection.CreateCommand();
            command.CommandText = $"SELECT Nom_Accessoire,quantite FROM composition_accessoires_standard WHERE NomBouquetStandard = '{bouquetForme}'";
            reader = command.ExecuteReader();
            cpt = 0;
            while (reader.Read())
            {
                nomAccessoires.Add(reader.GetValue(0).ToString().ToLower());
                quantiteAccessoires.Add(int.Parse(reader.GetValue(1).ToString()));
            }
            reader.Close();


            List<string> stockFleursMagasin = new List<string>();
            List<string> stockAccessoireMagasin = new List<string>();
            List<int> quantiteStockMagasinFleur = new List<int>();
            List<int> quantiteStockMagasinAccessoire = new List<int>();
            command = connection.CreateCommand();
            command.CommandText = $"SELECT NomFleur,stock FROM stock_fleurs WHERE idMagasin = {magasin};";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                stockFleursMagasin.Add(reader.GetValue(0).ToString());
                quantiteStockMagasinFleur.Add(int.Parse(reader.GetValue(1).ToString()));
            }
            reader.Close();
            command = connection.CreateCommand();
            command.CommandText = $"SELECT NomAccessoire,stock FROM stock_accessoires WHERE idMagasin = {magasin};";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                stockAccessoireMagasin.Add(reader.GetValue(0).ToString());
                quantiteStockMagasinAccessoire.Add(int.Parse(reader.GetValue(1).ToString()));
            }
            reader.Close();

            int erreur = 0;
            cpt = 0;
            int quantiteFleurActuelle, stockFleurActuelle;
            foreach (string i in nomFleurs)
            {
                quantiteFleurActuelle = quantiteFleurs[cpt];
                command = connection.CreateCommand();
                command.CommandText = $"SELECT stock FROM stock_fleurs NATURAL JOIN composition_fleurs_standard WHERE idMagasin = {magasin} AND NomFleur='{i}' AND Nom_Bouquet_Standard='{bouquetForme}';";
                reader = command.ExecuteReader();
                reader.Read();
                stockFleurActuelle = int.Parse(reader.GetValue(0).ToString());
                if (quantiteFleurActuelle > stockFleurActuelle) { erreur = 1; }
                cpt++;
                reader.Close();
            }
            cpt = 0;
            int quantiteAccessoireActuel, stockAccessoireActuel;
            foreach (string i in nomAccessoires)
            {
                quantiteAccessoireActuel = quantiteAccessoires[cpt];
                command = connection.CreateCommand();
                command.CommandText = $"SELECT stock FROM stock_accessoires AS sa JOIN composition_accessoires_standard AS ca ON sa.NomAccessoire = ca.Nom_Accessoire WHERE idMagasin = {magasin} AND NomAccessoire='{i}';";
                reader = command.ExecuteReader();
                reader.Read();
                stockAccessoireActuel = int.Parse(reader.GetValue(0).ToString());
                reader.Close();
                if (quantiteAccessoireActuel > stockAccessoireActuel) { erreur = 1; }
                cpt++;

            }

            if (erreur == 1) { Console.WriteLine("Impossible de créer le bouquet standard car pas assez de stock."); }

            else
            {
                //Mise à jour des tables
                string fleurActuelle, accessoireActuel;
                int quantiteAvant;
                cpt = 0;
                foreach (int i in quantiteFleurs)   //Pour les fleurs
                {
                    fleurActuelle = nomFleurs[cpt];

                    command = connection.CreateCommand();
                    command.CommandText = $"SELECT stock FROM stock_fleurs WHERE idMagasin={magasin} AND NomFleur = '{fleurActuelle}';";
                    reader = command.ExecuteReader();
                    reader.Read();
                    quantiteAvant = int.Parse(reader.GetValue(0).ToString());
                    reader.Close();

                    command.CommandText = $"UPDATE stock_fleurs SET stock = {quantiteAvant - i} WHERE NomFleur = '{fleurActuelle}' and idMagasin = {magasin};";
                    reader = command.ExecuteReader();
                    reader.Close();

                    cpt++;
                }

                cpt = 0;

                foreach (int i in quantiteAccessoires)   //Pour les accessoires
                {
                    accessoireActuel = nomAccessoires[cpt];

                    command = connection.CreateCommand();
                    command.CommandText = $"SELECT stock FROM stock_accessoires WHERE idMagasin={magasin} AND NomAccessoire = '{accessoireActuel}';";
                    reader = command.ExecuteReader();
                    reader.Read();
                    quantiteAvant = int.Parse(reader.GetValue(0).ToString());
                    reader.Close();

                    command.CommandText = $"UPDATE stock_accessoires SET stock = {quantiteAvant - i} WHERE NomAccessoire = '{accessoireActuel}' and idMagasin = {magasin};";
                    reader = command.ExecuteReader();
                    reader.Close();

                    cpt++;
                }

                List<string> listeBouquetsStandardMagasin = new List<string>();
                command = connection.CreateCommand();
                command.CommandText = $"SELECT NomBouquetStandard FROM Contient_Bouquet_Standard WHERE idMagasin={magasin};";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listeBouquetsStandardMagasin.Add(reader.GetValue(0).ToString().ToLower());
                }
                reader.Close();

                if (listeBouquetsStandardMagasin.Contains(bouquetForme.ToLower()) == false)
                {
                    command = connection.CreateCommand();
                    command.CommandText = $"INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('{bouquetForme}',{magasin},1);";
                    reader = command.ExecuteReader();
                    reader.Close();
                }


                else
                {
                    int stock_initial = 0;
                    command = connection.CreateCommand();
                    command.CommandText = $"SELECT Stock FROM Contient_Bouquet_Standard WHERE idMagasin = {magasin} AND NomBouquetStandard='{bouquetForme}';";
                    reader = command.ExecuteReader();
                    reader.Read();
                    stock_initial = int.Parse(reader.GetValue(0).ToString());
                    reader.Close();

                    int newstock = stock_initial + 1;
                    command = connection.CreateCommand();
                    command.CommandText = $"UPDATE Contient_Bouquet_Standard SET Stock={newstock} WHERE idMagasin = {magasin} AND NomBouquetStandard='{bouquetForme}';";
                    reader = command.ExecuteReader();
                    reader.Close();
                }


                Console.WriteLine("Le bouquet à bien été crée. Les stocks du magasin ont été mis à jour.");
            }
        }

        static void afficherFormationBouquetStandard(MySqlConnection connection)
        {
            Console.WriteLine();
            Console.Write("Quel bouquet voulez vous voir ? ");
            string bouquetForme = Console.ReadLine().ToLower();
            List<string> listeBouquetStandard = getPrimaryKeysLower("bouquet_standard", "NomBouquetStandard", connection);
            while (listeBouquetStandard.Contains(bouquetForme) == false)
            {
                Console.Write("Bouquet inexistant, veuillez réessayer : ");
                bouquetForme = Console.ReadLine().ToLower();
            }

            //Informations sur le bouquet
            List<string> nomFleurs = new List<string>();
            List<int> quantiteFleurs = new List<int>();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT NomFleur,quantite FROM composition_fleurs_standard WHERE Nom_Bouquet_Standard = '{bouquetForme}'";
            MySqlDataReader reader = command.ExecuteReader();
            int cpt = 0;
            int quantite;
            while (reader.Read())
            {
                nomFleurs.Add(reader.GetValue(0).ToString().ToLower());
                quantiteFleurs.Add(int.Parse(reader.GetValue(1).ToString()));
                cpt = 0;
                foreach (string i in nomFleurs)
                {
                    Console.WriteLine(i + " : " + quantiteFleurs[cpt]);
                }
            }
            reader.Close();

            List<string> nomAccessoires = new List<string>();
            List<int> quantiteAccessoires = new List<int>();
            command = connection.CreateCommand();
            command.CommandText = $"SELECT Nom_Accessoire,quantite FROM composition_accessoires_standard WHERE NomBouquetStandard = '{bouquetForme}'";
            reader = command.ExecuteReader();
            cpt = 0;
            while (reader.Read())
            {
                nomAccessoires.Add(reader.GetValue(0).ToString().ToLower());
                quantiteAccessoires.Add(int.Parse(reader.GetValue(1).ToString()));
                cpt = 0;
                foreach (string i in nomAccessoires)
                {
                    Console.WriteLine(i + " : " + quantiteAccessoires[cpt]);
                }
            }
            reader.Close();
        }

        static void ajouterCommande(string client, MySqlConnection connection)
        {
            MySqlCommand command;
            MySqlDataReader reader;
            Console.WriteLine();

            Console.Write("Dans quel magasin voulez vous passer la commande (code) : ");
            string magasin = Console.ReadLine();
            List<string> magasins = getPrimaryKeys("magasin", "idMagasin", connection);
            while (magasin.Contains(magasin) == false)
            {
                Console.Write("Magasin introuvable. Veuillez réessayer : ");
                magasin = Console.ReadLine();
            }

            Console.WriteLine();
            Console.Write("Quel type de bouquet voulez vous commander (standard ou personnalise) : ");
            string type = Console.ReadLine();
            while (type != "standard" && type != "personnalise")
            {
                Console.Write("Veuillez réessayer : ");
                type = Console.ReadLine();
            }

            Random rand = new Random();
            int numeroCommande = rand.Next();
            List<int> numeros = new List<int>();
            command = connection.CreateCommand();
            command.CommandText = $"SELECT Numero_de_commande FROM commande;";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                numeros.Add(int.Parse(reader.GetValue(0).ToString()));
            }
            reader.Close();

            while (numeros.Contains(numeroCommande))
            {
                numeroCommande = rand.Next();
            }

            DateTime date = DateTime.Now;
            string dateString = date.ToString("yyyy-MM-dd");



            if (type == "standard")
            {
                Console.Write("Quel bouquet standard voulez vous commander ? (taper liste pour voir la liste, taper composition pour voir la composition) : ");
                string bouquetstandard = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine();

                List<string> bouquetsStandards = getPrimaryKeysLower("bouquet_standard", "NomBouquetStandard", connection);
                while (bouquetsStandards.Contains(bouquetstandard) == false)
                {
                    if (bouquetstandard == "liste")
                    {
                        foreach (string i in bouquetsStandards) { Console.Write(i + ", "); }
                        Console.WriteLine();
                        Console.Write("Quel bouquet standard voulez vous commander ? (taper liste pour voir la liste, taper composition pour voir la composition) : ");
                        bouquetstandard = Console.ReadLine();
                        Console.WriteLine();
                    }
                    else if (bouquetstandard == "composition")
                    {
                        afficherFormationBouquetStandard(connection);
                        Console.WriteLine();
                        Console.Write("Quel bouquet standard voulez vous commander ? (taper liste pour voir la liste, taper composition pour voir la composition) : ");
                        bouquetstandard = Console.ReadLine();
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Write("Veuillez réessayer : ");
                        bouquetstandard = Console.ReadLine();
                        Console.WriteLine();
                    }
                }

                command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO commande (Numero_de_commande,Date_de_commande,Courriel,id_Bouquet_Personnalise,NomBouquetStandard,idMagasin,etat) VALUES ({numeroCommande},'{dateString}','{client}',NULL,'{bouquetstandard}',{magasin},'VINV');";
                reader = command.ExecuteReader();
                reader.Close();
            }

            else if (type == "personnalise")
            {
                Random random = new Random();
                int CodeBP = random.Next();
                string codeBPString = CodeBP.ToString();

                List<string> codes = getPrimaryKeysLower("bouquet_personnalise", "id_Bouquet_Personnalise", connection);
                while (codes.Contains(codeBPString) == true)
                {
                    CodeBP = random.Next();
                    codeBPString = CodeBP.ToString();
                }

                CodeBP = int.Parse(codeBPString);

                formerBouquetPersonnalise(client, CodeBP, magasin, connection);
                Console.WriteLine();

                command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO commande (Numero_de_commande,Date_de_commande,Courriel,id_Bouquet_Personnalise,NomBouquetStandard,idMagasin,etat) VALUES ({numeroCommande},'{dateString}','{client}',{CodeBP},NULL,{magasin},'CC');";
                reader = command.ExecuteReader();
                reader.Close();
            }

            Console.WriteLine("Commande bien créée ! ");
        }

        static void nouveauBouquetStandard(MySqlConnection connection)
        {
            MySqlCommand command;
            MySqlDataReader reader;
            Console.WriteLine();
            Console.Write("Veuillez entrer un nom pour ce nouveau bouquet standard : ");
            string nom = Console.ReadLine();
            List<string> nomBouquetsStandards = getPrimaryKeysLower("Bouquet_Standard", "NomBouquetStandard", connection);
            while (nomBouquetsStandards.Contains(nom.ToLower()))
            {
                Console.WriteLine("Bouquet déjà dans la base de données. Veuillez réessayer : ");
                nom = Console.ReadLine();
            }

            Console.WriteLine();
            Console.Write("Veuillez entrer une catégorie pour le bouquet : ");
            string categorie = Console.ReadLine();

            command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO bouquet_standard (NomBouquetStandard,Prix,Categorie) VALUES ('{nom}',0,'{categorie}');";
            reader = command.ExecuteReader();
            reader.Close();
            Console.WriteLine();

            List<string> nomFleurs = getPrimaryKeysLower("fleur", "NomFleur", connection);
            List<string> nomAccessoires = getPrimaryKeysLower("accessoire", "NomAccessoire", connection);

            string fleur = "";
            int quantité = 0;

            List<string> listeFleursDejaDedans = new List<string>();
            int stock = 0;
            float prix = 0;
            while (fleur != "next")
            {
                Console.Write("Veuillez entrer un nom de fleur : ");
                fleur = Console.ReadLine();
                while (nomFleurs.Contains(fleur.ToLower()) == false && fleur != "next")
                {
                    Console.Write("Fleur inexistante dans la base veuillez réessayer : ");
                    fleur = Console.ReadLine();
                }

                if (fleur == "next") { }
                else
                {
                    Console.WriteLine($"Combien de {fleur.ToLower()}s voulez vous ajouter à ce nouveau bouquet ?");
                    quantité = int.Parse(Console.ReadLine());

                    command = connection.CreateCommand();
                    command.CommandText = $"SELECT Prix FROM fleur WHERE NomFleur='{fleur}';";
                    reader = command.ExecuteReader();
                    reader.Read();
                    prix += quantité * float.Parse(reader.GetValue(0).ToString());
                    reader.Close();

                    command = connection.CreateCommand();
                    command.CommandText = $"SELECT NomFleur FROM composition_fleurs_standard WHERE Nom_Bouquet_Standard='{nom}';";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string valueAsString = reader.GetValue(i).ToString().ToLower();
                            listeFleursDejaDedans.Add(valueAsString);
                        }
                    }
                    reader.Close();

                    if (listeFleursDejaDedans.Contains(fleur.ToLower()) == false)
                    {
                        command = connection.CreateCommand();
                        command.CommandText = $"INSERT INTO composition_fleurs_standard (NomFleur,Nom_Bouquet_Standard,Quantite) VALUES ('{fleur}','{nom}',{quantité});";
                        reader = command.ExecuteReader();
                        reader.Close();
                    }

                    else
                    {
                        command = connection.CreateCommand();
                        command.CommandText = $"SELECT Quantite FROM composition_fleurs_standard WHERE Nom_Bouquet_Standard={nom} AND NomFleur='{fleur};";
                        reader = command.ExecuteReader();
                        reader.Read();
                        stock = int.Parse(reader.GetValue(0).ToString());
                        stock += quantité;
                        reader.Close();
                        command = connection.CreateCommand();
                        command.CommandText = $"UPDATE composition_fleurs_standard SET Quantite={stock} WHERE Nom_Bouquet_Standard={nom} AND NomFleur='{fleur};";
                        reader = command.ExecuteReader();
                        reader.Close();
                    }
                }

            }

            string accessoire = "";
            quantité = 0;

            List<string> listeAccessoiresDejaDedans = new List<string>();
            stock = 0;
            while (accessoire != "next")
            {
                Console.Write("Veuillez entrer un nom d'accessoire : ");
                accessoire = Console.ReadLine();
                while (nomAccessoires.Contains(accessoire.ToLower()) == false && accessoire != "next")
                {
                    Console.Write("Accessoire inexistant dans la base veuillez réessayer : ");
                    accessoire = Console.ReadLine();
                }

                if (accessoire == "next") { }

                else
                {
                    Console.WriteLine($"Combien de {accessoire.ToLower()}s voulez vous ajouter à ce nouveau bouquet ?");
                    quantité = int.Parse(Console.ReadLine());

                    command = connection.CreateCommand();
                    command.CommandText = $"SELECT Prix FROM accessoire WHERE NomAccessoire='{accessoire}';";
                    reader = command.ExecuteReader();
                    reader.Read();
                    prix += quantité * float.Parse(reader.GetValue(0).ToString());
                    reader.Close();

                    command = connection.CreateCommand();
                    command.CommandText = $"SELECT Nom_Accessoire FROM composition_accessoires_standard WHERE NomBouquetStandard='{nom}';";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string valueAsString = reader.GetValue(i).ToString().ToLower();
                            listeAccessoiresDejaDedans.Add(valueAsString);
                        }
                    }
                    reader.Close();

                    if (listeFleursDejaDedans.Contains(fleur.ToLower()) == false)
                    {
                        command = connection.CreateCommand();
                        command.CommandText = $"INSERT INTO composition_accessoires_standard (NomBouquetStandard,Nom_Accessoire,Quantite) VALUES ('{nom}','{accessoire}',{quantité});";
                        reader = command.ExecuteReader();
                        reader.Close();
                    }

                    else
                    {
                        command = connection.CreateCommand();
                        command.CommandText = $"SELECT Quantite FROM composition_accessoires_standard WHERE NomBouquetStandard={nom} AND NomFleur='{fleur};";
                        reader = command.ExecuteReader();
                        reader.Read();
                        stock = int.Parse(reader.GetValue(0).ToString());
                        stock += quantité;
                        reader.Close();
                        command = connection.CreateCommand();
                        command.CommandText = $"UPDATE composition_accessoires_standard SET Quantite={stock} WHERE NomBouquetStandard={nom} AND NomFleur='{fleur};";
                        reader = command.ExecuteReader();
                        reader.Close();
                    }
                }


            }

            string pri = prix.ToString().Replace(",", ".");

            command = connection.CreateCommand();
            command.CommandText = $"UPDATE bouquet_standard SET Prix={pri} WHERE NomBouquetStandard='{nom}';";
            reader = command.ExecuteReader();
            reader.Close();

            //Résumé du bouquet

            Console.WriteLine();
            Console.WriteLine("Voici un résumé du bouquet : ");
            Console.WriteLine();
            command = connection.CreateCommand();
            command.CommandText = $"SELECT NomFleur,Quantite FROM composition_fleurs_standard WHERE Nom_Bouquet_Standard='{nom}';";
            reader = command.ExecuteReader();
            while (reader.Read()) { Console.WriteLine(reader.GetValue(0).ToString() + " : " + reader.GetValue(1).ToString()); }
            reader.Close();
            command = connection.CreateCommand();
            command.CommandText = $"SELECT Nom_Accessoire,Quantite FROM composition_accessoires_standard WHERE NomBouquetStandard='{nom}';";
            reader = command.ExecuteReader();
            while (reader.Read()) { Console.WriteLine(reader.GetValue(0).ToString() + " : " + reader.GetValue(1).ToString()); }
            reader.Close();
            Console.WriteLine("Le prix total est : " + prix + " euros.");
        }

        static void afficherCommandes(MySqlConnection connection)
        {
            Console.WriteLine();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM commande WHERE etat!='CL'";
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < 7; i++)
                {
                    Console.Write(reader.GetValue(i).ToString() + ", ");
                }
                Console.WriteLine();
            }
            reader.Close();
            Console.WriteLine();
        }

        static void afficherCommandesClient(string client, MySqlConnection connection)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM commande WHERE courriel='{client}';";
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < 7; i++)
                {
                    Console.Write(reader.GetValue(i).ToString() + ", ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            reader.Close();
        }

        static void preparerCommande(MySqlConnection connection)
        {
            MySqlCommand command;
            MySqlDataReader reader;

            Console.WriteLine();
            Console.Write("Quelle commande va être préparée : ");
            string commande = Console.ReadLine();
            List<string> numerosCommande = getPrimaryKeysLower("commande", "Numero_de_commande", connection);

            while (commande == "liste")
            {
                command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM commande WHERE etat!='CL' and etat!='CAL';";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetValue(i).ToString() + ", ");
                    }
                    Console.WriteLine();
                }
                commande = Console.ReadLine();
                reader.Close();
            }

            while (numerosCommande.Contains(commande) == false && commande != "exit")
            {
                Console.Write("Commande introuvable, veuillez réessayer : ");
                commande = Console.ReadLine();
            }

            if (commande != "exit")
            {
                command = connection.CreateCommand();
                command.CommandText = $"SELECT id_Bouquet_Personnalise,NomBouquetStandard FROM commande WHERE Numero_de_commande={commande};";
                reader = command.ExecuteReader();
                reader.Read();
                string bouquetP = reader.GetValue(0).ToString();
                string bouquetS = reader.GetValue(1).ToString();
                reader.Close();


                if (bouquetS != "")
                {

                    Console.WriteLine($"Allez chercher un {bouquetS} dans la réserve et envoyez !");
                    command = connection.CreateCommand();
                    command.CommandText = $"UPDATE commande SET etat='CAL' WHERE Numero_de_commande={commande};";
                    reader = command.ExecuteReader();
                    reader.Close();
                    Console.WriteLine($"La commande {commande} est prête à être livrée !");
                }

                else
                {
                    command = connection.CreateCommand();
                    command.CommandText = $"SELECT NomFleur,Quantite FROM Composition_Fleurs_Personnalise WHERE id_Bouquet_Personnalise='{bouquetP}';";
                    reader = command.ExecuteReader();
                    List<string> listeitems = new List<string>();
                    List<int> quantiteItems = new List<int>();
                    while (reader.Read())
                    {
                        listeitems.Add(reader.GetValue(0).ToString());
                        quantiteItems.Add(int.Parse(reader.GetValue(1).ToString()));
                    }
                    reader.Close();

                    command = connection.CreateCommand();
                    command.CommandText = $"SELECT NomAccessoire,Quantite FROM composition_accessoires_personnalises WHERE id_Bouquet_Personnalise='{bouquetP}';";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        listeitems.Add(reader.GetValue(0).ToString());
                        quantiteItems.Add(int.Parse(reader.GetValue(1).ToString()));
                    }
                    reader.Close();

                    int cpt = 0;
                    string action;
                    foreach (string i in listeitems)
                    {
                        int quantiteactuelle = quantiteItems[cpt];
                        if (quantiteactuelle > 0)
                        {
                            Console.Write($"Aller chercher {quantiteactuelle} {i.ToLower()}s : ");
                            action = Console.ReadLine().ToLower();
                            while (action != "ok")
                            {
                                Console.Write("Entrer OK quand c'est fait : ");
                                action = Console.ReadLine();
                            }
                        }
                        cpt++;
                    }

                    command = connection.CreateCommand();
                    command.CommandText = $"UPDATE commande SET etat='CAL' WHERE Numero_de_commande={commande};";
                    reader = command.ExecuteReader();
                    reader.Close();
                    Console.WriteLine($"La commande {commande} est maintenant prête à être livrée !");
                }
            }
        }

        static void statistiques(MySqlConnection connection)
        {
            MySqlCommand command;
            MySqlDataReader reader;

            Console.WriteLine();
            Console.Write("Quelle statistique voulez vous voir ? (liste pour tout voir) : ");
            string stat = Console.ReadLine();

            List<string> statistiquesValides = new List<string>();
            statistiquesValides.AddRange(new string[] { "client du mois", "client de l'annee", "bouquet du mois", "bouquet de l'annee" });

            while (statistiquesValides.Contains(stat) == false && stat != "liste")
            {
                Console.Write("Veuillez réessayer (liste pour tout voir) : ");
                stat = Console.ReadLine();
            }

            if (stat == "liste")
            {
                foreach (string i in statistiquesValides)
                {
                    Console.Write(i + ", ");
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Veuillez entrer une statistique parmi la liste proposée : ");
                stat = Console.ReadLine();
                while (statistiquesValides.Contains(stat) == false)
                {
                    Console.Write("Veuillez réessayer : ");
                    stat = Console.ReadLine();
                }
                Console.WriteLine();
            }

            DateTime date = DateTime.Now;
            string year = date.ToString("yyyy");
            string yearmonth = date.ToString("yyyy-MM");

            if (stat == "client de l'annee")
            {
                string courrielGagnant = "";
                int nbMaxCommande = 0;
                command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT(courriel), courriel FROM commande WHERE Date_de_commande LIKE '%{year}%' GROUP BY courriel ORDER BY COUNT(courriel) DESC LIMIT 1;";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nbMaxCommande = int.Parse(reader.GetValue(0).ToString());
                    courrielGagnant = reader.GetValue(1).ToString();
                }
                reader.Close();

                string nomGagnant = "";
                string prenomGagnant = "";
                command = connection.CreateCommand();
                command.CommandText = $"SELECT Nom,Prenom FROM client WHERE courriel = '{courrielGagnant}';";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nomGagnant = reader.GetValue(0).ToString();
                    prenomGagnant = reader.GetValue(1).ToString();
                }

                Console.WriteLine($"Bravo à {prenomGagnant} {nomGagnant} qui est actuellement le meilleur client de l'année {year} avec {nbMaxCommande} commandes passées !");
                Console.WriteLine();
            }

            if (stat == "client du mois")
            {
                string courrielGagnant = "";
                int nbMaxCommande = 0;
                command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT(courriel), courriel FROM commande WHERE Date_de_commande LIKE '%{yearmonth}%' GROUP BY courriel ORDER BY COUNT(courriel) DESC LIMIT 1;";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nbMaxCommande = int.Parse(reader.GetValue(0).ToString());
                    courrielGagnant = reader.GetValue(1).ToString();
                }
                reader.Close();

                string nomGagnant = "";
                string prenomGagnant = "";
                command = connection.CreateCommand();
                command.CommandText = $"SELECT Nom,Prenom FROM client WHERE courriel = '{courrielGagnant}';";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nomGagnant = reader.GetValue(0).ToString();
                    prenomGagnant = reader.GetValue(1).ToString();
                }
                reader.Close();

                Console.WriteLine($"Bravo à {prenomGagnant} {nomGagnant} qui est actuellement le meilleur client du mois {yearmonth} avec {nbMaxCommande} commandes passées !");
                Console.WriteLine();
            }

            if (stat == "bouquet de l'annee")
            {
                string nomBouquetGagnant = "";
                int nbMaxCommande = 0;
                command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT(NomBouquetStandard), NomBouquetStandard FROM commande WHERE Date_de_commande LIKE '%{year}%' GROUP BY NomBouquetStandard ORDER BY COUNT(NomBouquetStandard) DESC LIMIT 1;";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nbMaxCommande = int.Parse(reader.GetValue(0).ToString());
                    nomBouquetGagnant = reader.GetValue(1).ToString();
                }
                reader.Close();

                Console.WriteLine($"Le bouquet le plus commandé de l'année {year} est : {nomBouquetGagnant}");
                Console.WriteLine();
            }

            if (stat == "bouquet du mois")
            {
                string nomBouquetGagnant = "";
                int nbMaxCommande = 0;
                command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT(NomBouquetStandard), NomBouquetStandard FROM commande WHERE Date_de_commande LIKE '%{yearmonth}%' GROUP BY NomBouquetStandard ORDER BY COUNT(NomBouquetStandard) DESC LIMIT 1;";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nbMaxCommande = int.Parse(reader.GetValue(0).ToString());
                    nomBouquetGagnant = reader.GetValue(1).ToString();
                }
                reader.Close();

                Console.WriteLine($"Le bouquet le plus commandé du mois {yearmonth} est : {nomBouquetGagnant}");
                Console.WriteLine();
            }



        }

        static void changerMDP(MySqlConnection connection, string client)
        {
            Console.WriteLine();
            Console.Write("Email envoyé à votre adresse mail. Cliquez sur le lien et entrez le numéro donné : ");
            int numéro = int.Parse(Console.ReadLine());
            Console.Write("Veuillez saisir votre nouveau mot de passe : ");
            string mdp1 = Console.ReadLine();
            Console.Write("Veuillez confirmer ce mot de passe : ");
            string mdp2 = Console.ReadLine();
            while(mdp1!=mdp2 && mdp1!="exit")
            {
                Console.Write("Les deux mots de passe ne sont pas identiques. Veuillez réessayer :");
                mdp1= Console.ReadLine();
                Console.Write("Veuillez confirmer ce mot de passe : ");
                mdp2 = Console.ReadLine();
            }

            if(mdp1!="exit")
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"UPDATE client SET motdepasse='{mdp1}' WHERE courriel='{client}';";
                MySqlDataReader reader = command.ExecuteReader();
                reader.Close();
                Console.WriteLine("Le mot de passe à bien été changé ! ");
            }
        }

        static void supprimerItem(MySqlConnection connection)
        {
            Console.Write("Veuillez entrer un item à supprimer : ");
            string item = Console.ReadLine().ToLower(); ;
            List<string> items = new List<string>();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM fleur;";
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(reader.GetValue(0).ToString().ToLower());
            }
            reader.Close();

            if (items.Contains(item))
            {
                command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM fleur WHERE NomFleur = '{item}';";
                reader = command.ExecuteReader();
                reader.Close();
                Console.WriteLine($"Table fleur bien mise à jour : élément {item} supprimé !");
            }

            else
            {
                command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM accessoire;";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(reader.GetValue(0).ToString().ToLower());
                }
                reader.Close();

                if (items.Contains(item))
                {
                    command = connection.CreateCommand();
                    command.CommandText = $"DELETE FROM accessoire WHERE NomAccessoire = '{item}';";
                    reader = command.ExecuteReader();
                    reader.Close();
                    Console.WriteLine($"Table accessoire bien mise à jour : élément {item} supprimé !");
                }

                else
                {
                    Console.WriteLine("Item non trouvé ... pas de modification apportée");
                }
            }
        }


        // Exports


        //EN XML, clients ayant commandés plusieurs fois au cours du mois

        static void exportXMLClientsFideles(MySqlConnection connection)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT courriel, Nom,Prenom,Numero_de_telephone,Adresse,Statut_Fidelite FROM client NATURAL JOIN commande WHERE Date_de_commande >= '2023-04-11' GROUP BY client.courriel HAVING COUNT(*) > 1;";


            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter xmlWriter = XmlWriter.Create(@"C:\Users\willr\OneDrive - De Vinci\Année 3\Semestre 6\BDD & Interopérabilité\Projet\export.xml", settings);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Table");

                while (reader.Read())
                {
                    xmlWriter.WriteStartElement("Row");

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        xmlWriter.WriteStartElement(reader.GetName(i));
                        xmlWriter.WriteString(reader[i].ToString());
                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
            }

            xmlWriter.Close();
            command.Dispose();
            connection.Close();
        }

    }
      
}
