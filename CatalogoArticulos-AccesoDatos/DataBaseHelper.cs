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

                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT a.Id, a.Codigo, a.Nombre, a.Descripcion, a.IdMarca, m.Descripcion AS Marca, a.IdCategoria, c.Descripcion AS Categoria, a.Precio, a.ImagenUrl FROM Articulos a JOIN Marcas m ON a.IdMarca = m.Id JOIN Categorias c ON a.IdCategoria = c.Id;";
                comando.Connection = conexion;

              
                conexion.Open();
                lector = comando.ExecuteReader();

                
                while (lector.Read())
                {
                    Articulo articulo = new Articulo();

                    articulo.Id = lector.GetInt32(0); 
                    articulo.Codigo = lector["Codigo"].ToString();
                    articulo.Nombre = lector["Nombre"].ToString();
                    articulo.Descripcion = lector["Descripcion"].ToString();

                    
                    articulo.IdMarca = lector.GetInt32(4); 
                    articulo.IdCategoria = lector.GetInt32(6); 

                    articulo.Marca = new Marca
                    {
                        Id = articulo.IdMarca,
                        Descripcion = lector["Marca"].ToString() 
                    };

                    articulo.Categoria = new Categoria
                    {
                        Id = articulo.IdCategoria,
                        Descripcion = lector["Categoria"].ToString() 
                    };

                    articulo.Precio = lector.GetDecimal(8);

                    //Primera manera para validar DBnull
                    //if (!lector.IsDBNull(lector.GetOrdinal("ImagenUrl")))                  
                    //articulo.ImagenUrl = lector["ImagenUrl"].ToString();
                    if (!(lector["ImagenUrl"] is DBNull))
                        articulo.ImagenUrl = lector["ImagenUrl"].ToString();
                    articulos.Add(articulo);
                }

               
                conexion.Close();
                return articulos;
            }
            catch (Exception ex)
            {
                
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
                datos.setearParametro("@id", articulo.Id); 

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

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from ARTICULOS where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
                 
            try
            {
                string consulta = "SELECT a.Id, a.Codigo, a.Nombre, a.Descripcion, a.IdMarca, m.Descripcion AS Marca, a.IdCategoria, c.Descripcion AS Categoria, a.Precio, a.ImagenUrl FROM Articulos a JOIN Marcas m ON a.IdMarca = m.Id JOIN Categorias c ON a.IdCategoria = c.Id AND ";
                switch (campo)
                {
                    case "Codigo":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "a.Codigo LIKE @filtro + '%'";
                                break;
                            case "Termina con":
                                consulta += "a.Codigo LIKE '%' + @filtro";
                                break;
                            case "Contiene":
                                consulta += "a.Codigo LIKE '%' + @filtro + '%'";
                                break;
                        }
                        break;

                    case "Nombre":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "a.Nombre LIKE @filtro + '%'";
                                break;
                            case "Termina con":
                                consulta += "a.Nombre LIKE '%' + @filtro";
                                break;
                            case "Contiene":
                                consulta += "a.Nombre LIKE '%' + @filtro + '%'";
                                break;
                        }
                        break;

                    case "Descripción":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "a.Descripcion LIKE @filtro + '%'";
                                break;
                            case "Termina con":
                                consulta += "a.Descripcion LIKE '%' + @filtro";
                                break;
                            case "Contiene":
                                consulta += "a.Descripcion LIKE '%' + @filtro + '%'";
                                break;
                        }
                        break;

                    default:
                        throw new Exception("Campo de búsqueda no reconocido");
                }

                datos.setearConsulta(consulta);
                datos.setearParametro("@filtro", filtro); 
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo articulo = new Articulo
                    {
                        Id = datos.Lector.GetInt32(0),
                        Codigo = datos.Lector["Codigo"].ToString(),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Descripcion = datos.Lector["Descripcion"].ToString(),
                        IdMarca = datos.Lector.GetInt32(4),
                        IdCategoria = datos.Lector.GetInt32(6),
                        Marca = new Marca
                        {
                            Id = datos.Lector.GetInt32(4),
                            Descripcion = datos.Lector["Marca"].ToString()
                        },
                        Categoria = new Categoria
                        {
                            Id = datos.Lector.GetInt32(6), 
                            Descripcion = datos.Lector["Categoria"].ToString() 
                        },
                        Precio = datos.Lector.GetDecimal(8)
                    };

                    
                    if (!datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl")))
                    {
                        articulo.ImagenUrl = datos.Lector["ImagenUrl"].ToString();
                    }

                    lista.Add(articulo);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar los artículos en la base de datos.", ex);
            }
        }
    }
}