package ec.edu.monster.controlador;

import ec.edu.monster.modelo.Usuario;
import ec.edu.monster.vista.VistaLogin;
import ec.edu.monster.ws.WSLogin;
import ec.edu.monster.ws.WSLogin_Service;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import javax.swing.JOptionPane;

public class LoginControlador {
    private final VistaLogin vista;
    private final WSLogin_Service servicio;

    public LoginControlador(VistaLogin vista) {
        this.vista = vista;
        this.servicio = new WSLogin_Service();
        this.vista.setControlador(this);  // Asocia la vista con el controlador
    }

    public boolean iniciarLogin() {
        String nombreUsuario = vista.obtenerNombreUsuario();
        String contraseña = vista.obtenerContraseña();

        Usuario usuario = new Usuario(nombreUsuario, hashMD5(contraseña));
        
        String resultado = realizarLogin(usuario);
        boolean loginExitoso = "¡Login exitoso! Bienvenido al sistema.".equals(resultado);

        // Imprime en la consola si el inicio de sesión fue exitoso o no
        if (loginExitoso) {
            System.out.println("Inicio de sesión exitoso");
            vista.setVisible(false);
        } else {
            JOptionPane.showMessageDialog(vista, "Usuario o contraseña incorrectos.", "Error", JOptionPane.ERROR_MESSAGE);
            System.out.println("Inicio de sesión fallido");
        }
        
        return loginExitoso;
    }

    private String realizarLogin(Usuario usuario) {
        WSLogin port = servicio.getWSLoginPort();
        return port.login(usuario.getNombreUsuario(), usuario.getContraseña());
    }
    
    private String hashMD5(String input) {
        try {
            MessageDigest md = MessageDigest.getInstance("MD5");
            byte[] hashBytes = md.digest(input.getBytes());
            
            // Convertir bytes a formato hexadecimal
            StringBuilder hexString = new StringBuilder();
            for (byte b : hashBytes) {
                String hex = Integer.toHexString(0xff & b);
                if (hex.length() == 1) {
                    hexString.append('0');
                }
                hexString.append(hex);
            }
            return hexString.toString().toUpperCase();
        } catch (NoSuchAlgorithmException e) {
            throw new RuntimeException("Error al aplicar hash MD5: " + e.getMessage(), e);
        }
    }
}
