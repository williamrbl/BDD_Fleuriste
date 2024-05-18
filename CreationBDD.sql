DROP DATABASE IF EXISTS Fleurs;
CREATE DATABASE Fleurs;
use Fleurs;


#------------------------------------------------------------
# Table: Client
#------------------------------------------------------------

DROP TABLE IF EXISTS Client;
CREATE TABLE Client(
        Courriel            Varchar (50) NOT NULL ,
        motdepasse          Varchar(50) NOT NULL,
        Nom                 Varchar (50) NOT NULL ,
        Prenom              Varchar (50) NOT NULL ,
        Numero_de_telephone Int NOT NULL ,
        Adresse             Varchar (50) NOT NULL ,
        Statut_Fidelite     Varchar (50) NOT NULL
	,CONSTRAINT Client_PK PRIMARY KEY (Courriel)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Bouquet Personnalisé
#------------------------------------------------------------
DROP TABLE IF EXISTS Bouquet_Personnalise;
CREATE TABLE Bouquet_Personnalise(
        id_Bouquet_Personnalise Int ,
        Prix                    Float
	,CONSTRAINT Bouquet_Personnalise_PK PRIMARY KEY (id_Bouquet_Personnalise)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Bouquet Standard
#------------------------------------------------------------
DROP TABLE IF EXISTS Bouquet_Standard;
CREATE TABLE Bouquet_Standard(
        NomBouquetStandard       Varchar (50) ,
        Prix      Float ,
        Categorie Varchar (50) 
	,CONSTRAINT Bouquet_Standard_PK PRIMARY KEY (NomBouquetStandard)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Fleur
#------------------------------------------------------------
DROP TABLE IF EXISTS Fleur;
CREATE TABLE Fleur(
        NomFleur           Varchar (50) NOT NULL ,
        Prix          Float NOT NULL ,
        Disponibilite Varchar (50) NOT NULL
	,CONSTRAINT Fleur_PK PRIMARY KEY (NomFleur)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Accessoire
#------------------------------------------------------------
DROP TABLE IF EXISTS Accessoire;
CREATE TABLE Accessoire(
        NomAccessoire Varchar (50) NOT NULL ,
        Prix FLOAT NOT NULL
	,CONSTRAINT Accessoire_PK PRIMARY KEY (NomAccessoire)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Magasin
#------------------------------------------------------------
DROP TABLE IF EXISTS Magasin;
CREATE TABLE Magasin(
        idMagasin Int NOT NULL ,
        NomMagasin Varchar(50) NOT NULL ,
        Localisation Varchar(50) NOT NULL
	,CONSTRAINT Magasin_PK PRIMARY KEY (idMagasin)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Commande
#------------------------------------------------------------
DROP TABLE IF EXISTS Commande;
CREATE TABLE Commande(
        Numero_de_commande      Int NOT NULL ,
        Date_de_commande        Varchar(50) NOT NULL ,
        Courriel                Varchar (50) NOT NULL ,
        id_Bouquet_Personnalise Int ,
        NomBouquetStandard      Varchar (50),
        idMagasin               Int NOT NULL,
        etat					Varchar(50)
	,CONSTRAINT Commande_PK PRIMARY KEY (Numero_de_commande)
	,CONSTRAINT Commande_Client_FK FOREIGN KEY (Courriel) REFERENCES Client(Courriel)
	,CONSTRAINT Commande_Bouquet_Personnalise0_FK FOREIGN KEY (id_Bouquet_Personnalise) REFERENCES Bouquet_Personnalise(id_Bouquet_Personnalise)
	,CONSTRAINT Commande_Bouquet_Standard1_FK FOREIGN KEY (NomBouquetStandard) REFERENCES Bouquet_Standard(NomBouquetStandard)
	,CONSTRAINT Commande_Magasin2_FK FOREIGN KEY (idMagasin) REFERENCES Magasin(idMagasin)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Composition Fleurs Standard
#------------------------------------------------------------
DROP TABLE IF EXISTS Composition_Fleurs_Standard;
CREATE TABLE Composition_Fleurs_Standard(
        NomFleur                  Varchar (50) NOT NULL ,
        Nom_Bouquet_Standard Varchar (50) NOT NULL,
        Quantite INT
	,CONSTRAINT Composition_Fleurs_Standard_PK PRIMARY KEY (NomFleur,Nom_Bouquet_Standard)
	,CONSTRAINT Composition_Fleurs_Standard_Fleur_FK FOREIGN KEY (NomFleur) REFERENCES Fleur(NomFleur)
	,CONSTRAINT Composition_Fleurs_Standard_Bouquet_Standard0_FK FOREIGN KEY (Nom_Bouquet_Standard) REFERENCES Bouquet_Standard(NomBouquetStandard)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Composition Fleurs Personnalisé
#------------------------------------------------------------
DROP TABLE IF EXISTS Composition_Fleurs_Personnalise;
CREATE TABLE Composition_Fleurs_Personnalise(
        id_Bouquet_Personnalise Int NOT NULL ,
        NomFleur                     Varchar (50) NOT NULL,
        Quantite INT
	,CONSTRAINT Composition_Fleurs_Personnalise_PK PRIMARY KEY (id_Bouquet_Personnalise,NomFleur)

	,CONSTRAINT Composition_Fleurs_Personnalise_Bouquet_Personnalise_FK FOREIGN KEY (id_Bouquet_Personnalise) REFERENCES Bouquet_Personnalise(id_Bouquet_Personnalise)
	,CONSTRAINT Composition_Fleurs_Personnalise_Fleur0_FK FOREIGN KEY (NomFleur) REFERENCES Fleur(NomFleur)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Composition Accessoires Personnalisés
#------------------------------------------------------------
DROP TABLE IF EXISTS Composition_Accessoires_Personnalises;
CREATE TABLE Composition_Accessoires_Personnalises(
        id_Bouquet_Personnalise Int NOT NULL ,
        NomAccessoire                     Varchar (50) NOT NULL,
        Quantite Int
	,CONSTRAINT Composition_Accessoires_Personnalises_PK PRIMARY KEY (id_Bouquet_Personnalise,NomAccessoire)
	,CONSTRAINT Composition_Accessoires_Personnalises_Bouquet_Personnalise_FK FOREIGN KEY (id_Bouquet_Personnalise) REFERENCES Bouquet_Personnalise(id_Bouquet_Personnalise)
	,CONSTRAINT Composition_Accessoires_Personnalises_Accessoire0_FK FOREIGN KEY (NomAccessoire) REFERENCES Accessoire(NomAccessoire)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Composition Accessoires Standard
#------------------------------------------------------------
DROP TABLE IF EXISTS Composition_Accessoires_Standard;
CREATE TABLE Composition_Accessoires_Standard(
        NomBouquetStandard            Varchar (50) NOT NULL ,
        Nom_Accessoire Varchar (50) NOT NULL ,
        Quantite Int
	,CONSTRAINT Composition_Accessoires_Standard_PK PRIMARY KEY (NomBouquetStandard,Nom_Accessoire)
	,CONSTRAINT Composition_Accessoires_Standard_Bouquet_Standard_FK FOREIGN KEY (NomBouquetStandard) REFERENCES Bouquet_Standard(NomBouquetStandard)
	,CONSTRAINT Composition_Accessoires_Standard_Accessoire0_FK FOREIGN KEY (Nom_Accessoire) REFERENCES Accessoire(NomAccessoire)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: stock_accessoires
#------------------------------------------------------------
DROP TABLE IF EXISTS stock_accessoires;
CREATE TABLE stock_accessoires(
        NomAccessoire       Varchar (50) NOT NULL ,
        idMagasin Int NOT NULL ,
        Stock     Int NOT NULL
	,CONSTRAINT Contient_Accessoires_PK PRIMARY KEY (NomAccessoire,idMagasin)
	,CONSTRAINT Contient_Accessoires_Accessoire_FK FOREIGN KEY (NomAccessoire) REFERENCES Accessoire(NomAccessoire)
	,CONSTRAINT Contient_Accessoires_Magasin0_FK FOREIGN KEY (idMagasin) REFERENCES Magasin(idMagasin)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: stock_fleurs
#------------------------------------------------------------
DROP TABLE IF EXISTS stock_fleurs;
CREATE TABLE stock_fleurs(
        idMagasin Int NOT NULL ,
        NomFleur       Varchar (50) NOT NULL ,
        Stock     Int NOT NULL
	,CONSTRAINT Contient_Fleurs_PK PRIMARY KEY (idMagasin,NomFleur)
	,CONSTRAINT Contient_Fleurs_Magasin_FK FOREIGN KEY (idMagasin) REFERENCES Magasin(idMagasin)
	,CONSTRAINT Contient_Fleurs_Fleur0_FK FOREIGN KEY (NomFleur) REFERENCES Fleur(NomFleur)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Contient Bouquet Standard
#------------------------------------------------------------
DROP TABLE IF EXISTS Contient_Bouquet_Standard;
CREATE TABLE Contient_Bouquet_Standard(
        NomBouquetStandard       Varchar (50) NOT NULL ,
        idMagasin Int NOT NULL ,
        Stock     Int NOT NULL
	,CONSTRAINT Contient_Bouquet_Standard_PK PRIMARY KEY (NomBouquetStandard,idMagasin)
    ,CONSTRAINT Contient_BS_Magasin_FK FOREIGN KEY (idMagasin) REFERENCES Magasin(idMagasin)
    ,CONSTRAINT NomBouquetS FOREIGN KEY (NomBouquetStandard) REFERENCES Bouquet_Standard(NomBouquetStandard)
)ENGINE=InnoDB;