/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/WebServices/WebService.java to edit this template
 */
package ec.edu.monster.ws;

import ec.edu.monster.servicio.SumaService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import jakarta.jws.WebService;

/**
 *
 * @author danie
 */
@WebService(serviceName = "WSSuma")
public class WSSuma {

    /**
     * This is a sample web service operation
     */
    @WebMethod(operationName = "sumar")
    public int sumar(@WebParam(name="n1") int n1, @WebParam(name="n2") int n2){
     SumaService service  = new SumaService();
     int suma = service.sumar(n1,n2);
     return suma;
    }
}
