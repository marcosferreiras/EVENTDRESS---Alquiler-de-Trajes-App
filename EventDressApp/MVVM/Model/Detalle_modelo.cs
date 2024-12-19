using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventDressApp.MVVM.Model
{
    public class DETALLE_MODELO : INotifyPropertyChanged
    {
        private int trajeid;
        private int categoriaid;
        private string nombretraje;
        private string descripciontraje;
        private string generotraje;
        private string tallatraje;
        private string colortraje;
        private decimal preciodiariotraje;
        private string estadotraje;
        private string rutaimagentraje;


        public int TrajeId { get => trajeid; set { if (trajeid != value) { trajeid = value; OnPropertyChanged(); } } }
        public int CategoriaId { get =>categoriaid; set { if (categoriaid != value) { categoriaid = value; OnPropertyChanged(); } } }
        public string NombreTraje { get => nombretraje; set { if (nombretraje != value) { nombretraje = value; OnPropertyChanged(); } } }
        public string DescripcionTraje { get => descripciontraje; set { if (descripciontraje != value) { descripciontraje = value; OnPropertyChanged(); } } }
        public string GeneroTraje { get => generotraje; set { if (generotraje != value) { generotraje = value; OnPropertyChanged(); } } }
        public string TallaTraje { get => tallatraje; set { if (tallatraje != value) { tallatraje = value; OnPropertyChanged(); } } }
        public string ColorTraje { get => colortraje; set { if (colortraje != value) { colortraje = value; OnPropertyChanged(); } } }
        public decimal PrecioDiarioTraje { get => preciodiariotraje; set { if (preciodiariotraje != value) { preciodiariotraje = value; OnPropertyChanged(); } } }
        public string EstadoTraje { get => estadotraje; set { if (estadotraje != value) { estadotraje = value; OnPropertyChanged(); } } }
        public string RutaImagenTraje { get => rutaimagentraje; set { if (rutaimagentraje != value) { rutaimagentraje = value; OnPropertyChanged(); } } }

        // Implementación de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
