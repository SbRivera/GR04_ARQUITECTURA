<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Conversor de Unidades - Iniciar Sesión</title>
    
    <!-- Fuente Google -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600;700;800&display=swap" rel="stylesheet">
    
    <style>
        /* ====== LOGIN PREMIUM STYLE ====== */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        
        body {
            font-family: 'Poppins', sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 50%, #f093fb 100%);
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            overflow: hidden;
            position: relative;
        }

        /* Efecto de partículas animadas en el fondo */
        body::before {
            content: '';
            position: absolute;
            width: 200%;
            height: 200%;
            background: 
                radial-gradient(circle at 20% 50%, rgba(255, 255, 255, 0.1) 0%, transparent 50%),
                radial-gradient(circle at 80% 80%, rgba(255, 255, 255, 0.08) 0%, transparent 50%),
                radial-gradient(circle at 40% 20%, rgba(255, 255, 255, 0.06) 0%, transparent 50%);
            animation: drift 20s ease-in-out infinite;
            pointer-events: none;
        }

        @keyframes drift {
            0%, 100% { transform: translate(0, 0); }
            50% { transform: translate(-50px, -30px); }
        }

        .login-container {
            display: flex;
            justify-content: center;
            align-items: center;
            width: 100%;
            padding: 20px;
            position: relative;
            z-index: 1;
        }

        .login-card {
            background: rgba(255, 255, 255, 0.15);
            backdrop-filter: blur(25px) saturate(180%);
            border: 1px solid rgba(255, 255, 255, 0.25);
            border-radius: 30px;
            padding: 45px 40px 40px;
            width: 100%;
            max-width: 420px;
            text-align: center;
            transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
            position: relative;
            box-shadow: 
                0 20px 60px rgba(0, 0, 0, 0.3),
                0 0 100px rgba(255, 255, 255, 0.1) inset;
            animation: float 4s ease-in-out infinite;
        }

        @keyframes float {
            0%, 100% { transform: translateY(0) scale(1); }
            50% { transform: translateY(-12px) scale(1.01); }
        }

        .login-card::before {
            content: '';
            position: absolute;
            top: -1px;
            left: -1px;
            right: -1px;
            bottom: -1px;
            border-radius: 30px;
            background: linear-gradient(135deg, rgba(255,255,255,0.3), rgba(255,255,255,0) 50%);
            z-index: -1;
            opacity: 0;
            transition: opacity 0.3s ease;
        }

        .login-card:hover::before {
            opacity: 1;
        }

        .avatar {
            width: 140px;
            height: 140px;
            border-radius: 50%;
            object-fit: cover;
            margin: 0 auto 20px;
            box-shadow: 
                0 8px 32px rgba(0, 0, 0, 0.3),
                0 0 0 4px rgba(255, 255, 255, 0.2);
            transition: all 0.3s ease;
            border: 3px solid rgba(255, 255, 255, 0.4);
        }

        .login-card:hover .avatar {
            transform: scale(1.05);
            box-shadow: 
                0 12px 40px rgba(0, 0, 0, 0.4),
                0 0 0 6px rgba(255, 255, 255, 0.3);
        }

        h1 {
            color: #fff;
            font-size: 32px;
            font-weight: 700;
            margin: 10px 0 8px;
            text-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
            letter-spacing: -0.5px;
        }

        p {
            color: rgba(255, 255, 255, 0.9);
            margin-bottom: 28px;
            font-size: 15px;
            font-weight: 300;
        }

        .input-group {
            margin-bottom: 18px;
            position: relative;
        }

        .input-group input {
            width: 100%;
            padding: 16px 18px;
            border: 2px solid rgba(255, 255, 255, 0.25);
            border-radius: 15px;
            background: rgba(255, 255, 255, 0.18);
            color: #fff;
            font-size: 15px;
            outline: none;
            transition: all 0.3s ease;
            font-weight: 400;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1) inset;
        }

        .input-group input::placeholder {
            color: rgba(255, 255, 255, 0.7);
            font-weight: 300;
        }

        .input-group input:focus {
            background: rgba(255, 255, 255, 0.25);
            border-color: rgba(255, 255, 255, 0.5);
            box-shadow: 
                0 0 0 3px rgba(255, 255, 255, 0.1),
                0 4px 15px rgba(0, 0, 0, 0.15) inset;
            transform: translateY(-2px);
        }

        .input-group input:hover {
            border-color: rgba(255, 255, 255, 0.35);
        }

        .btn-login {
            width: 100%;
            padding: 16px;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: #fff;
            border: none;
            border-radius: 15px;
            font-size: 16px;
            font-weight: 700;
            letter-spacing: 1px;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            box-shadow: 
                0 10px 30px rgba(102, 126, 234, 0.4),
                0 0 0 0 rgba(255, 255, 255, 0.3);
            text-transform: uppercase;
            position: relative;
            overflow: hidden;
        }

        .btn-login::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.3), transparent);
            transition: left 0.5s ease;
        }

        .btn-login:hover::before {
            left: 100%;
        }

        .btn-login:hover {
            transform: translateY(-3px);
            box-shadow: 
                0 15px 40px rgba(102, 126, 234, 0.5),
                0 0 0 3px rgba(255, 255, 255, 0.2);
        }

        .btn-login:active {
            transform: translateY(-1px);
            box-shadow: 0 8px 20px rgba(102, 126, 234, 0.4);
        }

        .note {
            margin-top: 16px;
            font-size: 13px;
            color: rgba(255, 255, 255, 0.75);
            font-weight: 300;
        }

        .alert {
            background: rgba(239, 68, 68, 0.2);
            border: 1px solid rgba(239, 68, 68, 0.4);
            color: #fff;
            font-size: 14px;
            margin-top: 16px;
            padding: 12px 16px;
            border-radius: 12px;
            backdrop-filter: blur(10px);
            animation: shake 0.4s ease;
            box-shadow: 0 4px 15px rgba(239, 68, 68, 0.2);
        }

        @keyframes shake {
            0%, 100% { transform: translateX(0); }
            25% { transform: translateX(-8px); }
            75% { transform: translateX(8px); }
        }

        /* Responsive */
        @media (max-width: 480px) {
            .login-card {
                padding: 35px 25px 30px;
                max-width: 100%;
                border-radius: 25px;
            }
            
            h1 {
                font-size: 26px;
            }
            
            .avatar {
                width: 120px;
                height: 120px;
            }
        }
    </style>
</head>
<body>
    <div class="login-container">
        <div class="login-card">
            <!-- Imagen de perfil -->
            <img src="${pageContext.request.contextPath}/Perfil.jpg" 
                 alt="Avatar" 
                 class="avatar"
                 onerror="this.src='data:image/svg+xml,%3Csvg xmlns=%22http://www.w3.org/2000/svg%22 viewBox=%220 0 100 100%22%3E%3Ccircle cx=%2250%22 cy=%2250%22 r=%2250%22 fill=%22%23667eea%22/%3E%3Ctext x=%2250%25%22 y=%2250%25%22 dominant-baseline=%22middle%22 text-anchor=%22middle%22 font-size=%2240%22 fill=%22white%22 font-family=%22Arial%22%3E👤%3C/text%3E%3C/svg%3E'">
            
            <h1>¡Bienvenido!</h1>
            <p>Conversor de Unidades</p>
            
            <!-- Formulario con la estructura correcta del proyecto RESTful -->
            <form id="loginForm" action="Login" method="POST">
                <div class="input-group">
                    <input 
                        type="text" 
                        id="usuario" 
                        name="usuario" 
                        placeholder="Usuario" 
                        autocomplete="username" 
                        required
                        minlength="3">
                </div>
                
                <div class="input-group">
                    <input 
                        type="password" 
                        id="clave" 
                        name="clave" 
                        placeholder="Contraseña"
                        autocomplete="current-password" 
                        required
                        minlength="4">
                </div>
                
                <button type="submit" class="btn-login">INICIAR SESIÓN</button>
                
                <p class="note">Por favor, ingrese sus credenciales</p>
                
                <!-- Manejo de errores -->
                <% if (request.getAttribute("error") != null) { %>
                    <p class="alert"><%= request.getAttribute("error") %></p>
                <% } %>
            </form>
        </div>
    </div>

    <script>
        // Validación del formulario
        document.getElementById('loginForm').addEventListener('submit', function(e) {
            const usuario = document.getElementById('usuario').value.trim();
            const clave = document.getElementById('clave').value.trim();

            if (usuario === '' || usuario.length < 3) {
                e.preventDefault();
                alert('El usuario debe tener al menos 3 caracteres');
                document.getElementById('usuario').focus();
                return false;
            }

            if (clave === '' || clave.length < 4) {
                e.preventDefault();
                alert('La contraseña debe tener al menos 4 caracteres');
                document.getElementById('clave').focus();
                return false;
            }
        });
    </script>
</body>
</html>