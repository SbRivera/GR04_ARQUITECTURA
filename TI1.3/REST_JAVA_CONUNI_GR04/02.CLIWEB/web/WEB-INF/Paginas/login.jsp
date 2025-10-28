<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>ConUni - Iniciar Sesión</title>
    
    <!-- Tailwind CSS CDN -->
    <script src="https://cdn.tailwindcss.com"></script>
    
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css">
    
    <!-- Toastify CSS -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/toastify-js/src/toastify.min.css">
    
    <style>
        @keyframes float {
            0%, 100% { transform: translateY(0px); }
            50% { transform: translateY(-20px); }
        }
        
        .float-animation {
            animation: float 3s ease-in-out infinite;
        }
        
        .gradient-java {
            background: linear-gradient(135deg, #f89820 0%, #5382a1 100%);
        }
        
        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .fade-in {
            animation: fadeIn 0.6s ease-out;
        }
        
        .profile-image-container {
            width: 280px;
            height: 280px;
            border-radius: 50%;
            overflow: hidden;
            border: 8px solid white;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            position: relative;
        }
        
        .profile-image {
            width: 100%;
            height: 100%;
            object-fit: cover;
            object-position: center;
        }
    </style>
</head>
<body class="bg-gradient-to-br from-blue-50 via-white to-orange-50 min-h-screen">
    
    <!-- Header -->
    <header class="bg-white shadow-md">
        <div class="container mx-auto px-4 py-4 flex items-center justify-between">
            <div class="flex items-center space-x-2">
                <i class="fa-brands fa-java text-4xl text-orange-600"></i>
                <h1 class="text-2xl font-bold text-gray-800">Convertor<span class="text-orange-600"> - Unidades</span></h1>
            </div>
            <div class="flex space-x-4">
                <!-- Enlace de inicio removido - La aplicación inicia directamente en login -->
            </div>
        </div>
    </header>

    <!-- Main Content -->
    <main class="container mx-auto px-4 py-12">
        <div class="max-w-6xl mx-auto">
            
            <div class="grid lg:grid-cols-2 gap-0 bg-white rounded-3xl shadow-2xl overflow-hidden fade-in">
                
                <!-- Left Side - Info & Image -->
                <div class="gradient-java p-12 flex flex-col justify-center items-center text-white relative">
                    <!-- Decorative circles -->
                    <div class="absolute top-10 left-10 w-20 h-20 bg-white opacity-10 rounded-full"></div>
                    <div class="absolute bottom-10 right-10 w-32 h-32 bg-white opacity-10 rounded-full"></div>
                    <div class="absolute top-1/2 right-5 w-16 h-16 bg-white opacity-10 rounded-full"></div>
                    
                    <div class="relative z-10 text-center">
                        <!-- Profile Image -->
                        <div class="mb-8 float-animation flex justify-center">
                            <div class="profile-image-container">
                                <img src="${pageContext.request.contextPath}/Perfil.jpg" 
                                     alt="Profile" 
                                     class="profile-image"
                                     onerror="this.style.display='none'; this.nextElementSibling.style.display='flex';">
                                <div style="display:none;" class="profile-image bg-white/20 backdrop-blur flex items-center justify-center">
                                    <i class="fas fa-user text-7xl text-white"></i>
                                </div>
                            </div>
                        </div>
                        
                        <h2 class="text-4xl font-bold mb-4">¡Bienvenido!</h2>
                        <p class="text-xl text-blue-100 mb-8">
                            Sistema de Conversión de Unidades
                        </p>
                        
                        <div class="space-y-4 text-left max-w-sm mx-auto">
                            <div class="flex items-start space-x-3 bg-white/10 backdrop-blur p-4 rounded-lg">
                                <i class="fas fa-check-circle text-2xl mt-1"></i>
                                <div>
                                    <h3 class="font-bold text-lg">Conversiones Precisas</h3>
                                    <p class="text-sm text-blue-100">Resultados exactos al instante</p>
                                </div>
                            </div>
                            
                            <div class="flex items-start space-x-3 bg-white/10 backdrop-blur p-4 rounded-lg">
                                <i class="fas fa-bolt text-2xl mt-1"></i>
                                <div>
                                    <h3 class="font-bold text-lg">Tecnología REST</h3>
                                    <p class="text-sm text-blue-100">API moderna con Java EE</p>
                                </div>
                            </div>
                            
                            <div class="flex items-start space-x-3 bg-white/10 backdrop-blur p-4 rounded-lg">
                                <i class="fas fa-shield-alt text-2xl mt-1"></i>
                                <div>
                                    <h3 class="font-bold text-lg">Seguro y Confiable</h3>
                                    <p class="text-sm text-blue-100">Datos protegidos</p>
                                </div>
                            </div>
                        </div>
                        
                        <div class="mt-8 pt-8 border-t border-white/20">
                            <p class="text-sm text-blue-100">
                                <i class="fas fa-code mr-2"></i>
                                Desarrollado con Java, Jakarta EE & REST API
                            </p>
                        </div>
                    </div>
                </div>

                <!-- Right Side - Login Form -->
                <div class="p-12 flex flex-col justify-center">
                    <div class="max-w-md mx-auto w-full">
                        <div class="text-center mb-8">
                            <div class="inline-block p-4 bg-gradient-to-br from-orange-400 to-blue-500 rounded-2xl mb-4">
                                <i class="fas fa-lock text-4xl text-white"></i>
                            </div>
                            <h3 class="text-3xl font-bold text-gray-800 mb-2">Iniciar Sesión</h3>
                            <p class="text-gray-600">Ingresa tus credenciales para continuar</p>
                        </div>

                        <form id="loginForm" action="Login" method="POST" class="space-y-6">
                            
                            <!-- Usuario -->
                            <div class="relative">
                                <label class="block text-sm font-semibold text-gray-700 mb-2">
                                    <i class="fas fa-user mr-2 text-orange-600"></i>Usuario
                                </label>
                                <div class="relative">
                                    <input 
                                        type="text" 
                                        id="usuario" 
                                        name="usuario" 
                                        required
                                        minlength="3"
                                        class="w-full pl-12 pr-4 py-3 border-2 border-gray-300 rounded-xl focus:border-orange-500 focus:outline-none transition duration-300"
                                        placeholder="Ingrese su usuario"
                                    >
                                    <i class="fas fa-user absolute left-4 top-4 text-gray-400"></i>
                                </div>
                            </div>

                            <!-- Contraseña -->
                            <div class="relative">
                                <label class="block text-sm font-semibold text-gray-700 mb-2">
                                    <i class="fas fa-lock mr-2 text-orange-600"></i>Contraseña
                                </label>
                                <div class="relative">
                                    <input 
                                        type="password" 
                                        id="clave" 
                                        name="clave" 
                                        required
                                        minlength="4"
                                        class="w-full pl-12 pr-12 py-3 border-2 border-gray-300 rounded-xl focus:border-orange-500 focus:outline-none transition duration-300"
                                        placeholder="Ingrese su contraseña"
                                    >
                                    <i class="fas fa-lock absolute left-4 top-4 text-gray-400"></i>
                                    <button 
                                        type="button" 
                                        onclick="togglePassword()"
                                        class="absolute right-4 top-4 text-gray-400 hover:text-gray-600"
                                    >
                                        <i id="eyeIcon" class="fas fa-eye"></i>
                                    </button>
                                </div>
                            </div>

                            <!-- Remember Me -->
                            <div class="flex items-center justify-between text-sm">
                                <label class="flex items-center cursor-pointer">
                                    <input type="checkbox" class="w-4 h-4 text-orange-600 border-gray-300 rounded focus:ring-orange-500">
                                    <span class="ml-2 text-gray-600">Recordarme</span>
                                </label>
                                <a href="#" class="text-orange-600 hover:text-orange-700 font-semibold">
                                    <i class="fas fa-question-circle mr-1"></i>
                                    ¿Olvidaste tu contraseña?
                                </a>
                            </div>

                            <!-- Submit Button -->
                            <button 
                                type="submit"
                                class="w-full gradient-java text-white font-bold py-4 rounded-xl hover:shadow-xl transform hover:-translate-y-1 transition duration-300 flex items-center justify-center space-x-2"
                            >
                                <i class="fas fa-sign-in-alt"></i>
                                <span>Iniciar Sesión</span>
                            </button>

                            <!-- Error Message -->
                            <% if (request.getAttribute("error") != null) { %>
                            <div class="bg-red-50 border-l-4 border-red-500 p-4 rounded-lg fade-in">
                                <div class="flex items-center">
                                    <i class="fas fa-exclamation-circle text-red-500 text-xl mr-3"></i>
                                    <p class="text-red-800 font-semibold"><%= request.getAttribute("error") %></p>
                                </div>
                            </div>
                            <% } %>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </main>


    <!-- Toastify JS -->
    <script src="https://cdn.jsdelivr.net/npm/toastify-js"></script>
    
    <script>
        // Toggle Password Visibility
        function togglePassword() {
            const passwordInput = document.getElementById('clave');
            const eyeIcon = document.getElementById('eyeIcon');
            
            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                eyeIcon.classList.remove('fa-eye');
                eyeIcon.classList.add('fa-eye-slash');
            } else {
                passwordInput.type = 'password';
                eyeIcon.classList.remove('fa-eye-slash');
                eyeIcon.classList.add('fa-eye');
            }
        }

        // Form Validation
        document.getElementById('loginForm').addEventListener('submit', function(e) {
            const usuario = document.getElementById('usuario').value.trim();
            const clave = document.getElementById('clave').value.trim();

            if (usuario === '') {
                e.preventDefault();
                showToast('Por favor, ingrese su usuario', 'error');
                document.getElementById('usuario').focus();
                return false;
            }

            if (usuario.length < 3) {
                e.preventDefault();
                showToast('El usuario debe tener al menos 3 caracteres', 'error');
                document.getElementById('usuario').focus();
                return false;
            }

            if (clave === '') {
                e.preventDefault();
                showToast('Por favor, ingrese su contraseña', 'error');
                document.getElementById('clave').focus();
                return false;
            }

            if (clave.length < 4) {
                e.preventDefault();
                showToast('La contraseña debe tener al menos 4 caracteres', 'error');
                document.getElementById('clave').focus();
                return false;
            }

            showToast('Validando credenciales...', 'info');
        });

        // Real-time validation feedback
        document.getElementById('usuario').addEventListener('input', function() {
            if (this.value.length > 0 && this.value.length < 3) {
                this.classList.add('border-red-500');
                this.classList.remove('border-green-500');
            } else if (this.value.length >= 3) {
                this.classList.add('border-green-500');
                this.classList.remove('border-red-500');
            } else {
                this.classList.remove('border-red-500', 'border-green-500');
            }
        });

        document.getElementById('clave').addEventListener('input', function() {
            if (this.value.length > 0 && this.value.length < 4) {
                this.classList.add('border-red-500');
                this.classList.remove('border-green-500');
            } else if (this.value.length >= 4) {
                this.classList.add('border-green-500');
                this.classList.remove('border-red-500');
            } else {
                this.classList.remove('border-red-500', 'border-green-500');
            }
        });

        // Toast Notification Function
        function showToast(message, type = 'info') {
            const colors = {
                success: 'linear-gradient(to right, #00b09b, #96c93d)',
                error: 'linear-gradient(to right, #ff5f6d, #ffc371)',
                warning: 'linear-gradient(to right, #f89820, #ffb347)',
                info: 'linear-gradient(to right, #5382a1, #0575e6)'
            };

            Toastify({
                text: message,
                duration: 3000,
                gravity: "top",
                position: "right",
                style: {
                    background: colors[type] || colors.info,
                },
                stopOnFocus: true,
            }).showToast();
        }

        // Check for error in request
        <% if (request.getAttribute("error") != null) { %>
            showToast('<%= request.getAttribute("error") %>', 'error');
        <% } %>

        // Welcome message
        window.addEventListener('load', function() {
            showToast('¡Bienvenido! Ingresa tus credenciales', 'info');
        });
    </script>
</body>
</html>
                <a href="index.html" class="text-gray-600 hover:text-orange-600 transition flex items-center justify-center space-x-2">
                    <i class="fas fa-arrow-left"></i>
                    <span>Volver al inicio</span>
                </a>
            </div>
        </div>
    </main>

    <!-- Footer -->
    <footer class="bg-gray-800 text-white mt-12 py-6">
        <div class="container mx-auto px-4 text-center">
            <p class="text-gray-400">© 2025 ConUni Converter - GR04 Arquitectura</p>
        </div>
    </footer>

    <!-- Toastify JS -->
    <script src="https://cdn.jsdelivr.net/npm/toastify-js"></script>
    
    <script>
        // Toggle Password Visibility
        function togglePassword() {
            const passwordInput = document.getElementById('clave');
            const eyeIcon = document.getElementById('eyeIcon');
            
            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                eyeIcon.classList.remove('fa-eye');
                eyeIcon.classList.add('fa-eye-slash');
            } else {
                passwordInput.type = 'password';
                eyeIcon.classList.remove('fa-eye-slash');
                eyeIcon.classList.add('fa-eye');
            }
        }

        // Form Validation
        document.getElementById('loginForm').addEventListener('submit', function(e) {
            const usuario = document.getElementById('usuario').value.trim();
            const clave = document.getElementById('clave').value.trim();

            // Validar usuario vacío
            if (usuario === '') {
                e.preventDefault();
                showToast('Por favor, ingrese su usuario', 'error');
                document.getElementById('usuario').focus();
                return false;
            }

            // Validar longitud mínima usuario
            if (usuario.length < 3) {
                e.preventDefault();
                showToast('El usuario debe tener al menos 3 caracteres', 'error');
                document.getElementById('usuario').focus();
                return false;
            }

            // Validar contraseña vacía
            if (clave === '') {
                e.preventDefault();
                showToast('Por favor, ingrese su contraseña', 'error');
                document.getElementById('clave').focus();
                return false;
            }

            // Validar longitud mínima contraseña
            if (clave.length < 4) {
                e.preventDefault();
                showToast('La contraseña debe tener al menos 4 caracteres', 'error');
                document.getElementById('clave').focus();
                return false;
            }

            showToast('Validando credenciales...', 'info');
        });

        // Real-time validation feedback
        document.getElementById('usuario').addEventListener('input', function() {
            if (this.value.length > 0 && this.value.length < 3) {
                this.classList.add('border-red-500');
                this.classList.remove('border-green-500');
            } else if (this.value.length >= 3) {
                this.classList.add('border-green-500');
                this.classList.remove('border-red-500');
            } else {
                this.classList.remove('border-red-500', 'border-green-500');
            }
        });

        document.getElementById('clave').addEventListener('input', function() {
            if (this.value.length > 0 && this.value.length < 4) {
                this.classList.add('border-red-500');
                this.classList.remove('border-green-500');
            } else if (this.value.length >= 4) {
                this.classList.add('border-green-500');
                this.classList.remove('border-red-500');
            } else {
                this.classList.remove('border-red-500', 'border-green-500');
            }
        });

        // Toast Notification Function
        function showToast(message, type = 'info') {
            const colors = {
                success: 'linear-gradient(to right, #00b09b, #96c93d)',
                error: 'linear-gradient(to right, #ff5f6d, #ffc371)',
                warning: 'linear-gradient(to right, #f89820, #ffb347)',
                info: 'linear-gradient(to right, #5382a1, #0575e6)'
            };

            Toastify({
                text: message,
                duration: 3000,
                gravity: "top",
                position: "right",
                style: {
                    background: colors[type] || colors.info,
                },
                stopOnFocus: true,
            }).showToast();
        }

        // Check for error in request
        <% if (request.getAttribute("error") != null) { %>
            showToast('<%= request.getAttribute("error") %>', 'error');
        <% } %>

        // Welcome message
        window.addEventListener('load', function() {
            showToast('¡Bienvenido! Ingresa tus credenciales', 'info');
        });
    </script>
</body>
</html>
