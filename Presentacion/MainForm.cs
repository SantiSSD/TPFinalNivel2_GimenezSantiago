using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Vamos a invocar la lectura de base de datos
            DataBaseHelper DbHelper = new DataBaseHelper();
            dataGridViewArticulos.DataSource = DbHelper.ObtenerArticulos();
        }
    }
}
