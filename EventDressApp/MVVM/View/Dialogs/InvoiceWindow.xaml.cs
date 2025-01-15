using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

            // Crear detalles de la factura
            CreateInvoiceDetails();
        }

        private void DisplayClientInfo()
        {
            StringBuilder clientInfo = new StringBuilder();
            clientInfo.AppendLine($"Cliente ID: {_clienteId}");
            clientInfo.AppendLine($"Nombre: {_nombreCliente} {_apellidoCliente}");
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

            InvoiceTextBox.Text = invoiceDetails.ToString();
            TotalTextBlock.Text = $"Total: ${totalAmount:F2}";
        }

        private bool GenerateAndStoreInvoice()
        {
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
                    new SqlParameter("@UsuarioID", SqlDbType.Int) { Value = 9 }, // Cambiar por el ID del usuario actual
                    new SqlParameter("@FechaInicio", SqlDbType.DateTime) { Value = DateTime.Now },
                    new SqlParameter("@FechaDevolucion", SqlDbType.DateTime) { Value = DateTime.Now.AddDays(7) },
                    new SqlParameter("@DetallesReservas", SqlDbType.NVarChar) { Value = detallesJson }
                };

                // Ejecutar el stored procedure usando el helper
                int result = DatabaseHelper.Instance.ExecuteStoredProcedure("GenerarFactura", parameters);
                if (result > 0)
                {
                    MessageBox.Show("Factura generada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("No se pudo generar la factura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar la factura: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void PrintInvoice(object sender, RoutedEventArgs e)
        {
            try
            {
                // Intentar generar y almacenar la factura en la base de datos
                if (!GenerateAndStoreInvoice())
                {
                    MessageBox.Show("No se puede imprimir porque la factura no se generó correctamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Crear un FlowDocument para la vista previa de impresión
                FlowDocument document = CreateInvoiceDocument(_selectedRows.Sum(row => Convert.ToDouble(row["precio_diario_prenda"])));

                // Mostrar vista previa
                Window previewWindow = CreatePreviewWindow(document);
                previewWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir la factura: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Window CreatePreviewWindow(FlowDocument document)
        {
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
            return previewWindow;
        }

        private FlowDocument CreateInvoiceDocument(double totalAmount)
        {
            FlowDocument document = new FlowDocument
            {
                FontFamily = new FontFamily("Arial"),
                FontSize = 12,
                PagePadding = new Thickness(40)
            };

            Paragraph header = new Paragraph(new Run("SISTEMA DE INVENTARIO - RECIBO DE VENTA"))
            {
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };
            document.Blocks.Add(header);

            Paragraph clientInfo = new Paragraph(new Run($"Cliente ID: {_clienteId}\nNombre: {_nombreCliente} {_apellidoCliente}"))
            {
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Left,
                Margin = new Thickness(0, 0, 0, 20)
            };
            document.Blocks.Add(clientInfo);

            Table itemsTable = new Table();
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(300) });
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(100) });

            TableRowGroup itemsGroup = new TableRowGroup();
            TableRow headerRow = new TableRow();
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Artículo")) { FontWeight = FontWeights.Bold }));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Precio")) { FontWeight = FontWeights.Bold }));
            itemsGroup.Rows.Add(headerRow);

            foreach (var row in _selectedRows)
            {
                string name = row["nombre_prenda"].ToString();
                double price = Convert.ToDouble(row["precio_diario_prenda"]);

                TableRow itemRow = new TableRow();
                itemRow.Cells.Add(new TableCell(new Paragraph(new Run(name))));
                itemRow.Cells.Add(new TableCell(new Paragraph(new Run($"${price:F2}"))));
                itemsGroup.Rows.Add(itemRow);
            }

            itemsTable.RowGroups.Add(itemsGroup);
            document.Blocks.Add(itemsTable);

            Paragraph totalParagraph = new Paragraph(new Run($"Total: ${totalAmount:F2}"))
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Right,
                Margin = new Thickness(0, 20, 0, 0)
            };
            document.Blocks.Add(totalParagraph);

            return document;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
