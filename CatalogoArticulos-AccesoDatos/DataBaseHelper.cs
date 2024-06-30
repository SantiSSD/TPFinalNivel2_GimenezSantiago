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

            // Cadena de conexión
            string connectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true;";

            // Uso de using para asegurar la liberación de recursos
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                // Comando SQL
                string query = "SELECT Id, Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio, ImagenUrl FROM ARTICULOS";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    try
                    {
                        conexion.Open(); // Abrir conexión

                        // Ejecutar comando y obtener el lector de datos
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            // Iterar sobre los resultados
                            while (lector.Read())
                            {
                                Articulo articulo = new Articulo();
                                articulo.Id = lector.GetInt32(0); //obtener id
                                articulo.Codigo = lector["Codigo"].ToString();
                                articulo.Nombre = lector["Nombre"].ToString();
                                articulo.Descripcion = lector["Descripcion"].ToString();
                                articulo.IdMarca = lector.GetInt32(4);//Obtener IdMarca
                                articulo.IdCategoria = lector.GetInt32(5);
                                articulo.Precio = lector.GetDecimal(6);
                                articulo.ImagenUrl = lector["ImagenUrl"].ToString();
                                articulos.Add(articulo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones: puedes registrar el error, mostrar un mensaje al usuario, etc.
                        throw new Exception("Error al obtener los artículos de la base de datos.", ex);
                    }
                }
            }

            return articulos;
           


        }
    }
}
