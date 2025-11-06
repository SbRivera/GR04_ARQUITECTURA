<%@page contentType="text/html" pageEncoding="UTF-8"%>
<%@page import="jakarta.servlet.http.HttpSession" %>
<%@page import="ec.edu.monster.modelo.Resultado" %>
<%@taglib prefix="c" uri="jakarta.tags.core"%>
<%
    // === Guardia de sesión (mismo comportamiento que en el otro proyecto)
    HttpSession sesion = request.getSession(false);
    if (sesion == null || sesion.getAttribute("usuario") == null) {
        response.sendRedirect(request.getContextPath() + "/index.jsp");
        return;
    }

    // === Adaptador: si tu backend actual entrega un objeto Resultado como atributo
    //     lo exponemos como parámetros "param_*" para que la vista funcione igual
    //     que la del otro proyecto (que lee de param.resultado/param.error/param.valor/param.tipo)
    Resultado res = (Resultado) request.getAttribute("resultado");
    if (res != null) {
        request.setAttribute("param_resultado",
        String.format(java.util.Locale.US, "%.2f", res.getValorConvertido()));
request.setAttribute("param_valor",
        String.format(java.util.Locale.US, "%.2f", res.getValorOriginal()));

        request.setAttribute("param_tipo", (res.getUnidadOrigen() + " → " + res.getUnidadDestino()));
    }
    String err = (String) request.getAttribute("error");
    if (err != null) {
        request.setAttribute("param_error", err);
    }
%>
<!DOCTYPE html>
<html lang="es">
    <head>
        <meta charset="UTF-8">
        <title>Conversión de Unidades</title>
        <!-- Fuente -->
        <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600;700&display=swap" rel="stylesheet">

        <!-- CSS (mismo archivo del otro proyecto). Ajusta el query param si quieres bust de caché. -->
        <link rel="stylesheet" href="<%= request.getContextPath() %>/styles.css?v=conuni">
    </head>
    <body class="main-bg">

        <header class="topbar">
            <h1>Conversiones de Unidades</h1>
            <div class="user-badge">
                <div class="user-info">
                    <span>Conectado como</span>
                    <strong><%= sesion.getAttribute("usuario") %></strong>
                </div>
                <!-- Nota: en el otro proyecto hacían GET a index.jsp como "logout" visual. -->
                <!-- Si en este proyecto tienes un servlet Logout real, cambia la action abajo. -->
                <form action="<%= request.getContextPath() %>/index.jsp" method="get" style="margin: 0;">
                    <button type="submit" class="logout">Cerrar sesión</button>
                </form>
            </div>
        </header>

        <!-- ======= Shell (desktop: 2 columnas / mobile: 1) ======= -->
        <div class="conv-shell">

            <!-- data-* sirve para rehidratar tras el POST -->
            <main class="conv-wrapper"
                  data-cat="${param.cat}"
                  data-tipo="${empty param.tipo ? requestScope.param_tipo : param.tipo}"
                  data-valor="${empty param.valor ? requestScope.param_valor : param.valor}">

                <!-- Paso 1 -->
                <section class="conv-card">
                    <div class="conv-card__header header--blue">
                        <span class="header-icon" aria-hidden="true">i</span>
                        <h2>Categoría de Conversión</h2>
                    </div>
                    <div class="conv-card__body">
                        <div class="input-outline">
                            <select id="categoria" aria-label="Seleccionar Categoría">
                                <option value="" selected>Seleccionar Categoría</option>
                                <!-- En tu app original la categoría era "peso"; en el otro proyecto es "masa".  -->
                                <!-- Mantendremos "longitud/temperatura/peso" para que coincida con TU backend actual. -->
                                <option value="longitud">Longitud</option>
                                <option value="temperatura">Temperatura</option>
                                <option value="peso">Peso</option>
                            </select>
                        </div>
                    </div>
                </section>

                <!-- Paso 2 -->
                <section class="conv-card">
                    <div class="conv-card__header header--orange">
                        <span class="header-icon" aria-hidden="true">i</span>
                        <h2>Tipo de Conversión</h2>
                    </div>
                    <div class="conv-card__body">
                        <div class="input-outline">
                            <select id="tipo" aria-label="Seleccionar tipo de conversión" disabled>
                                <option value="">Primero seleccione una</option>
                            </select>
                        </div>
                    </div>
                </section>

                <!-- Paso 3 -->
                <!-- IMPORTANTE: Cambia la URL de action si en tu proyecto actual el endpoint es distinto. -->
                <!-- El JSP anterior usaba action="Conversion" (servlet). Aquí la dejamos apuntando ahí por defecto. -->
                <form id="form-conversion" class="conv-card"
                      action="<%= request.getContextPath() %>/Conversion" method="post" novalidate>

                    <div class="conv-card__header header--green">
                        <span class="header-icon" aria-hidden="true">i</span>
                        <h2>Ingrese el Valor</h2>
                    </div>

                    <div class="conv-card__body">
                        <!-- Hidden para enviar lo seleccionado -->
                        <input type="hidden" name="tipo" id="tipo-hidden">
                        <input type="hidden" name="cat" id="cat-hidden">

                        <div class="input-outline">
                            <input type="number" inputmode="decimal" id="valor" name="valor"
                                   placeholder="Ingrese el valor numérico" required>
                        </div>

                        <button type="submit" class="btn-primary btn-block">CONVERTIR</button>

                        <!-- Resultado para móviles -->
                        <c:choose>
                            <c:when test="${not empty param.resultado || not empty requestScope.param_resultado}">
                                <div class="result-chip only-mobile">
                                    <strong>${empty param.resultado ? requestScope.param_resultado : param.resultado}</strong>
                                    <small>
                                        Valor inicial: ${empty param.valor ? requestScope.param_valor : param.valor}
                                        · Operación: ${empty param.tipo ? requestScope.param_tipo : param.tipo}
                                    </small>
                                </div>
                            </c:when>
                            <c:when test="${not empty param.error || not empty requestScope.param_error}">
                                <p class="alert only-mobile">${empty param.error ? requestScope.param_error : param.error}</p>
                            </c:when>
                        </c:choose>
                    </div>
                </form>
            </main>

            <!-- Panel de resultado (solo desktop) -->
            <aside class="result-panel only-desktop">
                <h3>Resultado</h3>
                <c:choose>
                    <c:when test="${not empty param.resultado || not empty requestScope.param_resultado}">
                        <p class="result-big">${empty param.resultado ? requestScope.param_resultado : param.resultado}</p>
                        <p class="muted">Valor inicial: <b>${empty param.valor ? requestScope.param_valor : param.valor}</b></p>
                        <p class="muted">Operación: <b>${empty param.tipo ? requestScope.param_tipo : param.tipo}</b></p>
                    </c:when>
                    <c:when test="${not empty param.error || not empty requestScope.param_error}">
                        <p class="alert">${empty param.error ? requestScope.param_error : param.error}</p>
                    </c:when>
                    <c:otherwise>
                        <p class="muted">Ingresa un valor y presiona <b>Convertir</b>.</p>
                    </c:otherwise>
                </c:choose>
            </aside>
        </div>

        <!-- FAB limpiar -->
        <button type="button" class="fab danger" id="btn-limpiar" title="Limpiar">
            <span class="trash" aria-hidden="true">🗑️</span>
        </button>

        <script>
            // === Opciones por categoría ===
            // NOTA: Ajustamos los valores (val) a los que usas en TU proyecto actual:
            //       En tu JSP original enviabas valores tipo 'cm-to-in', 'in-to-cm', 'c-to-f', etc.
            //       Los dejamos así para no romper tus servicios.
            const opciones = {
                longitud: [
                    {val: "cm-to-in", txt: "Centímetros → Pulgadas", min0: true},
                    {val: "in-to-cm", txt: "Pulgadas → Centímetros", min0: true}
                ],
                temperatura: [
                    {val: "c-to-f", txt: "Celsius → Fahrenheit", min0: false},
                    {val: "f-to-c", txt: "Fahrenheit → Celsius", min0: false}
                ],
                // En tu vista antigua la categoría era "peso" (no "masa").
                peso: [
                    {val: "kg-to-lb", txt: "Kilogramos → Libras", min0: true},
                    {val: "lb-to-kg", txt: "Libras → Kilogramos", min0: true}
                ]
            };

            // === DOM ===
            const categoria = document.getElementById('categoria');
            const tipo = document.getElementById('tipo');
            const tipoHidden = document.getElementById('tipo-hidden');
            const catHidden = document.getElementById('cat-hidden');
            const valor = document.getElementById('valor');
            const form = document.getElementById('form-conversion');
            const btnLimpiar = document.getElementById('btn-limpiar');

            // === Listeners ===
            categoria.addEventListener('change', () => {
                const cat = categoria.value;
                catHidden.value = cat || '';
                tipo.innerHTML = '';
                if (!cat) {
                    tipo.disabled = true;
                    tipo.innerHTML = '<option value="">Primero seleccione una</option>';
                    tipoHidden.value = '';
                    return;
                }
                opciones[cat].forEach(o => {
                    const opt = document.createElement('option');
                    opt.value = o.val;
                    opt.textContent = o.txt;
                    tipo.appendChild(opt);
                });
                tipo.disabled = false;
                aplicarReglas(opciones[cat][0].min0);
                tipoHidden.value = tipo.value;
            });

            tipo.addEventListener('change', () => {
                const cat = categoria.value;
                const sel = opciones[cat]?.find(o => o.val === tipo.value);
                aplicarReglas(sel ? sel.min0 : true);
                tipoHidden.value = tipo.value || '';
            });

            function aplicarReglas(min0) {
                if (min0) {
                    valor.setAttribute('min', '0');
                } else {
                    valor.removeAttribute('min');
                }
            }

            form.addEventListener('submit', (e) => {
                if (!categoria.value) {
                    e.preventDefault();
                    toast("Seleccione una categoría.");
                    return;
                }
                if (!tipo.value) {
                    e.preventDefault();
                    toast("Seleccione el tipo de conversión.");
                    return;
                }
                if (valor.value.trim() === '' || !isFinite(parseFloat(valor.value))) {
                    e.preventDefault();
                    toast("Ingrese un valor numérico válido.");
                    return;
                }
                tipoHidden.value = tipo.value;
                catHidden.value = categoria.value;
            });

            btnLimpiar.addEventListener('click', () => {
                categoria.value = '';
                tipo.innerHTML = '<option value="">Primero seleccione una</option>';
                tipo.disabled = true;
                valor.value = '';
                tipoHidden.value = '';
                catHidden.value = '';
                toast("Formulario reiniciado.");
            });

            // === Rehidratar tras POST (mantener selección) ===
            (function rehidratar() {
                const wrapper = document.querySelector('.conv-wrapper');
                let cat = (wrapper.dataset.cat || '').trim();
                const tpo = (wrapper.dataset.tipo || '').trim();
                const val = (wrapper.dataset.valor || '').trim();

                // Inferir cat por tipo si no vino (usa tus códigos actuales con guiones)
                if (!cat && tpo) {
                    if (/cm-to-in|in-to-cm/.test(tpo))
                        cat = 'longitud';
                    else if (/c-to-f|f-to-c/.test(tpo))
                        cat = 'temperatura';
                    else if (/kg-to-lb|lb-to-kg/.test(tpo))
                        cat = 'peso';
                }

                if (cat) {
                    categoria.value = cat;
                    categoria.dispatchEvent(new Event('change'));
                    if (tpo) {
                        tipo.value = tpo;
                        tipo.dispatchEvent(new Event('change'));
                    }
                }
                if (val)
                    valor.value = val;

                catHidden.value = categoria.value || '';
                tipoHidden.value = tipo.value || '';
            })();

            // === Toast mínimo sin librerías (como en el otro proyecto) ===
            function toast(t) {
                let b = document.querySelector('.banner');
                if (!b) {
                    b = document.createElement('div');
                    b.className = 'banner';
                    document.body.appendChild(b);
                }
                b.textContent = t;
                b.classList.add('visible');
                setTimeout(() => b.classList.remove('visible'), 2200);
            }
        </script>
    </body>
</html>

<!--
============================================================
styles.css (extracto mínimo) — solo si no lo tienes ya.
Copia/pega en /styles.css del proyecto y ajústalo a tu gusto.
============================================================

:root { --blue:#2b6cb0; --orange:#ed8936; --green:#2f855a; --bg:#f6f7fb; --text:#1a202c; }
*{box-sizing:border-box}
body{margin:0; font-family:'Poppins',system-ui,-apple-system,Segoe UI,Roboto,Ubuntu,"Helvetica Neue",Arial; color:var(--text)}

.main-bg{background:var(--bg)}
.topbar{display:flex; align-items:center; justify-content:space-between; padding:14px 20px; background:#fff; box-shadow:0 2px 8px rgba(0,0,0,.06)}
.topbar h1{font-size:20px; margin:0}
.user-badge{display:flex; align-items:center; gap:14px}
.user-info span{display:block; font-size:12px; opacity:.7; margin-bottom:2px}
.logout{background:#e53e3e; color:#fff; border:0; padding:8px 12px; border-radius:8px; cursor:pointer}

.conv-shell{display:grid; grid-template-columns:1fr 360px; gap:22px; padding:22px; max-width:1100px; margin:0 auto}
.only-mobile{display:none}
.only-desktop{display:block}
@media (max-width: 980px){
  .conv-shell{grid-template-columns:1fr}
  .only-mobile{display:block}
  .only-desktop{display:none}
}

.conv-card{background:#fff; border-radius:16px; box-shadow:0 8px 24px rgba(0,0,0,.06); overflow:hidden}
.conv-card__header{display:flex; align-items:center; gap:10px; padding:14px 18px; color:#fff}
.header--blue{background:linear-gradient(135deg,var(--blue),#4299e1)}
.header--orange{background:linear-gradient(135deg,var(--orange),#f6ad55)}
.header--green{background:linear-gradient(135deg,var(--green),#48bb78)}
.header-icon{display:inline-grid; place-items:center; width:28px; height:28px; background:rgba(255,255,255,.22); border-radius:50%; font-weight:700}
.conv-card__body{padding:18px}

.input-outline select,.input-outline input{width:100%; padding:12px 14px; border:2px solid #e2e8f0; border-radius:10px; font-size:16px; outline:none}
.input-outline select:focus,.input-outline input:focus{border-color:#718096}
.btn-primary{background:#2b6cb0; color:#fff; border:0; padding:12px 14px; border-radius:10px; cursor:pointer; font-weight:700}
.btn-block{display:block; width:100%; margin-top:10px}

.result-panel{background:#fff; border-radius:16px; padding:18px; box-shadow:0 8px 24px rgba(0,0,0,.06)}
.result-big{font-size:38px; font-weight:800; margin:10px 0}
.muted{opacity:.7}
.alert{background:#fff5f5; color:#742a2a; border:1px solid #fed7d7; padding:10px 12px; border-radius:8px}
.result-chip{margin-top:12px; background:#edf2f7; border-radius:999px; padding:10px 14px}

.fab{position:fixed; right:22px; bottom:22px; border:0; border-radius:50%; width:56px; height:56px; cursor:pointer; box-shadow:0 10px 24px rgba(0,0,0,.15); font-size:22px}
.fab.danger{background:#e53e3e; color:#fff}

.banner{position:fixed; left:50%; transform:translateX(-50%); bottom:22px; background:#2d3748; color:#fff; padding:10px 14px; border-radius:12px; opacity:0; transition:opacity .2s}
.banner.visible{opacity:1}

-->
