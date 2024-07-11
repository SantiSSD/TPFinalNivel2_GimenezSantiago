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
using System.IO;
using System.Configuration;

namespace Presentacion
{
    public partial class FrmAgregarArticulo : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;

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
                if (!ValidarCampos())
                    return;

                if (articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;

                // Validación para el campo Precio
                decimal precio;
                if (!decimal.TryParse(txtPrecio.Text, out precio))
                {
                    MessageBox.Show("El campo Precio debe contener un valor numérico válido.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrecio.Focus();
                    return;
                }
                articulo.Precio = precio;

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

                // Guardo la imagen si la levanto localmente:
                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
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

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

                // Guardo la imagen
                // File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                MessageBox.Show("El código no puede estar vacío.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("El nombre no puede estar vacío.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                MessageBox.Show("La descripción no puede estar vacía.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            decimal precio;
            if (!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("El precio ingresado no es válido. Debe ser un número.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrecio.Focus();
                return false;
            }

            return true;
        }

        private bool ValidarPrecio(string precio)
        {
            decimal resultado;
            return decimal.TryParse(precio, out resultado);
        }

        
    }
}
