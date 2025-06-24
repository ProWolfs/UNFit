using System.Data.SQLite;

public static class Conexion
{
    private static readonly string cadena = "Data Source=UNFitdb.db;Version=3;";

    public static SQLiteConnection Conectar()
    {
        return new SQLiteConnection(cadena);
    }
}
public static class Utilidades
{
    public static void ActualizarEstadosSocios()
    {
        using (var conn = Conexion.Conectar())
        {
            conn.Open();

            string query = @"
                UPDATE SOCIO
                SET Estado_suscripcion = CASE
                    WHEN Fecha_fin >= date('now') THEN 'Activo'
                    ELSE 'Inactivo'
                END";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
