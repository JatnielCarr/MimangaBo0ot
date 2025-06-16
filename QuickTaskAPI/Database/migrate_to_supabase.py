import mysql.connector
import psycopg2
from datetime import datetime

# Configuración de MySQL (origen)
mysql_config = {
    'host': 'localhost',
    'user': 'root',
    'password': 'J4flores24',
    'database': 'JatnielCarr24'
}

# Configuración de Supabase (destino)
supabase_config = {
    'host': 'db.gdmzrpstylhqurhxqocw.supabase.co',
    'port': 5432,
    'database': 'postgres',
    'user': 'postgres',
    'password': 'J@flores2006'
}

def migrate_data():
    print("Iniciando migración de datos...")
    
    # Conectar a MySQL
    mysql_conn = mysql.connector.connect(**mysql_config)
    mysql_cursor = mysql_conn.cursor(dictionary=True)
    
    # Conectar a Supabase
    supabase_conn = psycopg2.connect(**supabase_config)
    supabase_cursor = supabase_conn.cursor()
    
    try:
        # Obtener todos los mangas de MySQL
        mysql_cursor.execute("SELECT * FROM Mangas")
        mangas = mysql_cursor.fetchall()
        
        print(f"Se encontraron {len(mangas)} mangas para migrar")
        
        # Preparar la consulta de inserción para Supabase
        insert_query = """
        INSERT INTO "Mangas" ("Title", "Author", "Genre", "PublicationDate", "Volumes", "IsOngoing")
        VALUES (%s, %s, %s, %s, %s, %s)
        """
        
        # Migrar los datos en lotes
        batch_size = 100
        total_migrated = 0
        
        for i in range(0, len(mangas), batch_size):
            batch = mangas[i:i + batch_size]
            batch_data = []
            
            for manga in batch:
                batch_data.append((
                    manga['Title'],
                    manga['Author'],
                    manga['Genre'],
                    manga['PublicationDate'],
                    manga['Volumes'],
                    bool(manga['IsOngoing'])
                ))
            
            # Insertar lote en Supabase
            supabase_cursor.executemany(insert_query, batch_data)
            supabase_conn.commit()
            
            total_migrated += len(batch)
            print(f"Progreso: {total_migrated}/{len(mangas)} mangas migrados")
        
        print("¡Migración completada exitosamente!")
        
    except Exception as e:
        print(f"Error durante la migración: {str(e)}")
        supabase_conn.rollback()
    finally:
        # Cerrar conexiones
        mysql_cursor.close()
        mysql_conn.close()
        supabase_cursor.close()
        supabase_conn.close()
        print("Conexiones cerradas.")

if __name__ == "__main__":
    migrate_data() 