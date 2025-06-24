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
    public partial class FormAdmin : Form
    {
        public FormAdmin()
        {
            InitializeComponent();
        }

        private void btnRegistrarMonitor_Click(object sender, EventArgs e)
        {
            using (var conn= Conexion.Conectar())
            {
                conn.Open();
                string insertMonitor = @"
                    INSERT INTO MONITOR (Nombre, Apellidos, Telefono, Email)
                    VALUES (@nombre, @apellidos, @telefono, @email);";
                using (var cmd = new SQLiteCommand(insertMonitor, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@apellidos", txtApellidos.Text.Trim());
                    cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Monitor registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al registrar el monitor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnRegistrarActividad_Click(object sender, EventArgs e)
        {
            if (cmbMonitor.SelectedItem == null) 
            {
                MessageBox.Show("Seleccione un monitor valido para la actividad.", "Campos faltantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var conn = Conexion.Conectar())
            {
                conn.Open();
                var idMonitor = ((ComboBoxItem)cmbMonitor.SelectedItem).Value;
                string insertActividad = @"
                    INSERT INTO ACTIVIDAD (Nombre, Descripcion, Cupo_maximo, Horario, Id_monitor)
                    VALUES (@nombre, @descripcion, @cupomaximo, @horario, @monitor);";
                using (var cmd = new SQLiteCommand(insertActividad, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", txtNombreActividad.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());
                    cmd.Parameters.AddWithValue("@cupomaximo", txtCupo.Text.Trim());
                    cmd.Parameters.AddWithValue("@horario", txtHorario.Text.Trim());
                    cmd.Parameters.AddWithValue("@monitor", idMonitor);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Actividad registrada exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al registrar la actividad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            using (var conn = Conexion.Conectar())
            {
                conn.Open();

                using(var cmd= new SQLiteCommand("SELECT Id_monitor, nombre FROM MONITOR", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        cmbMonitor.Items.Add(new ComboBoxItem(reader["Nombre"].ToString(), reader["Id_monitor"].ToString()));

            }

        }
    }
}
