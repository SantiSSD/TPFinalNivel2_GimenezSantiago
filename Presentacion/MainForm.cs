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
            cboCampo.Items.Add("Id");
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
                // Invocar la lectura de base de datos
                DataBaseHelper DbHelper = new DataBaseHelper();
                ListaArticulos = DbHelper.ObtenerArticulos();

                // Verificar si ListaArticulos es null
                if (ListaArticulos == null)
                {
                    MessageBox.Show("La lista de artículos es nula. Revisa la conexión a la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Asignar los datos al DataGridView
                dataGridViewArticulos.DataSource = ListaArticulos;

                // Ocultar columnas específicas
                OcultarColumnas();

                // Verificar que la lista no esté vacía antes de cargar la imagen
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
            dataGridViewArticulos.Columns["Id"].Visible = true;

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
            // Aquí puedes realizar acciones después de cerrar el formulario de agregar, como actualizar una lista de artículos mostrados.
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dataGridViewArticulos.CurrentRow.DataBoundItem;

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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            DataBaseHelper dataBaseHelper = new DataBaseHelper();
            try
            {
              string campo = cboCampo.SelectedItem.ToString();
              string criterio = cboCriterio.SelectedItem.ToString();
              string filtro = txtFiltroAvanzado.Text;
                dataGridViewArticulos.DataSource = dataBaseHelper.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> Listafiltrada;
            string filtro = txtFiltro.Text;

            if (filtro != "")
            {
                Listafiltrada = ListaArticulos.FindAll(SANTI => SANTI.Nombre.ToUpper().Contains(filtro.ToUpper()) || (SANTI.Marca != null && SANTI.Marca.Descripcion != null && SANTI.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper())) || (SANTI.Descripcion != null && SANTI.Descripcion.ToUpper().Contains(filtro.ToUpper())));


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
            if (opcion == "Id")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
    }
}

