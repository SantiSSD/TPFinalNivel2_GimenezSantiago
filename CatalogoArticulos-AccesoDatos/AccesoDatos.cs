using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CatalogoArticulos.AccesoDatos
{
    internal class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public AccesoDatos() 
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true;");
            comando = new SqlCommand();
        }
        public void setearConsulta(string consulta) 
        {
           comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta; 
        }
    }
}
