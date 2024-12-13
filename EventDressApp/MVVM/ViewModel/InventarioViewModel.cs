using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EventDressApp.MVVM.Model;
using EventDressApp.Helpers;

namespace EventDressApp.MVVM.ViewModel
{
    internal class InventarioViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DETALLE_MODELO> _trajes;
        private DETALLE_MODELO _trajeSeleccionado;

        public ObservableCollection<DETALLE_MODELO> Trajes
        {
            get => _trajes;
            set
            {
                _trajes = value;
                OnPropertyChanged();
            }
        }

        public DETALLE_MODELO TrajeSeleccionado
        {
            get => _trajeSeleccionado;
            set
            {
                _trajeSeleccionado = value;
                OnPropertyChanged();
            }
        }

        public ICommand AgregarTrajeCommand { get; }
        public ICommand EditarTrajeCommand { get; }
        public ICommand EliminarTrajeCommand { get; }

        public InventarioViewModel()
        {
            Trajes = new ObservableCollection<DETALLE_MODELO>
            {
                new DETALLE_MODELO
                {
                    trajeid = 1,
                    categoriaid = "Formal",
                    nombre_traje = "Traje",
                    descripcion_traje = "Traje formal",
                    genero_traje = "Masculino",
                    talla_traje = "M",
                    color_traje = "Negro",
                    precio_diario_traje = 50,
                    estado_traje = "Disponible",
                    ruta_imagen_traje = ""
                }
            };

            AgregarTrajeCommand = new RelayCommand(AgregarTraje);
            EditarTrajeCommand = new RelayCommand(EditarTraje, CanEditarTraje);
            EliminarTrajeCommand = new RelayCommand(EliminarTraje, CanEliminarTraje);
        }

        private void AgregarTraje(object obj)
        {
            System.Diagnostics.Debug.WriteLine("Agregar Traje");
        }

        private void EditarTraje(object obj)
        {
            System.Diagnostics.Debug.WriteLine($"Editar Traje: {TrajeSeleccionado?.nombre_traje}");
        }

        private bool CanEditarTraje(object obj)
        {
            return TrajeSeleccionado != null;
        }

        private void EliminarTraje(object obj)
        {
            if (TrajeSeleccionado != null)
            {
                Trajes.Remove(TrajeSeleccionado);
            }
        }

        private bool CanEliminarTraje(object obj)
        {
            return TrajeSeleccionado != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}