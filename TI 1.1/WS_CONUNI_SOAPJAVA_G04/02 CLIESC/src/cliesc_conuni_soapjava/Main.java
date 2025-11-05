package cliesc_conuni_soapjava;


import ec.edu.monster.controlador.ConversionControlador;
import ec.edu.monster.controlador.LoginControlador;
import ec.edu.monster.vista.VistaConversion;
import ec.edu.monster.vista.VistaLogin;

public class Main {
    public static void main(String[] args) {
        VistaLogin vistaL = new VistaLogin();
        LoginControlador controladorL = new LoginControlador(vistaL);
      
           vistaL.setVisible(true);
    }
}
