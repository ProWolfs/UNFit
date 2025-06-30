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
    public partial class FormRegistro : Form
    {
        public FormRegistro()
        {
            InitializeComponent(); // Inicializa el formulario y sus componentes
        }

        // Evento que se ejecuta al cargar el formulario
        private void FormRegistro_Load(object sender, EventArgs e)
        {
            using (var conn = Conexion.Conectar())
            {
                conn.Open();

                // Cargar actividades disponibles en el ComboBox
                using (var cmd = new SQLiteCommand("SELECT Id_actividad, Nombre FROM ACTIVIDAD", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        cmbActividad.Items.Add(new ComboBoxItem(reader["Nombre"].ToString(), reader["Id_actividad"].ToString()));

                // Cargar tipos de suscripción
                using (var cmd = new SQLiteCommand("SELECT Id_tipo_suscripcion, Nombre FROM Tipo_suscripcion", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        cmbSuscripcion.Items.Add(new ComboBoxItem(reader["Nombre"].ToString(), reader["Id_tipo_suscripcion"].ToString()));

                // Cargar tipos de pago
                using (var cmd = new SQLiteCommand("SELECT Id_tipo_pago, Nombre FROM Tipo_pago", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        cmbPago.Items.Add(new ComboBoxItem(reader["Nombre"].ToString(), reader["Id_tipo_pago"].ToString()));
            }
        }

        // Evento que se ejecuta al hacer clic en el botón "Registrar"
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Validar que se hayan seleccionado todos los campos requeridos
            if (cmbActividad.SelectedItem == null || cmbSuscripcion.SelectedItem == null || cmbPago.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una suscripción, actividad y tipo de pago.", "Campos faltantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = Conexion.Conectar())
            {
                conn.Open();

                // Obtener los valores seleccionados de los ComboBox
                var idSuscripcion = ((ComboBoxItem)cmbSuscripcion.SelectedItem).Value;
                var idActividad = ((ComboBoxItem)cmbActividad.SelectedItem).Value;
                var idTipoPago = ((ComboBoxItem)cmbPago.SelectedItem).Value;

                // Insertar un nuevo socio
                string insertSocio = @"
        INSERT INTO SOCIO (
            Nombre, Apellidos, Cedula, Fecha_nacimiento,
            Telefono, Email, Fecha_inicio, Fecha_fin,
            Estado_suscripcion, Id_tipo_suscripcion, Id_actividad
        )
        VALUES (
            @Nombre, @Apellidos, @Cedula, @FechaNacimiento,
            @Telefono, @Email,
            date('now'),
            date('now', (
                SELECT '+' || Duracion_dias || ' days'
                FROM Tipo_suscripcion
                WHERE Id_tipo_suscripcion = @IdSuscripcion
            )),
            'Activo', @IdSuscripcion, @IdActividad
        )";

                using (var cmd = new SQLiteCommand(insertSocio, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Apellidos", txtApellidos.Text.Trim());
                    cmd.Parameters.AddWithValue("@Cedula", txtCedula.Text.Trim());
                    cmd.Parameters.AddWithValue("@FechaNacimiento", txtNacimiento.Text.Trim());
                    cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@IdSuscripcion", idSuscripcion);
                    cmd.Parameters.AddWithValue("@IdActividad", idActividad);

                    cmd.ExecuteNonQuery(); // Ejecuta el insert
                }

                // Obtener el ID del socio recién insertado
                long idSocio = conn.LastInsertRowId;

                // Obtener el valor del pago correspondiente al tipo de suscripción
                double valorPago = 0;
                using (var cmdValor = new SQLiteCommand("SELECT Valor FROM Tipo_suscripcion WHERE Id_tipo_suscripcion = @id", conn))
                {
                    cmdValor.Parameters.AddWithValue("@id", idSuscripcion);
                    valorPago = Convert.ToDouble(cmdValor.ExecuteScalar());
                }

                // Insertar el pago inicial del socio
                string insertPago = @"
        INSERT INTO PAGO (Id_socio, Fecha_pago, Valor, Id_tipo_pago)
        VALUES (@IdSocio, date('now'), @Valor, @IdTipoPago)";

                using (var cmdPago = new SQLiteCommand(insertPago, conn))
                {
                    cmdPago.Parameters.AddWithValue("@IdSocio", idSocio);
                    cmdPago.Parameters.AddWithValue("@Valor", valorPago);
                    cmdPago.Parameters.AddWithValue("@IdTipoPago", idTipoPago);
                    cmdPago.ExecuteNonQuery(); // Ejecuta el insert
                }

                MessageBox.Show("Socio registrado correctamente.");
            }
            // Limpia los campos del formulario después de registrar
            txtNombre.Clear();
            txtApellidos.Clear();
            txtCedula.Clear();
            txtNacimiento.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            cmbActividad.SelectedIndex = -1; // Resetea el ComboBox de actividades
            cmbSuscripcion.SelectedIndex = -1; // Resetea el ComboBox de suscripciones
            cmbPago.SelectedIndex = -1; // Resetea el ComboBox de tipos de pago

        }

        // Botón para regresar o cerrar el formulario
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Hide(); // Oculta el formulario
        }
    }

    // Clase auxiliar para manejar ítems personalizados en ComboBox
    public class ComboBoxItem
    {
        public string Text { get; set; }   // Nombre que se muestra
        public string Value { get; set; }  // Valor que se usa en la base de datos

        public ComboBoxItem(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text; // Esto permite que se muestre el texto en el ComboBox
        }
    }
}
