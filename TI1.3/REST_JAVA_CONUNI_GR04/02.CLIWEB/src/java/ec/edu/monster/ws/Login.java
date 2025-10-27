/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.ws;

import jakarta.servlet.*;
import jakarta.servlet.http.*;
import jakarta.servlet.annotation.WebServlet;
import java.io.IOException;

@WebServlet(name = "Login", urlPatterns = {"/Login"})
public class Login extends HttpServlet {

    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp)
            throws ServletException, IOException {
        // Redirigir al formulario de login
        RequestDispatcher rd = req.getRequestDispatcher("/WEB-INF/Paginas/login.jsp");
        rd.forward(req, resp);
    }

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp)
            throws ServletException, IOException {

        String usuario = req.getParameter("usuario");
        String clave = req.getParameter("clave");

        // Validación básica
        if (usuario == null || usuario.trim().isEmpty()) {
            req.setAttribute("error", "El usuario es requerido");
            RequestDispatcher rd = req.getRequestDispatcher("/WEB-INF/Paginas/login.jsp");
            rd.forward(req, resp);
            return;
        }

        if (clave == null || clave.trim().isEmpty()) {
            req.setAttribute("error", "La contraseña es requerida");
            RequestDispatcher rd = req.getRequestDispatcher("/WEB-INF/Paginas/login.jsp");
            rd.forward(req, resp);
            return;
        }

        // Credenciales hardcoded para demo
        if ("MONSTER".equals(usuario) && "MONSTER9".equals(clave)) {
            HttpSession session = req.getSession();
            session.setAttribute("usuario", usuario);
            // Redirigir a la página del conversor usando el servlet
            resp.sendRedirect(req.getContextPath() + "/Conversion");
        } else {
            req.setAttribute("error", "Usuario o contraseña incorrectos");
            RequestDispatcher rd = req.getRequestDispatcher("/WEB-INF/Paginas/login.jsp");
            rd.forward(req, resp);
        }
    }
}
