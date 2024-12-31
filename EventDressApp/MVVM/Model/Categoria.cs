using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EventDressApp.MVVM.Model
{
    public class Categoria : INotifyPropertyChanged
    {
        private int _categoriaId;
        private string _nombreCategoria;
        private string _descripcionCategoria;
        private string _estadoCategoria;

        public int CategoriaId
        {
            get => _categoriaId;
            set
            {
                if (_categoriaId != value)
                {
                    _categoriaId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NombreCategoria
        {
            get => _nombreCategoria;
            set
            {
                if (_nombreCategoria != value)
                {
                    _nombreCategoria = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DescripcionCategoria
        {
            get => _descripcionCategoria;
            set
            {
                if (_descripcionCategoria != value)
                {
                    _descripcionCategoria = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EstadoCategoria
        {
            get => _estadoCategoria;
            set
            {
                if (_estadoCategoria != value)
                {
                    _estadoCategoria = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}