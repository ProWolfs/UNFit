using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using Microsoft.VisualBasic;

namespace UNFit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); // Inicializa todos los controles del formulario principal
        }

        // Evento que se ejecuta al hacer clic en el botón de administrador
        private void btnAdmin_Click(object sender, EventArgs e)
        {
            // Se solicita una clave al usuario mediante un cuadro de entrada (InputBox)
            string clave = Interaction.InputBox("Ingrese la clave de administrador:", "Acceso restringido", "", -1, -1);

            if (string.IsNullOrWhiteSpace(clave))
            {
                // Si no se ingresó ninguna clave, se muestra una advertencia y se detiene la ejecución
                MessageBox.Show("Debe ingresar una clave.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cadena de conexión a la base de datos SQLite
            string connectionString = "Data Source=UNFitdb.db;Version=3;";

            // Se abre la conexión a la base de datos
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open(); // Abre la conexión

                    // Consulta SQL para verificar si la clave ingresada existe en la tabla ADMIN
                    string query = "SELECT COUNT(*) FROM ADMIN WHERE clave = @clave";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        // Se asigna el valor de la clave al parámetro de la consulta
                        cmd.Parameters.AddWithValue("@clave", clave);

                        // Ejecuta la consulta y obtiene el número de coincidencias
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            // Si la clave es válida, se abre el formulario de administración
                            FormAdmin admin = new FormAdmin();
                            admin.ShowDialog();
                        }
                        else
                        {
                            // Si la clave es incorrecta, se muestra un mensaje de error
                            MessageBox.Show("Clave incorrecta. Acceso denegado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Si hay un error de conexión o ejecución, se muestra un mensaje con el error
                    MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Evento que abre el formulario de ingreso de socios
        private void btnIngresar_Click_1(object sender, EventArgs e)
        {
            FormIngreso ingreso = new FormIngreso();
            ingreso.ShowDialog(); // Muestra el formulario de ingreso 
        }

        // Evento que abre el formulario de registro de nuevos socios
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            FormRegistro registro = new FormRegistro();
            registro.ShowDialog(); // Muestra el formulario de registro 
        }

        // Evento que abre el formulario para buscar un socio existente
        private void btnSocio_Click_1(object sender, EventArgs e)
        {
            FormBuscarSocio buscarSocio = new FormBuscarSocio();
            buscarSocio.ShowDialog(); // Muestra el formulario de búsqueda
        }
    }
}
