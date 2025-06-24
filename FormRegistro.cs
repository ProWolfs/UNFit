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
            InitializeComponent();
        }
        private void FormRegistro_Load(object sender, EventArgs e)
        
        {
            using (var conn = Conexion.Conectar())
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT Id_actividad, Nombre FROM ACTIVIDAD", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        cmbActividad.Items.Add(new ComboBoxItem(reader["Nombre"].ToString(), reader["Id_actividad"].ToString()));

                using (var cmd = new SQLiteCommand("SELECT Id_tipo_suscripcion, Nombre FROM Tipo_suscripcion", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        cmbSuscripcion.Items.Add(new ComboBoxItem(reader["Nombre"].ToString(), reader["Id_tipo_suscripcion"].ToString()));

                using (var cmd = new SQLiteCommand("SELECT Id_tipo_pago, Nombre FROM Tipo_pago", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        cmbPago.Items.Add(new ComboBoxItem(reader["Nombre"].ToString(), reader["Id_tipo_pago"].ToString()));
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbActividad.SelectedItem == null || cmbSuscripcion.SelectedItem == null || cmbPago.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una suscripción, actividad y tipo de pago.", "Campos faltantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = Conexion.Conectar())
            {
                conn.Open();

                var idSuscripcion = ((ComboBoxItem)cmbSuscripcion.SelectedItem).Value;
                var idActividad = ((ComboBoxItem)cmbActividad.SelectedItem).Value;
                var idTipoPago = ((ComboBoxItem)cmbPago.SelectedItem).Value;

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

                    cmd.ExecuteNonQuery();
                }

                long idSocio = conn.LastInsertRowId;
                double valorPago = 0;
                using (var cmdValor = new SQLiteCommand("SELECT Valor FROM Tipo_suscripcion WHERE Id_tipo_suscripcion = @id", conn))
                {
                    cmdValor.Parameters.AddWithValue("@id", idSuscripcion);
                    valorPago = Convert.ToDouble(cmdValor.ExecuteScalar());
                }

                string insertPago = @"
        INSERT INTO PAGO (Id_socio, Fecha_pago, Valor, Id_tipo_pago)
        VALUES (@IdSocio, date('now'), @Valor, @IdTipoPago)";

                using (var cmdPago = new SQLiteCommand(insertPago, conn))
                {
                    cmdPago.Parameters.AddWithValue("@IdSocio", idSocio);
                    cmdPago.Parameters.AddWithValue("@Valor", valorPago);
                    cmdPago.Parameters.AddWithValue("@IdTipoPago", idTipoPago);
                    cmdPago.ExecuteNonQuery();
                }

                MessageBox.Show("Socio registrado correctamente.");
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public ComboBoxItem(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }

}

