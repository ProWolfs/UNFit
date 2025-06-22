using System.Data.SQLite;

public static class Conexion
{
    private static readonly string cadena = "Data Source=UNFitdb.db;Version=3;";

    public static SQLiteConnection Conectar()
    {
        return new SQLiteConnection(cadena);
    }
}