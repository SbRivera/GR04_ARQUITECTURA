using _02.CLIMOV.Servicio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Util.EventLogTags;

namespace _02.CLIMOV.Vista
{
    public partial class ConversionPage : ContentPage
    {
        private readonly ISoapService _soapService;
        private string _tipoConversionActual;
        private string _unidadOrigen;
        private string _unidadDestino;

        // Diccionarios de conversiones por tipo
        private readonly Dictionary<string, List<string>> _conversionesPorTipo = new Dictionary<string, List<string>>
        {
            { "📏 Longitud", new List<string> { "Centímetros → Pulgadas", "Pulgadas → Centímetros" } },
            { "🌡️ Temperatura", new List<string> { "Celsius → Fahrenheit", "Fahrenheit → Celsius" } },
            { "⚖️ Peso", new List<string> { "Kilogramos → Libras", "Libras → Kilogramos" } }
        };

        public ConversionPage()
        {
            InitializeComponent();

            // Inicializar servicio
            _soapService = new SoapService();

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

            // Actualizar las opciones de conversión según el tipo seleccionado
            var conversiones = _conversionesPorTipo[_tipoConversionActual];

            PickerConversion.ItemsSource = conversiones;
            PickerConversion.SelectedIndex = 0;

            // Inicializar las unidades con la primera opción
            if (conversiones.Count > 0)
            {
                var partes = conversiones[0].Split('→');
                if (partes.Length == 2)
                {
                    _unidadOrigen = partes[0].Trim();
                    _unidadDestino = partes[1].Trim();
                }
            }

            // Limpiar resultado
            OcultarResultado();
            EntryValor.Text = string.Empty;
        }

        private void OnConversionChanged(object sender, EventArgs e)
        {
            if (PickerConversion.SelectedIndex == -1)
                return;

            string conversionSeleccionada = PickerConversion.SelectedItem.ToString();
            
            // Parsear la conversión seleccionada para obtener origen y destino
            var partes = conversionSeleccionada.Split('→');
            if (partes.Length == 2)
            {
                _unidadOrigen = partes[0].Trim();
                _unidadDestino = partes[1].Trim();
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

            if (PickerConversion.SelectedIndex == -1)
            {
                await DisplayAlert("⚠️ Error", "Por favor, seleccione el tipo de conversión", "OK");
                return;
            }

            try
            {
                // Mostrar loading
                MostrarLoading(true);
                OcultarResultado();

                float resultado = 0;

                // Llamar al método correcto según el tipo de conversión
                switch (_tipoConversionActual)
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

        private async Task<float> ConvertirLongitud(float valor, string origen, string destino)
        {
            if (origen == "Centímetros" && destino == "Pulgadas")
                return await _soapService.CentimetrosAPulgadasAsync(valor);
            else if (origen == "Pulgadas" && destino == "Centímetros")
                return await _soapService.PulgadasACentimetrosAsync(valor);

            return valor;
        }

        private async Task<float> ConvertirTemperatura(float valor, string origen, string destino)
        {
            if (origen == "Celsius" && destino == "Fahrenheit")
                return await _soapService.CelsiusAFahrenheitAsync(valor);
            else if (origen == "Fahrenheit" && destino == "Celsius")
                return await _soapService.FahrenheitACelsiusAsync(valor);

            return valor;
        }

        private async Task<float> ConvertirPeso(float valor, string origen, string destino)
        {
            if (origen == "Kilogramos" && destino == "Libras")
                return await _soapService.KilogramosALibrasAsync(valor);
            else if (origen == "Libras" && destino == "Kilogramos")
                return await _soapService.LibrasAKilogramosAsync(valor);

            return valor;
        }

        private void OnLimpiarClicked(object sender, EventArgs e)
        {
            EntryValor.Text = string.Empty;
            OcultarResultado();
            
            if (PickerConversion.ItemsSource != null && PickerConversion.ItemsSource is List<string> list && list.Count > 0)
            {
                PickerConversion.SelectedIndex = 0;
                
                // Reinicializar las unidades con la primera opción
                var partes = list[0].Split('→');
                if (partes.Length == 2)
                {
                    _unidadOrigen = partes[0].Trim();
                    _unidadDestino = partes[1].Trim();
                }
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
            PickerTipoConversion.IsEnabled = !mostrar;
            PickerConversion.IsEnabled = !mostrar;
            EntryValor.IsEnabled = !mostrar;
            BtnConvertir.Text = mostrar ? "CONVIRTIENDO..." : "CONVERTIR";
        }

        private void MostrarResultado(float resultado, float valorOriginal, string unidadOrigen, string unidadDestino)
        {
            LabelResultado.Text = $"{resultado.ToString("F2", CultureInfo.InvariantCulture)} {unidadDestino}";
            LabelDetalles.Text = $"{valorOriginal.ToString("F2", CultureInfo.InvariantCulture)} {unidadOrigen} = " +
                                  $"{resultado.ToString("F2", CultureInfo.InvariantCulture)} {unidadDestino}";
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
