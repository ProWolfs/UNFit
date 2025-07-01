using System;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;

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
public static class Validador
{
    // Verifica si un email es válido usando una expresión regular
    public static bool EsEmailValido(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    // Verifica si la cédula contiene solo números y al menos 10 dígitos
    public static bool EsCedulaValida(string cedula)
    {
        return !string.IsNullOrEmpty(cedula) && cedula.All(char.IsDigit) && cedula.Length >= 10;
    }

    // Verifica si una fecha es válida (formato flexible)
    public static bool EsFechaValida(string fecha)
    {
        return DateTime.TryParse(fecha, out _);
    }

    // Verifica que un campo no esté vacío o con espacios
    public static bool NoVacio(string texto)
    {
        return !string.IsNullOrWhiteSpace(texto);
    }

    // Verifica si un teléfono contiene solo dígitos y al menos 10 caracteres
    public static bool EsTelefonoValido(string telefono)
    {
        return telefono.All(char.IsDigit) && telefono.Length >= 10;
    }
}
