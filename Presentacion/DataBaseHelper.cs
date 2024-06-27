using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;

namespace Presentacion
{
    internal class DataBaseHelper
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
                string query = "SELECT Codigo, Nombre, Descripcion FROM ARTICULOS";
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
                                articulo.Codigo = lector["Codigo"].ToString();
                                articulo.Nombre = lector["Nombre"].ToString();
                                articulo.Descripcion = lector["Descripcion"].ToString();
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
            //Cambios Realizados:
            //Clase DataBaseHelper: Renombré la clase a DataBaseHelper.
            //Clase Articulo: Asumí que tienes una clase Articulo con propiedades Codigo, Nombre y Descripcion.
            //Cadena de Conexión: La cadena de conexión está configurada para CATALOGO_DB.
            //Comando SQL: El comando SQL selecciona Codigo, Nombre y Descripcion de la tabla ARTICULOS.
            //Asegúrate de que Articulo esté definido adecuadamente en tu proyecto(puedes utilizar la definición que vimos anteriormente). Este código debería funcionar correctamente para obtener la lista de artículos de tu base de datos CATALOGO_DB.



        }
    }
}
