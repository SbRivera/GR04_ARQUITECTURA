import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faExchangeAlt, faUser, faSignOutAlt } from '@fortawesome/free-solid-svg-icons';

const Navbar = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    toast.success('Sesión cerrada correctamente');
    navigate('/login');
  };

  return (
    <nav className="bg-gradient-to-r from-indigo-600 via-purple-600 to-pink-500 shadow-xl sticky top-0 z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          {/* Logo y título */}
          <div className="flex items-center space-x-4">
            <div className="flex items-center space-x-3">
              <div className="bg-white/20 backdrop-blur-sm p-2.5 rounded-xl">
                <FontAwesomeIcon icon={faExchangeAlt} className="text-white text-xl" />
              </div>
              <div>
                <h1 className="text-white text-2xl font-bold tracking-tight">Conversor de Unidades</h1>
              </div>
            </div>
          </div>

          {/* Usuario y logout */}
          {user && (
            <div className="flex items-center space-x-4">
              <div className="hidden sm:flex items-center space-x-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-xl">
                <FontAwesomeIcon icon={faUser} className="text-white" />
                <span className="text-white font-semibold text-sm">
                  {user.username}
                </span>
              </div>
              <button
                onClick={handleLogout}
                className="bg-white text-purple-600 px-4 py-2 rounded-xl font-semibold hover:bg-purple-50 transition-all duration-200 flex items-center space-x-2 shadow-lg hover:shadow-xl transform hover:scale-105"
              >
                <FontAwesomeIcon icon={faSignOutAlt} />
                <span className="hidden sm:inline">Cerrar Sesión</span>
              </button>
            </div>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;