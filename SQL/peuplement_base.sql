DROP DATABASE IF EXISTS baselivinparis;
CREATE DATABASE baselivinparis;
USE baselivinparis;

DROP TABLE IF EXISTS inclue;
DROP TABLE IF EXISTS Contient;
DROP TABLE IF EXISTS avis;
DROP TABLE IF EXISTS Ligne_de_commande_;
DROP TABLE IF EXISTS ingrédients;
DROP TABLE IF EXISTS Commande;
DROP TABLE IF EXISTS custommer;
DROP TABLE IF EXISTS plat;
DROP TABLE IF EXISTS Cuisinier;
DROP TABLE IF EXISTS Pays;
DROP TABLE IF EXISTS livraison;
DROP TABLE IF EXISTS utilisateur;

CREATE TABLE utilisateur(
   id INT,
   Prenom VARCHAR(50) NOT NULL,
   email VARCHAR(50) NOT NULL,
   tel LONG NOT NULL,
   adresse VARCHAR(50) NOT NULL,
   entreprise VARCHAR(50),
   Nom VARCHAR(50) NOT NULL,
   mdp VARCHAR(50) NOT NULL,
   PRIMARY KEY(id)
);

CREATE TABLE livraison(
   id_livraison INT,
   date_heure_livraison DATETIME,
   PRIMARY KEY(id_livraison)
);

CREATE TABLE Pays(
   idpays INT,
   nom VARCHAR(50),
   PRIMARY KEY(idpays)
);

CREATE TABLE Cuisinier(
   id_cuisinier INT,
   id INT NOT NULL,
   PRIMARY KEY(id_cuisinier),
   UNIQUE(id),
   FOREIGN KEY(id) REFERENCES utilisateur(id)
);

CREATE TABLE plat(
   id SMALLINT,

   nom VARCHAR(50),
   photo_du_plat VARCHAR(50),
   type_de_plat VARCHAR(50),
   prix VARCHAR(50),
   idpays INT NOT NULL,
   PRIMARY KEY(id),
   FOREIGN KEY(idpays) REFERENCES Pays(idpays)
);

CREATE TABLE custommer(
   id_client INT,
   id INT NOT NULL,
   PRIMARY KEY(id_client),
   UNIQUE(id),
   FOREIGN KEY(id) REFERENCES utilisateur(id)
);

CREATE TABLE Commande(
   commande INT,
   
   date_heure_commande DATETIME,
   id_client INT NOT NULL,
   id_cuisinier INT NOT NULL,
   PRIMARY KEY(commande),
   FOREIGN KEY(id_client) REFERENCES custommer(id_client),
   FOREIGN KEY(id_cuisinier) REFERENCES Cuisinier(id_cuisinier)
);

CREATE TABLE ingrédients(
   id_ingr INT,
   nom_ingr VARCHAR(50),
   prix_kg DECIMAL(15,2) NOT NULL,
   
   idpays INT NOT NULL,
   PRIMARY KEY(id_ingr),
   FOREIGN KEY(idpays) REFERENCES Pays(idpays)
);

CREATE TABLE Ligne_de_commande_(
   id_ligne_de_commande INT,
   id_livraison INT NOT NULL,
   id_client INT NOT NULL,
   commande INT NOT NULL,
   PRIMARY KEY(id_ligne_de_commande),
   FOREIGN KEY(id_livraison) REFERENCES livraison(id_livraison),
   FOREIGN KEY(id_client) REFERENCES custommer(id_client),
   FOREIGN KEY(commande) REFERENCES Commande(commande)
);

CREATE TABLE avis(
   idavis INT,
   nombre_étoiles VARCHAR(50),
   retour_cuisinier VARCHAR(50),
   retour_id_client VARCHAR(50),
   date_avis DATETIME NOT NULL,
   commande INT NOT NULL,
   PRIMARY KEY(idavis),
   FOREIGN KEY(commande) REFERENCES Commande(commande)
);

CREATE TABLE Contient(
   id SMALLINT,
   id_ingr INT,
   quantité INT NOT NULL,
   PRIMARY KEY(id, id_ingr),
   FOREIGN KEY(id) REFERENCES plat(id),
   FOREIGN KEY(id_ingr) REFERENCES ingrédients(id_ingr)
);

CREATE TABLE inclue(
   id SMALLINT,
   id_ligne_de_commande INT,
   PRIMARY KEY(id, id_ligne_de_commande),
   FOREIGN KEY(id) REFERENCES plat(id),
   FOREIGN KEY(id_ligne_de_commande) REFERENCES Ligne_de_commande_(id_ligne_de_commande)
);
INSERT INTO pays (idpays, nom) VALUES
(1, 'France'),
(2, 'Italie'),
(3, 'Espagne'),
(4, 'Allemagne'),
(5, 'Canada'),
(6, 'États-Unis'),
(7, 'Chine'),
(8, 'Japon'),
(9, 'Brésil'),
(10, 'Maroc');

INSERT INTO plat (id, nom, prix, type_de_plat, idpays) VALUES
(1,  'Salade Fraîcheur',           7.50,  'Entrée',         1),
(2,  'Spaghetti Bolognese',       12.90,  'Plat Principal', 2),
(3,  'Tortilla Española',         10.00,  'Entrée',         3),
(4,  'Schnitzel',                 14.50,  'Plat Principal', 4),
(5,  'Poutine Classique',          9.90,  'Plat Principal', 5),
(6,  'Cheeseburger',              11.50,  'Plat Principal', 6),
(7,  'Canard Laqué',              18.00,  'Plat Principal', 7),
(8,  'Sushi Maki Assortis',       16.20,  'Plat Principal', 8),
(9,  'Feijoada',                  13.70,  'Plat Principal', 9),
(10, 'Tajine de Poulet',          15.80,  'Plat Principal', 10),

(11, 'Soupe à l\'Oignon',          6.50,  'Entrée',         1),
(12, 'Tiramisu',                   5.90,  'Dessert',        2),
(13, 'Gazpacho',                   5.50,  'Entrée',         3),
(14, 'Bretzel Géant',              3.90,  'Amuse-Bouche',   4),
(15, 'Tourtière Québécoise',      13.40,  'Plat Principal', 5),
(16, 'Brownie au Chocolat',        4.80,  'Dessert',        6),
(17, 'Riz Cantonais',              8.20,  'Plat Principal', 7),
(18, 'Tempura de Légumes',         9.50,  'Entrée',         8),
(19, 'Brigadeiro',                 4.20,  'Dessert',        9),
(20, 'Couscous Royal',            14.90,  'Plat Principal', 10),

(21, 'Quiche Lorraine',            6.90,  'Entrée',         1),
(22, 'Pizza Margherita',           9.50,  'Plat Principal', 2),
(23, 'Paella',                    15.00,  'Plat Principal', 3),
(24, 'Strudel aux Pommes',         5.10,  'Dessert',        4),
(25, 'Sirop d\'Érable sur Crêpe',  4.50,  'Dessert',        5),
(26, 'Hot Dog Américain',          5.90,  'Amuse-Bouche',   6),
(27, 'Nouilles Sautées',           7.80,  'Plat Principal', 7),
(28, 'Ramen',                     10.90,  'Plat Principal', 8),
(29, 'Coxinha',                    3.80,  'Amuse-Bouche',   9),
(30, 'Pastilla',                  12.00,  'Plat Principal', 10),

(31, 'Crêpe Suzette',             5.70,  'Dessert',        1),
(32, 'Risotto au Champignon',     11.50, 'Plat Principal', 2),
(33, 'Churros',                    4.50,  'Dessert',        3),
(34, 'Eisbein',                   14.00, 'Plat Principal', 4),
(35, 'Mac & Cheese',               6.50,  'Plat Principal', 6),
(36, 'Canard Pékinois',           19.90, 'Plat Principal', 7),
(37, 'Sashimi de Saumon',         17.00, 'Entrée',         8),
(38, 'Moqueca de Poisson',        15.50, 'Plat Principal', 9),
(39, 'Harira',                     8.40,  'Entrée',         10),
(40, 'Bœuf Bourguignon',          16.00, 'Plat Principal', 1),

(41, 'Ratatouille',                8.30,  'Plat Principal', 1),
(42, 'Lasagnes Maison',           13.90,  'Plat Principal', 2),
(43, 'Pulpo a la Gallega',        14.80,  'Entrée',         3),
(44, 'Forêt-Noire',                5.90,  'Dessert',        4),
(45, 'Soupe aux Pois',             6.00,  'Entrée',         5),
(46, 'Apple Pie',                  4.50,  'Dessert',        6),
(47, 'Dim Sum',                    7.90,  'Entrée',         7),
(48, 'Okonomiyaki',               12.90,  'Plat Principal', 8),
(49, 'Empadão',                   11.00,  'Plat Principal', 9),
(50, 'Brochette Kefta',           12.60,  'Plat Principal', 10),

(51, 'Croque-Monsieur',            5.40,  'Entrée',         1),
(52, 'Calzone',                   10.70,  'Plat Principal', 2),
(53, 'Flan Espagnol',              4.00,  'Dessert',        3),
(54, 'Currywurst',                 4.80,  'Plat Principal', 4),
(55, 'Beignes au Sirop',           3.50,  'Dessert',        5),
(56, 'Donut Chocolat',             2.90,  'Dessert',        6),
(57, 'Boulettes de Porc',          9.20,  'Plat Principal', 7),
(58, 'Yakitori',                  11.20,  'Amuse-Bouche',   8),
(59, 'Bolinho de Bacalhau',        6.00,  'Entrée',         9),
(60, 'Msemen',                     4.70,  'Amuse-Bouche',   10),

(61, 'Tarte Tatin',                4.60,  'Dessert',        1),
(62, 'Penne Arrabiata',            9.30,  'Plat Principal', 2),
(63, 'Tortillas de Maïs',          7.20,  'Entrée',         3),
(64, 'Spätzle Maison',            10.50,  'Plat Principal', 4),
(65, 'Fèves au Lard',              6.80,  'Entrée',         5),
(66, 'Bagel au Saumon',            5.90,  'Entrée',         6),
(67, 'Tofu Sauce Soja',            7.50,  'Plat Principal', 7),
(68, 'Takoyaki',                   6.90,  'Amuse-Bouche',   8),
(69, 'Pão de Queijo',              4.20,  'Amuse-Bouche',   9),
(70, 'Zaalouk',                    5.10,  'Entrée',         10),

(71, 'Escargots au Beurre',        7.90,  'Entrée',         1),
(72, 'Minestrone',                 6.40,  'Entrée',         2),
(73, 'Crema Catalana',             5.00,  'Dessert',        3),
(74, 'Käsespätzle',               11.40,  'Plat Principal', 4),
(75, 'Pâté Chinois',              10.20,  'Plat Principal', 5),
(76, 'Ribs BBQ',                  13.80,  'Plat Principal', 6),
(77, 'Bao Zi',                     4.70,  'Amuse-Bouche',   7),
(78, 'Onigiri',                    5.50,  'Amuse-Bouche',   8),
(79, 'Vatapá',                    12.60,  'Plat Principal', 9),
(80, 'Mkhanfar',                   6.40,  'Dessert',        10),

(81, 'Blanquette de Veau',        14.20,  'Plat Principal', 1),
(82, 'Gelato',                     4.60,  'Dessert',        2),
(83, 'Sangria Fruitée',            3.90,  'Amuse-Bouche',   3),
(84, 'Leberkäse',                  8.90,  'Plat Principal', 4),
(85, 'Bagel Montréalais',          4.50,  'Amuse-Bouche',   5),
(86, 'Milkshake Vanille',          3.80,  'Dessert',        6),
(87, 'Nems au Porc',               5.60,  'Entrée',         7),
(88, 'Dorayaki',                   4.20,  'Dessert',        8),
(89, 'Acarajé',                    5.80,  'Entrée',         9),
(90, 'Briouates',                  6.70,  'Entrée',         10),

(91, 'Gratin Dauphinois',          8.80,  'Plat Principal', 1),
(92, 'Focaccia',                   3.90,  'Amuse-Bouche',   2),
(93, 'Tapas Variées',              6.50,  'Amuse-Bouche',   3),
(94, 'Bockwurst',                  4.50,  'Amuse-Bouche',   4),
(95, 'Pouding Chômeur',            3.60,  'Dessert',        5),
(96, 'Cheesecake',                 5.80,  'Dessert',        6),
(97, 'Mapo Tofu',                  9.70,  'Plat Principal', 7),
(98, 'Katsudon',                  12.00,  'Plat Principal', 8),
(99, 'Brigadeiro Blanc',           4.20,  'Dessert',        9),
(100,'Harira Royale',              9.80,  'Entrée',         10);

INSERT INTO `utilisateur` (`id`,`Nom`,`Prenom`,`tel`,`email`,`adresse`,`mdp`)
VALUES
  (1,"Palmer","Beatrice","08 11 04 61 11","at.iaculis@google.com","849-9569 Fringilla St.","sagittis placerat."),
  (2,"Pierce","Rhoda","03 42 34 74 51","mauris.aliquam@google.com","174-7697 Neque Road","justo faucibus lectus, a"),
  (3,"Chan","Colt","05 57 60 86 97","posuere.cubilia@google.com","304-6564 Proin Avenue","dui lectus rutrum urna, nec"),
  (4,"Perez","Mohammad","04 43 34 30 15","orci.adipiscing@google.com","Ap #214-4616 Elit, Rd.","Aliquam erat"),
  (5,"Solomon","Flavia","04 14 78 71 25","molestie@google.com","203-6180 Turpis Road","non,"),
  (6,"Ellis","Amela","07 23 74 25 33","nec@google.com","711-6187 Laoreet, St.","Suspendisse non leo."),
  (7,"Olson","Janna","04 87 81 88 22","pede.malesuada@google.com","Ap #684-6377 Neque Av.","metus. Aliquam erat"),
  (8,"Griffith","Ezekiel","02 29 68 33 56","orci@google.com","P.O. Box 985, 6172 Arcu St.","amet, consectetuer adipiscing"),
  (9,"Guerrero","Daria","08 81 22 50 11","enim.mauris.quis@google.com","Ap #332-4433 In Rd.","neque sed dictum"),
  (10,"Wise","Christian","03 96 13 40 57","odio.nam@google.com","987-3365 Arcu. Av.","sem");
INSERT INTO `utilisateur` (`id`,`Nom`,`Prenom`,`tel`,`email`,`adresse`,`mdp`)
VALUES
  (11,"Langley","Buckminster","08 67 52 14 47","nisl.quisque@google.com","P.O. Box 433, 3959 Fermentum Rd.","nec, leo. Morbi"),
  (12,"Haney","Colt","05 13 93 54 80","ac.turpis.egestas@google.com","Ap #731-1839 Nulla Rd.","egestas a, dui."),
  (13,"Molina","Azalia","08 78 62 60 37","sollicitudin.adipiscing@google.com","123-9254 Metus. Rd.","lacinia mattis. Integer"),
  (14,"Stephens","Evelyn","06 13 62 92 59","a.sollicitudin@google.com","459-5072 Nec, Road","odio, auctor vitae,"),
  (15,"Banks","Guy","08 53 41 77 34","dolor.dolor.tempus@google.com","Ap #430-9457 Arcu Ave","lectus pede et risus. Quisque"),
  (16,"Santiago","Gabriel","08 14 13 81 71","consectetuer.mauris@google.com","Ap #859-6202 Mi Ave","nibh dolor,"),
  (17,"Hayden","Kiayada","03 03 49 40 64","rutrum.eu@google.com","P.O. Box 275, 7063 Arcu. Av.","nec luctus"),
  (18,"Rios","Ingrid","06 14 22 69 44","aliquam@google.com","238-9816 Aptent Street","feugiat metus"),
  (19,"Bowers","Inez","08 53 56 45 23","aliquet.nec.imperdiet@google.com","Ap #350-8015 Eros Rd.","ac turpis egestas. Fusce"),
  (20,"Walsh","Olga","07 96 32 31 64","dictum.placerat.augue@google.com","P.O. Box 159, 7254 Velit St.","cursus purus. Nullam scelerisque neque");
INSERT INTO `utilisateur` (`id`,`Nom`,`Prenom`,`tel`,`email`,`adresse`,`mdp`)
VALUES
  (21,"Estrada","Kelly","04 66 10 32 87","neque.nullam.ut@google.com","Ap #245-9994 Lacus St.","eget, dictum placerat,"),
  (22,"Dixon","Breanna","03 40 85 79 63","nonummy.ut@google.com","628-9460 Consequat Street","et netus et malesuada fames"),
  (23,"Hodge","Brett","06 14 06 87 76","sagittis.augue@google.com","P.O. Box 275, 3723 Est Rd.","dolor, nonummy"),
  (24,"Bray","Tallulah","06 63 37 72 68","nam@google.com","362-1304 Diam Street","aptent taciti"),
  (25,"Collins","Harrison","03 35 13 63 48","tincidunt.dui@google.com","Ap #176-3157 Mi Road","Nunc"),
  (26,"Campos","Geoffrey","05 13 52 54 45","quisque.porttitor@google.com","P.O. Box 483, 6795 Pellentesque St.","Mauris ut quam"),
  (27,"Lang","Kiayada","04 74 58 53 91","vulputate.mauris@google.com","Ap #277-5284 Habitant Rd.","lorem fringilla ornare placerat, orci"),
  (28,"Flynn","Nigel","03 57 56 57 51","suspendisse.sed@google.com","Ap #138-9921 Vestibulum Road","vestibulum. Mauris magna. Duis"),
  (29,"Barr","Colin","01 06 12 89 54","euismod.urna@google.com","613-2404 Lacus St.","magna. Cras convallis"),
  (30,"Carpenter","Palmer","05 68 51 26 24","nisl@google.com","698-1701 Rutrum St.","sollicitudin commodo ipsum.");
INSERT INTO `utilisateur` (`id`,`Nom`,`Prenom`,`tel`,`email`,`adresse`,`mdp`)
VALUES
  (31,"Stanton","Kirby","02 97 52 39 44","dapibus@google.com","Ap #312-3255 Risus. Rd.","quam dignissim pharetra. Nam ac"),
  (32,"Hunter","Geoffrey","07 36 19 32 61","ante.lectus.convallis@google.com","4662 Vulputate, Rd.","eros turpis"),
  (33,"Sykes","Susan","08 15 75 20 41","ullamcorper.nisl@google.com","8774 Eros Road","Duis a"),
  (34,"Morris","Hilary","05 35 56 79 28","pellentesque.tincidunt@google.com","987-6572 Risus. Ave","congue"),
  (35,"Bowen","Colt","05 95 21 12 09","in.consequat.enim@google.com","102-3629 Elementum Av.","feugiat. Lorem ipsum dolor"),
  (36,"Holmes","Colton","04 78 44 35 14","molestie.sed@google.com","435-2456 Luctus. Road","Maecenas iaculis"),
  (37,"Salinas","Simon","01 86 62 37 72","tristique.ac@google.com","222-525 Sed Av.","diam. Duis"),
  (38,"Combs","Macey","07 89 54 12 17","bibendum.ullamcorper.duis@google.com","1853 Non Av.","Nunc ut"),
  (39,"Riddle","Keelie","09 97 70 26 69","dictum.mi@google.com","Ap #620-242 Scelerisque St.","sociis natoque penatibus et magnis"),
  (40,"Benjamin","Lewis","06 57 43 46 41","sem.egestas@google.com","819-1661 Eget Av.","non dui");
INSERT INTO `utilisateur` (`id`,`Nom`,`Prenom`,`tel`,`email`,`adresse`,`mdp`)
VALUES
  (41,"Booker","Charles","06 04 19 20 89","orci.ut@google.com","Ap #465-9248 Ipsum. Ave","Cras lorem"),
  (42,"Hayes","Rose","01 67 97 07 63","tempor.lorem.eget@google.com","Ap #700-2113 Lectus, Street","Aenean gravida"),
  (43,"Gregory","Bert","07 58 85 04 33","tristique@google.com","P.O. Box 932, 6607 Praesent Road","Vivamus nisi."),
  (44,"Cotton","Jada","03 11 72 38 20","dolor.fusce.mi@google.com","751-5100 Ac, Avenue","vulputate ullamcorper magna. Sed eu"),
  (45,"Christian","Belle","08 27 15 16 24","pharetra.sed@google.com","Ap #436-685 Sem Road","diam. Pellentesque habitant morbi"),
  (46,"Pearson","Bell","02 21 06 82 37","cursus.non@google.com","Ap #271-6593 Aliquet Road","ante ipsum primis"),
  (47,"Leonard","Katelyn","04 83 61 72 37","ac.nulla@google.com","P.O. Box 253, 7416 Pede. Avenue","leo. Morbi"),
  (48,"Snider","Erin","05 96 20 53 03","aliquet@google.com","P.O. Box 709, 5479 Risus. St.","sit amet, consectetuer"),
  (49,"Goodwin","Aquila","04 16 56 69 55","porttitor@google.com","121-137 Amet Rd.","dictum cursus. Nunc"),
  (50,"Scott","Chancellor","04 48 78 82 72","ipsum.non@google.com","889-4031 Eros Av.","vitae");
INSERT INTO `utilisateur` (`id`,`Nom`,`Prenom`,`tel`,`email`,`adresse`,`mdp`)
VALUES
  (51,"Guy","Hakeem","04 73 52 49 21","a.neque@google.com","609-2506 Luctus. Avenue","semper auctor."),
  (52,"Randolph","Tyler","07 83 37 83 57","aptent@google.com","602-3975 Nec Avenue","odio"),
  (53,"Shields","Dennis","08 55 57 13 17","fringilla@google.com","Ap #759-2195 In Rd.","amet risus. Donec"),
  (54,"Rosario","Warren","08 19 10 58 62","ornare.lectus@google.com","171-1023 Maecenas Av.","quis diam. Pellentesque"),
  (55,"Small","Dante","08 53 86 91 79","magna.lorem@google.com","923-4884 Ac St.","sapien imperdiet ornare. In"),
  (56,"Faulkner","Fritz","09 64 26 40 52","a@google.com","154-4951 Pretium Street","risus"),
  (57,"Rivas","Keefe","08 04 63 87 63","nulla.facilisis@google.com","669-2031 Quisque Avenue","et magnis dis parturient montes,"),
  (58,"Martinez","Ciaran","06 77 36 26 55","dolor.tempus@google.com","Ap #635-1714 Accumsan Road","mauris id sapien. Cras dolor"),
  (59,"Griffin","Lester","03 16 65 66 20","duis@google.com","Ap #731-3756 Purus St.","Lorem ipsum dolor"),
  (60,"Gill","Priscilla","08 47 46 13 32","nonummy.ipsum@google.com","2884 Quis Avenue","ultrices. Duis");
INSERT INTO `utilisateur` (`id`,`Nom`,`Prenom`,`tel`,`email`,`adresse`,`mdp`)
VALUES
  (61,"Jensen","Wallace","03 73 60 71 94","pellentesque.habitant.morbi@google.com","Ap #152-5740 Tristique Rd.","et"),
  (62,"Schultz","Pascale","01 18 50 84 34","lobortis@google.com","5506 Ullamcorper Road","eu tellus. Phasellus elit pede,"),
  (63,"O'brien","Kevyn","05 02 33 07 73","porttitor.scelerisque@google.com","P.O. Box 553, 6210 Scelerisque St.","nec orci. Donec nibh."),
  (64,"Alexander","Jamalia","02 96 67 23 16","mi.lacinia.mattis@google.com","P.O. Box 326, 473 Ut, Avenue","arcu."),
  (65,"Sherman","Ainsley","07 21 86 60 28","sed.eu.nibh@google.com","391-903 Velit. St.","ultricies adipiscing,"),
  (66,"Joseph","Maxine","03 39 95 31 32","lorem@google.com","811-7881 Dictum Road","mollis"),
  (67,"Acosta","Gloria","09 09 53 14 95","lacus.pede.sagittis@google.com","Ap #243-2131 Non, Street","nec ligula consectetuer rhoncus."),
  (68,"Moran","Vanna","08 03 71 32 33","magna.nec@google.com","Ap #139-1256 Non Road","pretium neque. Morbi"),
  (69,"Maxwell","Guy","02 94 68 25 18","aliquet.phasellus.fermentum@google.com","6360 Quis St.","a, facilisis non,"),
  (70,"Drake","Thor","09 86 97 67 36","nulla@google.com","4288 Sodales. Avenue","eget laoreet");
INSERT INTO `utilisateur` (`id`,`Nom`,`Prenom`,`tel`,`email`,`adresse`,`mdp`)
VALUES
  (71,"Duncan","Sharon","07 46 25 34 94","adipiscing.elit.curabitur@google.com","601-2927 Bibendum St.","aliquet. Phasellus fermentum"),
  (72,"Silva","Jermaine","08 72 67 20 58","ut.pharetra@google.com","P.O. Box 248, 7111 Cras Av.","lorem fringilla ornare placerat,"),
  (73,"Gilliam","Perry","05 92 53 89 83","ultricies.sem.magna@google.com","Ap #907-7460 Luctus Rd.","consequat enim diam vel"),
  (74,"Riley","Knox","03 78 65 68 45","quis@google.com","P.O. Box 698, 6987 Erat Road","pellentesque, tellus sem"),
  (75,"Fernandez","Maile","01 26 90 69 17","egestas.aliquam@google.com","249-4509 Velit Av.","pede et risus. Quisque"),
  (76,"Bartlett","Richard","02 34 43 74 28","nec.mollis.vitae@google.com","Ap #578-2771 Tincidunt Rd.","lacus. Aliquam"),
  (77,"Hart","Abraham","03 16 11 49 55","tristique.pharetra@google.com","306-4754 Ligula. Ave","sem, vitae"),
  (78,"Mcconnell","Reed","04 53 06 53 73","cras@google.com","Ap #992-6400 Vivamus Rd.","Cras eu tellus eu"),
  (79,"Watkins","Elmo","05 73 87 64 05","magnis@google.com","538-2973 Orci, Road","Cum sociis natoque penatibus"),
  (80,"Figueroa","Adrian","03 93 66 88 74","id.enim@google.com","339-2938 Eu, Avenue","Proin vel arcu");
INSERT INTO `utilisateur` (`id`,`Nom`,`Prenom`,`tel`,`email`,`adresse`,`mdp`)
VALUES
  (81,"Carlson","Velma","05 59 08 83 91","facilisis.magna@google.com","Ap #140-6967 Molestie. Rd.","auctor"),
  (82,"Byrd","Rashad","04 00 24 59 23","aliquam.adipiscing@google.com","4727 Et Ave","natoque penatibus et magnis"),
  (83,"Frank","Violet","03 37 63 63 87","velit.cras.lorem@google.com","7717 Lorem St.","Donec est mauris, rhoncus id,"),
  (84,"Spencer","Oliver","05 61 75 36 37","aenean@google.com","528-3458 Ornare St.","rutrum lorem ac"),
  (85,"Clemons","Lenore","08 36 61 57 36","metus.in.nec@google.com","P.O. Box 275, 5576 Nunc Rd.","torquent per conubia nostra,"),
  (86,"Ware","Chaney","05 31 61 74 16","nec.diam@google.com","Ap #149-8514 Cras Street","non, luctus"),
  (87,"Sosa","Garrett","05 55 78 91 01","semper.dui@google.com","Ap #784-1614 Leo Rd.","Nunc commodo auctor velit."),
  (88,"Williamson","Herman","04 64 76 25 94","dapibus.gravida.aliquam@google.com","840-8944 Nunc Road","at, egestas"),
  (89,"Faulkner","Jameson","08 25 12 67 14","ac.facilisis.facilisis@google.com","Ap #332-7894 Eu St.","vehicula et,"),
  (90,"Hodge","Hilel","06 18 21 39 67","sem.molestie@google.com","8987 Libero Avenue","a, scelerisque sed,");
INSERT INTO `utilisateur` (`id`,`Nom`,`Prenom`,`tel`,`email`,`adresse`,`mdp`)
VALUES
  (91,"Carney","Grant","02 11 59 59 51","risus.in.mi@google.com","9671 Lorem St.","metus. In"),
  (92,"Henderson","Xyla","05 71 71 70 37","dui.nec@google.com","286-6559 Netus Avenue","metus sit amet ante."),
  (93,"Sanford","Erin","05 21 66 48 28","in.dolor.fusce@google.com","410 Cras Avenue","lacus. Nulla"),
  (94,"Miranda","Shad","03 40 97 85 82","per.inceptos@google.com","973-3305 Luctus Avenue","lacus. Quisque imperdiet,"),
  (95,"Preston","Cooper","04 51 76 97 16","integer.id@google.com","Ap #587-7562 Maecenas St.","scelerisque sed,"),
  (96,"Boone","Fitzgerald","08 32 64 31 12","quisque@google.com","656-3585 Vehicula. Rd.","dui. Suspendisse ac metus vitae"),
  (97,"Jefferson","Constance","07 56 35 11 54","fermentum.convallis.ligula@google.com","356-6131 Vel St.","tincidunt, neque vitae semper"),
  (98,"Vasquez","Kimberley","09 25 24 76 75","eget.nisi@google.com","Ap #939-1679 Erat, St.","ipsum. Phasellus"),
  (99,"Davenport","Teegan","03 61 91 36 62","lacus.ut@google.com","3600 Phasellus Ave","Aliquam"),
  (100,"Johns","Neve","01 34 27 33 13","at.fringilla.purus@google.com","P.O. Box 621, 2997 Non, Av.","tempus risus. Donec egestas.");
  
  INSERT INTO custommer (id_client, id)
VALUES
(1,1),
(2,2),
(3,3),
(4,4),
(5,5),
(6,6),
(7,7),
(8,8),
(9,9),
(10,10),
(11,11),
(12,12),
(13,13),
(14,14),
(15,15),
(16,16),
(17,17),
(18,18),
(19,19),
(20,20),
(21,21),
(22,22),
(23,23),
(24,24),
(25,25),
(26,26),
(27,27),
(28,28),
(29,29),
(30,30),
(31,31),
(32,32),
(33,33),
(34,34),
(35,35),
(36,36),
(37,37),
(38,38),
(39,39),
(40,40),
(41,41),
(42,42),
(43,43),
(44,44),
(45,45),
(46,46),
(47,47),
(48,48),
(49,49),
(50,50);

INSERT INTO cuisinier (id_cuisinier, id)
VALUES
(1,1),
(2,2),
(3,3),
(4,4),
(5,5),
(6,6),
(7,7),
(8,8),
(9,9),
(10,10),
(11,11),
(12,12),
(13,13),
(14,14),
(15,15),
(16,16),
(17,17),
(18,18),
(19,19),
(20,20),
(21,21),
(22,22),
(23,23),
(24,24),
(25,25),
(26,26),
(27,27),
(28,28),
(29,29),
(30,30),
(31,31),
(32,32),
(33,33),
(34,34),
(35,35),
(36,36),
(37,37),
(38,38),
(39,39),
(40,40),
(41,41),
(42,42),
(43,43),
(44,44),
(45,45),
(46,46),
(47,47),
(48,48),
(49,49),
(50,50),
(51,51),
(52,52),
(53,53),
(54,54),
(55,55),
(56,56),
(57,57),
(58,58),
(59,59),
(60,60),
(61,61),
(62,62),
(63,63),
(64,64),
(65,65),
(66,66),
(67,67),
(68,68),
(69,69),
(70,70),
(71,71),
(72,72),
(73,73),
(74,74),
(75,75),
(76,76),
(77,77),
(78,78),
(79,79),
(80,80),
(81,81),
(82,82),
(83,83),
(84,84),
(85,85),
(86,86),
(87,87),
(88,88),
(89,89),
(90,90),
(91,91),
(92,92),
(93,93),
(94,94),
(95,95),
(96,96),
(97,97),
(98,98),
(99,99),
(100,100);


INSERT INTO ingrédients (id_ingr, nom_ingr, prix_kg, idpays) VALUES
(1,  'Tomate',             2.50,  1),
(2,  'Mozzarella',         5.00,  2),
(3,  'Olive',              4.00,  3),
(4,  'Farine de Blé',      1.20,  1),
(5,  'Beurre',             3.50,  1),
(6,  'Oignon',             1.00,  1),
(7,  'Ail',                2.00,  1),
(8,  'Pomme de Terre',     1.10,  4),
(9,  'Riz Basmati',        2.80,  7),
(10, 'Bœuf',               9.50,  1),

(11, 'Crevette',           12.00, 3),
(12, 'Champignon',         4.50,  1),
(13, 'Poulet',             6.80,  1),
(14, 'Poivron Rouge',      3.10,  1),
(15, 'Parmesan',           8.50,  2),
(16, 'Jambon Cru',         9.00,  2),
(17, 'Chorizo',            7.20,  3),
(18, 'Saumon',             14.50, 1),
(19, 'Fromage à la Crème', 6.00,  5),
(20, 'Lait de Coco',       3.20,  9),

(21, 'Bacon',              6.40,  6),
(22, 'Sirop d’Érable',     10.0,  5),
(23, 'Cacao en Poudre',    7.50,  6),
(24, 'Crevettes Roses',    13.0,  9),
(25, 'Fraise',             4.00,  1),
(26, 'Sucre',              1.90,  1),
(27, 'Sel de Mer',         1.00,  1),
(28, 'Huile d’Olive',      6.20,  3),
(29, 'Concombre',          1.50,  1),
(30, 'Raisin',             3.60,  1),

(31, 'Piment',             4.10,  9),
(32, 'Haricot Rouge',      2.80,  9),
(33, 'Farine de Maïs',     2.20,  6),
(34, 'Nouilles de Riz',    3.00,  7),
(35, 'Tofu Ferme',         2.70,  7),
(36, 'Gingembre',          5.00,  7),
(37, 'Poisson Blanc',      9.30,  3),
(38, 'Wasabi',             20.0,  8),
(39, 'Algues Nori',        15.0,  8),
(40, 'Sauce Soja',         2.50,  7),

(41, 'Champignon Noir',    7.60,  7),
(42, 'Pois Chiche',        1.70,  10),
(43, 'Cannelle',           10.0,  10),
(44, 'Cumin',              12.0,  10),
(45, 'Citron',             3.20,  1),
(46, 'Basilic',            12.0,  2),
(47, 'Menthe',             8.60,  10),
(48, 'Oeuf',               2.00,  1),
(49, 'Yaourt Nature',      2.50,  1),
(50, 'Miel',               8.00,  1),

(51, 'Échalote',           2.40,  1),
(52, 'Vin Blanc',          4.70,  1),
(53, 'Vin Rouge',          5.20,  1),
(54, 'Fumet de Poisson',   6.60,  1),
(55, 'Crème Fraîche',      4.00,  1),
(56, 'Pâte Feuilletée',    3.80,  1),
(57, 'Pâte Brisée',        3.60,  1),
(58, 'Nutella',            8.50,  6),
(59, 'Banane',             2.10,  1),
(60, 'Avocat',             5.00,  9),

(61, 'Haricot Vert',       1.90,  1),
(62, 'Carotte',            1.20,  1),
(63, 'Piment d\'Espelette',12.5,  3),
(64, 'Tabasco',            9.00,  6),
(65, 'Fromage Cheddar',    7.50,  6),
(66, 'Huile de Sésame',    6.70,  7),
(67, 'Châtaigne d\'Eau',   5.50,  7),
(68, 'Vinaigre de Riz',    3.20,  8),
(69, 'Farine de Riz',      2.10,  7),
(70, 'Cassonade',          2.50,  1),

(71, 'Beurre de Cacahuète',4.80,  6),
(72, 'Câpres',             6.00,  3),
(73, 'Céleri',             1.60,  1),
(74, 'Moutarde',           2.30,  1),
(75, 'Poudre d\'Amande',   9.60,  1),
(76, 'Fève de Cacao',      15.0,  6),
(77, 'Clou de Girofle',    14.0,  10),
(78, 'Pâte à Pizza',       2.60,  2),
(79, 'Ricotta',            5.40,  2),
(80, 'Mascarpone',         6.80,  2),

(81, 'Thon',               13.0,  8),
(82, 'Crevettes Grises',   14.0,  3),
(83, 'Poulet Fumé',        8.20,  6),
(84, 'Laitue',             1.30,  1),
(85, 'Navet',              1.00,  1),
(86, 'Radis',              2.00,  1),
(87, 'Fraise des Bois',    10.0,  1),
(88, 'Pâte Miso',          7.50,  8),
(89, 'Épices Cajun',       12.0,  6),
(90, 'Noix de Coco Râpée', 5.00,  9),

(91, 'Fenouil',            3.40,  1),
(92, 'Pois Cassés',        2.50,  5),
(93, 'Blanc d\'Œuf',       2.80,  1),
(94, 'Gelée Royale',       25.0,  1),
(95, 'Paprika',            8.20,  3),
(96, 'Origan',             12.0,  2),
(97, 'Romarin',            11.0,  1),
(98, 'Thym',               10.0,  1),
(99, 'Zeste de Citron',    8.50,  1),
(100,'Petit Pois',         1.90,  1);

INSERT INTO Contient (id, id_ingr, quantité) VALUES
  -- Plat 1
  (1,1,40),
  (1,2,50),
  (1,3,60),
  (1,4,70),
  (1,5,80),
  (1,6,90),
  (1,7,100),
  (1,8,110),
  (1,9,120),
  (1,10,130),

  -- Plat 2
  (2,11,40),
  (2,12,50),
  (2,13,60),
  (2,14,70),
  (2,15,80),
  (2,16,90),
  (2,17,100),
  (2,18,110),
  (2,19,120),
  (2,20,130),

  -- Plat 3
  (3,21,40),
  (3,22,50),
  (3,23,60),
  (3,24,70),
  (3,25,80),
  (3,26,90),
  (3,27,100),
  (3,28,110),
  (3,29,120),
  (3,30,130),

  -- Plat 4
  (4,31,40),
  (4,32,50),
  (4,33,60),
  (4,34,70),
  (4,35,80),
  (4,36,90),
  (4,37,100),
  (4,38,110),
  (4,39,120),
  (4,40,130),

  -- Plat 5
  (5,41,40),
  (5,42,50),
  (5,43,60),
  (5,44,70),
  (5,45,80),
  (5,46,90),
  (5,47,100),
  (5,48,110),
  (5,49,120),
  (5,50,130),

  -- Plat 6
  (6,51,40),
  (6,52,50),
  (6,53,60),
  (6,54,70),
  (6,55,80),
  (6,56,90),
  (6,57,100),
  (6,58,110),
  (6,59,120),
  (6,60,130),

  -- Plat 7
  (7,61,40),
  (7,62,50),
  (7,63,60),
  (7,64,70),
  (7,65,80),
  (7,66,90),
  (7,67,100),
  (7,68,110),
  (7,69,120),
  (7,70,130),

  -- Plat 8
  (8,71,40),
  (8,72,50),
  (8,73,60),
  (8,74,70),
  (8,75,80),
  (8,76,90),
  (8,77,100),
  (8,78,110),
  (8,79,120),
  (8,80,130),

  -- Plat 9
  (9,81,40),
  (9,82,50),
  (9,83,60),
  (9,84,70),
  (9,85,80),
  (9,86,90),
  (9,87,100),
  (9,88,110),
  (9,89,120),
  (9,90,130),

  -- Plat 10
  (10,91,40),
  (10,92,50),
  (10,93,60),
  (10,94,70),
  (10,95,80),
  (10,96,90),
  (10,97,100),
  (10,98,110),
  (10,99,120),
  (10,100,130),

  -- Plat 11
  (11,1,40),
  (11,2,50),
  (11,3,60),
  (11,4,70),
  (11,5,80),
  (11,6,90),
  (11,7,100),
  (11,8,110),
  (11,9,120),
  (11,10,130),

  -- Plat 12
  (12,11,40),
  (12,12,50),
  (12,13,60),
  (12,14,70),
  (12,15,80),
  (12,16,90),
  (12,17,100),
  (12,18,110),
  (12,19,120),
  (12,20,130),

  -- Plat 13
  (13,21,40),
  (13,22,50),
  (13,23,60),
  (13,24,70),
  (13,25,80),
  (13,26,90),
  (13,27,100),
  (13,28,110),
  (13,29,120),
  (13,30,130),

  -- Plat 14
  (14,31,40),
  (14,32,50),
  (14,33,60),
  (14,34,70),
  (14,35,80),
  (14,36,90),
  (14,37,100),
  (14,38,110),
  (14,39,120),
  (14,40,130),

  -- Plat 15
  (15,41,40),
  (15,42,50),
  (15,43,60),
  (15,44,70),
  (15,45,80),
  (15,46,90),
  (15,47,100),
  (15,48,110),
  (15,49,120),
  (15,50,130),

  -- Plat 16
  (16,51,40),
  (16,52,50),
  (16,53,60),
  (16,54,70),
  (16,55,80),
  (16,56,90),
  (16,57,100),
  (16,58,110),
  (16,59,120),
  (16,60,130),

  -- Plat 17
  (17,61,40),
  (17,62,50),
  (17,63,60),
  (17,64,70),
  (17,65,80),
  (17,66,90),
  (17,67,100),
  (17,68,110),
  (17,69,120),
  (17,70,130),

  -- Plat 18
  (18,71,40),
  (18,72,50),
  (18,73,60),
  (18,74,70),
  (18,75,80),
  (18,76,90),
  (18,77,100),
  (18,78,110),
  (18,79,120),
  (18,80,130),

  -- Plat 19
  (19,81,40),
  (19,82,50),
  (19,83,60),
  (19,84,70),
  (19,85,80),
  (19,86,90),
  (19,87,100),
  (19,88,110),
  (19,89,120),
  (19,90,130),

  -- Plat 20
  (20,91,40),
  (20,92,50),
  (20,93,60),
  (20,94,70),
  (20,95,80),
  (20,96,90),
  (20,97,100),
  (20,98,110),
  (20,99,120),
  (20,100,130),

  -- Plat 21
  (21,1,40),
  (21,2,50),
  (21,3,60),
  (21,4,70),
  (21,5,80),
  (21,6,90),
  (21,7,100),
  (21,8,110),
  (21,9,120),
  (21,10,130),

  -- Plat 22
  (22,11,40),
  (22,12,50),
  (22,13,60),
  (22,14,70),
  (22,15,80),
  (22,16,90),
  (22,17,100),
  (22,18,110),
  (22,19,120),
  (22,20,130),

  -- Plat 23
  (23,21,40),
  (23,22,50),
  (23,23,60),
  (23,24,70),
  (23,25,80),
  (23,26,90),
  (23,27,100),
  (23,28,110),
  (23,29,120),
  (23,30,130),

  -- Plat 24
  (24,31,40),
  (24,32,50),
  (24,33,60),
  (24,34,70),
  (24,35,80),
  (24,36,90),
  (24,37,100),
  (24,38,110),
  (24,39,120),
  (24,40,130),

  -- Plat 25
  (25,41,40),
  (25,42,50),
  (25,43,60),
  (25,44,70),
  (25,45,80),
  (25,46,90),
  (25,47,100),
  (25,48,110),
  (25,49,120),
  (25,50,130),

  -- Plat 26
  (26,51,40),
  (26,52,50),
  (26,53,60),
  (26,54,70),
  (26,55,80),
  (26,56,90),
  (26,57,100),
  (26,58,110),
  (26,59,120),
  (26,60,130),

  -- Plat 27
  (27,61,40),
  (27,62,50),
  (27,63,60),
  (27,64,70),
  (27,65,80),
  (27,66,90),
  (27,67,100),
  (27,68,110),
  (27,69,120),
  (27,70,130),

  -- Plat 28
  (28,71,40),
  (28,72,50),
  (28,73,60),
  (28,74,70),
  (28,75,80),
  (28,76,90),
  (28,77,100),
  (28,78,110),
  (28,79,120),
  (28,80,130),

  -- Plat 29
  (29,81,40),
  (29,82,50),
  (29,83,60),
  (29,84,70),
  (29,85,80),
  (29,86,90),
  (29,87,100),
  (29,88,110),
  (29,89,120),
  (29,90,130),

  -- Plat 30
  (30,91,40),
  (30,92,50),
  (30,93,60),
  (30,94,70),
  (30,95,80),
  (30,96,90),
  (30,97,100),
  (30,98,110),
  (30,99,120),
  (30,100,130),

  -- Plat 31
  (31,1,40),
  (31,2,50),
  (31,3,60),
  (31,4,70),
  (31,5,80),
  (31,6,90),
  (31,7,100),
  (31,8,110),
  (31,9,120),
  (31,10,130),

  -- Plat 32
  (32,11,40),
  (32,12,50),
  (32,13,60),
  (32,14,70),
  (32,15,80),
  (32,16,90),
  (32,17,100),
  (32,18,110),
  (32,19,120),
  (32,20,130),

  -- Plat 33
  (33,21,40),
  (33,22,50),
  (33,23,60),
  (33,24,70),
  (33,25,80),
  (33,26,90),
  (33,27,100),
  (33,28,110),
  (33,29,120),
  (33,30,130),

  -- Plat 34
  (34,31,40),
  (34,32,50),
  (34,33,60),
  (34,34,70),
  (34,35,80),
  (34,36,90),
  (34,37,100),
  (34,38,110),
  (34,39,120),
  (34,40,130),

  -- Plat 35
  (35,41,40),
  (35,42,50),
  (35,43,60),
  (35,44,70),
  (35,45,80),
  (35,46,90),
  (35,47,100),
  (35,48,110),
  (35,49,120),
  (35,50,130),

  -- Plat 36
  (36,51,40),
  (36,52,50),
  (36,53,60),
  (36,54,70),
  (36,55,80),
  (36,56,90),
  (36,57,100),
  (36,58,110),
  (36,59,120),
  (36,60,130),

  -- Plat 37
  (37,61,40),
  (37,62,50),
  (37,63,60),
  (37,64,70),
  (37,65,80),
  (37,66,90),
  (37,67,100),
  (37,68,110),
  (37,69,120),
  (37,70,130),

  -- Plat 38
  (38,71,40),
  (38,72,50),
  (38,73,60),
  (38,74,70),
  (38,75,80),
  (38,76,90),
  (38,77,100),
  (38,78,110),
  (38,79,120),
  (38,80,130),

  -- Plat 39
  (39,81,40),
  (39,82,50),
  (39,83,60),
  (39,84,70),
  (39,85,80),
  (39,86,90),
  (39,87,100),
  (39,88,110),
  (39,89,120),
  (39,90,130),

  -- Plat 40
  (40,91,40),
  (40,92,50),
  (40,93,60),
  (40,94,70),
  (40,95,80),
  (40,96,90),
  (40,97,100),
  (40,98,110),
  (40,99,120),
  (40,100,130),

  -- Plat 41
  (41,1,40),
  (41,2,50),
  (41,3,60),
  (41,4,70),
  (41,5,80),
  (41,6,90),
  (41,7,100),
  (41,8,110),
  (41,9,120),
  (41,10,130),

  -- Plat 42
  (42,11,40),
  (42,12,50),
  (42,13,60),
  (42,14,70),
  (42,15,80),
  (42,16,90),
  (42,17,100),
  (42,18,110),
  (42,19,120),
  (42,20,130),

  -- Plat 43
  (43,21,40),
  (43,22,50),
  (43,23,60),
  (43,24,70),
  (43,25,80),
  (43,26,90),
  (43,27,100),
  (43,28,110),
  (43,29,120),
  (43,30,130),

  -- Plat 44
  (44,31,40),
  (44,32,50),
  (44,33,60),
  (44,34,70),
  (44,35,80),
  (44,36,90),
  (44,37,100),
  (44,38,110),
  (44,39,120),
  (44,40,130),

  -- Plat 45
  (45,41,40),
  (45,42,50),
  (45,43,60),
  (45,44,70),
  (45,45,80),
  (45,46,90),
  (45,47,100),
  (45,48,110),
  (45,49,120),
  (45,50,130),

  -- Plat 46
  (46,51,40),
  (46,52,50),
  (46,53,60),
  (46,54,70),
  (46,55,80),
  (46,56,90),
  (46,57,100),
  (46,58,110),
  (46,59,120),
  (46,60,130),

  -- Plat 47
  (47,61,40),
  (47,62,50),
  (47,63,60),
  (47,64,70),
  (47,65,80),
  (47,66,90),
  (47,67,100),
  (47,68,110),
  (47,69,120),
  (47,70,130),

  -- Plat 48
  (48,71,40),
  (48,72,50),
  (48,73,60),
  (48,74,70),
  (48,75,80),
  (48,76,90),
  (48,77,100),
  (48,78,110),
  (48,79,120),
  (48,80,130),

  -- Plat 49
  (49,81,40),
  (49,82,50),
  (49,83,60),
  (49,84,70),
  (49,85,80),
  (49,86,90),
  (49,87,100),
  (49,88,110),
  (49,89,120),
  (49,90,130),

  -- Plat 50
  (50,91,40),
  (50,92,50),
  (50,93,60),
  (50,94,70),
  (50,95,80),
  (50,96,90),
  (50,97,100),
  (50,98,110),
  (50,99,120),
  (50,100,130),

  -- Plat 51
  (51,1,40),
  (51,2,50),
  (51,3,60),
  (51,4,70),
  (51,5,80),
  (51,6,90),
  (51,7,100),
  (51,8,110),
  (51,9,120),
  (51,10,130),

  -- Plat 52
  (52,11,40),
  (52,12,50),
  (52,13,60),
  (52,14,70),
  (52,15,80),
  (52,16,90),
  (52,17,100),
  (52,18,110),
  (52,19,120),
  (52,20,130),

  -- Plat 53
  (53,21,40),
  (53,22,50),
  (53,23,60),
  (53,24,70),
  (53,25,80),
  (53,26,90),
  (53,27,100),
  (53,28,110),
  (53,29,120),
  (53,30,130),

  -- Plat 54
  (54,31,40),
  (54,32,50),
  (54,33,60),
  (54,34,70),
  (54,35,80),
  (54,36,90),
  (54,37,100),
  (54,38,110),
  (54,39,120),
  (54,40,130),

  -- Plat 55
  (55,41,40),
  (55,42,50),
  (55,43,60),
  (55,44,70),
  (55,45,80),
  (55,46,90),
  (55,47,100),
  (55,48,110),
  (55,49,120),
  (55,50,130),

  -- Plat 56
  (56,51,40),
  (56,52,50),
  (56,53,60),
  (56,54,70),
  (56,55,80),
  (56,56,90),
  (56,57,100),
  (56,58,110),
  (56,59,120),
  (56,60,130),

  -- Plat 57
  (57,61,40),
  (57,62,50),
  (57,63,60),
  (57,64,70),
  (57,65,80),
  (57,66,90),
  (57,67,100),
  (57,68,110),
  (57,69,120),
  (57,70,130),

  -- Plat 58
  (58,71,40),
  (58,72,50),
  (58,73,60),
  (58,74,70),
  (58,75,80),
  (58,76,90),
  (58,77,100),
  (58,78,110),
  (58,79,120),
  (58,80,130),

  -- Plat 59
  (59,81,40),
  (59,82,50),
  (59,83,60),
  (59,84,70),
  (59,85,80),
  (59,86,90),
  (59,87,100),
  (59,88,110),
  (59,89,120),
  (59,90,130),

  -- Plat 60
  (60,91,40),
  (60,92,50),
  (60,93,60),
  (60,94,70),
  (60,95,80),
  (60,96,90),
  (60,97,100),
  (60,98,110),
  (60,99,120),
  (60,100,130),

  -- Plat 61
  (61,1,40),
  (61,2,50),
  (61,3,60),
  (61,4,70),
  (61,5,80),
  (61,6,90),
  (61,7,100),
  (61,8,110),
  (61,9,120),
  (61,10,130),

  -- Plat 62
  (62,11,40),
  (62,12,50),
  (62,13,60),
  (62,14,70),
  (62,15,80),
  (62,16,90),
  (62,17,100),
  (62,18,110),
  (62,19,120),
  (62,20,130),

  -- Plat 63
  (63,21,40),
  (63,22,50),
  (63,23,60),
  (63,24,70),
  (63,25,80),
  (63,26,90),
  (63,27,100),
  (63,28,110),
  (63,29,120),
  (63,30,130),

  -- Plat 64
  (64,31,40),
  (64,32,50),
  (64,33,60),
  (64,34,70),
  (64,35,80),
  (64,36,90),
  (64,37,100),
  (64,38,110),
  (64,39,120),
  (64,40,130),

  -- Plat 65
  (65,41,40),
  (65,42,50),
  (65,43,60),
  (65,44,70),
  (65,45,80),
  (65,46,90),
  (65,47,100),
  (65,48,110),
  (65,49,120),
  (65,50,130),

  -- Plat 66
  (66,51,40),
  (66,52,50),
  (66,53,60),
  (66,54,70),
  (66,55,80),
  (66,56,90),
  (66,57,100),
  (66,58,110),
  (66,59,120),
  (66,60,130),

  -- Plat 67
  (67,61,40),
  (67,62,50),
  (67,63,60),
  (67,64,70),
  (67,65,80),
  (67,66,90),
  (67,67,100),
  (67,68,110),
  (67,69,120),
  (67,70,130),

  -- Plat 68
  (68,71,40),
  (68,72,50),
  (68,73,60),
  (68,74,70),
  (68,75,80),
  (68,76,90),
  (68,77,100),
  (68,78,110),
  (68,79,120),
  (68,80,130),

  -- Plat 69
  (69,81,40),
  (69,82,50),
  (69,83,60),
  (69,84,70),
  (69,85,80),
  (69,86,90),
  (69,87,100),
  (69,88,110),
  (69,89,120),
  (69,90,130),

  -- Plat 70
  (70,91,40),
  (70,92,50),
  (70,93,60),
  (70,94,70),
  (70,95,80),
  (70,96,90),
  (70,97,100),
  (70,98,110),
  (70,99,120),
  (70,100,130),

  -- Plat 71
  (71,1,40),
  (71,2,50),
  (71,3,60),
  (71,4,70),
  (71,5,80),
  (71,6,90),
  (71,7,100),
  (71,8,110),
  (71,9,120),
  (71,10,130),

  -- Plat 72
  (72,11,40),
  (72,12,50),
  (72,13,60),
  (72,14,70),
  (72,15,80),
  (72,16,90),
  (72,17,100),
  (72,18,110),
  (72,19,120),
  (72,20,130),

  -- Plat 73
  (73,21,40),
  (73,22,50),
  (73,23,60),
  (73,24,70),
  (73,25,80),
  (73,26,90),
  (73,27,100),
  (73,28,110),
  (73,29,120),
  (73,30,130),

  -- Plat 74
  (74,31,40),
  (74,32,50),
  (74,33,60),
  (74,34,70),
  (74,35,80),
  (74,36,90),
  (74,37,100),
  (74,38,110),
  (74,39,120),
  (74,40,130),

  -- Plat 75
  (75,41,40),
  (75,42,50),
  (75,43,60),
  (75,44,70),
  (75,45,80),
  (75,46,90),
  (75,47,100),
  (75,48,110),
  (75,49,120),
  (75,50,130),

  -- Plat 76
  (76,51,40),
  (76,52,50),
  (76,53,60),
  (76,54,70),
  (76,55,80),
  (76,56,90),
  (76,57,100),
  (76,58,110),
  (76,59,120),
  (76,60,130),

  -- Plat 77
  (77,61,40),
  (77,62,50),
  (77,63,60),
  (77,64,70),
  (77,65,80),
  (77,66,90),
  (77,67,100),
  (77,68,110),
  (77,69,120),
  (77,70,130),

  -- Plat 78
  (78,71,40),
  (78,72,50),
  (78,73,60),
  (78,74,70),
  (78,75,80),
  (78,76,90),
  (78,77,100),
  (78,78,110),
  (78,79,120),
  (78,80,130),

  -- Plat 79
  (79,81,40),
  (79,82,50),
  (79,83,60),
  (79,84,70),
  (79,85,80),
  (79,86,90),
  (79,87,100),
  (79,88,110),
  (79,89,120),
  (79,90,130),

  -- Plat 80
  (80,91,40),
  (80,92,50),
  (80,93,60),
  (80,94,70),
  (80,95,80),
  (80,96,90),
  (80,97,100),
  (80,98,110),
  (80,99,120),
  (80,100,130),

  -- Plat 81
  (81,1,40),
  (81,2,50),
  (81,3,60),
  (81,4,70),
  (81,5,80),
  (81,6,90),
  (81,7,100),
  (81,8,110),
  (81,9,120),
  (81,10,130),

  -- Plat 82
  (82,11,40),
  (82,12,50),
  (82,13,60),
  (82,14,70),
  (82,15,80),
  (82,16,90),
  (82,17,100),
  (82,18,110),
  (82,19,120),
  (82,20,130),

  -- Plat 83
  (83,21,40),
  (83,22,50),
  (83,23,60),
  (83,24,70),
  (83,25,80),
  (83,26,90),
  (83,27,100),
  (83,28,110),
  (83,29,120),
  (83,30,130),

  -- Plat 84
  (84,31,40),
  (84,32,50),
  (84,33,60),
  (84,34,70),
  (84,35,80),
  (84,36,90),
  (84,37,100),
  (84,38,110),
  (84,39,120),
  (84,40,130),

  -- Plat 85
  (85,41,40),
  (85,42,50),
  (85,43,60),
  (85,44,70),
  (85,45,80),
  (85,46,90),
  (85,47,100),
  (85,48,110),
  (85,49,120),
  (85,50,130),

  -- Plat 86
  (86,51,40),
  (86,52,50),
  (86,53,60),
  (86,54,70),
  (86,55,80),
  (86,56,90),
  (86,57,100),
  (86,58,110),
  (86,59,120),
  (86,60,130),

  -- Plat 87
  (87,61,40),
  (87,62,50),
  (87,63,60),
  (87,64,70),
  (87,65,80),
  (87,66,90),
  (87,67,100),
  (87,68,110),
  (87,69,120),
  (87,70,130),

  -- Plat 88
  (88,71,40),
  (88,72,50),
  (88,73,60),
  (88,74,70),
  (88,75,80),
  (88,76,90),
  (88,77,100),
  (88,78,110),
  (88,79,120),
  (88,80,130),

  -- Plat 89
  (89,81,40),
  (89,82,50),
  (89,83,60),
  (89,84,70),
  (89,85,80),
  (89,86,90),
  (89,87,100),
  (89,88,110),
  (89,89,120),
  (89,90,130),

  -- Plat 90
  (90,91,40),
  (90,92,50),
  (90,93,60),
  (90,94,70),
  (90,95,80),
  (90,96,90),
  (90,97,100),
  (90,98,110),
  (90,99,120),
  (90,100,130),

  -- Plat 91
  (91,1,40),
  (91,2,50),
  (91,3,60),
  (91,4,70),
  (91,5,80),
  (91,6,90),
  (91,7,100),
  (91,8,110),
  (91,9,120),
  (91,10,130),

  -- Plat 92
  (92,11,40),
  (92,12,50),
  (92,13,60),
  (92,14,70),
  (92,15,80),
  (92,16,90),
  (92,17,100),
  (92,18,110),
  (92,19,120),
  (92,20,130),

  -- Plat 93
  (93,21,40),
  (93,22,50),
  (93,23,60),
  (93,24,70),
  (93,25,80),
  (93,26,90),
  (93,27,100),
  (93,28,110),
  (93,29,120),
  (93,30,130),

  -- Plat 94
  (94,31,40),
  (94,32,50),
  (94,33,60),
  (94,34,70),
  (94,35,80),
  (94,36,90),
  (94,37,100),
  (94,38,110),
  (94,39,120),
  (94,40,130),

  -- Plat 95
  (95,41,40),
  (95,42,50),
  (95,43,60),
  (95,44,70),
  (95,45,80),
  (95,46,90),
  (95,47,100),
  (95,48,110),
  (95,49,120),
  (95,50,130),

  -- Plat 96
  (96,51,40),
  (96,52,50),
  (96,53,60),
  (96,54,70),
  (96,55,80),
  (96,56,90),
  (96,57,100),
  (96,58,110),
  (96,59,120),
  (96,60,130),

  -- Plat 97
  (97,61,40),
  (97,62,50),
  (97,63,60),
  (97,64,70),
  (97,65,80),
  (97,66,90),
  (97,67,100),
  (97,68,110),
  (97,69,120),
  (97,70,130),

  -- Plat 98
  (98,71,40),
  (98,72,50),
  (98,73,60),
  (98,74,70),
  (98,75,80),
  (98,76,90),
  (98,77,100),
  (98,78,110),
  (98,79,120),
  (98,80,130),

  -- Plat 99
  (99,81,40),
  (99,82,50),
  (99,83,60),
  (99,84,70),
  (99,85,80),
  (99,86,90),
  (99,87,100),
  (99,88,110),
  (99,89,120),
  (99,90,130),

  -- Plat 100
  (100,91,40),
  (100,92,50),
  (100,93,60),
  (100,94,70),
  (100,95,80),
  (100,96,90),
  (100,97,100),
  (100,98,110),
  (100,99,120),
  (100,100,130);
  
INSERT INTO Commande (commande, date_heure_commande, id_client, id_cuisinier)
VALUES
 (1,        '2025-02-27 10:00:00',  1, 1),
 (2,  '2025-02-27 10:10:00',  2, 1),
 (3,  '2025-02-27 10:20:00',  3, 1),
 (4,  '2025-02-27 10:30:00',  4, 2),
 (5,  '2025-02-27 10:40:00',  5, 2),
 (6,  '2025-02-27 10:50:00',  6, 2),
 (7,  '2025-02-27 11:00:00',  7, 3),
 (8,       '2025-02-27 11:10:00',  8, 3),
 (9,  '2025-02-27 11:20:00',  9, 3),
 (10, '2025-02-27 11:30:00', 10, 4),
 (11, '2025-02-27 11:40:00', 11, 4),
 (12, '2025-02-27 11:50:00', 12, 4),
 (13, '2025-02-27 12:00:00', 13, 1),
 (14, '2025-02-27 12:10:00', 14, 1),
 (15, '2025-02-27 12:20:00', 15, 2),
 (16, '2025-02-27 12:30:00', 16, 2),
 (17,       '2025-02-27 12:40:00', 17, 3),
 (18, '2025-02-27 12:50:00', 18, 3),
 (19, '2025-02-27 13:00:00', 19, 4),
 (20, '2025-02-27 13:10:00', 20, 4);

INSERT INTO livraison (id_livraison, date_heure_livraison)
VALUES
  (1, '2025-02-28 14:00:00'),
  (2, '2025-02-28 15:00:00'), 
  (99, NULL);                 


            

-- Commande 1 => lignes 1..4
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(1, 1, 1, 1),
(2, 1, 1, 1),
(3, 1, 1, 1),
(4, 1, 1, 1);

-- Commande 2 => lignes 5..8
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(5, 2, 2, 2),
(6, 2, 2, 2),
(7, 2, 2, 2),
(8, 2, 2, 2);

-- Commande 3 => lignes 9..12
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(9,  99, 3, 3),
(10, 99, 3, 3),
(11, 99, 3, 3),
(12, 99, 3, 3);

-- Commande 4 => lignes 13..16
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(13, 99, 4, 4),
(14, 99, 4, 4),
(15, 99, 4, 4),
(16, 99, 4, 4);

-- Commande 5 => lignes 17..20
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(17, 99, 5, 5),
(18, 99, 5, 5),
(19, 99, 5, 5),
(20, 99, 5, 5);

-- Commande 6 => lignes 21..24
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(21, 99, 6, 6),
(22, 99, 6, 6),
(23, 99, 6, 6),
(24, 99, 6, 6);

-- Commande 7 => lignes 25..28
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(25, 99, 7, 7),
(26, 99, 7, 7),
(27, 99, 7, 7),
(28, 99, 7, 7);

-- Commande 8 => lignes 29..32
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(29, 99, 8, 8),
(30, 99, 8, 8),
(31, 99, 8, 8),
(32, 99, 8, 8);

-- Commande 9 => lignes 33..36
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(33, 99, 9, 9),
(34, 99, 9, 9),
(35, 99, 9, 9),
(36, 99, 9, 9);

-- Commande 10 => lignes 37..40
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(37, 99, 10, 10),
(38, 99, 10, 10),
(39, 99, 10, 10),
(40, 99, 10, 10);

-- Commande 11 => lignes 41..44
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(41, 99, 11, 11),
(42, 99, 11, 11),
(43, 99, 11, 11),
(44, 99, 11, 11);

-- Commande 12 => lignes 45..48
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(45, 99, 12, 12),
(46, 99, 12, 12),
(47, 99, 12, 12),
(48, 99, 12, 12);

-- Commande 13 => lignes 49..52
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(49, 99, 13, 13),
(50, 99, 13, 13),
(51, 99, 13, 13),
(52, 99, 13, 13);

-- Commande 14 => lignes 53..56
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(53, 99, 14, 14),
(54, 99, 14, 14),
(55, 99, 14, 14),
(56, 99, 14, 14);

-- Commande 15 => lignes 57..60
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(57, 99, 15, 15),
(58, 99, 15, 15),
(59, 99, 15, 15),
(60, 99, 15, 15);

-- Commande 16 => lignes 61..64
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(61, 99, 16, 16),
(62, 99, 16, 16),
(63, 99, 16, 16),
(64, 99, 16, 16);

-- Commande 17 => lignes 65..68
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(65, 99, 17, 17),
(66, 99, 17, 17),
(67, 99, 17, 17),
(68, 99, 17, 17);

-- Commande 18 => lignes 69..72
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(69, 99, 18, 18),
(70, 99, 18, 18),
(71, 99, 18, 18),
(72, 99, 18, 18);

-- Commande 19 => lignes 73..76
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(73, 99, 19, 19),
(74, 99, 19, 19),
(75, 99, 19, 19),
(76, 99, 19, 19);

-- Commande 20 => lignes 77..80
INSERT INTO Ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande) VALUES
(77, 99, 20, 20),
(78, 99, 20, 20),
(79, 99, 20, 20),
(80, 99, 20, 20);

-- lister les plats
SELECT * FROM plats; 

-- lister les ingrédients avec leurs prix au kg
SELECT nom_ingr, prix_kg FROM ingrédients;

-- Trouver les plats inclus dans une ligne de commande spécifique
SELECT p.nom 
FROM plat p 
JOIN inclue i ON p.id = i.id 
WHERE i.id_ligne_de_commande = 5;
