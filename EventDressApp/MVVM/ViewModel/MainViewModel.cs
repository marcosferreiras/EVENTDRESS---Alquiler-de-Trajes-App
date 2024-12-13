using System;
using EventDressApp.Core;

namespace EventDressApp.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand ClientesViewCommand { get; set; }
        public RelayCommand InventarioViewCommand { get; set; }

        public HomeViewModel HomeVm{ get; set; }  
        public ClientesViewModel ClientesVm{ get; set; }
        public InventarioViewModel InventarioVm { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set {  
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel() { 
            HomeVm = new HomeViewModel();
            ClientesVm = new ClientesViewModel();
            InventarioVm = new InventarioViewModel();
            CurrentView = HomeVm;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVm;
            });

            ClientesViewCommand = new RelayCommand(o =>
            {
                CurrentView = ClientesVm;
            });

            InventarioViewCommand = new RelayCommand(o =>
            {
                CurrentView = InventarioVm;
            });
        }
    }
}
