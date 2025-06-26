using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNFit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            FormAdmin admin = new FormAdmin();
            admin.ShowDialog();
        }

        private void btnIngresar_Click_1(object sender, EventArgs e)
        {
            FormIngreso ingreso = new FormIngreso();
            ingreso.ShowDialog();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            FormRegistro registro = new FormRegistro();
            registro.ShowDialog();
        }

        private void btnSocio_Click_1(object sender, EventArgs e)
        {
            FormBuscarSocio buscarSocio = new FormBuscarSocio();
            buscarSocio.ShowDialog();
        }
    }
}
