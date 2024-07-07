
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
using CatalogoArticulos_AccesoDatos;

namespace Presentacion
{
    public partial class FrmAgregarArticulo : Form
    {

        private Articulo articulo = null;   

        public FrmAgregarArticulo()
        {
            InitializeComponent();
        }

        public FrmAgregarArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            
            DataBaseHelper dataBaseHelper = new DataBaseHelper();
            try
            {
                if (articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;

                // Validar que el precio sea un valor decimal válido
                if (decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    articulo.Precio = precio;
                }
                else
                {
                    MessageBox.Show("El precio ingresado no tiene un formato válido.");
                    return;
                }

                articulo.Marca = new Marca();
                articulo.Categoria = new Categoria();
                articulo.Marca.Id = (int)cboMarca.SelectedValue;
                articulo.ImagenUrl = txtUrlImagen.Text;
                articulo.Categoria.Id = (int)cboCategoria.SelectedValue;

                if (articulo.Id != 0)
                {
                    dataBaseHelper.ModificarArticulo(articulo);
                    MessageBox.Show("Modificado exitosamente!");
                }
                else
                {
                    dataBaseHelper.InsertarArticulo(articulo);
                    MessageBox.Show("Agregado exitosamente!");
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FrmAgregarArticulo_Load(object sender, EventArgs e)
        {
            NegocioArticulo negocioArticulo = new NegocioArticulo();
            try
            {
                // Asignar al ComboBox de Categorías
                cboCategoria.DataSource = negocioArticulo.ObtenerCategorias();
                cboCategoria.DisplayMember = "Descripcion";
                cboCategoria.ValueMember = "Id";

                // Asignar al ComboBox de Marcas
                cboMarca.DataSource = negocioArticulo.ObtenerMarcas();
                cboMarca.DisplayMember = "Descripcion";
                cboMarca.ValueMember = "Id";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    txtPrecio.Text = articulo.Precio.ToString();
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pictureBoxTienda.Load(imagen);
            }
            catch (Exception ex)
            {

                pictureBoxTienda.Load("https://t4.ftcdn.net/jpg/05/17/53/57/360_F_517535712_q7f9QC9X6TQxWi6xYZZbMmw5cnLMr279.jpg");
            }
        }
    }
}
