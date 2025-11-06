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
        private string _unidadOrigen;
        private string _unidadDestino;
        private string _categoria;

        // Mapeo de conversiones por categoría
        private readonly Dictionary<string, List<string>> _conversionesPorCategoria = new Dictionary<string, List<string>>
        {
            { "📏 Longitud", new List<string> { "Centímetros → Pulgadas", "Pulgadas → Centímetros" } },
            { "🌡️ Temperatura", new List<string> { "Celsius → Fahrenheit", "Fahrenheit → Celsius" } },
            { "⚖️ Peso", new List<string> { "Kilogramos → Libras", "Libras → Kilogramos" } }
        };

        // Mapeo de conversiones directas
        private readonly Dictionary<string, (string origen, string destino, string categoria)> _conversionesDirectas = new Dictionary<string, (string, string, string)>
        {
            { "Centímetros → Pulgadas", ("Centímetros", "Pulgadas", "📏 Longitud") },
            { "Pulgadas → Centímetros", ("Pulgadas", "Centímetros", "📏 Longitud") },
            { "Celsius → Fahrenheit", ("Celsius", "Fahrenheit", "🌡️ Temperatura") },
            { "Fahrenheit → Celsius", ("Fahrenheit", "Celsius", "🌡️ Temperatura") },
            { "Kilogramos → Libras", ("Kilogramos", "Libras", "⚖️ Peso") },
            { "Libras → Kilogramos", ("Libras", "Kilogramos", "⚖️ Peso") }
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

            // Actualizar las conversiones disponibles según la categoría
            if (_conversionesPorCategoria.ContainsKey(_tipoConversionActual))
            {
                var conversiones = _conversionesPorCategoria[_tipoConversionActual];
                PickerConversionDirecta.ItemsSource = conversiones;
                PickerConversionDirecta.SelectedIndex = 0;
            }

            // Limpiar resultado
            OcultarResultado();
            EntryValor.Text = string.Empty;
        }

        private void OnConversionDirectaChanged(object sender, EventArgs e)
        {
            if (PickerConversionDirecta.SelectedIndex == -1)
                return;

            string conversionSeleccionada = PickerConversionDirecta.SelectedItem.ToString();

            if (_conversionesDirectas.ContainsKey(conversionSeleccionada))
            {
                var conversion = _conversionesDirectas[conversionSeleccionada];
                _unidadOrigen = conversion.origen;
                _unidadDestino = conversion.destino;
                _categoria = conversion.categoria;
            }

            // Limpiar resultado
            OcultarResultado();
            EntryValor.Text = string.Empty;
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

            if (PickerConversionDirecta.SelectedIndex == -1)
            {
                await DisplayAlert("⚠️ Error", "Por favor, seleccione el tipo de conversión", "OK");
                return;
            }

            try
            {
                // Mostrar loading
                MostrarLoading(true);
                OcultarResultado();

                double resultado = 0;

                // Llamar al método correcto según el tipo de conversión
                switch (_categoria)
                {
                    case "📏 Longitud":
                        resultado = await ConvertirLongitud(valor, _unidadOrigen, _unidadDestino);
                        break;

                    case "🌡️ Temperatura":
                        resultado = await ConvertirTemperatura(valor, _unidadOrigen, _unidadDestino);
                        break;

                    case "⚖️ Peso":
                        resultado = await ConvertirPeso(valor, _unidadOrigen, _unidadDestino);
                        break;
                }

                // Mostrar resultado
                MostrarResultado(resultado, valor, _unidadOrigen, _unidadDestino);
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
            PickerTipoConversion.SelectedIndex = 0;
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
            PickerConversionDirecta.IsEnabled = !mostrar;
            EntryValor.IsEnabled = !mostrar;
            BtnConvertir.Text = mostrar ? "CONVIRTIENDO..." : "CONVERTIR";
        }

        private void MostrarResultado(double resultado, double valorOriginal, string unidadOrigen, string unidadDestino)
        {
            LabelResultado.Text = $"{resultado:F2} {unidadDestino}";
            LabelDetalles.Text = $"{valorOriginal:F2} {unidadOrigen} = {resultado:F2} {unidadDestino}";
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
