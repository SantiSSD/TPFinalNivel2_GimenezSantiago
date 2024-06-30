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

        private void dataGridViewArticulos_SelectionChanged(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dataGridViewArticulos.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.ImagenUrl);

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

