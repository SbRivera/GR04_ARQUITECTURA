import { useState } from 'react';
import Navbar from '../components/Navbar';
import { conversions } from '../services/api';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { 
  faRuler, 
  faTemperatureHigh, 
  faWeight, 
  faArrowsAltH, 
  faThermometerHalf, 
  faBalanceScale,
  faArrowRight,
  faExchangeAlt,
  faSyncAlt,
  faEye,
  faEyeSlash
} from '@fortawesome/free-solid-svg-icons';
import toast from 'react-hot-toast';

const Converter = () => {
  const [selectedConversion, setSelectedConversion] = useState(null);
  const [inputValue, setInputValue] = useState('');
  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(false);

  const conversionOptions = [
    {
      id: 'cm-to-in',
      title: 'Centímetros → Pulgadas',
      icon: faRuler,
      fromUnit: 'cm',
      toUnit: 'in',
      conversionFn: conversions.cmToIn,
      gradient: 'from-blue-500 via-cyan-500 to-teal-500',
      category: 'Longitud'
    },
    {
      id: 'in-to-cm',
      title: 'Pulgadas → Centímetros',
      icon: faArrowsAltH,
      fromUnit: 'in',
      toUnit: 'cm',
      conversionFn: conversions.inToCm,
      gradient: 'from-indigo-500 via-purple-500 to-pink-500',
      category: 'Longitud'
    },
    {
      id: 'c-to-f',
      title: 'Celsius → Fahrenheit',
      icon: faTemperatureHigh,
      fromUnit: '°C',
      toUnit: '°F',
      conversionFn: conversions.cToF,
      gradient: 'from-red-500 via-orange-500 to-yellow-500',
      category: 'Temperatura'
    },
    {
      id: 'f-to-c',
      title: 'Fahrenheit → Celsius',
      icon: faThermometerHalf,
      fromUnit: '°F',
      toUnit: '°C',
      conversionFn: conversions.fToC,
      gradient: 'from-cyan-400 via-blue-500 to-indigo-600',
      category: 'Temperatura'
    },
    {
      id: 'kg-to-lb',
      title: 'Kilogramos → Libras',
      icon: faWeight,
      fromUnit: 'kg',
      toUnit: 'lb',
      conversionFn: conversions.kgToLb,
      gradient: 'from-green-500 via-emerald-500 to-teal-600',
      category: 'Peso'
    },
    {
      id: 'lb-to-kg',
      title: 'Libras → Kilogramos',
      icon: faBalanceScale,
      fromUnit: 'lb',
      toUnit: 'kg',
      conversionFn: conversions.lbToKg,
      gradient: 'from-purple-500 via-pink-500 to-rose-500',
      category: 'Peso'
    }
  ];

  const handleConvert = async () => {
    if (!inputValue || isNaN(inputValue)) {
      toast.error('Por favor ingresa un valor numérico válido');
      return;
    }

    setLoading(true);
    try {
      const response = await selectedConversion.conversionFn(parseFloat(inputValue));
      setResult(response.data);
      toast.success('¡Conversión exitosa!');
    } catch (error) {
      console.error('Error completo:', error);
      
      if (error.code === 'ERR_NETWORK' || error.message === 'Network Error') {
        toast.error('⚠️ Servidor no disponible. Verifica que el servidor Java esté corriendo en el puerto 8080');
      } else if (error.response) {
        toast.error(`Error del servidor: ${error.response.status}`);
      } else {
        toast.error('Error de conexión. Verifica el servidor backend');
      }
    } finally {
      setLoading(false);
    }
  };

  const handleReset = () => {
    setSelectedConversion(null);
    setInputValue('');
    setResult(null);
  };

  const handleKeyPress = (e) => {
    if (e.key === 'Enter') {
      handleConvert();
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900 relative overflow-hidden">
      {/* Partículas animadas de fondo */}
      <div className="absolute inset-0 overflow-hidden pointer-events-none">
        {[...Array(20)].map((_, i) => (
          <div
            key={i}
            className="absolute bg-purple-400 rounded-full opacity-20 animate-float"
            style={{
              width: Math.random() * 6 + 2 + 'px',
              height: Math.random() * 6 + 2 + 'px',
              left: Math.random() * 100 + '%',
              top: Math.random() * 100 + '%',
              animationDelay: Math.random() * 5 + 's',
              animationDuration: Math.random() * 10 + 10 + 's'
            }}
          />
        ))}
      </div>

      <Navbar />
      
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8 md:py-12 relative z-10">
        {/* Vista de selección de conversión */}
        {!selectedConversion ? (
          <div className="animate-fadeIn">
            {/* Header */}
            <div className="text-center mb-12">
              <div className="inline-block mb-6">
                <div className="relative">
                  <div className="absolute inset-0 bg-gradient-to-r from-purple-500 to-pink-500 rounded-full blur-xl opacity-60 animate-pulse"></div>
                  <div className="relative bg-white/10 backdrop-blur-xl p-6 rounded-full border-4 border-white/20">
                    <FontAwesomeIcon icon={faExchangeAlt} className="text-6xl text-white" />
                  </div>
                </div>
              </div>
              <h1 className="text-4xl md:text-6xl font-black text-white mb-4 drop-shadow-2xl">
                ¿Qué deseas convertir?
              </h1>
              <p className="text-purple-200 text-lg md:text-xl max-w-2xl mx-auto">
                Selecciona el tipo de conversión que necesitas realizar
              </p>
            </div>

            {/* Grid de opciones de conversión - Grandes y llamativas */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 max-w-6xl mx-auto">
              {conversionOptions.map((option, index) => (
                <button
                  key={option.id}
                  onClick={() => setSelectedConversion(option)}
                  className={`
                    group relative p-8 rounded-3xl overflow-hidden
                    transform transition-all duration-300 hover:scale-105
                    bg-gradient-to-br ${option.gradient}
                    shadow-2xl hover:shadow-purple-500/50
                    border-2 border-white/20
                    animate-slideIn
                  `}
                  style={{ animationDelay: `${index * 0.1}s` }}
                >
                  {/* Efecto de brillo al hover */}
                  <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/20 to-transparent -translate-x-full group-hover:translate-x-full transition-transform duration-1000"></div>
                  
                  <div className="relative z-10">
                    <div className="flex items-center justify-between mb-4">
                      <span className="text-xs font-bold text-white/80 uppercase tracking-wider">
                        {option.category}
                      </span>
                      <FontAwesomeIcon 
                        icon={option.icon} 
                        className="text-4xl text-white group-hover:scale-110 transition-transform" 
                      />
                    </div>
                    
                    <h3 className="text-2xl font-black text-white mb-2">
                      {option.title}
                    </h3>
                    
                    <div className="flex items-center justify-center space-x-2 text-white/90 mt-4">
                      <span className="text-lg font-semibold">{option.fromUnit}</span>
                      <FontAwesomeIcon icon={faArrowRight} className="text-sm" />
                      <span className="text-lg font-semibold">{option.toUnit}</span>
                    </div>

                    <div className="mt-4 flex items-center justify-center">
                      <span className="text-white/80 text-sm font-medium group-hover:text-white transition-colors">
                        Click para convertir →
                      </span>
                    </div>
                  </div>
                </button>
              ))}
            </div>
          </div>
        ) : (
          /* Vista de conversión activa */
          <div className="max-w-2xl mx-auto animate-fadeIn">
            <button
              onClick={handleReset}
              className="mb-8 flex items-center space-x-2 text-purple-200 hover:text-white transition-colors group"
            >
              <FontAwesomeIcon icon={faArrowRight} className="rotate-180 group-hover:-translate-x-2 transition-transform" />
              <span className="font-semibold">Volver a seleccionar</span>
            </button>

            <div className={`bg-gradient-to-br ${selectedConversion.gradient} rounded-3xl shadow-2xl overflow-hidden border-4 border-white/20`}>
              {/* Header de la conversión */}
              <div className="bg-black/20 backdrop-blur-sm p-6 border-b-2 border-white/20">
                <div className="flex items-center justify-between">
                  <div>
                    <span className="text-xs font-bold text-white/80 uppercase tracking-wider block mb-2">
                      {selectedConversion.category}
                    </span>
                    <h2 className="text-3xl font-black text-white">
                      {selectedConversion.title}
                    </h2>
                  </div>
                  <div className="bg-white/20 backdrop-blur-xl p-4 rounded-2xl">
                    <FontAwesomeIcon icon={selectedConversion.icon} className="text-5xl text-white" />
                  </div>
                </div>
              </div>

              {/* Formulario de conversión */}
              <div className="p-8">
                <div className="space-y-6">
                  {/* Input */}
                  <div>
                    <label className="block text-white text-sm font-bold mb-3 ml-1">
                      Valor a convertir ({selectedConversion.fromUnit})
                    </label>
                    <input
                      type="number"
                      value={inputValue}
                      onChange={(e) => setInputValue(e.target.value)}
                      onKeyPress={handleKeyPress}
                      placeholder={`Ingresa el valor en ${selectedConversion.fromUnit}`}
                      className="w-full px-6 py-4 rounded-2xl border-2 border-white/30 bg-white/20 backdrop-blur-xl text-white text-2xl font-bold placeholder-white/50 focus:border-white focus:ring-4 focus:ring-white/30 focus:outline-none transition-all"
                      autoFocus
                    />
                  </div>

                  {/* Botón convertir */}
                  <button
                    onClick={handleConvert}
                    disabled={loading || !inputValue}
                    className="w-full bg-white text-gray-900 font-black py-5 rounded-2xl hover:bg-gray-100 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed transform hover:scale-105 active:scale-95 shadow-2xl flex items-center justify-center space-x-3 text-lg"
                  >
                    {loading ? (
                      <>
                        <svg className="animate-spin h-6 w-6" viewBox="0 0 24 24">
                          <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none"></circle>
                          <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                        </svg>
                        <span>Convirtiendo...</span>
                      </>
                    ) : (
                      <>
                        <FontAwesomeIcon icon={faExchangeAlt} />
                        <span>CONVERTIR</span>
                      </>
                    )}
                  </button>

                  {/* Resultado */}
                  {result && (
                    <div className="bg-white/20 backdrop-blur-xl rounded-2xl p-6 border-2 border-white/30 animate-fadeIn">
                      <p className="text-white text-sm font-semibold mb-2 uppercase tracking-wider">
                        Resultado:
                      </p>
                      <div className="flex items-baseline space-x-2">
                        <p className="text-white text-5xl font-black">
                          {result.output.toFixed(4)}
                        </p>
                        <p className="text-white/80 text-2xl font-bold">
                          {result.outputUnit}
                        </p>
                      </div>
                      <div className="mt-4 pt-4 border-t border-white/20">
                        <p className="text-white/80 text-sm">
                          {inputValue} {selectedConversion.fromUnit} = {result.output.toFixed(4)} {selectedConversion.toUnit}
                        </p>
                      </div>
                    </div>
                  )}
                </div>
              </div>
            </div>
          </div>
        )}
      </div>

      <style>{`
        @keyframes float {
          0%, 100% {
            transform: translateY(0) translateX(0);
          }
          25% {
            transform: translateY(-20px) translateX(10px);
          }
          50% {
            transform: translateY(-10px) translateX(-10px);
          }
          75% {
            transform: translateY(-30px) translateX(5px);
          }
        }
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
        .animate-float {
          animation: float 15s ease-in-out infinite;
        }
        .animate-slideIn {
          animation: slideIn 0.5s ease-out forwards;
          opacity: 0;
        }
      `}</style>
    </div>
  );
};

export default Converter;