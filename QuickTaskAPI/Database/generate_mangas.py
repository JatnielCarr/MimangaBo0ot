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

# Lista de géneros de manga
generos = [
    "Acción", "Aventura", "Comedia", "Drama", "Fantasía", "Horror",
    "Misterio", "Romance", "Ciencia Ficción", "Slice of Life", "Deportes",
    "Psicológico", "Sobrenatural", "Mecha", "Histórico", "Musical"
]

# Lista de autores famosos de manga
autores = [
    "Eiichiro Oda", "Hirohiko Araki", "Tite Kubo", "Masashi Kishimoto",
    "Hiro Mashima", "Yoshihiro Togashi", "Kentaro Miura", "Naoki Urasawa",
    "Takehiko Inoue", "Akira Toriyama", "Osamu Tezuka", "Rumiko Takahashi",
    "CLAMP", "Junji Ito", "Makoto Yukimura", "Yuki Tabata", "Gege Akutami",
    "Koyoharu Gotouge", "Tatsuki Fujimoto", "Yusei Matsui"
]

def generar_titulo_unico(titulos_existentes):
    """Genera un título único para el manga"""
    while True:
        # Generar un título con estructura variada
        estructuras = [
            f"{fake.word().title()} no {fake.word().title()}",
            f"The {fake.word().title()} {fake.word().title()}",
            f"{fake.word().title()} {fake.word().title()}",
            f"{fake.word().title()}: {fake.word().title()}",
            f"{fake.word().title()} - {fake.word().title()}"
        ]
        titulo = random.choice(estructuras)
        if titulo not in titulos_existentes:
            return titulo

def generar_manga(titulos_existentes):
    """Genera un manga con datos aleatorios"""
    titulo = generar_titulo_unico(titulos_existentes)
    titulos_existentes.add(titulo)
    
    # Generar fecha de publicación entre 1990 y 2024
    fecha_publicacion = fake.date_between(start_date='-34y', end_date='today')
    
    # Generar número de volúmenes entre 1 y 100
    volumenes = random.randint(1, 100)
    
    # 70% de probabilidad de que esté en curso
    en_curso = random.random() < 0.7
    
    # Seleccionar 1-3 géneros aleatorios
    num_generos = random.randint(1, 3)
    generos_seleccionados = random.sample(generos, num_generos)
    
    return {
        'titulo': titulo,
        'autor': random.choice(autores),
        'genero': ', '.join(generos_seleccionados),
        'fecha_publicacion': fecha_publicacion,
        'volumenes': volumenes,
        'en_curso': en_curso
    }

def main():
    print("Iniciando generación de mangas...")
    
    # Conectar a la base de datos
    conn = mysql.connector.connect(**db_config)
    cursor = conn.cursor()
    
    # Obtener títulos existentes
    cursor.execute("SELECT Title FROM Mangas")
    titulos_existentes = {row[0] for row in cursor.fetchall()}
    
    # Generar e insertar mangas en lotes
    batch_size = 100
    total_mangas = 2500
    mangas_generados = 0
    
    while mangas_generados < total_mangas:
        batch = []
        for _ in range(min(batch_size, total_mangas - mangas_generados)):
            manga = generar_manga(titulos_existentes)
            batch.append((
                manga['titulo'],
                manga['autor'],
                manga['genero'],
                manga['fecha_publicacion'],
                manga['volumenes'],
                manga['en_curso']
            ))
        
        # Insertar lote
        query = """
        INSERT INTO Mangas (Title, Author, Genre, PublicationDate, Volumes, IsOngoing)
        VALUES (%s, %s, %s, %s, %s, %s)
        """
        cursor.executemany(query, batch)
        conn.commit()
        
        mangas_generados += len(batch)
        print(f"Progreso: {mangas_generados}/{total_mangas} mangas generados")
    
    print("¡Generación de mangas completada!")
    
    # Cerrar conexión
    cursor.close()
    conn.close()
    print("Conexión a la base de datos cerrada.")

if __name__ == "__main__":
    main() 