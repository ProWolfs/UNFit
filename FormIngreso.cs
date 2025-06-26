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
    public partial class FormIngreso : Form
    {
        public FormIngreso()
        {
            InitializeComponent();
        }

        private void FormIngreso_Load(object sender, EventArgs e)
        {
            Utilidades.ActualizarEstadosSocios();
        }

        private void btnRegresar_Click_1(object sender, EventArgs e)
        {
            this.Close();
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

                string query = @"
        SELECT 
            S.Nombre, 
            S.Apellidos, 
            M.Nombre || ' ' || M.Apellidos AS Monitor,
            S.Fecha_fin AS 'Fecha de Vencimiento',
            S.Estado_suscripcion AS 'Estado'
        FROM SOCIO S
        JOIN ACTIVIDAD A ON S.Id_actividad = A.Id_actividad
        JOIN MONITOR M ON A.Id_monitor = M.Id_monitor
        WHERE S.Cedula = @cedula;";

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
    }
}
