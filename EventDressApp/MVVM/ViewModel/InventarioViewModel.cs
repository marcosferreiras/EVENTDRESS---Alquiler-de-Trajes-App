using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EventDressApp.MVVM.Model;
using EventDressApp.Helpers;
using EventDressApp.MVVM.View.Dialogs;
using EventDressApp.MVVM.ViewModel.DialogViewModels;

namespace EventDressApp.MVVM.ViewModel
{
    internal class InventarioViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DETALLE_MODELO> _modelo;
        private DETALLE_MODELO _modeloSeleccionado;

        public ObservableCollection<DETALLE_MODELO> modelo
        {
            get => _modelo;
            set
            {
                _modelo = value;
                OnPropertyChanged();
            }
        }

        public DETALLE_MODELO modeloSeleccionado
        {
            get => _modeloSeleccionado;
            set
            {
                _modeloSeleccionado = value;
                OnPropertyChanged();
            }
        }

        public ICommand AgregarmodeloCommand { get; }
        public ICommand EditarmodeloCommand { get; }
        public ICommand EliminarmodeloCommand { get; }

        public InventarioViewModel()
        {
            modelo = new ObservableCollection<DETALLE_MODELO>();
            CargarDatos();

            AgregarmodeloCommand = new RelayCommand(Agregarmodelo);
            EditarmodeloCommand = new RelayCommand(Editarmodelo, CanEditarmodelo);
            EliminarmodeloCommand = new RelayCommand(Eliminarmodelo, CanEliminarmodelo);
        }

        private void CargarDatos()
        {
            modelo.Add(new DETALLE_MODELO
            {
                TrajeId = 1,
                CategoriaId = 1,
                NombreTraje = "Traje",
                DescripcionTraje = "Traje formal",
                GeneroTraje = "Masculino",
                TallaTraje = "M",
                ColorTraje = "Negro",
                PrecioDiarioTraje = 50,
                EstadoTraje = "Disponible",
                RutaImagenTraje = ""
            });
         }

            
        private void Agregarmodelo(object obj)
        {
            var dialog = new DialogoInventario();
            if (dialog.ShowDialog() == true)
            {
                var nuevoTraje = ((DialogoInventarioViewModel)dialog.DataContext).Traje;
                // TODO: Guardar en la base de datos
                modelo.Add(nuevoTraje);
            }
        }

        private void Editarmodelo(object obj)
        {
            var trajeParaEditar = new DETALLE_MODELO
            {
                TrajeId = modeloSeleccionado.TrajeId,
                CategoriaId = modeloSeleccionado.CategoriaId,
                NombreTraje = modeloSeleccionado.NombreTraje,
                DescripcionTraje = modeloSeleccionado.DescripcionTraje,
                GeneroTraje = modeloSeleccionado.GeneroTraje,
                TallaTraje = modeloSeleccionado.TallaTraje,
                ColorTraje = modeloSeleccionado.ColorTraje,
                PrecioDiarioTraje = modeloSeleccionado.PrecioDiarioTraje,
                EstadoTraje = modeloSeleccionado.EstadoTraje,
                RutaImagenTraje = modeloSeleccionado.RutaImagenTraje
            };

            var dialog = new DialogoInventario(trajeParaEditar);
            if (dialog.ShowDialog() == true)
            {
                var trajeActualizado = ((DialogoInventarioViewModel)dialog.DataContext).Traje;

                // TODO: Actualizar en la base de datos

                // Actualizar en la colección
                var index = modelo.IndexOf(modeloSeleccionado);
                if (index != -1)
                {
                    modelo[index] = trajeActualizado;
                }
            }
        }

        private bool CanEditarmodelo(object obj)
        {
            return modeloSeleccionado != null;
        }

        private void Eliminarmodelo(object obj)
        {
            if (modeloSeleccionado != null)
            {
                _modelo.Remove(modeloSeleccionado);
            }
        }

        private bool CanEliminarmodelo(object obj)
        {
            return modeloSeleccionado != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}