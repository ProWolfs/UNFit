using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace UNFit
{
    public partial class FormRenovar : Form
    {
        private int idSocio; // Guarda el ID del socio que se está renovando

        // Constructor: recibe el ID del socio y lo guarda
        public FormRenovar(int socioId)
        {
            InitializeComponent();
            idSocio = socioId;
        }

        // Evento que se ejecuta al cargar el formulario
        private void FormRenovar_Load(object sender, EventArgs e)
        {
            using (var conn = Conexion.Conectar())
            {
                conn.Open();

                // Cargar los tipos de suscripción en el comboBox cmbSuscripcion
                string querySus = "SELECT Id_tipo_suscripcion, Nombre FROM Tipo_suscripcion";
                using (var cmd = new SQLiteCommand(querySus, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    cmbSuscripcion.DataSource = dt;
                    cmbSuscripcion.DisplayMember = "Nombre";         // Se mostrará el nombre
                    cmbSuscripcion.ValueMember = "Id_tipo_suscripcion"; // Pero se usará el ID internamente
                }

                // Cargar las actividades disponibles en el comboBox cmbActividades
                string queryAct = "SELECT Id_actividad, Nombre FROM ACTIVIDAD";
                using (var cmd = new SQLiteCommand(queryAct, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt2 = new DataTable();
                    dt2.Load(reader);
                    cmbActividades.DataSource = dt2;
                    cmbActividades.DisplayMember = "Nombre";         // Se muestra el nombre
                    cmbActividades.ValueMember = "Id_actividad";     // Se usa el ID internamente
                }
            }
        }

        // Evento que se ejecuta al hacer clic en el botón Cancelar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra el formulario sin hacer cambios
        }

        // Evento que se ejecuta al hacer clic en el botón Confirmar renovación
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            // Se obtienen los valores seleccionados en los combos
            int idSuscripcion = Convert.ToInt32(cmbSuscripcion.SelectedValue);
            int idActividad = Convert.ToInt32(cmbActividades.SelectedValue);

            using (var conn = Conexion.Conectar())
            {
                conn.Open();

                // Obtener duración en días y valor del tipo de suscripción seleccionado
                string getDatos = @"SELECT Duracion_dias, Valor FROM Tipo_suscripcion WHERE Id_tipo_suscripcion = @id";
                int duracion = 0;
                double valor = 0;

                using (var cmd = new SQLiteCommand(getDatos, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idSuscripcion);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            duracion = Convert.ToInt32(reader["Duracion_dias"]);
                            valor = Convert.ToDouble(reader["Valor"]);
                        }
                    }
                }

                // Se calculan las nuevas fechas de inicio y fin de la suscripción
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = fechaInicio.AddDays(duracion);

                // Actualiza los datos del socio: fecha de inicio, fin, estado, tipo de suscripción y actividad
                string updateSocio = @"UPDATE SOCIO SET 
                     Fecha_inicio = @inicio,
                     Fecha_fin = @fin,
                     Estado_suscripcion = 'Activo',
                     Id_tipo_suscripcion = @idSuscripcion,
                     Id_actividad = @idActividad
                     WHERE Id_socio = @idSocio";

                using (var cmd = new SQLiteCommand(updateSocio, conn))
                {
                    cmd.Parameters.AddWithValue("@inicio", fechaInicio.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@fin", fechaFin.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@idSuscripcion", idSuscripcion);
                    cmd.Parameters.AddWithValue("@idActividad", idActividad);
                    cmd.Parameters.AddWithValue("@idSocio", idSocio);
                    cmd.ExecuteNonQuery(); // Ejecuta el UPDATE
                }

                // Registra un nuevo pago en la tabla PAGO (tipo de pago = 1 por defecto)
                string insertPago = @"INSERT INTO PAGO (Id_socio, Fecha_pago, Valor, Id_tipo_pago) 
                                      VALUES (@id, @fecha, @valor, 1);";

                using (var cmd = new SQLiteCommand(insertPago, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idSocio);
                    cmd.Parameters.AddWithValue("@fecha", fechaInicio.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@valor", valor);
                    cmd.ExecuteNonQuery(); // Ejecuta el INSERT
                }

                // Muestra mensaje de confirmación
                MessageBox.Show("Suscripción renovada con éxito.", "Renovación completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Cierra el formulario
            }
        }

        // Estos dos métodos quedaron vacíos porque ya no se muestran datos en etiquetas
        private void cmbSuscripcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ya no se muestra información adicional del plan
        }

        private void cmbActividades_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ya no se muestra información adicional de la actividad
        }
    }
}
