using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CatalogoArticulos.AccesoDatos;

namespace Presentacion
{
    public partial class FrmAgregarArticulo : Form
    {
        public FrmAgregarArticulo()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            Articulo nuevoArticulo = new Articulo();
            DataBaseHelper dataBaseHelper = new DataBaseHelper();
            try
            {
                nuevoArticulo.Codigo = txtCodigo.Text;
                nuevoArticulo.Nombre = (txtNombre.Text);
                nuevoArticulo.Descripcion = txtDescripcion.Text;
                nuevoArticulo.Precio = int.Parse(txtPrecio.Text);

                dataBaseHelper.InsertarArticulo(nuevoArticulo);
                MessageBox.Show("Agregado exitosamente!");
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
