using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02.CLIMOV.Servicio;

namespace _02.CLIMOV.Vista
{
    public partial class ConversionPage : ContentPage
    {
        private readonly IRestService _restService;
        private string _tipoConversionActual;

        // Diccionarios de unidades por tipo
        private readonly Dictionary<string, List<string>> _unidadesPorTipo = new Dictionary<string, List<string>>
        {
            { "📏 Longitud", new List<string> { "Centímetros", "Pulgadas" } },
            { "🌡️ Temperatura", new List<string> { "Celsius", "Fahrenheit" } },
            { "⚖️ Peso", new List<string> { "Kilogramos", "Libras" } }
        };

        public ConversionPage()
        {
            InitializeComponent();

            // Inicializar servicio REST
            _restService = new RestService();

            // Configurar tipo de conversión por defecto
            PickerTipoConversion.SelectedIndex = 0;

            // Mostrar usuario logueado
            string username = Preferences.Get("username", "Usuario");
            LabelUsuario.Text = $"Bienvenido, {username}";
        }

        private void OnTipoConversionChanged(object sender, EventArgs e)
        {
            if (PickerTipoConversion.SelectedIndex == -1)
                return;

            _tipoConversionActual = PickerTipoConversion.SelectedItem.ToString();

            // Actualizar las unidades según el tipo seleccionado
            var unidades = _unidadesPorTipo[_tipoConversionActual];

            PickerOrigen.ItemsSource = unidades;
            PickerDestino.ItemsSource = unidades;

            PickerOrigen.SelectedIndex = 0;
            PickerDestino.SelectedIndex = 1;

            // Actualizar texto informativo
            ActualizarTextoConversion();

            // Limpiar resultado
            OcultarResultado();
            EntryValor.Text = string.Empty;
        }

        private void OnPickerSelectionChanged(object sender, EventArgs e)
        {
            ActualizarTextoConversion();
        }

        private void ActualizarTextoConversion()
        {
            if (PickerOrigen.SelectedIndex != -1 && PickerDestino.SelectedIndex != -1)
            {
                string origen = PickerOrigen.SelectedItem?.ToString() ?? "";
                string destino = PickerDestino.SelectedItem?.ToString() ?? "";
                LabelConversionInfo.Text = $"Convertir de {origen} a {destino}";
                LabelConversionInfo.TextColor = Color.FromArgb("#F97316");
                LabelConversionInfo.FontAttributes = FontAttributes.Bold;
            }
            else
            {
                LabelConversionInfo.Text = "Seleccione ambas unidades";
                LabelConversionInfo.TextColor = Color.FromArgb("#6B7280");
                LabelConversionInfo.FontAttributes = FontAttributes.None;
            }
        }

        private async void OnConvertirClicked(object sender, EventArgs e)
        {
            // Validar entrada
            if (string.IsNullOrWhiteSpace(EntryValor.Text))
            {
                await DisplayAlert("⚠️ Error", "Por favor, ingrese un valor para convertir", "OK");
                return;
            }

            if (!float.TryParse(EntryValor.Text, out float valor))
            {
                await DisplayAlert("⚠️ Error", "Por favor, ingrese un número válido", "OK");
                return;
            }

            if (PickerOrigen.SelectedIndex == -1 || PickerDestino.SelectedIndex == -1)
            {
                await DisplayAlert("⚠️ Error", "Por favor, seleccione las unidades de origen y destino", "OK");
                return;
            }

            string unidadOrigen = PickerOrigen.SelectedItem.ToString();
            string unidadDestino = PickerDestino.SelectedItem.ToString();

            if (unidadOrigen == unidadDestino)
            {
                await DisplayAlert("⚠️ Advertencia", "Las unidades de origen y destino son iguales", "OK");
                return;
            }

            try
            {
                // Mostrar loading
                MostrarLoading(true);
                OcultarResultado();

                double resultado = 0;

                // Llamar al método correcto según el tipo de conversión
                switch (_tipoConversionActual)
                {
                    case "📏 Longitud":
                        resultado = await ConvertirLongitud(valor, unidadOrigen, unidadDestino);
                        break;

                    case "🌡️ Temperatura":
                        resultado = await ConvertirTemperatura(valor, unidadOrigen, unidadDestino);
                        break;

                    case "⚖️ Peso":
                        resultado = await ConvertirPeso(valor, unidadOrigen, unidadDestino);
                        break;
                }

                // Mostrar resultado
                MostrarResultado(resultado, valor, unidadOrigen, unidadDestino);
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Error", $"Error al realizar la conversión:\n{ex.Message}", "OK");
            }
            finally
            {
                MostrarLoading(false);
            }
        }

        private async Task<double> ConvertirLongitud(double valor, string origen, string destino)
        {
            if (origen == "Centímetros" && destino == "Pulgadas")
                return await _restService.CentimetrosAPulgadasAsync(valor);
            else if (origen == "Pulgadas" && destino == "Centímetros")
                return await _restService.PulgadasACentimetrosAsync(valor);

            return valor;
        }

        private async Task<double> ConvertirTemperatura(double valor, string origen, string destino)
        {
            if (origen == "Celsius" && destino == "Fahrenheit")
                return await _restService.CelsiusAFahrenheitAsync(valor);
            else if (origen == "Fahrenheit" && destino == "Celsius")
                return await _restService.FahrenheitACelsiusAsync(valor);

            return valor;
        }

        private async Task<double> ConvertirPeso(double valor, string origen, string destino)
        {
            if (origen == "Kilogramos" && destino == "Libras")
                return await _restService.KilogramosALibrasAsync(valor);
            else if (origen == "Libras" && destino == "Kilogramos")
                return await _restService.LibrasAKilogramosAsync(valor);

            return valor;
        }

        private void OnLimpiarClicked(object sender, EventArgs e)
        {
            EntryValor.Text = string.Empty;
            OcultarResultado();
            
            if (PickerOrigen.ItemsSource != null && PickerOrigen.ItemsSource is List<string> list && list.Count > 0)
            {
                PickerOrigen.SelectedIndex = 0;
                PickerDestino.SelectedIndex = Math.Min(1, list.Count - 1);
            }
        }

        private async void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            bool confirmacion = await DisplayAlert(
                "🚪 Cerrar Sesión",
                "¿Está seguro que desea cerrar sesión?",
                "Sí, salir",
                "Cancelar"
            );

            if (confirmacion)
            {
                // Limpiar preferencias
                Preferences.Remove("isLoggedIn");
                Preferences.Remove("username");

                // Navegar a login
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        private void MostrarLoading(bool mostrar)
        {
            LoadingIndicator.IsRunning = mostrar;
            LoadingIndicator.IsVisible = mostrar;
            BtnConvertir.IsEnabled = !mostrar;
            BtnLimpiar.IsEnabled = !mostrar;
            PickerTipoConversion.IsEnabled = !mostrar;
            PickerOrigen.IsEnabled = !mostrar;
            PickerDestino.IsEnabled = !mostrar;
            EntryValor.IsEnabled = !mostrar;
            BtnConvertir.Text = mostrar ? "CONVIRTIENDO..." : "CONVERTIR";
        }

        private void MostrarResultado(double resultado, double valorOriginal, string unidadOrigen, string unidadDestino)
        {
            LabelResultado.Text = $"{resultado:F4} {unidadDestino}";
            LabelDetalles.Text = $"{valorOriginal} {unidadOrigen} = {resultado:F4} {unidadDestino}";
            FrameResultado.IsVisible = true;
        }

        private void OcultarResultado()
        {
            FrameResultado.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Actualizar nombre de usuario si cambió
            string username = Preferences.Get("username", "Usuario");
            LabelUsuario.Text = $"Bienvenido, {username}";
        }
    }
}
