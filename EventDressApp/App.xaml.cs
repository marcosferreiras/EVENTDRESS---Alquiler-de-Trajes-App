using System;
using System.Windows;
using EventDressApp.MVVM.Model;
using EventDressApp.Views;
using Microsoft.Data.SqlClient; // Asegúrate de usar el mismo namespace que en DatabaseHelper

namespace EventDressApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Verifica si existen datos de la empresa
                var empresaData = DatabaseHelper.Instance.GetAllRecords("empresa");

                if (empresaData.Rows.Count == 0)
                {
                    // Si no hay datos de la empresa, muestra SetupWindow
                    SetupWindow setupWindow = new SetupWindow();
                    bool? result = setupWindow.ShowDialog();

                    if (result != true)
                    {
                        // Si el usuario cancela el formulario, cierra la aplicación
                        Shutdown();
                        return;
                    }
                }

                // Si todo está correcto, abre la ventana de login
                var loginWindow = new LoginWindow();
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la aplicación: {ex.Message}");
                Shutdown(); // Cierra la aplicación en caso de error
            }
        }
    }
}
        /*


                        // Inserta los datos en la base de datos
                        string insertQuery = "INSERT INTO empresa (empresa_id, nombre_empresa, logo_empresa, direccion_empresa, telefono_empresa, email_empresa, sitioweb_empresa, fecharegistro_empresa) VALUES (@EmpresaId, @NombreEmpresa, @LogoEmpresa, @DireccionEmpresa, @TelefonoEmpresa, @EmailEmpresa, @SitioWebEmpresa, @FechaRegistroEmpresa)";

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@EmpresaId", 1),
                            new SqlParameter("@NombreEmpresa", setupWindow.NombreEmpresa),
                            new SqlParameter("@LogoEmpresa", setupWindow.LogoEmpresa),
                            new SqlParameter("@DireccionEmpresa", setupWindow.DireccionEmpresa),
                            new SqlParameter("@TelefonoEmpresa", setupWindow.TelefonoEmpresa),
                            new SqlParameter("@EmailEmpresa", setupWindow.EmailEmpresa),
                            new SqlParameter("@SitioWebEmpresa", setupWindow.SitioWebEmpresa),
                            new SqlParameter("@FechaRegistroEmpresa", DateTime.Now)
                        };

                        DatabaseHelper.Instance.ExecuteNonQuery(insertQuery, parameters);
                    }
                    else
                    {
                        // Si el usuario cancela el formulario, cierra la aplicación
                        Shutdown();
                        return;
                    }
                }

                // Si todo está correcto, abre la ventana principal
                var loginWindow = new LoginWindow();
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la aplicación: {ex.Message}");
                Shutdown(); // Cierra la aplicación en caso de error
            }
        }
    }
}*/
