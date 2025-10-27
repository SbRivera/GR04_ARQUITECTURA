/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.ws;

import ec.edu.monster.servicio.ConUniServicio;
import ec.edu.monster.modelo.Resultado;
import jakarta.servlet.*;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.*;
import java.io.IOException;

@WebServlet(name = "Conversion", urlPatterns = {"/Conversion"})
public class Conversion extends HttpServlet {

    private ConUniServicio service = new ConUniServicio();

    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp)
            throws ServletException, IOException {
        // Verificar si el usuario está logueado
        HttpSession session = req.getSession(false);
        if (session == null || session.getAttribute("usuario") == null) {
            resp.sendRedirect(req.getContextPath() + "/Login");
            return;
        }
        
        // Mostrar página del conversor
        RequestDispatcher rd = req.getRequestDispatcher("/WEB-INF/Paginas/conversion.jsp");
        rd.forward(req, resp);
    }

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp)
            throws ServletException, IOException {

        // Verificar si el usuario está logueado
        HttpSession session = req.getSession(false);
        if (session == null || session.getAttribute("usuario") == null) {
            resp.sendRedirect(req.getContextPath() + "/Login");
            return;
        }

        String tipo = req.getParameter("tipo");
        String valorStr = req.getParameter("valor");

        // Validaciones
        if (tipo == null || tipo.trim().isEmpty()) {
            req.setAttribute("error", "Debe seleccionar un tipo de conversión");
            RequestDispatcher rd = req.getRequestDispatcher("/WEB-INF/Paginas/conversion.jsp");
            rd.forward(req, resp);
            return;
        }

        if (valorStr == null || valorStr.trim().isEmpty()) {
            req.setAttribute("error", "Debe ingresar un valor numérico");
            RequestDispatcher rd = req.getRequestDispatcher("/WEB-INF/Paginas/conversion.jsp");
            rd.forward(req, resp);
            return;
        }

        try {
            double valor = Double.parseDouble(valorStr);
            
            if (valor < 0) {
                req.setAttribute("error", "El valor no puede ser negativo");
                RequestDispatcher rd = req.getRequestDispatcher("/WEB-INF/Paginas/conversion.jsp");
                rd.forward(req, resp);
                return;
            }

            Resultado r = service.convertir(tipo, valor);
            req.setAttribute("resultado", r);
            RequestDispatcher rd = req.getRequestDispatcher("/WEB-INF/Paginas/conversion.jsp");
            rd.forward(req, resp);
        } catch (NumberFormatException e) {
            req.setAttribute("error", "Valor numérico inválido");
            RequestDispatcher rd = req.getRequestDispatcher("/WEB-INF/Paginas/conversion.jsp");
            rd.forward(req, resp);
        } catch (Exception e) {
            e.printStackTrace();
            req.setAttribute("error", "Error al conectar con el servicio REST: " + e.getMessage());
            RequestDispatcher rd = req.getRequestDispatcher("/WEB-INF/Paginas/conversion.jsp");
            rd.forward(req, resp);
        }
    }
}