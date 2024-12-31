using System.Windows.Input;
using EventDressApp.MVVM.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EventDressApp.Helpers;
using System.Windows;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace EventDressApp.MVVM.ViewModel.DialogViewModels
{
    public class DialogoInventarioViewModel : INotifyPropertyChanged
    {
        private DETALLE_MODELO _traje;
        private string _titulo;
        private bool _esNuevo;
        private ObservableCollection<Categoria> _categorias;

        public DETALLE_MODELO Traje
        {
            get => _traje;
            set
            {
                _traje = value;
                OnPropertyChanged();
            }
        }

        public string Titulo
        {
            get => _titulo;
            set
            {
                _titulo = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Categoria> Categorias
        {
            get => _categorias;
            set
            {
                _categorias = value;
                OnPropertyChanged();
            }
        }

        public ICommand GuardarCommand { get; }
        public ICommand CancelarCommand { get; }
        public ICommand SeleccionarImagenCommand { get; }

        public DialogoInventarioViewModel(DETALLE_MODELO traje = null)
        {
            _esNuevo = traje == null;
            Traje = traje ?? new DETALLE_MODELO();
            Titulo = _esNuevo ? "Nuevo Traje" : "Editar Traje";

            // TODO: Cargar categorías desde la base de datos
            CargarCategorias();

            GuardarCommand = new RelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
            SeleccionarImagenCommand = new RelayCommand(SeleccionarImagen);
        }

        private void CargarCategorias()
        {
            // TODO: Carga desde base de datos
            Categorias = new ObservableCollection<Categoria>
            {
                new Categoria { CategoriaId = 1, NombreCategoria = "Formal" },
                new Categoria { CategoriaId = 2, NombreCategoria = "Casual" },
                new Categoria { CategoriaId = 3, NombreCategoria = "Gala" }
            };
        }

        private void Guardar(object obj)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(Traje.NombreTraje) ||
                string.IsNullOrWhiteSpace(Traje.DescripcionTraje) ||
                Traje.CategoriaId == 0 ||
                Traje.PrecioDiarioTraje <= 0)
            {
                MessageBox.Show("Por favor complete todos los campos obligatorios");
                return;
            }

            // TODO: Guardar en base de datos
            DialogResult = true;
            CloseDialog();
        }

        private void Cancelar(object obj)
        {
            DialogResult = false;
            CloseDialog();
        }

        private void SeleccionarImagen(object obj)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif|Todos los archivos|*.*",
                Title = "Seleccionar imagen del traje"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Traje.RutaImagenTraje = openFileDialog.FileName;
            }
        }

        private void CloseDialog()
        {
            if (Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) is Window window)
                window.Close();
        }

        public bool? DialogResult { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}