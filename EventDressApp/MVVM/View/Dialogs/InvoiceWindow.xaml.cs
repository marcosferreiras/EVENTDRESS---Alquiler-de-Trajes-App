using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using EventDressApp.MVVM.Model;
using Microsoft.Data.SqlClient;

namespace EventDressApp.MVVM.View.Dialogs
{
    public partial class InvoiceWindow : Window
    {
        private List<DataRowView> _selectedRows;
        private int _clienteId;
        private string _nombreCliente;
        private string _apellidoCliente;

        public InvoiceWindow(List<DataRowView> selectedRows, int clienteId, string nombreCliente, string apellidoCliente)
        {
            InitializeComponent();

            _selectedRows = selectedRows;
            _clienteId = clienteId;
            _nombreCliente = nombreCliente;
            _apellidoCliente = apellidoCliente;

            // Mostrar información del cliente
            DisplayClientInfo();

            // Crear detalles de factura
            CreateInvoiceDetails();
        }

        private void DisplayClientInfo()
        {
            StringBuilder clientInfo = new StringBuilder();
            clientInfo.AppendLine($"Cliente ID: {_clienteId}");
            clientInfo.AppendLine($"Nombre: {_nombreCliente} {_apellidoCliente}");

            // Mostrar la información en un TextBox o TextBlock (asegúrate de que exista en el XAML)
            ClientInfoTextBox.Text = clientInfo.ToString();
        }

        private void CreateInvoiceDetails()
        {
            StringBuilder invoiceDetails = new StringBuilder();
            double totalAmount = 0;

            foreach (var row in _selectedRows)
            {
                string name = row["nombre_prenda"].ToString();
                double price = Convert.ToDouble(row["precio_diario_prenda"]);
                invoiceDetails.AppendLine($"{name} - ${price:F2}");
                totalAmount += price;
            }

            // Mostrar los detalles en la ventana
            InvoiceTextBox.Text = invoiceDetails.ToString();
            TotalTextBlock.Text = $"Total: ${totalAmount:F2}";
        }

        private void PrintInvoice(object sender, RoutedEventArgs e)
        {
            if (_selectedRows == null || _selectedRows.Count == 0)
            {
                MessageBox.Show("No hay elementos seleccionados para imprimir.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Convertir las prendas seleccionadas a JSON para el stored procedure
                var detallesReservas = new List<object>();
                foreach (var row in _selectedRows)
                {
                    detallesReservas.Add(new
                    {
                        PrendaID = Convert.ToInt32(row["prenda_id"]),
                        Cantidad = 1 // Cantidad fija como ejemplo
                    });
                }
                string detallesJson = System.Text.Json.JsonSerializer.Serialize(detallesReservas);

                // Parámetros para el stored procedure
                var parameters = new[]
                {
                    new SqlParameter("@ClienteID", SqlDbType.Int) { Value = _clienteId },
                    new SqlParameter("@UsuarioID", SqlDbType.Int) { Value = 1 }, // Cambiar por el ID del usuario actual
                    new SqlParameter("@FechaInicio", SqlDbType.DateTime) { Value = DateTime.Now },
                    new SqlParameter("@FechaDevolucion", SqlDbType.DateTime) { Value = DateTime.Now.AddDays(7) },
                    new SqlParameter("@DetallesReservas", SqlDbType.NVarChar) { Value = detallesJson }
                };

                // Ejecutar el stored procedure usando el helper
                int result = DatabaseHelper.Instance.ExecuteStoredProcedure("GenerarFactura", parameters);
                if (result > 0)
                {
                    MessageBox.Show("Factura generada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo generar la factura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Crear un FlowDocument para la vista previa de impresión
                FlowDocument document = CreateInvoiceDocument();

                // Mostrar vista previa
                FlowDocumentScrollViewer viewer = new FlowDocumentScrollViewer
                {
                    Document = document,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                };

                Window previewWindow = new Window
                {
                    Title = "Vista previa de la factura",
                    Width = 800,
                    Height = 600,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this
                };

                // Agregar botón de impresión
                Button printButton = new Button
                {
                    Content = "Imprimir",
                    Margin = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                printButton.Click += (s, args) =>
                {
                    try
                    {
                        PrintDialog printDialog = new PrintDialog();
                        if (printDialog.ShowDialog() == true)
                        {
                            IDocumentPaginatorSource paginator = document as IDocumentPaginatorSource;
                            printDialog.PrintDocument(paginator.DocumentPaginator, "Factura");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al imprimir: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                };

                StackPanel previewPanel = new StackPanel();
                previewPanel.Children.Add(viewer);
                previewPanel.Children.Add(printButton);

                previewWindow.Content = previewPanel;
                previewWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar la factura: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private FlowDocument CreateInvoiceDocument()
        {
            FlowDocument document = new FlowDocument
            {
                FontFamily = new FontFamily("Arial"),
                FontSize = 12,
                PagePadding = new Thickness(40)
            };

            // Encabezado
            Paragraph header = new Paragraph(new Run("SISTEMA DE INVENTARIO - RECIBO DE VENTA"))
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };
            document.Blocks.Add(header);

            // Información del cliente
            Paragraph clientInfo = new Paragraph(new Run($"Cliente ID: {_clienteId}\nNombre: {_nombreCliente} {_apellidoCliente}"))
            {
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Left
            };
            document.Blocks.Add(clientInfo);

            // Detalles de los artículos
            // (El resto del método CreateInvoiceDocument permanece igual)

            return document;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
