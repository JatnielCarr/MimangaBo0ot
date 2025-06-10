from faker import Faker
import mysql.connector
from datetime import datetime, timedelta
import random

# Configuración de la base de datos
db_config = {
    'host': 'localhost',
    'user': 'root',
    'password': 'J4flores24',
    'database': 'JatnielCarr24'
}

# Inicializar Faker
fake = Faker()

# Lista de géneros de manga comunes
genres = [
    'Shonen', 'Seinen', 'Shojo', 'Josei', 'Mecha', 'Isekai', 'Slice of Life',
    'Romance', 'Comedy', 'Drama', 'Action', 'Adventure', 'Fantasy', 'Sci-Fi',
    'Horror', 'Mystery', 'Sports', 'Supernatural', 'Psychological', 'Thriller'
]

# Lista de autores japoneses comunes
authors = [
    'Masashi Kishimoto', 'Eiichiro Oda', 'Akira Toriyama', 'Hirohiko Araki',
    'Tite Kubo', 'Hiro Mashima', 'Yoshihiro Togashi', 'Kentaro Miura',
    'Naoki Urasawa', 'Takehiko Inoue', 'Osamu Tezuka', 'Rumiko Takahashi',
    'CLAMP', 'Yuki Tabata', 'Gege Akutami', 'Koyoharu Gotouge', 'Tatsuki Fujimoto',
    'Makoto Yukimura', 'Yusuke Murata', 'Sui Ishida'
]

# Función para generar un título único de manga
def generate_unique_manga_title(existing_titles):
    while True:
        # Generar un título con una estructura común en mangas
        title_structure = random.choice([
            f"{fake.word().title()} {fake.word().title()}",
            f"The {fake.word().title()} {fake.word().title()}",
            f"{fake.word().title()} no {fake.word().title()}",
            f"{fake.word().title()} to {fake.word().title()}",
            f"{fake.word().title()}: {fake.word().title()}",
            f"{fake.word().title()} {fake.word().title()} {fake.word().title()}"
        ])
        if title_structure not in existing_titles:
            return title_structure

# Función para generar un manga
def generate_manga(existing_titles):
    title = generate_unique_manga_title(existing_titles)
    existing_titles.add(title)
    
    # Fecha de publicación entre 1970 y 2024
    start_date = datetime(1970, 1, 1)
    end_date = datetime(2024, 1, 1)
    publication_date = fake.date_time_between(start_date, end_date)
    
    return {
        'title': title,
        'author': random.choice(authors),
        'genre': random.choice(genres),
        'publication_date': publication_date,
        'volumes': random.randint(1, 100),
        'is_ongoing': random.choice([True, False])
    }

# Conectar a la base de datos
try:
    conn = mysql.connector.connect(**db_config)
    cursor = conn.cursor()

    # Obtener títulos existentes
    cursor.execute("SELECT Title FROM Mangas")
    existing_titles = {row[0] for row in cursor.fetchall()}

    # Generar y insertar 3500 mangas
    batch_size = 100
    total_mangas = 3500
    mangas_generated = 0

    print("Iniciando generación de mangas...")

    while mangas_generated < total_mangas:
        batch = []
        for _ in range(min(batch_size, total_mangas - mangas_generated)):
            manga = generate_manga(existing_titles)
            batch.append((
                manga['title'],
                manga['author'],
                manga['genre'],
                manga['publication_date'],
                manga['volumes'],
                manga['is_ongoing']
            ))

        # Insertar el lote
        insert_query = """
        INSERT INTO Mangas (Title, Author, Genre, PublicationDate, Volumes, IsOngoing)
        VALUES (%s, %s, %s, %s, %s, %s)
        """
        cursor.executemany(insert_query, batch)
        conn.commit()

        mangas_generated += len(batch)
        print(f"Progreso: {mangas_generated}/{total_mangas} mangas generados")

    print("¡Generación de mangas completada!")

except mysql.connector.Error as err:
    print(f"Error: {err}")

finally:
    if 'conn' in locals() and conn.is_connected():
        cursor.close()
        conn.close()
        print("Conexión a la base de datos cerrada.") 