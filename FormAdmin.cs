using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace UNFit
{
    public partial class FormAdmin : Form
    {
        public FormAdmin()
        {
            InitializeComponent();
        }

        // Evento para registrar un nuevo monitor
        private void btnRegistrarMonitor_Click(object sender, EventArgs e)
        {
            // Validaciones antes de registrar el monitor
            if (!Validador.NoVacio(txtNombre.Text) || !Validador.NoVacio(txtApellidos.Text))
            {
                MessageBox.Show("Ingrese nombre y apellidos del monitor.");
                return;
            }

            if (!Validador.EsTelefonoValido(txtTelefono.Text))
            {
                MessageBox.Show("Número de teléfono inválido.");
                return;
            }

            if (!Validador.EsEmailValido(txtEmail.Text))
            {
                MessageBox.Show("Correo electrónico inválido.");
                return;
            }

            using (var conn = Conexion.Conectar())
            {
                conn.Open();
                // SQL para insertar un nuevo monitor
                string insertMonitor = @"
                    INSERT INTO MONITOR (Nombre, Apellidos, Telefono, Email)
                    VALUES (@nombre, @apellidos, @telefono, @email);";

                using (var cmd = new SQLiteCommand(insertMonitor, conn))
                {
                    // Asignar valores desde los campos de texto
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@apellidos", txtApellidos.Text.Trim());
                    cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Monitor registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCamposMonitor(); // Limpia los campos después del registro
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al registrar el monitor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                cmbMonitor.Items.Clear();
                using (var cmd = new SQLiteCommand("SELECT Id_monitor, Nombre FROM MONITOR", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        cmbMonitor.Items.Add(new ComboBoxItem(reader["Nombre"].ToString(), reader["Id_monitor"].ToString()));
                }
            }


        }

        // Evento para registrar una nueva actividad
        private void btnRegistrarActividad_Click(object sender, EventArgs e)
        {
            if (cmbMonitor.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un monitor válido para la actividad.", "Campos faltantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = Conexion.Conectar())
            {
                conn.Open();
                var idMonitor = ((ComboBoxItem)cmbMonitor.SelectedItem).Value;

                // SQL para insertar una nueva actividad
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
                        MessageBox.Show("Actividad registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCamposActividad(); // Limpia los campos después del registro
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al registrar la actividad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Cierra el formulario
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        // Cargar los monitores en el ComboBox al cargar el formulario
        private void FormAdmin_Load(object sender, EventArgs e)
        {
            using (var conn = Conexion.Conectar())
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT Id_monitor, Nombre FROM MONITOR", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cmbMonitor.Items.Add(new ComboBoxItem(reader["Nombre"].ToString(), reader["Id_monitor"].ToString()));
                    }
                }
            }
        }

        //  Función para limpiar campos de monitor
        private void LimpiarCamposMonitor()
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            txtApellido.Clear();
            txtEmail.Clear();
        }

        //  Función para limpiar campos de actividad
        private void LimpiarCamposActividad()
        {
            txtNombreActividad.Clear();
            txtDescripcion.Clear();
            txtCupo.Clear();
            txtHorario.Clear();
            cmbMonitor.SelectedIndex = -1;
        }
    }
}
