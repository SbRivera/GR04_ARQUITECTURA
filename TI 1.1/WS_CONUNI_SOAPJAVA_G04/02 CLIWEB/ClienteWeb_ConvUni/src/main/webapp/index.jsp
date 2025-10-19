<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Login - Cliente WS</title>
    <link rel="stylesheet" href="styles.css">
</head>
<body class="login-bg">
    <div class="login-wrapper">
        <section class="login-card">
            <h1>Panel Quantum</h1>
            <p>Ingresa tus credenciales para acceder al conversor.</p>

            <form action="login" method="post">
                <div class="field">
                    <label for="usuario">Usuario</label>
                    <input type="text" id="usuario" name="usuario" autocomplete="username" required>
                </div>

                <div class="field">
                    <label for="password">Contraseña</label>
                    <input type="password" id="password" name="password"
                           autocomplete="current-password" required>
                </div>

                <button type="submit">Iniciar Sesión</button>

                <c:if test="${not empty param.error}">
                    <p class="alert">Usuario o contraseña incorrectos.</p>
                </c:if>
            </form>
        </section>
    </div>
</body>
</html>