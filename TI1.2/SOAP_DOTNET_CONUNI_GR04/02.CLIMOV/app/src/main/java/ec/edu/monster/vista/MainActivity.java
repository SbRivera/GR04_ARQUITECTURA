package ec.edu.monster.vista;

import android.os.Bundle;
import android.view.View;
import android.widget.*;
import androidx.appcompat.app.AppCompatActivity;
import androidx.cardview.widget.CardView;
import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.google.android.material.textfield.TextInputEditText;
import ec.edu.monster.R;
import ec.edu.monster.controller.SoapClient;

public class MainActivity extends AppCompatActivity {

    private Spinner spinnerCategoria, spinnerConversion;
    private TextInputEditText etValor;
    private TextView tvResultado;
    private Button btnConvertir;
    private CardView cardResultado;
    private FloatingActionButton fabLimpiar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        // Inicializar vistas
        spinnerCategoria = findViewById(R.id.spinnerCategoria);
        spinnerConversion = findViewById(R.id.spinnerConversion);
        etValor = findViewById(R.id.etValor);
        tvResultado = findViewById(R.id.tvResultado);
        btnConvertir = findViewById(R.id.btnConvertir);
        cardResultado = findViewById(R.id.cardResultado);
        fabLimpiar = findViewById(R.id.fabLimpiar);

        // Configurar spinner de categorías
        String[] categorias = {"Seleccionar Categoría", "Longitud", "Temperatura", "Masa"};
        ArrayAdapter<String> adapterCat = new ArrayAdapter<>(this, R.layout.spinner_item, categorias);
        adapterCat.setDropDownViewResource(R.layout.spinner_dropdown_item);
        spinnerCategoria.setAdapter(adapterCat);

        // Listener para cambio de categoría
        spinnerCategoria.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                actualizarConversiones(position);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {}
        });

        // Listener para botón convertir
        btnConvertir.setOnClickListener(v -> realizarConversion());
        
        // Listener para botón flotante limpiar
        fabLimpiar.setOnClickListener(v -> limpiarCampos());
    }
    
    /**
     * Limpia el campo de entrada y oculta el resultado
     */
    private void limpiarCampos() {
        etValor.setText("");
        etValor.clearFocus();
        cardResultado.setVisibility(View.GONE);
        Toast.makeText(this, "✨ Campos limpiados", Toast.LENGTH_SHORT).show();
    }

    private void actualizarConversiones(int categoria) {
        String[] conversiones;
        switch (categoria) {
            case 1: // Longitud
                conversiones = new String[]{"Seleccionar Conversión", "Centímetros a Pulgadas", "Pulgadas a Centímetros"};
                break;
            case 2: // Temperatura
                conversiones = new String[]{"Seleccionar Conversión", "Celsius a Fahrenheit", "Fahrenheit a Celsius"};
                break;
            case 3: // Masa
                conversiones = new String[]{"Seleccionar Conversión", "Kilogramos a Libras", "Libras a Kilogramos"};
                break;
            default:
                conversiones = new String[]{"Primero seleccione una categoría"};
                break;
        }
        ArrayAdapter<String> adapterConv = new ArrayAdapter<>(this, R.layout.spinner_item, conversiones);
        adapterConv.setDropDownViewResource(R.layout.spinner_dropdown_item);
        spinnerConversion.setAdapter(adapterConv);
    }

    private void realizarConversion() {
        String valor = etValor.getText().toString().trim();
        
        // ✅ Validación 1: Campo vacío
        if (valor.isEmpty()) {
            Toast.makeText(this, "⚠️ Por favor ingrese un valor", Toast.LENGTH_SHORT).show();
            etValor.setError("Campo requerido");
            return;
        }

        // ✅ Validación 2: Formato numérico válido
        float valorNumerico;
        try {
            valorNumerico = Float.parseFloat(valor);
        } catch (NumberFormatException e) {
            Toast.makeText(this, "⚠️ Ingrese un número válido", Toast.LENGTH_SHORT).show();
            etValor.setError("Formato inválido");
            return;
        }

        // ✅ Validación 3: Selección de categoría y conversión
        int categoria = spinnerCategoria.getSelectedItemPosition();
        int conversion = spinnerConversion.getSelectedItemPosition();

        if (categoria == 0 || conversion == 0) {
            Toast.makeText(this, "⚠️ Por favor seleccione categoría y conversión", Toast.LENGTH_SHORT).show();
            return;
        }

        // ✅ Validación 4: Rangos específicos por tipo de conversión
        if (categoria == 2) { // Temperatura
            if (conversion == 1) { // Celsius a Fahrenheit
                if (valorNumerico < -273.15) {
                    Toast.makeText(this, "⚠️ Temperatura mínima: -273.15°C (cero absoluto)", Toast.LENGTH_LONG).show();
                    etValor.setError("Valor fuera de rango");
                    return;
                }
            } else { // Fahrenheit a Celsius
                if (valorNumerico < -459.67) {
                    Toast.makeText(this, "⚠️ Temperatura mínima: -459.67°F (cero absoluto)", Toast.LENGTH_LONG).show();
                    etValor.setError("Valor fuera de rango");
                    return;
                }
            }
        }

        // ✅ Validación 5: Valores negativos para Longitud y Masa (advertencia)
        if ((categoria == 1 || categoria == 3) && valorNumerico < 0) {
            Toast.makeText(this, "⚠️ Advertencia: Valor negativo detectado", Toast.LENGTH_SHORT).show();
        }

        String metodo = "";
        String unidadOrigen = "";
        String unidadDestino = "";

        // Determinar método SOAP y unidades según selección
        // NOTA: Los métodos .NET usan Mayúscula inicial (PascalCase)
        if (categoria == 1) { // Longitud
            if (conversion == 1) {
                metodo = "CentimetrosAPulgadas";
                unidadOrigen = "cm";
                unidadDestino = "in";
            } else {
                metodo = "PulgadasACentimetros";
                unidadOrigen = "in";
                unidadDestino = "cm";
            }
        } else if (categoria == 2) { // Temperatura
            if (conversion == 1) {
                metodo = "CelsiusAFahrenheit";
                unidadOrigen = "°C";
                unidadDestino = "°F";
            } else {
                metodo = "FahrenheitACelsius";
                unidadOrigen = "°F";
                unidadDestino = "°C";
            }
        } else if (categoria == 3) { // Masa
            if (conversion == 1) {
                metodo = "KilogramosALibras";
                unidadOrigen = "kg";
                unidadDestino = "lb";
            } else {
                metodo = "LibrasAKilogramos";
                unidadOrigen = "lb";
                unidadDestino = "kg";
            }
        }

        convertir(metodo, valor, unidadOrigen, unidadDestino);
    }

    private void convertir(String metodo, String valorStr, String unidadOrigen, String unidadDestino) {
        // Mostrar loading
        cardResultado.setVisibility(View.VISIBLE);
        tvResultado.setText("⏳ Convirtiendo...");
        
        new Thread(() -> {
            try {
                float valor = Float.parseFloat(valorStr);
                float resultado = SoapClient.callConversion(metodo, valor);
                
                runOnUiThread(() -> {
                    // Formato simple con símbolo de unidad
                    String resultadoFormateado = String.format("%.2f %s", 
                        resultado, 
                        unidadDestino
                    );
                    
                    cardResultado.setVisibility(View.VISIBLE);
                    tvResultado.setText(resultadoFormateado);
                });
            } catch (Exception e) {
                runOnUiThread(() -> {
                    // Ocultar la tarjeta de resultado
                    cardResultado.setVisibility(View.GONE);
                    
                    // Mostrar el error como Toast en lugar de en el TextView
                    String mensajeError = "❌ Error de conexión";
                    if (e.getMessage() != null) {
                        if (e.getMessage().contains("Failed to connect") || 
                            e.getMessage().contains("Connection refused")) {
                            mensajeError = "❌ No se puede conectar al servidor";
                        } else if (e.getMessage().contains("timeout")) {
                            mensajeError = "❌ Tiempo de espera agotado";
                        } else if (e.getMessage().contains("Network")) {
                            mensajeError = "❌ Error de red";
                        } else {
                            mensajeError = "❌ Error: " + e.getMessage();
                        }
                    }
                    Toast.makeText(MainActivity.this, mensajeError, Toast.LENGTH_LONG).show();
                });
            }
        }).start();
    }
}
