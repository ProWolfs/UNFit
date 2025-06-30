using System.Data.SQLite;

// Clase estática para manejar la conexión a la base de datos SQLite
public static class Conexion
{
    // Cadena de conexión a la base de datos local UNFitdb.db
    private static readonly string cadena = "Data Source=UNFitdb.db;Version=3;";

    // Método público para obtener una nueva conexión a la base de datos
    public static SQLiteConnection Conectar()
    {
        // Retorna una nueva instancia de conexión usando la cadena
        return new SQLiteConnection(cadena);
    }
}
// Clase estática que contiene métodos utilitarios globales
public static class Utilidades
{
    // Método que actualiza el estado de todos los socios según la fecha actual
    public static void ActualizarEstadosSocios()
    {
        // Abre una conexión a la base de datos
        using (var conn = Conexion.Conectar())
        {
            conn.Open();

            // Consulta SQL que actualiza el estado de suscripción
            string query = @"
                UPDATE SOCIO
                SET Estado_suscripcion = CASE
                    WHEN Fecha_fin >= date('now') THEN 'Activo'
                    ELSE 'Inactivo'
                END";

            // Ejecuta la consulta de actualización
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.ExecuteNonQuery(); // Ejecuta sin esperar resultados (UPDATE)
            }
        }
    }
}
