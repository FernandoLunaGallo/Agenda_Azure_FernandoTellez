using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;



namespace Agenda_Azure_FernandoTellez
{
    public class App : Application
    {
        public App()

        {
            MobileServiceClient client;
            IMobileServiceTable<AgendaTabla> tabla;
            client = new MobileServiceClient(Conexion.conexion);
            tabla = client.GetTable<AgendaTabla>();


            Label titulo = new Label();
            titulo.Text = "Agenda Gallo";
            titulo.TextColor = Color.Navy;
            titulo.FontSize = 40;
            titulo.HorizontalTextAlignment = TextAlignment.Center;
            titulo.BackgroundColor = Color.Olive;

            Entry nom = new Entry();
            nom.Placeholder = "Nombre... ";
            nom.HorizontalTextAlignment = TextAlignment.Center;
            nom.TextColor = Color.Black;
            nom.FontSize = 20;

            Entry apellido = new Entry();
            apellido.Placeholder = "Apellido... ";
            apellido.HorizontalTextAlignment = TextAlignment.Center;
            apellido.TextColor = Color.Black;
            apellido.FontSize = 20;

            Entry telefono = new Entry();
            telefono.Placeholder = "Telefono... ";
            telefono.HorizontalTextAlignment = TextAlignment.Center;
            telefono.TextColor = Color.Black;
            telefono.FontSize = 20;
            telefono.Keyboard = Keyboard.Numeric;

            apellido.IsEnabled = false;

            nom.TextChanged += (sender, args) =>
            {
                if (nom.Text == null)
                {
                    apellido.IsEnabled = false;
                    //telefono.IsEnabled = false;
                }
                else
                {
                    apellido.IsEnabled = true;
                    //telefono.IsEnabled = true;
                }
            };

           

            Button enviar = new Button()
            {
                Text = "Insertar Datos"
            };

            Button mostrar = new Button()
            {
                Text = "Mostrar Datos"
            };
            ListView lista = new ListView();


                ListView lista2 = new ListView();
            ListView lista3 = new ListView();
            mostrar.Clicked += async (sender, args) =>
            {
                IEnumerable<AgendaTabla> items = await tabla.ToEnumerableAsync();
                string[] arreglo = new string[items.Count()];
                string[] arreglo2 = new string[items.Count()];
                string[] arreglo3 = new string[items.Count()];
                int i = 0;
                foreach (var x in items)
                {
                    arreglo[i] = x.Name;
                    arreglo2[i] = x.Ape;
                    arreglo3[i] = x.Tel;
                    i++;
                }
                lista.ItemsSource = arreglo;
                lista2.ItemsSource = arreglo2;
                lista3.ItemsSource = arreglo3;
            };
            enviar.Clicked += async (sender, args) =>
            {
                if (nom.Text == null || apellido.Text == null || telefono.Text == null)
                {
                    await MainPage.DisplayAlert("Aviso", "Falta llenar un campo :", "OK");

                }
                else
                {
                    var datos = new AgendaTabla { Name = nom.Text, Ape = apellido.Text, Tel = telefono.Text };
                    await tabla.InsertAsync(datos);
                    IEnumerable<AgendaTabla> items = await tabla.ToEnumerableAsync();
                    string[] arreglo = new string[items.Count()];
                    string[] arreglo2 = new string[items.Count()];
                    int i = 0;
                    foreach (var x in items)
                    {
                        arreglo[i] = x.Name;
                        arreglo2[i] = x.Ape;
                        i++;
                    }
                    nom.Text = null;
                    apellido.Text = null;
                    telefono.Text = null;
                    lista.ItemsSource = arreglo;
                    lista2.ItemsSource = arreglo2;
                }
            };

            Entry buscar = new Entry();
            buscar.Placeholder = "buscar... ";
            buscar.HorizontalTextAlignment = TextAlignment.Center;
            buscar.TextColor = Color.Black;
            buscar.FontSize = 20;
            buscar.TextChanged += async (sender, args) =>
            {
                try
                {
                    IEnumerable<AgendaTabla> items = await tabla.ToEnumerableAsync();
                    string[] arreglo = new string[items.Count()];
                    string[] arreglo2 = new string[items.Count()];
                    string[] arreglo3 = new string[items.Count()];
                    int i = 0;
                    foreach (var x in items)
                    {
                        arreglo[i] = x.Name;
                        arreglo2[i] = x.Ape;
                        arreglo3[i] = x.Tel;
                        i++;


                        if (x.Name == buscar.Text)
                        {

                            nom.Text = x.Name;
                            apellido.Text = x.Ape;
                            telefono.Text = x.Tel;
                        }
                        if (x.Tel == buscar.Text)
                        {

                            nom.Text = x.Name;
                            apellido.Text = x.Ape;
                            telefono.Text = x.Tel;
                        }

                    }

                    lista.ItemsSource = from nombre in arreglo
                                        where nombre.StartsWith("" + buscar.Text)
                                        select nombre;
                }
                catch (Exception ex)
                {
                    await MainPage.DisplayAlert("Alerta", "no existe el contacto ", "OK");
                    buscar.Text = "";

                }
            };
            lista.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            {
                buscar.Text = "" + e.SelectedItem;
            };

            Button actualizar = new Button()
            {
                Text = "Actualizar dato"
            };
            actualizar.Clicked += async (sender, args) =>
            {
                IEnumerable<AgendaTabla> items = await tabla.ToEnumerableAsync();
                string[] arreglo = new string[items.Count()];
                string[] arreglo2 = new string[items.Count()];
                string[] ids = new string[items.Count()];
                string[] arreglo3 = new string[items.Count()];
                int i = 0;
                foreach (var x in items)
                {
                    arreglo[i] = x.Name;
                    arreglo2[i] = x.Ape;
                    ids[i] = x.Id;
                    arreglo3[i] = x.Tel;
                    if (x.Tel == telefono.Text)
                    {
                        if (x.Name != nom.Text)
                        {
                            x.Name = nom.Text;
                        }
                        if (x.Ape != apellido.Text)
                        {
                            x.Ape = apellido.Text;
                        }
                    }
                    await tabla.UpdateAsync(x);
                    i++;
                    nom.Text = null;
                    apellido.Text = null;
                    telefono.Text = null;
                }
                lista.ItemsSource = arreglo;
                lista2.ItemsSource = arreglo2;
            };

            Button Eliminar = new Button()
            {
                Text = "Eliminar dato"
            };
            Eliminar.Clicked += async (sender, args) =>
            {

                IEnumerable<AgendaTabla> items = await tabla
.ToEnumerableAsync();
                string[] arreglo = new string[items.Count()];
                string[] arreglo2 = new string[items.Count()];
                string[] ids = new string[items.Count()];
                string[] arreglo3 = new string[items.Count()];
                int i = 0;
                foreach (var x in items)
                {
                    arreglo[i] = x.Name;
                    arreglo2[i] = x.Ape;
                    ids[i] = x.Id;
                    arreglo3[i] = x.Tel;
                    if (x.Tel == telefono.Text)
                    {
                        if (x.Name != nom.Text)
                        {
                            x.Name = nom.Text;
                        }
                        if (x.Ape != apellido.Text)
                        {
                            x.Ape = apellido.Text;
                        }
                        if (await MainPage.DisplayAlert(" Eliminar el contacto?", "El Numero es: " + x.Tel + "?", "Si", "No"))
                        {
                            await tabla.DeleteAsync(x);
                            nom.Text = null;
                            apellido.Text = null;
                            telefono.Text = null;
                        }
                    }
                    i++;
                }
                lista.ItemsSource = arreglo;
                lista2.ItemsSource = arreglo2;
            };



            var controlGrid = new Grid { RowSpacing = 2, ColumnSpacing = 2 };
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            controlGrid.Children.Add(titulo, 0, 0);
            Grid.SetColumnSpan(titulo, 3);
            controlGrid.Children.Add(nom, 0, 1);
            controlGrid.Children.Add(apellido, 1, 1);
            controlGrid.Children.Add(telefono, 2, 1);

            var controlGrid2 = new Grid { RowSpacing = 2, ColumnSpacing = 2 };
            controlGrid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            controlGrid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            controlGrid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            controlGrid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            controlGrid2.Children.Add(enviar, 0, 0);
            controlGrid2.Children.Add(actualizar, 0, 1);
            controlGrid2.Children.Add(Eliminar, 1, 0);
            controlGrid2.Children.Add(mostrar, 1, 1);

            var controlGrid3 = new Grid { RowSpacing = 2, ColumnSpacing = 2 };
            controlGrid3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            controlGrid3.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            controlGrid3.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            controlGrid3.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //controlGrid3.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
           // controlGrid3.Children.Add(buscar, 0, 0);
            controlGrid3.Children.Add(lista, 1, 0);
            controlGrid3.Children.Add(lista2, 2, 0);
            controlGrid3.Children.Add(lista3, 3, 0);

            var layout = new StackLayout();
            layout.Children.Add(controlGrid);
            layout.Children.Add(controlGrid2);
            layout.Children.Add(controlGrid3);

            MainPage = new ContentPage()
            {
                Content = layout
            };

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
