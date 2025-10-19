/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.controlador;

import jakarta.servlet.*;
import jakarta.servlet.http.*;
import jakarta.servlet.annotation.*;
import java.io.IOException;

@WebServlet(name = "LoginServlet", urlPatterns = {"/login"})
public class LoginServlet extends HttpServlet {

    private static final String USER = "MONSTER";
    private static final String PASS = "MONSTER9";

    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {

        String usuario = request.getParameter("usuario");
        String password = request.getParameter("password");

        if (USER.equals(usuario) && PASS.equals(password)) {
            // Guardar sesión
            HttpSession sesion = request.getSession();
            sesion.setAttribute("usuario", usuario);

            // Redirigir al programa principal
            response.sendRedirect("conversion.jsp");
        } else {
            // Redirigir con mensaje de error
            response.sendRedirect("index.jsp?error=true");
        }
    }
}

