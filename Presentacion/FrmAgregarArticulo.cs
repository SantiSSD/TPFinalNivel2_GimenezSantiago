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
                if (ValidarCampos())
                {
                    if (articulo == null)
                        articulo = new Articulo();

                    articulo.Codigo = txtCodigo.Text;
                    articulo.Nombre = txtNombre.Text;
                    articulo.Descripcion = txtDescripcion.Text;
                    articulo.ImagenUrl = txtUrlImagen.Text;
                    articulo.Precio = decimal.Parse(txtPrecio.Text);
                    articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                    articulo.Marca = (Marca)cboMarca.SelectedItem;

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

                    if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                    {
                        File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                    }

                    Close();
                }
                else
                {
                    // Enfocar el primer campo que tenga un error
                    if (!string.IsNullOrWhiteSpace(lblErrorCodigo.Text))
                        txtCodigo.Focus();
                    else if (!string.IsNullOrWhiteSpace(lblErrorNombre.Text))
                        txtNombre.Focus();
                    else if (!string.IsNullOrWhiteSpace(lblErrorPrecio.Text))
                        txtPrecio.Focus();
                }
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
                cboCategoria.DataSource = negocioArticulo.ObtenerCategorias();
                cboCategoria.DisplayMember = "Descripcion";
                cboCategoria.ValueMember = "Id";

               
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

               
            }
        }

        private bool ValidarCampos()
        {
            bool esValido = true;


            lblErrorCodigo.Text = "";
            lblErrorNombre.Text = "";
            lblErrorPrecio.Text = "";


            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                lblErrorCodigo.Text = "El campo Código es obligatorio.";
                esValido = false;
            }


            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                lblErrorNombre.Text = "El campo Nombre es obligatorio.";
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtPrecio.Text) || !decimal.TryParse(txtPrecio.Text, out _))
            {
                lblErrorPrecio.Text = "Debes cargar un precio Valido, solo números.";
                esValido = false;
            }

            return esValido;





        }
    }
}
