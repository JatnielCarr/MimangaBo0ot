-- Crear la base de datos si no existe
CREATE DATABASE IF NOT EXISTS JatnielCarr24;

-- Usar la base de datos
USE JatnielCarr24;

-- Crear la tabla Mangas
CREATE TABLE IF NOT EXISTS Mangas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Author VARCHAR(255) NOT NULL,
    Genre VARCHAR(100),
    PublicationDate DATETIME NOT NULL,
    Volumes INT NOT NULL,
    IsOngoing BOOLEAN NOT NULL
);

-- Insertar algunos datos de ejemplo
INSERT INTO Mangas (Title, Author, Genre, PublicationDate, Volumes, IsOngoing)
VALUES 
    ('Naruto', 'Masashi Kishimoto', 'Shonen', '1999-09-21', 72, false),
    ('One Piece', 'Eiichiro Oda', 'Shonen', '1997-07-22', 100, true); 