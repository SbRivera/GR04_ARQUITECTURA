<%@page contentType="text/html" pageEncoding="UTF-8"%>
<%@page import="jakarta.servlet.http.HttpSession"%>
<%@taglib prefix="c" uri="jakarta.tags.core"%>
<%
    HttpSession sesion = request.getSession(false);
    if (sesion == null || sesion.getAttribute("usuario") == null) {
        response.sendRedirect(request.getContextPath() + "/index.jsp");
        return;
    }
%>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Conversión de Unidades</title>
    <link rel="stylesheet" href="<%= request.getContextPath() %>/styles.css">
</head>
<body class="main-bg">
    <div className="layout">
        <header class="hero">
            <div>
                <h1>Conversor Universal</h1>
                <p>Transforma magnitudes con precisión cuántica.</p>
            </div>
            <div class="user-badge">
                <span>Conectado como</span>
                <strong><%= sesion.getAttribute("usuario") %></strong>
            </div>
        </header>

        <main class="grid">
            <!-- Longitud -->
            <section class="card">
                <h2>Longitud</h2>
                <form action="<%= request.getContextPath() %>/convertir" method="post"
                      data-role="conversion" data-allow-negative="false">
                    <div class="field">
                        <label for="valor-longitud">Valor</label>
                        <input type="number" id="valor-longitud" name="valor"
                               step="any" min="0" placeholder="Ej. 12.5" required>
                    </div>
                    <div class="field">
                        <label for="tipo-longitud">Conversión</label>
                        <select id="tipo-longitud" name="tipo" required>
                            <option value="cmToIn">Centímetros → Pulgadas</option>
                            <option value="inToCm">Pulgadas → Centímetros</option>
                        </select>
                    </div>
                    <div class="actions">
                        <button type="submit">Convertir</button>
                    </div>
                </form>
            </section>

            <!-- Temperatura -->
            <section class="card">
                <h2>Temperatura</h2>
                <form action="<%= request.getContextPath() %>/convertir" method="post"
                      data-role="conversion" data-allow-negative="true">
                    <div class="field">
                        <label for="valor-temperatura">Valor</label>
                        <input type="number" id="valor-temperatura" name="valor"
                               step="any" placeholder="Ej. -18" required>
                    </div>
                    <div class="field">
                        <label for="tipo-temperatura">Conversión</label>
                        <select id="tipo-temperatura" name="tipo" required>
                            <option value="cToF">Celsius → Fahrenheit</option>
                            <option value="fToC">Fahrenheit → Celsius</option>
                        </select>
                    </div>
                    <div class="actions">
                        <button type="submit">Convertir</button>
                    </div>
                </form>
            </section>

            <!-- Masa -->
            <section class="card">
                <h2>Masa</h2>
                <form action="<%= request.getContextPath() %>/convertir" method="post"
                      data-role="conversion" data-allow-negative="false">
                    <div class="field">
                        <label for="valor-masa">Valor</label>
                        <input type="number" id="valor-masa" name="valor"
                               step="any" min="0" placeholder="Ej. 4.3" required>
                    </div>
                    <div class="field">
                        <label for="tipo-masa">Conversión</label>
                        <select id="tipo-masa" name="tipo" required>
                            <option value="kgToLb">Kilogramos → Libras</option>
                            <option value="lbToKg">Libras → Kilogramos</option>
                        </select>
                    </div>
                    <div class="actions">
                        <button type="submit">Convertir</button>
                    </div>
                </form>
            </section>
        </main>

        <aside class="result-card">
            <h2>Resultado</h2>
            <c:choose>
                <c:when test="${not empty param.resultado}">
                    <p class="result-value">${param.resultado}</p>
                    <p class="result-details">
                        Valor inicial: <span>${param.valor}</span><br>
                        Operación: <span>${param.tipo}</span>
                    </p>
                </c:when>
                <c:when test="${not empty param.error}">
                    <p class="alert">${param.error}</p>
                </c:when>
                <c:otherwise>
                    <p class="placeholder">Ingresa un valor y obtén el resultado al instante.</p>
                </c:otherwise>
            </c:choose>
        </aside>

        <footer class="footer">
            <form action="<%= request.getContextPath() %>/index.jsp" method="get">
                <button type="submit" class="logout">Cerrar sesión</button>
            </form>
        </footer>
    </div>

    <script>
        document.querySelectorAll('form[data-role="conversion"]').forEach(form => {
            const input = form.querySelector('input[name="valor"]');
            const allowNegative = form.dataset.allowNegative === "true";

            form.addEventListener('submit', event => {
                const value = parseFloat(input.value);
                if (!Number.isFinite(value)) {
                    event.preventDefault();
                    mostrarMensaje("Por favor ingresa un número válido.");
                    return;
                }
                if (!allowNegative && value < 0) {
                    event.preventDefault();
                    mostrarMensaje("Para esta categoría solo se aceptan valores mayores o iguales a 0.");
                    input.focus();
                }
            });
        });

        function mostrarMensaje(texto) {
            let banner = document.querySelector('.banner');
            if (!banner) {
                banner = document.createElement('div');
                banner.className = 'banner';
                document.body.appendChild(banner);
            }
            banner.textContent = texto;
            banner.classList.add('visible');
            setTimeout(() => banner.classList.remove('visible'), 3200);
        }
    </script>
</body>
</html>