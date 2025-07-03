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
            InitializeComponent(); // Inicializa los componentes del formulario
        }

        private void FormIngreso_Load(object sender, EventArgs e)
        {
            Utilidades.ActualizarEstadosSocios(); // Actualiza automáticamente el estado (Activo/Inactivo) de los socios al cargar el formulario
        }

        private void btnRegresar_Click_1(object sender, EventArgs e)
        {
            this.Close(); // Cierra el formulario actual y vuelve al form1
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string cedula = txtCedula.Text.Trim(); // Obtiene la cédula ingresada

            if (string.IsNullOrEmpty(cedula))
            {
                MessageBox.Show("Por favor ingrese la cédula del socio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = Conexion.Conectar())
            {
                conn.Open();

                // Consulta que obtiene información del socio, actividad y monitor
                string query = @"
        SELECT 
            S.Id_socio,
            S.Nombre, 
            S.Apellidos, 
            M.Nombre || ' ' || M.Apellidos AS Monitor,
            A.Nombre AS Actividad,
            A.Horario,    
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
                        lblResumenSocio.Text = ""; // Limpiar resumen si no hay resultados
                        btnRenovar.Enabled = false;
                    }
                    else
                    {
                        // Mostramos el resumen en formato legible usando StringBuilder
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"Nombre: {dt.Rows[0]["Nombre"]} {dt.Rows[0]["Apellidos"]}");
                        sb.AppendLine($"Monitor: {dt.Rows[0]["Monitor"]}");
                        sb.AppendLine($"Actividad: {dt.Rows[0]["Actividad"]}");
                        sb.AppendLine($"Horario: {dt.Rows[0]["Horario"]}");
                        sb.AppendLine($"Vencimiento: {Convert.ToDateTime(dt.Rows[0]["Fecha de Vencimiento"]).ToString("yyyy-MM-dd")}");
                        sb.AppendLine($"Estado: {dt.Rows[0]["Estado"]}");

                        lblResumenSocio.Text = sb.ToString(); // Mostrar en el Label

                        // Validar estado de suscripción
                        string estado = dt.Rows[0]["Estado"].ToString();
                        int idSocio = Convert.ToInt32(dt.Rows[0]["Id_socio"]);
                        btnRenovar.Tag = idSocio;

                        if (estado == "Activo")
                        {
                            btnRenovar.Enabled = false;

                            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
                            string hora = DateTime.Now.ToString("HH:mm:ss");

                            string insert = @"INSERT INTO ASISTENCIA (Id_socio, Fecha, Hora) VALUES (@id, @fecha, @hora);";

                            using (var asistenciaCmd = new SQLiteCommand(insert, conn))
                            {
                                asistenciaCmd.Parameters.AddWithValue("@id", idSocio);
                                asistenciaCmd.Parameters.AddWithValue("@fecha", fecha);
                                asistenciaCmd.Parameters.AddWithValue("@hora", hora);
                                asistenciaCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Asistencia registrada correctamente.", "Ingreso exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("La suscripción del socio ha vencido. No puede ingresar.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnRenovar.Enabled = true;
                        }
                    }
                }
            }

            txtCedula.Clear(); // Limpia el campo de cédula
        }

        private void btnRenovar_Click(object sender, EventArgs e)
        {
            if (btnRenovar.Tag != null) // Si se ha guardado un Id_socio
            {
                int idSocio = Convert.ToInt32(btnRenovar.Tag); // Lo recupera
                FormRenovar frm = new FormRenovar(idSocio); // Abre el formulario de renovación pasando el Id del socio
                frm.ShowDialog(); // Muestra el formulario de forma modal
                btnRenovar.Enabled = false; // Desactiva el botón tras la renovación
            }
        }
    }
}
