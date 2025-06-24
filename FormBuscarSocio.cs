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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string cedula = txtCedula.Text.Trim();

            if (string.IsNullOrEmpty(cedula))
            {
                MessageBox.Show("Por favor ingrese la cédula del socio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = Conexion.Conectar())
            {
                conn.Open();

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

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cedula", cedula);

                    var adapter = new SQLiteDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Socio no encontrado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.DataSource = null;
                    }
                    else
                    {
                        dataGridView1.DataSource = dt;
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }

            txtCedula.Clear();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Hide();

        }

        private void FormBuscarSocio_Load(object sender, EventArgs e)
        {
            Utilidades.ActualizarEstadosSocios();
        }
    }
}
