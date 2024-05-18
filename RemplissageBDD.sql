use Fleurs;

-- Table Accessoires
INSERT INTO accessoire (NomAccessoire,Prix) VALUES ('Vase',15.00);
INSERT INTO accessoire (NomAccessoire,Prix) VALUES ('Ruban',0.50);
INSERT INTO accessoire (NomAccessoire,Prix) VALUES ('Boite',3.00);
INSERT INTO accessoire (NomAccessoire,Prix) VALUES ('Panier',10.00);


-- Table bouquet_personnalise
-- INSERT INTO bouquet_personnalise (id_Bouquet_Personnalise,Prix) VALUES (115465,90);
-- INSERT INTO bouquet_personnalise (id_Bouquet_Personnalise,Prix) VALUES (586622,65);
-- INSERT INTO bouquet_personnalise (id_Bouquet_Personnalise,Prix) VALUES (785642,78);
-- INSERT INTO bouquet_personnalise (id_Bouquet_Personnalise,Prix) VALUES (659245,120);

-- Table bouquet_standard
INSERT INTO bouquet_standard (NomBouquetStandard,Prix,Categorie) VALUES ('Gros Merci',0,'Toute Occasion');
INSERT INTO bouquet_standard (NomBouquetStandard,Prix,Categorie) VALUES ('Printanier',0,'Printemps');
INSERT INTO bouquet_standard (NomBouquetStandard,Prix,Categorie) VALUES ('Champetre',0,'Toute Occasion');
INSERT INTO bouquet_standard (NomBouquetStandard,Prix,Categorie) VALUES ('Maman',0,'Fete des Meres');

-- Table magasin
INSERT INTO magasin (idMagasin,NomMagasin,Localisation) VALUES (4693,'Paris Fleurs','Paris');
INSERT INTO magasin (idMagasin,NomMagasin,Localisation) VALUES (8520,'Marseille Flowers','Marseille');
INSERT INTO magasin (idMagasin,NomMagasin,Localisation) VALUES (7410,'Di Jonquilles','Dijon');
INSERT INTO magasin (idMagasin,NomMagasin,Localisation) VALUES (9630,'Amiens et le tien','Amiens');

-- Table Client
INSERT INTO client (Courriel,motdepasse,Nom,Prenom,Numero_de_telephone,Adresse,Statut_Fidelite) VALUES ('william.rabolin@edu.devinci.fr','mdpRabolin','Rabolin','William',0654859874,'Courbevoie','OR');
INSERT INTO client (Courriel,motdepasse,Nom,Prenom,Numero_de_telephone,Adresse,Statut_Fidelite) VALUES ('mohamed.rakai@edu.devinci.fr','mdpRakai','Rakai','Mohamed',0668854575,'Paris','BRONZE');

-- Table Commande
INSERT INTO commande (Numero_de_commande,Date_de_commande,Courriel,id_Bouquet_Personnalise,NomBouquetStandard,idMagasin) VALUES (50245,'2022-04-05','william.rabolin@edu.devinci.fr',NULL,'Champetre',4693);

-- Table Fleur
INSERT INTO fleur (NomFleur,Prix,Disponibilite) VALUES ('Jonquille',1.50,'Janvier');
INSERT INTO fleur (NomFleur,Prix,Disponibilite) VALUES ('Rose',5.00,'Toute lannee');
INSERT INTO fleur (NomFleur,Prix,Disponibilite) VALUES ('Pivoine',7.00,'Eté');
INSERT INTO fleur (NomFleur,Prix,Disponibilite) VALUES ('Marguerite',1.00,'Printemps');
INSERT INTO fleur (NomFleur,Prix,Disponibilite) VALUES ('Gerbera',5.00,'A lannee');
INSERT INTO fleur (NomFleur,Prix,Disponibilite) VALUES ('Glaieul',1.00,'Mai à novembre');




-- Table composition_accessoires_personnalises
-- INSERT INTO composition_accessoires_personnalises (id_Bouquet_Personnalise,NomAccessoire,Quantite) VALUES (115465,'Ruban');
-- INSERT INTO composition_accessoires_personnalises (id_Bouquet_Personnalise,NomAccessoire,Quantite) VALUES (586622,'Vase');
-- INSERT INTO composition_accessoires_personnalises (id_Bouquet_Personnalise,NomAccessoire,Quantite) VALUES (785642,'Boite');
-- INSERT INTO composition_accessoires_personnalises (id_Bouquet_Personnalise,NomAccessoire,Quantite) VALUES (659245,'Vase');

-- Table composition_accessoires_standard
INSERT INTO composition_accessoires_standard (NomBouquetStandard,Nom_Accessoire,Quantite) VALUES ('Gros Merci','Ruban',1);
INSERT INTO composition_accessoires_standard (NomBouquetStandard,Nom_Accessoire,Quantite) VALUES ('Printanier','Boite',1);
INSERT INTO composition_accessoires_standard (NomBouquetStandard,Nom_Accessoire,Quantite) VALUES ('Champetre','Vase',1);
INSERT INTO composition_accessoires_standard (NomBouquetStandard,Nom_Accessoire,Quantite) VALUES ('Maman','Panier',1);

-- Table composition_fleurs_personnalise
-- INSERT INTO Composition_Fleurs_Personnalise (id_Bouquet_Personnalise,Nom,Quantite) VALUES (115465,'Gerbera');
-- INSERT INTO Composition_Fleurs_Personnalise (id_Bouquet_Personnalise,Nom,Quantite) VALUES (586622,'Pivoine');
-- INSERT INTO Composition_Fleurs_Personnalise (id_Bouquet_Personnalise,Nom,Quantite) VALUES (785642,'Marguerite');
-- INSERT INTO Composition_Fleurs_Personnalise (id_Bouquet_Personnalise,Nom,Quantite) VALUES (659245,'Rose');

-- Table composition_fleurs_standard
INSERT INTO composition_fleurs_standard (NomFleur,Nom_Bouquet_Standard,Quantite) VALUES ('Rose','Gros Merci',10);
INSERT INTO composition_fleurs_standard (NomFleur,Nom_Bouquet_Standard,Quantite) VALUES ('Pivoine','Maman',3);
INSERT INTO composition_fleurs_standard (NomFleur,Nom_Bouquet_Standard,Quantite) VALUES ('Marguerite','Champetre',20);
INSERT INTO composition_fleurs_standard (NomFleur,Nom_Bouquet_Standard,Quantite) VALUES ('Jonquille','Printanier',20);

-- contient_bouquet_standard
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Gros Merci',4693,5);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Gros Merci',8520,6);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Gros Merci',7410,10);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Gros Merci',9630,2);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Maman',4693,7);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Maman',8520,12);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Maman',7410,5);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Maman',9630,4);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Champetre',4693,8);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Champetre',8520,10);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Champetre',7410,7);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Champetre',9630,6);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Printanier',4693,7);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Printanier',8520,18);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Printanier',7410,6);
INSERT INTO Contient_Bouquet_Standard (NomBouquetStandard,idMagasin,Stock) VALUES ('Printanier',9630,4);

-- Table stock_accessoires
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Ruban',4693,50);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Ruban',8520,50);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Ruban',7410,50);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Ruban',9630,50);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Vase',4693,10);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Vase',8520,10);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Vase',7410,10);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Vase',9630,10);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Panier',4693,10);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Panier',8520,10);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Panier',7410,10);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Panier',9630,10);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Boite',4693,20);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Boite',8520,20);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Boite',7410,20);
INSERT INTO stock_accessoires (NomAccessoire,idMagasin,Stock) VALUES ('Boite',9630,20);

-- Table stock_fleurs
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (4693,'Jonquille',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (8520,'Jonquille',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (7410,'Jonquille',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (9630,'Jonquille',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (4693,'Rose',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (8520,'Rose',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (7410,'Rose',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (9630,'Rose',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (4693,'Pivoine',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (8520,'Pivoine',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (7410,'Pivoine',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (9630,'Pivoine',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (4693,'Marguerite',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (8520,'Marguerite',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (7410,'Marguerite',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (9630,'Marguerite',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (4693,'Gerbera',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (8520,'Gerbera',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (7410,'Gerbera',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (9630,'Gerbera',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (4693,'Glaieul',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (8520,'Glaieul',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (7410,'Glaieul',50);
INSERT INTO stock_fleurs (idMagasin,NomFleur,Stock) VALUES (9630,'Glaieul',50);