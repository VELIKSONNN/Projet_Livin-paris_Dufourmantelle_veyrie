CREATE DATABASE Baselivin_paris;
USE Baselinvin_paris;
CREATE TABLE plat(
    id_plat INT,
    nationalité VARCHAR(50),
    nom VARCHAR(50),
    photo_du_plat VARCHAR(50),
    type_de_plat VARCHAR(50),
    prix VARCHAR(50),
    PRIMARY KEY(id_plat)
);


CREATE TABLE ingrédients(
    id_ingr INT,
    nom_ingr VARCHAR(50),
    prix_kg DECIMAL(15,2) NOT NULL,
    provenance VARCHAR(50) NOT NULL,
    PRIMARY KEY(id_ingr)
);


CREATE TABLE utilisateur(
    id INT,
    prénom VARCHAR(50) NOT NULL,
    email VARCHAR(50) NOT NULL,
    num_téléphone INT NOT NULL,
    adresse VARCHAR(50) NOT NULL,
    entreprise VARCHAR(50),
    nom VARCHAR(50) NOT NULL,
    mdp VARCHAR(50) NOT NULL,
    PRIMARY KEY(id)
);

-- Table livraison
CREATE TABLE livraison(
    id_livraison INT,
    date_heure_livraison DATETIME,
    PRIMARY KEY(id_livraison)
);

-- Table Cuisinier
CREATE TABLE Cuisinier(
    cuisine INT,
    id INT NOT NULL,
    PRIMARY KEY(cuisine),
    UNIQUE(id),
    FOREIGN KEY(id) REFERENCES utilisateur(id)
);

-- Table client
CREATE TABLE client(
    client INT,
    id INT NOT NULL,
    PRIMARY KEY(client),
    UNIQUE(id),
    FOREIGN KEY(id) REFERENCES utilisateur(id)
);

-- Table Commande
CREATE TABLE Commande(
    commande INT,
    adresse VARCHAR(50),
    date_heure_commande DATETIME,
    cuisine INT NOT NULL,
    PRIMARY KEY(commande),
    FOREIGN KEY(cuisine) REFERENCES Cuisinier(cuisine)
);

-- Table Ligne_de_commande_
-- Le champ PLAT_ et sa référence deviennent id_plat
CREATE TABLE Ligne_de_commande_(
    id_ligne_de_commande INT,
    id_livraison INT NOT NULL,
    client INT NOT NULL,
    id_plat INT NOT NULL,
    commande INT NOT NULL,
    PRIMARY KEY(id_ligne_de_commande),
    FOREIGN KEY(id_livraison) REFERENCES livraison(id_livraison),
    FOREIGN KEY(client) REFERENCES client(client),
    FOREIGN KEY(id_plat) REFERENCES plat(id_plat),
    FOREIGN KEY(commande) REFERENCES Commande(commande)
);

-- Table appelle
CREATE TABLE appelle(
    client INT,
    commande INT,
    PRIMARY KEY(client, commande),
    FOREIGN KEY(client) REFERENCES client(client),
    FOREIGN KEY(commande) REFERENCES Commande(commande)
);

-- Table Contient
-- Le champ PLAT_ et sa référence deviennent id_plat
CREATE TABLE Contient(
    id_plat INT,
    id_ingr INT,
    quantité INT NOT NULL,
    PRIMARY KEY(id_plat, id_ingr),
    FOREIGN KEY(id_plat) REFERENCES plat(id_plat),
    FOREIGN KEY(id_ingr) REFERENCES ingrédients(id_ingr)
);

-- Table note_reçois
CREATE TABLE note_reçois(
    cuisine INT,
    client INT,
    date_avis DATETIME NOT NULL,
    retour_cuisinier VARCHAR(50),
    nombre_étoiles VARCHAR(50),
    retour_client VARCHAR(50),
    PRIMARY KEY(cuisine, client),
    FOREIGN KEY(cuisine) REFERENCES Cuisinier(cuisine),
    FOREIGN KEY(client) REFERENCES client(client)
);


