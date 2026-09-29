-- =========================================================
-- Modèle Logique de Données (MLD) - E-commerce ChemiseLab
-- Converti de MySQL vers PostgreSQL
--   - TAILLE : id_taille, libelle_sport, libelle_FR, libelle_USA
--   - Table CLIENT supprimée, infos client intégrées dans ORDERS
--   - AUTO_INCREMENT -> SERIAL
--   - DATETIME -> TIMESTAMP
--   - `ORDER` (mot réservé) -> renommé en ORDERS
-- =========================================================

CREATE TABLE categorie (
    id_categorie    SERIAL          PRIMARY KEY,
    libelle         VARCHAR(100)    NOT NULL
);

CREATE TABLE sous_categorie (
    id_sous_categorie   SERIAL          PRIMARY KEY,
    libelle              VARCHAR(100)    NOT NULL,
    id_categorie         INT             NOT NULL,
    FOREIGN KEY (id_categorie) REFERENCES categorie(id_categorie)
);

CREATE TABLE produits (
    id_produit           SERIAL          PRIMARY KEY,
    libelle               VARCHAR(150)    NOT NULL,
    description           TEXT,
    prix                  DECIMAL(10,2)   NOT NULL,
    reference             VARCHAR(50)     NOT NULL,
    id_sous_categorie     INT             NOT NULL,
    FOREIGN KEY (id_sous_categorie) REFERENCES sous_categorie(id_sous_categorie)
);

CREATE TABLE image (
    id_image     SERIAL          PRIMARY KEY,
    url_image    VARCHAR(255)    NOT NULL,
    id_produit   INT             NOT NULL,
    FOREIGN KEY (id_produit) REFERENCES produits(id_produit)
);

CREATE TABLE taille (
    id_taille       SERIAL          PRIMARY KEY,
    libelle_sport   VARCHAR(50),
    libelle_fr      VARCHAR(50),
    libelle_usa     VARCHAR(50)
);

CREATE TABLE couleur (
    id_couleur       SERIAL          PRIMARY KEY,
    libelle_couleur  VARCHAR(50)     NOT NULL
);

-- Relation ternaire STOCK (PRODUIT x TAILLE x COULEUR)
CREATE TABLE stock (
    id_produit  INT NOT NULL,
    id_taille   INT NOT NULL,
    id_couleur  INT NOT NULL,
    stock       INT NOT NULL DEFAULT 0,
    PRIMARY KEY (id_produit, id_taille, id_couleur),
    FOREIGN KEY (id_produit) REFERENCES produits(id_produit),
    FOREIGN KEY (id_taille)  REFERENCES taille(id_taille),
    FOREIGN KEY (id_couleur) REFERENCES couleur(id_couleur)
);

-- Table ORDERS : contient les infos client (table CLIENT supprimée)
-- Renommée depuis `ORDER` car c'est un mot réservé en PostgreSQL
CREATE TABLE orders (
    id_order              SERIAL          PRIMARY KEY,
    date_order             TIMESTAMP       NOT NULL,
    statut_order            VARCHAR(50)     NOT NULL,
    total_order              DECIMAL(10,2)   NOT NULL,
    reference_livraison    VARCHAR(50),
    nom_client              VARCHAR(100)    NOT NULL,
    prenom_client           VARCHAR(100)    NOT NULL,
    adresse_client          VARCHAR(255)    NOT NULL,
    telephone_client        VARCHAR(20),
    pays_client              VARCHAR(100),
    ville_client             VARCHAR(100)
);

CREATE TABLE ligne_order (
    id_ligne_order  SERIAL          PRIMARY KEY,
    quantite         INT             NOT NULL,
    prix_unitaire    DECIMAL(10,2)   NOT NULL,
    id_order          INT             NOT NULL,
    id_produit        INT             NOT NULL,
    id_taille          INT             NOT NULL,
    id_couleur          INT             NOT NULL,
    FOREIGN KEY (id_order)   REFERENCES orders(id_order),
    FOREIGN KEY (id_produit) REFERENCES produits(id_produit),
    FOREIGN KEY (id_taille)  REFERENCES taille(id_taille),
    FOREIGN KEY (id_couleur) REFERENCES couleur(id_couleur)
);

-- =========================================================
-- Index utiles pour les performances (FK non indexées par défaut en PG)
-- =========================================================
CREATE INDEX idx_sous_categorie_categorie ON sous_categorie(id_categorie);
CREATE INDEX idx_produit_sous_categorie ON produits(id_sous_categorie);
CREATE INDEX idx_image_produit ON image(id_produit);
CREATE INDEX idx_stock_produit ON stock(id_produit);
CREATE INDEX idx_stock_taille ON stock(id_taille);
CREATE INDEX idx_stock_couleur ON stock(id_couleur);
CREATE INDEX idx_ligne_order_order ON ligne_order(id_order);
CREATE INDEX idx_ligne_order_produit ON ligne_order(id_produit);
CREATE INDEX idx_ligne_order_taille ON ligne_order(id_taille);
CREATE INDEX idx_ligne_order_couleur ON ligne_order(id_couleur);
