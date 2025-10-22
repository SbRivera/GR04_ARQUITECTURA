import { useState } from 'react';
import toast from 'react-hot-toast';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSyncAlt, faArrowRight } from '@fortawesome/free-solid-svg-icons';

const ConversionCard = ({ title, icon, fromUnit, toUnit, conversionFn, gradient, description }) => {
  const [inputValue, setInputValue] = useState('');
  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleConvert = async () => {
    if (!inputValue || isNaN(inputValue)) {
      toast.error('Por favor ingresa un valor numérico válido');
      return;
    }

    setLoading(true);
    try {
      const response = await conversionFn(parseFloat(inputValue));
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

  const handleKeyPress = (e) => {
    if (e.key === 'Enter') {
      handleConvert();
    }
  };

  const handleReset = () => {
    setInputValue('');
    setResult(null);
  };

  return (
    <div className={`bg-gradient-to-br ${gradient} rounded-2xl shadow-xl overflow-hidden transform transition-all duration-300 hover:scale-105 hover:shadow-2xl`}>
      {/* Header de la tarjeta */}
      <div className="bg-black/10 backdrop-blur-sm p-5 border-b border-white/20">
        <div className="flex items-center justify-between">
          <div className="flex-1">
            <h3 className="text-white text-lg font-bold mb-1">{title}</h3>
            <p className="text-white/80 text-xs font-medium">{description}</p>
          </div>
          <div className="bg-white/20 backdrop-blur-sm p-3 rounded-xl">
            <FontAwesomeIcon icon={icon} className="text-white text-2xl" />
          </div>
        </div>
      </div>
      
      {/* Cuerpo de la tarjeta */}
      <div className="p-6 space-y-4">
        {/* Input */}
        <div>
          <label className="text-white text-sm font-semibold mb-2 block flex items-center justify-between">
            <span>Valor de entrada</span>
            <span className="bg-white/20 px-2 py-1 rounded text-xs">{fromUnit}</span>
          </label>
          <input
            type="number"
            value={inputValue}
            onChange={(e) => setInputValue(e.target.value)}
            onKeyPress={handleKeyPress}
            placeholder={`Ingresa el valor en ${fromUnit}`}
            className="w-full px-4 py-3 rounded-xl border-2 border-white/30 bg-white/20 text-white placeholder-white/60 focus:outline-none focus:border-white focus:bg-white/30 transition-all backdrop-blur-sm font-semibold"
          />
        </div>

        {/* Botones */}
        <div className="flex space-x-2">
          <button
            onClick={handleConvert}
            disabled={loading}
            className="flex-1 bg-white text-gray-800 font-bold py-3 rounded-xl hover:bg-gray-100 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed shadow-lg hover:shadow-xl flex items-center justify-center space-x-2"
          >
            {loading ? (
              <>
                <svg className="animate-spin h-5 w-5" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none"></circle>
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                <span>Procesando...</span>
              </>
            ) : (
              <>
                <FontAwesomeIcon icon={faArrowRight} />
                <span>Convertir</span>
              </>
            )}
          </button>
          {(inputValue || result) && (
            <button
              onClick={handleReset}
              className="bg-white/20 backdrop-blur-sm text-white font-bold px-4 rounded-xl hover:bg-white/30 transition-all duration-200 shadow-lg"
              title="Limpiar"
            >
              <FontAwesomeIcon icon={faSyncAlt} />
            </button>
          )}
        </div>

        {/* Resultado */}
        {result && (
          <div className="bg-white/20 backdrop-blur-sm rounded-xl p-5 border-2 border-white/30 animate-fadeIn">
            <div className="flex items-center justify-between mb-2">
              <p className="text-white text-sm font-semibold">Resultado:</p>
              <span className="bg-white/30 px-2 py-1 rounded text-white text-xs font-bold">{result.outputUnit}</span>
            </div>
            <p className="text-white text-4xl font-bold break-all">
              {result.output.toFixed(4)}
            </p>
            <div className="mt-3 pt-3 border-t border-white/20">
              <p className="text-white/80 text-xs">
                {inputValue} {fromUnit} = {result.output.toFixed(4)} {toUnit}
              </p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default ConversionCard;