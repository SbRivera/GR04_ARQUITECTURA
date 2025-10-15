/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.prueba;
import ec.edu.monster.servicio.SumaService;


/**
 *
 * @author danie
 */
public class Prueba01 {
    public static void main (String[] arg){
        //Datos
        int n1=18;
        int n2=15;
        
        //Proceso
        SumaService service = new SumaService();
        int suma = service.sumar(n1, n2);
        
        //Reporte
        System.out.println("n1: " + n1);
        System.out.println("n2: " + n2);
        System.out.println("suma: " + suma);


    }
}
