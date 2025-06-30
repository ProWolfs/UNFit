using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNFit
{
    public partial class FormBuscarSocio : Form
    {
        public FormBuscarSocio()
        {
            InitializeComponent();
        }

        // Evento que se ejecuta cuando se carga el formulario
        private void FormBuscarSocio_Load(object sender, EventArgs e)
        {
            // Actualiza los estados de los socios (Activo/Inactivo) según la fecha actual
            Utilidades.ActualizarEstadosSocios();
        }

        // Evento que se ejecuta al hacer clic en el botón "Buscar"
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Obtener la cédula ingresada por el usuario
            string cedula = txtCedula.Text.Trim();

            // Validar si el campo está vacío
            if (string.IsNullOrEmpty(cedula))
            {
                MessageBox.Show("Por favor ingrese la cédula del socio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Establecer la conexión con la base de datos
            using (var conn = Conexion.Conectar())
            {
                conn.Open();

                // Consulta SQL para obtener los datos del socio por cédula
                string query = @"SELECT 
                            Id_socio,
                            Nombre,
                            Apellidos,
                            Cedula,
                            Telefono,
                            Email,
                            Fecha_inicio,
                            Fecha_fin,
                            Estado_suscripcion,
                            Id_tipo_suscripcion,
                            Id_actividad
                        FROM SOCIO
                        WHERE Cedula = @cedula;";

                // Ejecutar la consulta con el parámetro @cedula
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cedula", cedula);

                    // Llenar los datos obtenidos en un DataTable
                    var adapter = new SQLiteDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    // Validar si se encontró algún resultado
                    if (dt.Rows.Count == 0)
                    {
                        // Mostrar mensaje si no se encuentra el socio
                        MessageBox.Show("Socio no encontrado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.DataSource = null;
                    }
                    else
                    {
                        // Mostrar los datos del socio en el DataGridView
                        dataGridView1.DataSource = dt;
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }

            // Limpiar el campo de cédula después de la búsqueda
            txtCedula.Clear();
        }

        // Evento para regresar al formulario anterior (oculta el formulario actual)
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
