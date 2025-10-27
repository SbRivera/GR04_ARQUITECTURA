package ec.edu.monster.controlador;

import ec.edu.monster.modelo.ConversionResult;
import ec.edu.monster.modelo.ConversionType;
import ec.edu.monster.servicio.ConUniService;
import ec.edu.monster.vista.ConversionView;

public class ConversionController {
    private final ConversionView view;
    private final ConUniService service;

    public ConversionController(ConversionView view, ConUniService service) {
        this.view = view;
        this.service = service;
    }

    public void performConversion(ConversionType type, double value) {
        if (value < 0 && type != ConversionType.C_TO_F) {
            view.showValidationError("Solo se permiten valores negativos en la conversión Celsius → Fahrenheit.");
            return;
        }
        if (type == ConversionType.C_TO_F && value < -273.15) {
            view.showValidationError("Por debajo de -273.15 °C estamos rompiendo las leyes de la física.");
            return;
        }

        try {
            ConversionResult result = service.convert(type.getPath(), value);
            view.showResult(result);
        } catch (Exception ex) {
            view.showError("No se pudo convertir: " + ex.getMessage());
        }
    }
}