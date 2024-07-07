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

                    // Asignar IdMarca y IdCategoria directamente si están como enteros en la base de datos
                    articulo.IdMarca = lector.GetInt32(4); // Obtener IdMarca
                    articulo.IdCategoria = lector.GetInt32(6); // Obtener IdCategoria

                    articulo.Marca = new Marca
                    {
                        Id = articulo.IdMarca,
                        Descripcion = lector["Marca"].ToString() // Obtener Marca
                    };

                    articulo.Categoria = new Categoria
                    {
                        Id = articulo.IdCategoria,
                        Descripcion = lector["Categoria"].ToString() // Obtener Categoria
                    };

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
                datos.setearConsulta("INSERT INTO Articulos (Codigo, Nombre, Descripcion, IdMarca, IdCategoria,ImagenUrl, Precio) VALUES (@codigo, @nombre, @descripcion, @idMarca, @idCategoria,@Imagenurl, @precio)");


                datos.setearParametro("@codigo", nuevo.Codigo);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@descripcion", nuevo.Descripcion);
                datos.setearParametro("@idMarca", nuevo.Marca.Id);
                datos.setearParametro("@idCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@Imagenurl", nuevo.ImagenUrl);
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
        public void ModificarArticulo(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Articulos SET Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca, IdCategoria = @idCategoria, ImagenUrl = @imagenUrl, Precio = @precio WHERE Id = @id");

                datos.setearParametro("@codigo", articulo.Codigo);
                datos.setearParametro("@nombre", articulo.Nombre);
                datos.setearParametro("@descripcion", articulo.Descripcion);
                datos.setearParametro("@idMarca", articulo.Marca.Id);
                datos.setearParametro("@idCategoria", articulo.Categoria.Id);
                datos.setearParametro("@imagenUrl", articulo.ImagenUrl);
                datos.setearParametro("@precio", articulo.Precio);
                datos.setearParametro("@id", articulo.Id); // Agregar el parámetro Id para identificar el artículo a modificar

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el artículo en la base de datos.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}