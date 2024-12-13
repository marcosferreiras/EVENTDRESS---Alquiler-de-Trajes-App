using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventDressApp.MVVM.Model
{
    public class DETALLE_MODELO
    {
        public int trajeid { get; set;  }
        public string categoriaid { get; set; }
        public string nombre_traje { get; set; }
        public string descripcion_traje { get; set; }
        public string genero_traje { get; set; }
        public string talla_traje { get; set; }
        public string color_traje { get; set; }
        public decimal precio_diario_traje { get; set; }
        public string estado_traje { get; set; }
        public string ruta_imagen_traje { get; set; }
    }
}
