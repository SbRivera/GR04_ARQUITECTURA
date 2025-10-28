<%@page contentType="text/html" pageEncoding="UTF-8"%>
<%@page import="ec.edu.monster.modelo.Resultado"%>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>ConUni - Conversor de Unidades</title>
    
    <!-- Tailwind CSS CDN -->
    <script src="https://cdn.tailwindcss.com"></script>
    
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css">
    
    <!-- Toastify CSS -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/toastify-js/src/toastify.min.css">
    
    <style>
        @keyframes slideIn {
            from {
                opacity: 0;
                transform: translateY(20px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
        
        .slide-in {
            animation: slideIn 0.5s ease-out;
        }
        
        .gradient-java {
            background: linear-gradient(135deg, #f89820 0%, #5382a1 100%);
        }
        
        .card-hover {
            transition: all 0.3s ease;
        }
        
        .card-hover:hover {
            transform: translateY(-5px);
            box-shadow: 0 20px 40px rgba(0,0,0,0.15);
        }
    </style>
</head>
<body class="bg-gradient-to-br from-blue-50 via-white to-orange-50 min-h-screen">
    
    <!-- Header -->
    <header class="bg-white shadow-md sticky top-0 z-50">
        <div class="container mx-auto px-4 py-4">
            <div class="flex items-center justify-between">
                <div class="flex items-center space-x-2">
                    <i class="fa-brands fa-java text-4xl text-orange-600"></i>
                    <h1 class="text-2xl font-bold text-gray-800">ConUni<span class="text-orange-600">Converter</span></h1>
                </div>
                
                <div class="flex items-center space-x-4">
                    <div class="hidden md:flex items-center space-x-3 text-gray-600">
                        <img src="${pageContext.request.contextPath}/Perfil.jpg" 
                             alt="Profile" 
                             class="w-10 h-10 rounded-full object-cover border-2 border-orange-500 shadow-md"
                             onerror="this.src=''; this.style.display='none'; this.nextElementSibling.style.display='inline-block';">
                        <i class="fas fa-user-circle text-3xl text-orange-600" style="display: none;"></i>
                        <span class="font-semibold text-gray-800"><%= session.getAttribute("usuario") != null ? session.getAttribute("usuario") : "Usuario" %></span>
                    </div>
                    <a href="${pageContext.request.contextPath}/Logout" class="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded-lg transition flex items-center space-x-2">
                        <i class="fas fa-sign-out-alt"></i>
                        <span>Cerrar Sesión</span>
                    </a>
                </div>
            </div>
        </div>
    </header>

    <!-- Main Content -->
    <main class="container mx-auto px-4 py-8">
        
        <!-- Welcome Banner -->
        <div class="gradient-java text-white rounded-2xl p-8 mb-8 slide-in">
            <div class="flex items-center justify-between">
                <div>
                    <h2 class="text-3xl font-bold mb-2">
                        <i class="fas fa-exchange-alt mr-3"></i>
                        Conversor de Unidades
                    </h2>
                    <p class="text-blue-100">Realiza conversiones precisas de manera instantánea</p>
                </div>
                <div class="hidden md:block">
                    <i class="fas fa-calculator text-7xl opacity-50"></i>
                </div>
            </div>
        </div>

        <div class="grid lg:grid-cols-3 gap-8">
            
            <!-- Conversion Form -->
            <div class="lg:col-span-2">
                <div class="bg-white rounded-2xl shadow-xl p-8 card-hover">
                    <div class="flex items-center mb-6">
                        <div class="bg-gradient-to-br from-orange-400 to-blue-500 p-3 rounded-lg mr-4">
                            <i class="fas fa-sliders-h text-2xl text-white"></i>
                        </div>
                        <div>
                            <h3 class="text-2xl font-bold text-gray-800">Nueva Conversión</h3>
                            <p class="text-gray-600">Selecciona el tipo y valor a convertir</p>
                        </div>
                    </div>

                    <form id="conversionForm" action="Conversion" method="POST" class="space-y-6">
                        
                        <!-- Selección de Tipo por Iconos -->
                        <div>
                            <label class="block text-sm font-semibold text-gray-700 mb-4">
                                <i class="fas fa-th-large mr-2 text-orange-600"></i>Tipo de Conversión
                            </label>
                            <div class="grid grid-cols-3 gap-4">
                                <!-- Longitud -->
                                <label class="cursor-pointer group">
                                    <input type="radio" name="categoriaConversion" value="longitud" class="hidden peer" required>
                                    <div class="p-6 border-2 border-gray-300 rounded-xl text-center hover:border-orange-500 hover:bg-orange-50 transition peer-checked:border-orange-500 peer-checked:bg-orange-50 peer-checked:shadow-lg">
                                        <i class="fas fa-ruler text-4xl text-gray-400 group-hover:text-orange-500 peer-checked:text-orange-500 mb-3"></i>
                                        <p class="font-semibold text-gray-700 group-hover:text-orange-600">Longitud</p>
                                    </div>
                                </label>

                                <!-- Temperatura -->
                                <label class="cursor-pointer group">
                                    <input type="radio" name="categoriaConversion" value="temperatura" class="hidden peer" required>
                                    <div class="p-6 border-2 border-gray-300 rounded-xl text-center hover:border-orange-500 hover:bg-orange-50 transition peer-checked:border-orange-500 peer-checked:bg-orange-50 peer-checked:shadow-lg">
                                        <i class="fas fa-temperature-high text-4xl text-gray-400 group-hover:text-orange-500 peer-checked:text-orange-500 mb-3"></i>
                                        <p class="font-semibold text-gray-700 group-hover:text-orange-600">Temperatura</p>
                                    </div>
                                </label>

                                <!-- Peso -->
                                <label class="cursor-pointer group">
                                    <input type="radio" name="categoriaConversion" value="peso" class="hidden peer" required>
                                    <div class="p-6 border-2 border-gray-300 rounded-xl text-center hover:border-orange-500 hover:bg-orange-50 transition peer-checked:border-orange-500 peer-checked:bg-orange-50 peer-checked:shadow-lg">
                                        <i class="fas fa-weight text-4xl text-gray-400 group-hover:text-orange-500 peer-checked:text-orange-500 mb-3"></i>
                                        <p class="font-semibold text-gray-700 group-hover:text-orange-600">Peso</p>
                                    </div>
                                </label>
                            </div>
                        </div>

                        <!-- Dropdown de Conversiones Específicas (aparece al seleccionar tipo) -->
                        <div id="conversionEspecificaContainer" style="display: none;">
                            <label class="block text-sm font-semibold text-gray-700 mb-3">
                                <i class="fas fa-exchange-alt mr-2 text-orange-600"></i>Conversión Específica
                            </label>
                            <select name="tipo" id="tipoConversion" required 
                                    class="w-full px-4 py-3 border-2 border-gray-300 rounded-lg focus:border-orange-500 focus:outline-none transition text-lg">
                                <option value="">-- Selecciona una conversión --</option>
                            </select>
                        </div>

                        <!-- Valor a Convertir -->
                        <div>
                            <label class="block text-sm font-semibold text-gray-700 mb-2">
                                <i class="fas fa-hashtag mr-2 text-orange-600"></i>Valor a Convertir
                            </label>
                            <div class="relative">
                                <input 
                                    type="number" 
                                    id="valor" 
                                    name="valor" 
                                    step="0.01"
                                    required
                                    disabled
                                    class="w-full pl-12 pr-4 py-4 border-2 border-gray-300 rounded-lg focus:border-orange-500 focus:outline-none transition text-lg disabled:bg-gray-100 disabled:cursor-not-allowed"
                                    placeholder="Primero seleccione el tipo de conversión"
                                >
                                <i class="fas fa-calculator absolute left-4 top-5 text-gray-400 text-xl"></i>
                            </div>
                            <p class="text-xs text-gray-500 mt-2">
                                <i class="fas fa-info-circle mr-1"></i>
                                <span id="valorHint">Seleccione primero una conversión específica</span>
                            </p>
                        </div>

                        <!-- Buttons -->
                        <div class="flex space-x-4">
                            <button 
                                type="submit"
                                class="flex-1 gradient-java text-white font-bold py-4 rounded-lg hover:shadow-xl transform hover:-translate-y-1 transition duration-300 flex items-center justify-center space-x-2"
                            >
                                <i class="fas fa-sync-alt"></i>
                                <span>Convertir Ahora</span>
                            </button>
                            
                            <button 
                                type="reset"
                                onclick="clearForm()"
                                class="bg-gray-200 hover:bg-gray-300 text-gray-700 font-bold py-4 px-6 rounded-lg transition flex items-center space-x-2"
                            >
                                <i class="fas fa-eraser"></i>
                                <span>Limpiar</span>
                            </button>
                        </div>
                    </form>
                </div>

                <!-- Resultado si existe -->
                <%
                    Resultado resultado = (Resultado) request.getAttribute("resultado");
                    if (resultado != null) {
                %>
                <div class="bg-gradient-to-r from-green-400 to-blue-500 rounded-2xl shadow-xl p-8 mt-8 text-white slide-in">
                    <div class="flex items-start justify-between">
                        <div class="flex-1">
                            <div class="flex items-center mb-4">
                                <i class="fas fa-check-circle text-4xl mr-4"></i>
                                <div>
                                    <h3 class="text-2xl font-bold">¡Conversión Exitosa!</h3>
                                    <p class="text-green-100">Resultado calculado correctamente</p>
                                </div>
                            </div>
                            
                            <div class="bg-white/20 backdrop-blur rounded-xl p-6 mt-4">
                                <div class="grid md:grid-cols-2 gap-4">
                                    <div>
                                        <p class="text-sm text-green-100 mb-1">Valor Original</p>
                                        <p class="text-3xl font-bold"><%= resultado.getValorOriginal() %></p>
                                        <p class="text-sm"><%= resultado.getUnidadOrigen() %></p>
                                    </div>
                                    <div>
                                        <p class="text-sm text-green-100 mb-1">Valor Convertido</p>
                                        <p class="text-3xl font-bold"><%= resultado.getValorConvertido() %></p>
                                        <p class="text-sm"><%= resultado.getUnidadDestino() %></p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <button onclick="this.parentElement.parentElement.remove()" class="text-white hover:text-red-200 text-2xl">
                            <i class="fas fa-times-circle"></i>
                        </button>
                    </div>
                </div>
                <% } %>
            </div>

            <!-- Info Panel -->
            <div class="space-y-6">
                
                <!-- Supported Conversions -->
                <div class="bg-white rounded-2xl shadow-xl p-6">
                    <h3 class="text-xl font-bold text-gray-800 mb-4">
                        <i class="fas fa-list-check mr-2 text-orange-600"></i>
                        Conversiones Soportadas
                    </h3>
                    <ul class="space-y-2 text-sm">
                        <li class="flex items-center text-gray-700">
                            <i class="fas fa-check text-green-500 mr-2"></i>
                            CM → IN (Centímetros a Pulgadas)
                        </li>
                        <li class="flex items-center text-gray-700">
                            <i class="fas fa-check text-green-500 mr-2"></i>
                            IN → CM (Pulgadas a Centímetros)
                        </li>
                        <li class="flex items-center text-gray-700">
                            <i class="fas fa-check text-green-500 mr-2"></i>
                            °C → °F (Celsius a Fahrenheit)
                        </li>
                        <li class="flex items-center text-gray-700">
                            <i class="fas fa-check text-green-500 mr-2"></i>
                            °F → °C (Fahrenheit a Celsius)
                        </li>
                        <li class="flex items-center text-gray-700">
                            <i class="fas fa-check text-green-500 mr-2"></i>
                            KG → LB (Kilogramos a Libras)
                        </li>
                        <li class="flex items-center text-gray-700">
                            <i class="fas fa-check text-green-500 mr-2"></i>
                            LB → KG (Libras a Kilogramos)
                        </li>
                    </ul>
                </div>

            </div>
        </div>
    </main>

    <!-- Footer -->
    <footer class="bg-gray-800 text-white mt-12 py-6">
        <div class="container mx-auto px-4 text-center">
            <p class="text-gray-400">© 2025 ConUni Converter - Powered by Java & REST API</p>
        </div>
    </footer>

    <!-- Toastify JS -->
    <script src="https://cdn.jsdelivr.net/npm/toastify-js"></script>
    
    <script>
        // Conversiones disponibles por categoría
        const conversionesDisponibles = {
            longitud: [
                { value: 'cm-to-in', text: 'Centímetros → Pulgadas' },
                { value: 'in-to-cm', text: 'Pulgadas → Centímetros' }
            ],
            temperatura: [
                { value: 'c-to-f', text: 'Celsius → Fahrenheit' },
                { value: 'f-to-c', text: 'Fahrenheit → Celsius' }
            ],
            peso: [
                { value: 'kg-to-lb', text: 'Kilogramos → Libras' },
                { value: 'lb-to-kg', text: 'Libras → Kilogramos' }
            ]
        };

        // Manejar cambio de categoría
        document.querySelectorAll('input[name="categoriaConversion"]').forEach(radio => {
            radio.addEventListener('change', function() {
                const categoria = this.value;
                const container = document.getElementById('conversionEspecificaContainer');
                const select = document.getElementById('tipoConversion');
                
                // Mostrar el dropdown
                container.style.display = 'block';
                
                // Limpiar opciones actuales
                select.innerHTML = '<option value="">-- Selecciona una conversión --</option>';
                
                // Agregar opciones según la categoría
                if (conversionesDisponibles[categoria]) {
                    conversionesDisponibles[categoria].forEach(conv => {
                        const option = document.createElement('option');
                        option.value = conv.value;
                        option.textContent = conv.text;
                        select.appendChild(option);
                    });
                }
                
                // Animación suave
                container.classList.add('slide-in');
            });
        });
        
        // Habilitar input de valor cuando se seleccione una conversión específica
        document.getElementById('tipoConversion').addEventListener('change', function() {
            const valorInput = document.getElementById('valor');
            const valorHint = document.getElementById('valorHint');
            
            if (this.value !== '') {
                // Habilitar el input
                valorInput.disabled = false;
                valorInput.placeholder = 'Ingrese el valor numérico';
                valorInput.focus();
                valorHint.textContent = 'Puede usar números decimales (ej: 10.5)';
            } else {
                // Deshabilitar el input
                valorInput.disabled = true;
                valorInput.value = '';
                valorInput.placeholder = 'Primero seleccione el tipo de conversión';
                valorHint.textContent = 'Seleccione primero una conversión específica';
            }
        });
        
        // Form Validation
        document.getElementById('conversionForm').addEventListener('submit', function(e) {
            const categoria = document.querySelector('input[name="categoriaConversion"]:checked');
            const tipo = document.getElementById('tipoConversion').value;
            const valor = document.getElementById('valor').value;

            if (!categoria) {
                e.preventDefault();
                showToast('Por favor, seleccione un tipo de conversión', 'error');
                return false;
            }

            if (!tipo || tipo === '') {
                e.preventDefault();
                showToast('Por favor, seleccione una conversión específica', 'error');
                return false;
            }

            if (valor === '' || isNaN(valor)) {
                e.preventDefault();
                showToast('Por favor, ingrese un valor numérico válido', 'error');
                return false;
            }

            if (parseFloat(valor) < 0) {
                e.preventDefault();
                showToast('El valor no puede ser negativo', 'error');
                return false;
            }

            showToast('Procesando conversión...', 'info');
        });

        // Clear Form
        function clearForm() {
            document.getElementById('conversionForm').reset();
            document.getElementById('conversionEspecificaContainer').style.display = 'none';
            
            // Deshabilitar el input de valor nuevamente
            const valorInput = document.getElementById('valor');
            const valorHint = document.getElementById('valorHint');
            valorInput.disabled = true;
            valorInput.placeholder = 'Primero seleccione el tipo de conversión';
            valorHint.textContent = 'Seleccione primero una conversión específica';
            
            showToast('Formulario limpiado', 'success');
        }

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

        // Show success toast if conversion was made
        <% if (resultado != null) { %>
            showToast('¡Conversión realizada con éxito!', 'success');
        <% } %>

        // Welcome message
        window.addEventListener('load', function() {
            showToast('¡Bienvenido al conversor de unidades!', 'success');
        });
    </script>
</body>
</html>
