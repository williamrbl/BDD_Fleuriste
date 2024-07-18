# Projet Base de Données Magasin de Fleurs

## Description

Ce projet consiste à créer une base de données pour gérer un magasin de fleurs, permettant le suivi des clients, des commandes, des bouquets standards et personnalisés, des fleurs et des accessoires. La base de données est conçue à l'aide de MySQL Workbench, et une interface utilisateur en C# est développée pour interagir avec celle-ci, adaptée aussi bien aux clients qu'aux administrateurs.

## Organisation de la Base de Données

### Diagramme Entité-Association (ERD)
Un diagramme ERD illustrant la structure de la base de données est fourni.

### Schéma

La base de données comprend les tables suivantes :

- **Client**
  - Attributs : Nom, Prénom, Adresse, Numéro de téléphone, Courriel, Mot de passe

- **Commande**
  - Attributs : Numéro de commande, Date de commande, ID Client (clé étrangère référençant Client)

- **BouquetStandard**
  - Attributs : Nom, Prix, Catégorie

- **BouquetPersonnalise**
  - Attributs : ID, Prix, ID Client (clé étrangère référençant Client)

- **Fleur**
  - Attributs : Nom de la fleur, Prix

- **Accessoire**
  - Attributs : Nom de l'accessoire, Prix

- **Magasin**
  - Attributs : ID Magasin, Localisation

- **StockFleur**
  - Attributs : ID Magasin (clé étrangère référençant Magasin), ID Fleur (clé étrangère référençant Fleur), Stock

- **StockAccessoire**
  - Attributs : ID Magasin (clé étrangère référençant Magasin), ID Accessoire (clé étrangère référençant Accessoire), Stock

## Structure du Code

Le code est organisé comme suit :

- **Fonction principale :** Établit la connexion et gère la connexion sécurisée des utilisateurs.
- **Page Client :** Permet aux clients de passer des commandes, voir les compositions de bouquets standards et consulter l'historique des commandes.
- **Page Administrateur :** Permet aux administrateurs de gérer des opérations telles que l'ajout de nouvelles fleurs/accessoires, l'exécution de requêtes SQL, la gestion des stocks, la visualisation des commandes en cours, et diverses tâches administratives.

## Fonctionnalités Principales

### Pour les Clients

- Passer des commandes
- Voir les compositions de bouquets standards
- Consulter l'historique des commandes

### Pour les Administrateurs

- Ajouter de nouvelles fleurs et accessoires
- Exécuter des requêtes SQL
- Gérer les stocks à travers les magasins
- Visualiser et préparer les commandes
- Accéder aux statistiques de l'entreprise
- Exporter en XML les clients avec plusieurs commandes le mois dernier
- Modifier les mots de passe
- Supprimer les fleurs ou accessoires non assignés

## Instructions d'Utilisation

- Pour se connecter en tant qu'employé de magasin, utilisez "admin" comme courriel.
- Entrez le code du magasin correspondant lorsqu'il est demandé.
- Une fois connecté, saisissez le nom de l'action à effectuer, "exit" pour terminer, ou "actions" pour afficher les actions disponibles.
- Suivez les instructions de la console pour gérer efficacement les magasins de fleurs et les commandes.
