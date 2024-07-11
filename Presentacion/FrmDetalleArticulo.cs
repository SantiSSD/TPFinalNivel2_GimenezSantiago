using CatalogoArticulos_AccesoDatos;
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

namespace Presentacion
{
    public partial class FrmDetalleArticulo : Form
    {
        private Articulo articuloDetalle;
        private NegocioArticulo negocioArticulo; 

        public FrmDetalleArticulo(Articulo articulo)
        {
            InitializeComponent();
            articuloDetalle = articulo;
            negocioArticulo = new NegocioArticulo(); 
        }

        private void FrmDetalleArticulo_Load(object sender, EventArgs e)
        {
            MostrarDatosArticulo();
        }

        private void MostrarDatosArticulo()
        {
            if (articuloDetalle != null)
            {
                txtNombreDetalle.Text = articuloDetalle.Nombre;
                txtDescripcionDetalle.Text = articuloDetalle.Descripcion;
                txtPrecioDetalle.Text = articuloDetalle.Precio.ToString();
                txtCodigoDetalle.Text = articuloDetalle.Codigo;

                
                if (!string.IsNullOrEmpty(articuloDetalle.ImagenUrl))
                {
                    pictureBoxTiendaDetalle.ImageLocation = articuloDetalle.ImagenUrl;
                    txtUrlImagenDetalle.Text = articuloDetalle.ImagenUrl;
                }
                else
                {
                   
                    pictureBoxTiendaDetalle.ImageLocation = "ruta/a/imagen_predeterminada.png"; 
                    txtUrlImagenDetalle.Text = "No hay imagen disponible";
                }

                CargarComboBoxCategorias();
                CargarComboBoxMarcas();
                
                cboCategoriaDetalle.SelectedValue = articuloDetalle.Categoria.Id;
                cboMarcaDetalle.SelectedValue = articuloDetalle.Marca.Id;

                txtNombreDetalle.Enabled = false;
                txtDescripcionDetalle.Enabled = false;
                txtPrecioDetalle.Enabled = false;
                txtCodigoDetalle.Enabled = false;
                pictureBoxTiendaDetalle.Enabled = false;
                txtUrlImagenDetalle.Enabled = false;
                cboCategoriaDetalle.Enabled = false;
                cboMarcaDetalle.Enabled = false;
            }
        }

        private void CargarComboBoxCategorias()
        {
            cboCategoriaDetalle.DataSource = negocioArticulo.ObtenerCategorias();
            cboCategoriaDetalle.DisplayMember = "Descripcion";
            cboCategoriaDetalle.ValueMember = "Id";
        }

        private void CargarComboBoxMarcas()
        {
            cboMarcaDetalle.DataSource = negocioArticulo.ObtenerMarcas();
            cboMarcaDetalle.DisplayMember = "Descripcion";
            cboMarcaDetalle.ValueMember = "Id";
        }

        private void btnCancelarDetalle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}