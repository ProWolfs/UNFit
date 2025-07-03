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
            string cedula = txtCedula.Text.Trim();

            if (string.IsNullOrEmpty(cedula))
            {
                MessageBox.Show("Por favor ingrese la cédula del socio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = Conexion.Conectar())
            {
                conn.Open();

                // Nueva consulta con JOIN para obtener actividad y tipo de suscripción
                string query = @"
                SELECT 
                    S.Nombre,
                    S.Apellidos,
                    S.Cedula,
                    S.Telefono,
                    S.Email,
                    S.Fecha_inicio,
                    S.Fecha_fin,
                    S.Estado_suscripcion,
                    TS.Nombre AS TipoSuscripcion,
                    A.Nombre AS Actividad,
                    A.Horario,
                    M.Nombre || ' ' || M.Apellidos AS Monitor,
                    (SELECT MAX(Fecha_pago)
                        FROM PAGO
                            WHERE Id_socio = S.Id_socio
                    ) AS UltimaFechaPago,
                    (
                        SELECT Valor
                        FROM PAGO
                        WHERE Id_socio = S.Id_socio
                        ORDER BY Fecha_pago DESC
                        LIMIT 1) 
                    AS UltimoValorPago
                    FROM SOCIO S
                    JOIN Tipo_suscripcion TS ON S.Id_tipo_suscripcion = TS.Id_tipo_suscripcion
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
                        lblResumenSocio.Text = "";
                    }
                    else
                    {
                        var row = dt.Rows[0];

                        // Construir resumen
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"Nombre: {row["Nombre"]} {row["Apellidos"]}");
                        sb.AppendLine($"Cédula: {row["Cedula"]}");
                        sb.AppendLine($"Teléfono: {row["Telefono"]}");
                        sb.AppendLine($"Email: {row["Email"]}");
                        sb.AppendLine($"Fecha Inicio: {Convert.ToDateTime(row["Fecha_inicio"]).ToString("yyyy-MM-dd")}");
                        sb.AppendLine($"Fecha Fin: {Convert.ToDateTime(row["Fecha_fin"]).ToString("yyyy-MM-dd")}");
                        sb.AppendLine($"Estado: {row["Estado_suscripcion"]}");
                        sb.AppendLine($"Tipo de Suscripción: {row["TipoSuscripcion"]}");
                        sb.AppendLine($"Actividad: {row["Actividad"]}");
                        sb.AppendLine($"Horario: {row["Horario"]}");
                        sb.AppendLine($"Monitor: {row["Monitor"]}");

                        if (row["UltimaFechaPago"] != DBNull.Value && row["UltimoValorPago"] != DBNull.Value)
                        {
                            sb.AppendLine($"Último pago: {Convert.ToDateTime(row["UltimaFechaPago"]).ToString("yyyy-MM-dd")} - Valor: ${Convert.ToDouble(row["UltimoValorPago"]):0.00}");
                        }
                        else
                        {
                            sb.AppendLine("Último pago: No disponible");
                        }

                        lblResumenSocio.Text = sb.ToString();
                    }

                }
            }

        }

        // Evento para regresar al formulario anterior (oculta el formulario actual)
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string cedula = txtCedula.Text.Trim();
            if (string.IsNullOrEmpty(cedula)) return;

            if (MessageBox.Show("¿Seguro que desea eliminar al socio?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var conn = Conexion.Conectar())
                {
                    conn.Open();
                    string query = "DELETE FROM SOCIO WHERE Cedula = @cedula";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cedula", cedula);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Socio eliminado correctamente.");
                lblResumenSocio.Text = "";
            }

            txtCedula.Clear();
        }


    }

}
