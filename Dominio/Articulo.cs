using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo
    {
        //Propiedades para almacenar datos del articulo
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        //Annotations
        [DisplayName ("Descripción")]
        public string Descripcion { get; set; }
        public int  IdMarca { get; set; } //Esta propiedad guarda el ID de la marca de la base de datos
        public Marca Marca { get; set; } //Esta propiedad guarda el objeto completo de la marca


        public int IdCategoria { get; set; } // Esta propiedad guarda el ID de la categoría en la base de datos
        public Categoria Categoria { get; set; } // Esta propiedad guarda el objeto completo de la categoría

        public string ImagenUrl { get; set; }
        public decimal Precio { get; set; }

    }
}
