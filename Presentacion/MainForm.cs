using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using CatalogoArticulos.AccesoDatos;
using CatalogoArticulos_AccesoDatos;

namespace Presentacion
{
    public partial class MainForm : Form
    {
        private List<Articulo> ListaArticulos;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Cargar();
            cboCampo.Items.Add("Codigo");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");
        }

        private void dataGridViewArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dataGridViewArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);

            }

        }

        private void Cargar()
        {
            try
            {
               
                DataBaseHelper DbHelper = new DataBaseHelper();
                ListaArticulos = DbHelper.ObtenerArticulos();

                
                if (ListaArticulos == null)
                {
                    MessageBox.Show("La lista de artículos es nula. Revisa la conexión a la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

               
                dataGridViewArticulos.DataSource = ListaArticulos;

                
                OcultarColumnas();

                
                if (ListaArticulos.Count > 0)
                {
                    try
                    {
                        pictureBoxTienda.Load(ListaArticulos[0].ImagenUrl);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron artículos en la base de datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OcultarColumnas()
        {
            dataGridViewArticulos.Columns["idMarca"].Visible = false;
            dataGridViewArticulos.Columns["idCategoria"].Visible = false;
            dataGridViewArticulos.Columns["ImagenUrl"].Visible = false;
            dataGridViewArticulos.Columns["Id"].Visible = false;

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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmAgregarArticulo frmAgregar = new FrmAgregarArticulo();
            frmAgregar.ShowDialog();
            Cargar();
           
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridViewArticulos.CurrentRow == null)
            {
                MessageBox.Show("No hay ningún artículo seleccionado para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Articulo seleccionado = (Articulo)dataGridViewArticulos.CurrentRow.DataBoundItem;

            FrmAgregarArticulo frmModificar = new FrmAgregarArticulo(seleccionado);
            frmModificar.ShowDialog();
            Cargar();

        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            // Instanciar la clase de negocio
            DataBaseHelper dataBaseHelper = new DataBaseHelper();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿De verdad queres eliminar este articulo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dataGridViewArticulos.CurrentRow.DataBoundItem;
                    dataBaseHelper.eliminar(seleccionado.Id);
                    Cargar();
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        private bool ValidaFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            if (cboCriterio.SelectedIndex < 0 && !string.IsNullOrWhiteSpace(txtFiltroAvanzado.Text.Trim()))
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Codigo")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text.Trim()))
                {
                    MessageBox.Show("El filtro no puede estar vacío para el campo 'Codigo', seleccione recargar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else if (cboCampo.SelectedItem.ToString() == "Nombre")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text.Trim()))
                {
                    MessageBox.Show("El filtro no puede estar vacío para el campo 'Nombre', seleccione recargar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else if (cboCampo.SelectedItem.ToString() == "Descripción")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text.Trim()))
                {
                    MessageBox.Show("El filtro no puede estar vacío para el campo 'Descripción', seleccione recargar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            return false;




        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            DataBaseHelper dataBaseHelper = new DataBaseHelper();
            try
            {
                if (ValidaFiltro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text.Trim();

                dataGridViewArticulos.DataSource = dataBaseHelper.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar los artículos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> Listafiltrada;
            string filtro = txtFiltro.Text;

            if (filtro != "")
            {
                Listafiltrada = ListaArticulos.FindAll(articulo =>
                articulo.Codigo.ToUpper().Contains(filtro.ToUpper()) ||
                articulo.Nombre.ToUpper().Contains(filtro.ToUpper()) ||
               (articulo.Marca != null && articulo.Marca.Descripcion != null && articulo.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper())) ||
               (articulo.Categoria != null && articulo.Categoria.Descripcion != null && articulo.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper())) ||
               (articulo.Descripcion != null && articulo.Descripcion.ToUpper().Contains(filtro.ToUpper())) || articulo.Precio.ToString().ToUpper().Contains(filtro.ToUpper()));

            }
            else
            {
                Listafiltrada = ListaArticulos;
            }

            dataGridViewArticulos.DataSource = null;
            dataGridViewArticulos.DataSource = Listafiltrada;
            OcultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            cboCriterio.Items.Clear();

            if (opcion == "Codigo" || opcion == "Nombre" || opcion == "Descripción")
            {
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void btnDescripcionarticulo_Click(object sender, EventArgs e)
        {
            if (dataGridViewArticulos.CurrentRow == null)
            {
                MessageBox.Show("No hay ningún artículo seleccionado para ver.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Articulo seleccionado = (Articulo)dataGridViewArticulos.CurrentRow.DataBoundItem;
            FrmDetalleArticulo frmDetalle = new FrmDetalleArticulo(seleccionado);
            frmDetalle.ShowDialog();
        }

        private void btnRecargar_Click(object sender, EventArgs e)
        {
            Cargar();
        }
        
        
    }
}

