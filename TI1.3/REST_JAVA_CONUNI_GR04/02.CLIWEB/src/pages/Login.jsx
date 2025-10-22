import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import toast from "react-hot-toast";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faUser,
  faLock,
  faArrowRight,
  faEye,
  faEyeSlash,
  faKey,
  faShieldAlt,
} from "@fortawesome/free-solid-svg-icons";

const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const navigate = useNavigate();
  const { login } = useAuth();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    const result = login(username, password);

    setTimeout(() => {
      if (result.success) {
        toast.success("¡Bienvenido!");
        navigate("/converter");
      } else {
        toast.error(result.message);
      }
      setLoading(false);
    }, 500);
  };

  return (
    <div className="min-h-screen flex items-center justify-center p-4 relative overflow-hidden bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900">
      {/* Partículas flotantes de fondo */}
      <div className="absolute inset-0 overflow-hidden pointer-events-none">
        <div className="absolute top-20 left-10 w-2 h-2 bg-purple-400 rounded-full animate-ping"></div>
        <div className="absolute top-40 right-20 w-3 h-3 bg-pink-400 rounded-full animate-pulse"></div>
        <div className="absolute bottom-32 left-32 w-2 h-2 bg-blue-400 rounded-full animate-ping"></div>
        <div className="absolute top-60 right-40 w-1 h-1 bg-yellow-400 rounded-full animate-pulse"></div>
        <div className="absolute bottom-20 right-10 w-2 h-2 bg-purple-400 rounded-full animate-ping"></div>
      </div>

      {/* Grid con diseño de dos columnas */}
      <div className="max-w-6xl w-full relative z-10 grid lg:grid-cols-2 gap-8 items-center">
        {/* Columna izquierda - Visual con imagen GRANDE */}
        <div className="hidden lg:flex flex-col items-center justify-center relative">
          <div className="relative group">
            {/* Anillos animados alrededor de la imagen */}
            <div className="absolute inset-0 rounded-full bg-gradient-to-r from-purple-500 via-pink-500 to-purple-500 blur-2xl opacity-60 group-hover:opacity-80 transition-all duration-700 animate-pulse"></div>
            <div className="absolute -inset-4 rounded-full border-4 border-purple-400/30 animate-spin-slow"></div>
            <div className="absolute -inset-8 rounded-full border-4 border-pink-400/20 animate-spin-reverse"></div>
            
            {/* Imagen principal GRANDE */}
            <div className="relative z-10">
              <img
                src="/Image/Profile.jpg"
                alt="ConUni"
                className="w-80 h-80 rounded-full object-cover border-8 border-white/20 shadow-2xl backdrop-blur-xl group-hover:scale-105 transition-transform duration-500"
                onError={(e) => {
                  e.target.style.display = "none";
                  e.target.nextElementSibling.style.display = "flex";
                }}
              />
              <div className="w-80 h-80 rounded-full bg-gradient-to-br from-purple-600 to-pink-600 border-8 border-white/20 shadow-2xl backdrop-blur-xl hidden items-center justify-center group-hover:scale-105 transition-transform duration-500">
                <FontAwesomeIcon icon={faShieldAlt} className="text-9xl text-white" />
              </div>
            </div>

            {/* Iconos flotantes decorativos */}
            <div className="absolute -top-8 -right-8 bg-white/10 backdrop-blur-xl p-4 rounded-2xl border border-white/20 animate-bounce-slow">
              <FontAwesomeIcon icon={faKey} className="text-4xl text-purple-400" />
            </div>
            <div className="absolute -bottom-8 -left-8 bg-white/10 backdrop-blur-xl p-4 rounded-2xl border border-white/20 animate-bounce-slow animation-delay-1000">
              <FontAwesomeIcon icon={faShieldAlt} className="text-4xl text-pink-400" />
            </div>
          </div>

          {/* Texto decorativo */}
          <div className="mt-12 text-center max-w-md">
            <h2 className="text-5xl font-black text-white mb-4 drop-shadow-2xl">
              Conversor de Unidades
            </h2>
           
            <p className="text-purple-300 text-sm">
              Convierte medidas con precisión y estilo
            </p>
          </div>
        </div>

        {/* Columna derecha - Formulario */}
        <div className="w-full max-w-md mx-auto lg:mx-0">
        {/* Columna derecha - Formulario */}
        <div className="w-full max-w-md mx-auto lg:mx-0">
          {/* Logo móvil */}
          <div className="lg:hidden text-center mb-8">
            <img
              src="/Image/Profile.jpg"
              alt="ConUni"
              className="w-32 h-32 rounded-full object-cover border-4 border-white/20 shadow-2xl mx-auto mb-4"
              onError={(e) => {
                e.target.style.display = "none";
                e.target.nextElementSibling.style.display = "flex";
              }}
            />
            <div className="w-32 h-32 rounded-full bg-gradient-to-br from-purple-600 to-pink-600 border-4 border-white/20 shadow-2xl mx-auto mb-4 hidden items-center justify-center">
              <FontAwesomeIcon icon={faShieldAlt} className="text-6xl text-white" />
            </div>
            <h2 className="text-4xl font-black text-white mb-2">Conversor de Unidades</h2>
          </div>

          {/* Card del formulario */}
          <div className="bg-white/10 backdrop-blur-2xl rounded-3xl shadow-2xl border border-white/20 overflow-hidden">
            <div className="p-8 md:p-10">
              <div className="text-center mb-8">
                <h3 className="text-3xl font-bold text-white mb-2">
                  Iniciar Sesión
                </h3>
                <p className="text-purple-200 text-sm">
                  Ingresa tus credenciales para continuar
                </p>
              </div>

              <form onSubmit={handleSubmit} className="space-y-6">
                {/* Input Usuario */}
                <div className="space-y-2">
                  <label className="block text-white text-sm font-bold ml-1">
                    Usuario
                  </label>
                  <div className="relative group">
                    <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none z-10">
                      <FontAwesomeIcon
                        icon={faUser}
                        className="text-purple-300 group-focus-within:text-purple-100 transition-colors duration-200"
                      />
                    </div>
                    <input
                      type="text"
                      value={username}
                      onChange={(e) => setUsername(e.target.value)}
                      className="w-full pl-12 pr-4 py-4 rounded-2xl border-2 border-white/20 bg-white/10 backdrop-blur-xl text-white placeholder-purple-200 focus:border-purple-400 focus:ring-4 focus:ring-purple-500/30 focus:outline-none transition-all duration-200 font-medium"
                      placeholder="Ingresa tu usuario"
                      required
                    />
                  </div>
                </div>

                {/* Input Contraseña */}
                <div className="space-y-2">
                  <label className="block text-white text-sm font-bold ml-1">
                    Contraseña
                  </label>
                  <div className="relative group">
                    <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none z-10">
                      <FontAwesomeIcon
                        icon={faLock}
                        className="text-purple-300 group-focus-within:text-purple-100 transition-colors duration-200"
                      />
                    </div>
                    <input
                      type={showPassword ? "text" : "password"}
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                      className="w-full pl-12 pr-14 py-4 rounded-2xl border-2 border-white/20 bg-white/10 backdrop-blur-xl text-white placeholder-purple-200 focus:border-purple-400 focus:ring-4 focus:ring-purple-500/30 focus:outline-none transition-all duration-200 font-medium"
                      placeholder="Ingresa tu contraseña"
                      required
                    />
                    <button
                      type="button"
                      onClick={() => setShowPassword(!showPassword)}
                      className="absolute inset-y-0 right-0 pr-4 flex items-center z-10 hover:scale-110 transition-transform"
                    >
                      <FontAwesomeIcon
                        icon={showPassword ? faEyeSlash : faEye}
                        className="text-purple-300 hover:text-purple-100 transition-colors duration-200"
                      />
                    </button>
                  </div>
                </div>

                {/* Botón de login */}
                <button
                  type="submit"
                  disabled={loading}
                  className="w-full bg-gradient-to-r from-purple-500 via-pink-500 to-purple-500 hover:from-purple-600 hover:via-pink-600 hover:to-purple-600 text-white font-bold py-4 rounded-2xl transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed transform hover:scale-105 active:scale-95 shadow-2xl hover:shadow-purple-500/50 flex items-center justify-center space-x-3 group relative overflow-hidden"
                >
                  <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/20 to-transparent translate-x-[-200%] group-hover:translate-x-[200%] transition-transform duration-1000"></div>
                  {loading ? (
                    <>
                      <svg className="animate-spin h-6 w-6" viewBox="0 0 24 24">
                        <circle
                          className="opacity-25"
                          cx="12"
                          cy="12"
                          r="10"
                          stroke="currentColor"
                          strokeWidth="4"
                          fill="none"
                        ></circle>
                        <path
                          className="opacity-75"
                          fill="currentColor"
                          d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                        ></path>
                      </svg>
                      <span className="text-lg">Ingresando...</span>
                    </>
                  ) : (
                    <>
                      <span className="text-lg relative z-10">Iniciar Sesión</span>
                      <FontAwesomeIcon
                        icon={faArrowRight}
                        className="group-hover:translate-x-2 transition-transform duration-300 relative z-10"
                      />
                    </>
                  )}
                </button>
              </form>

             
            </div>
          </div>

          {/* Footer */}
          <p className="text-center text-purple-200 text-sm mt-6 opacity-90">
            GRUPO 04
          </p>
        </div>
      </div>

      <style>{`
        @keyframes spin-slow {
          from { transform: rotate(0deg); }
          to { transform: rotate(360deg); }
        }
        @keyframes spin-reverse {
          from { transform: rotate(360deg); }
          to { transform: rotate(0deg); }
        }
        @keyframes bounce-slow {
          0%, 100% { transform: translateY(0); }
          50% { transform: translateY(-10px); }
        }
        .animate-spin-slow {
          animation: spin-slow 20s linear infinite;
        }
        .animate-spin-reverse {
          animation: spin-reverse 25s linear infinite;
        }
        .animate-bounce-slow {
          animation: bounce-slow 3s ease-in-out infinite;
        }
        .animation-delay-1000 {
          animation-delay: 1s;
        }
      `}</style>
      </div>
    </div>
  );
};

export default Login;
