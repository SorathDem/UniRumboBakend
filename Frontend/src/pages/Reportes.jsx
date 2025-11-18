import { useState } from 'react';
import { generarReporteRutas, generarReporteAlojamientos } from '../api/reportes';
import Navbar from '../components/Navbar';
import '../styles.css';

export default function Reportes() {
  const [tabActiva, setTabActiva] = useState('rutas');
  const [usuarioNombre, setUsuarioNombre] = useState('');
  const [cargando, setCargando] = useState(false);
  const [mensaje, setMensaje] = useState('');

  const handleGenerarReporteRutas = async (e) => {
    e.preventDefault();
    
    if (!usuarioNombre.trim()) {
      setMensaje('⚠️ Por favor ingresa tu nombre');
      return;
    }

    setCargando(true);
    setMensaje('');

    const resultado = await generarReporteRutas(usuarioNombre);
    
    if (resultado.success) {
      setMensaje('✅ Reporte de Rutas generado y descargado exitosamente');
    } else {
      setMensaje('❌ Error al generar el reporte: ' + resultado.error);
    }

    setCargando(false);
  };

  const handleGenerarReporteAlojamientos = async (e) => {
    e.preventDefault();
    
    if (!usuarioNombre.trim()) {
      setMensaje('⚠️ Por favor ingresa tu nombre');
      return;
    }

    setCargando(true);
    setMensaje('');

    const resultado = await generarReporteAlojamientos(usuarioNombre);
    
    if (resultado.success) {
      setMensaje('✅ Reporte de Alojamientos generado y descargado exitosamente');
    } else {
      setMensaje('❌ Error al generar el reporte: ' + resultado.error);
    }

    setCargando(false);
  };

  return (
    <>
      <Navbar persona="Estudiante" nombre="Usuario" />
      <div className="reportes-container">
        <div className="reportes-header">
          <h1>📊 Generación de Reportes PDF</h1>
          <p>Descarga reportes personalizados con información actualizada</p>
        </div>

      {/* Tabs */}
      <div className="tabs-container">
        <button 
          className={`tab ${tabActiva === 'rutas' ? 'tab-active' : ''}`}
          onClick={() => setTabActiva('rutas')}
        >
          🚗 Rutas Más Usadas
        </button>
        <button 
          className={`tab ${tabActiva === 'alojamientos' ? 'tab-active' : ''}`}
          onClick={() => setTabActiva('alojamientos')}
        >
          🏠 Alojamientos
        </button>
      </div>

      {/* Contenido de las tabs */}
      <div className="tab-content">
        {tabActiva === 'rutas' && (
          <div className="reporte-card">
            <div className="reporte-icon">🚗</div>
            <h2>Reporte de Rutas Más Usadas</h2>
            <p className="reporte-descripcion">
              Este reporte incluye estadísticas de las rutas más utilizadas en el sistema, 
              con información sobre origen, destino, cantidad de usos, usuarios, y distancias.
            </p>
            
            <div className="reporte-features">
              <div className="feature-item">
                <span className="feature-icon">📈</span>
                <span>Estadísticas de uso</span>
              </div>
              <div className="feature-item">
                <span className="feature-icon">👥</span>
                <span>Cantidad de usuarios</span>
              </div>
              <div className="feature-item">
                <span className="feature-icon">📍</span>
                <span>Distancias y días más usados</span>
              </div>
              <div className="feature-item">
                <span className="feature-icon">🎨</span>
                <span>Diseño personalizado</span>
              </div>
            </div>

            <form onSubmit={handleGenerarReporteRutas} className="reporte-form">
              <div className="form-group">
                <label htmlFor="usuario-rutas">Tu nombre completo:</label>
                <input
                  id="usuario-rutas"
                  type="text"
                  value={usuarioNombre}
                  onChange={(e) => setUsuarioNombre(e.target.value)}
                  placeholder="Ej: Joseph García"
                  className="input-usuario"
                  disabled={cargando}
                />
              </div>

              <button 
                type="submit" 
                className="btn-generar-pdf"
                disabled={cargando}
              >
                {cargando ? '⏳ Generando PDF...' : '📥 Generar Reporte de Rutas'}
              </button>
            </form>

            {mensaje && (
              <div className={`mensaje ${mensaje.includes('✅') ? 'mensaje-exito' : mensaje.includes('❌') ? 'mensaje-error' : 'mensaje-advertencia'}`}>
                {mensaje}
              </div>
            )}
          </div>
        )}

        {tabActiva === 'alojamientos' && (
          <div className="reporte-card">
            <div className="reporte-icon">🏠</div>
            <h2>Reporte de Alojamientos</h2>
            <p className="reporte-descripcion">
              Este reporte muestra el estado de ocupación de todos los alojamientos disponibles, 
              con detalles de capacidad, espacios ocupados y disponibilidad por sede.
            </p>
            
            <div className="reporte-features">
              <div className="feature-item">
                <span className="feature-icon">🏢</span>
                <span>Información por sede</span>
              </div>
              <div className="feature-item">
                <span className="feature-icon">📊</span>
                <span>Porcentaje de ocupación</span>
              </div>
              <div className="feature-item">
                <span className="feature-icon">✅</span>
                <span>Estado de disponibilidad</span>
              </div>
              <div className="feature-item">
                <span className="feature-icon">🎨</span>
                <span>Diseño personalizado</span>
              </div>
            </div>

            <form onSubmit={handleGenerarReporteAlojamientos} className="reporte-form">
              <div className="form-group">
                <label htmlFor="usuario-alojamientos">Tu nombre completo:</label>
                <input
                  id="usuario-alojamientos"
                  type="text"
                  value={usuarioNombre}
                  onChange={(e) => setUsuarioNombre(e.target.value)}
                  placeholder="Ej: Joseph García"
                  className="input-usuario"
                  disabled={cargando}
                />
              </div>

              <button 
                type="submit" 
                className="btn-generar-pdf alojamientos"
                disabled={cargando}
              >
                {cargando ? '⏳ Generando PDF...' : '📥 Generar Reporte de Alojamientos'}
              </button>
            </form>

            {mensaje && (
              <div className={`mensaje ${mensaje.includes('✅') ? 'mensaje-exito' : mensaje.includes('❌') ? 'mensaje-error' : 'mensaje-advertencia'}`}>
                {mensaje}
              </div>
            )}
          </div>
        )}
      </div>
      </div>
    </>
  );
}
