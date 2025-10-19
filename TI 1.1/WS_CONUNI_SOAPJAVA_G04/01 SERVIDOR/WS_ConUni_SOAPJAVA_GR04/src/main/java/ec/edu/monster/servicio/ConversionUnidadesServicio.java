package ec.edu.monster.servicio;

/**
 *
 * @author danie
 */
public class ConversionUnidadesServicio {

    // Conversión de longitud
    public float centimetrosAPulgadas(float centimetros) {
        return (centimetros / 2.54f);
    }

    public float pulgadasACentimetros(float pulgadas) {
        return (pulgadas * 2.54f);
    }

    // Conversión de temperatura
    public float celsiusAFahrenheit(float celsius) {
        return (celsius * 9 / 5) + 32;
    }

    public float fahrenheitACelsius(float fahrenheit) {
        return (fahrenheit - 32) * 5 / 9;
    }

    // Conversión de masa
    public float kilogramosALibras(float kilogramos) {
        return (kilogramos * 2.20462f);
    }

    public float librasAKilogramos(float libras) {
        return (libras / 2.20462f);
    }
}
