/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.servicio;

import com.google.gson.Gson;
import ec.edu.monster.modelo.Resultado;
import ec.edu.monster.util.HttpUtil;

public class ConUniServicio {

    private static final String BASE_URL = "http://localhost:8080/WS_ConUni_REST_JAVA_GR04/webresources/ConUni";

    public Resultado convertir(String tipo, double valor) throws Exception {
        String url = BASE_URL + "/" + tipo + "?value=" + valor;
        String json = HttpUtil.getJsonFromUrl(url);
        return new Gson().fromJson(json, Resultado.class);
    }
}
