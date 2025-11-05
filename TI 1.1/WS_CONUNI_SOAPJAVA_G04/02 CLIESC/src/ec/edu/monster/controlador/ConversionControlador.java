package ec.edu.monster.controlador;

import ec.edu.monster.modelo.Conversion;
import ec.edu.monster.vista.VistaConversion;
import ec.edu.monster.ws.WSConversionUnidades;
import ec.edu.monster.ws.WSConversionUnidades_Service;
import javax.swing.JOptionPane;

public class ConversionControlador {
    private final VistaConversion vista;
    private final WSConversionUnidades_Service servicio;
    Float valor = null;
    public ConversionControlador(VistaConversion vista) {
        this.vista = vista;
        this.servicio = new WSConversionUnidades_Service();
         this.vista.setControlador(this); 
    }

    public void convertirCentimetrosAPulgadas() {
        
        try {
                valor =Float.valueOf(vista.obtenerCentimetro()) ;
                
                if (valor < 0) {
                    System.out.println("El valor no puede ser negativo. Intente nuevamente.");
                    valor = null; // Reiniciar el valor para solicitar nuevamente
                }else if(valor>0){
                    Conversion conversion = new Conversion(valor);
                    float resultado = realizarConversionCentimetrosAPulgadas(conversion);
                    JOptionPane.showMessageDialog(vista, resultado + " pulgadas", "Resultado", JOptionPane.INFORMATION_MESSAGE);
                }
            } catch (NumberFormatException e) {
                JOptionPane.showMessageDialog(vista, "Entrada no válida. Asegúrese de ingresar un número positivo.", "Error", JOptionPane.ERROR_MESSAGE);
            
        }
        
    }

    public void convertirPulgadasACentimetros() {
        try {
                valor =Float.valueOf(vista.obtenerPulgada()) ;
                
                if (valor < 0) {
                    System.out.println("El valor no puede ser negativo. Intente nuevamente.");
                    valor = null; // Reiniciar el valor para solicitar nuevamente
                }else if(valor>0){
                    Conversion conversion = new Conversion(valor);
                    float resultado = realizarConversionPulgadasACentimetros(conversion);
                    JOptionPane.showMessageDialog(vista, resultado + " centímetros", "Resultado", JOptionPane.INFORMATION_MESSAGE);
                }
            } catch (NumberFormatException e) {
                JOptionPane.showMessageDialog(vista, "Entrada no válida. Asegúrese de ingresar un número positivo.", "Error", JOptionPane.ERROR_MESSAGE);
        }
        
    }

    private float realizarConversionCentimetrosAPulgadas(Conversion conversion) {
        WSConversionUnidades port = servicio.getWSConversionUnidadesPort();
        return port.centimetrosAPulgadas(conversion.getValor());
    }

    private float realizarConversionPulgadasACentimetros(Conversion conversion) {
        WSConversionUnidades port = servicio.getWSConversionUnidadesPort();
        return port.pulgadasACentimetros(conversion.getValor());
    }
}
