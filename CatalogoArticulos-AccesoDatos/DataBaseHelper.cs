using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using Dominio;

namespace CatalogoArticulos.AccesoDatos
{
    public class DataBaseHelper
    {
        public List<Articulo> ObtenerArticulos()
        {
            List<Articulo> articulos = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                // Cadena de conexión
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT a.Id, a.Codigo, a.Nombre, a.Descripcion, a.IdMarca, m.Descripcion AS Marca, a.IdCategoria, c.Descripcion AS Categoria, a.Precio, a.ImagenUrl FROM Articulos a JOIN Marcas m ON a.IdMarca = m.Id JOIN Categorias c ON a.IdCategoria = c.Id;";
                comando.Connection = conexion;

                // Abrir conexión
                conexion.Open();
                lector = comando.ExecuteReader();

                // Iterar sobre los resultados
                while (lector.Read())
                {
                    Articulo articulo = new Articulo();

                    articulo.Id = lector.GetInt32(0); // Obtener Id
                    articulo.Codigo = lector["Codigo"].ToString();
                    articulo.Nombre = lector["Nombre"].ToString();
                    articulo.Descripcion = lector["Descripcion"].ToString();

                    //crear Objeto Marca
                    Marca marca = new Marca();
                    marca.Id = lector.GetInt32(4);// Obtener IdMarca
                    marca.Descripcion = lector["Marca"].ToString(); // Obtener Descripcion de Marca
                    articulo.Marca = marca;
                    //crear Objeto Categoria


                    Categoria categoria = new Categoria();
                    categoria.Id = lector.GetInt32(6); // Obtener IdCategoria
                    categoria.Descripcion = lector["Categoria"].ToString(); // Obtener Descripcion de Categoria
                    articulo.Categoria = categoria;
                    articulo.Precio = lector.GetDecimal(8); // Obtener Precio

                    //Primera manera para validar DBnull
                    //if (!lector.IsDBNull(lector.GetOrdinal("ImagenUrl")))                  
                    //articulo.ImagenUrl = lector["ImagenUrl"].ToString();
                    if (!(lector["ImagenUrl"] is DBNull))
                        articulo.ImagenUrl = lector["ImagenUrl"].ToString();
                    articulos.Add(articulo);
                }

                // Cerrar conexión
                conexion.Close();
                return articulos;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: puedes registrar el error, mostrar un mensaje al usuario, etc.
                throw new Exception("Error al obtener los artículos de la base de datos.", ex);
            }
        }
        public void InsertarArticulo(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO Articulos (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) VALUES (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @precio)");


                datos.setearParametro("@codigo", nuevo.Codigo);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@descripcion", nuevo.Descripcion);
                datos.setearParametro("@idMarca", nuevo.Marca.Id);
                datos.setearParametro("@idCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@precio", nuevo.Precio);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }

        }
    }
}