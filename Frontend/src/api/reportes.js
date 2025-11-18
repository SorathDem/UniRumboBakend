import client from './client';

/**
 * Genera y descarga el reporte PDF de rutas más usadas
 * @param {string} usuarioNombre - Nombre del usuario que genera el reporte
 */
export async function generarReporteRutas(usuarioNombre) {
  try {
    const response = await client.post('/reportes/rutas-mas-usadas', 
      { usuarioNombre },
      { responseType: 'blob' } // importante para archivos binarios
    );
    
    // Crear blob y descargar
    const blob = new Blob([response.data], { type: 'application/pdf' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `Reporte_Rutas_${new Date().toISOString().slice(0,10)}.pdf`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
    
    return { success: true };
  } catch (error) {
    console.error('Error generando reporte de rutas:', error);
    return { success: false, error: error.message };
  }
}

/**
 * Genera y descarga el reporte PDF de alojamientos
 * @param {string} usuarioNombre - Nombre del usuario que genera el reporte
 */
export async function generarReporteAlojamientos(usuarioNombre) {
  try {
    const response = await client.post('/reportes/alojamientos', 
      { usuarioNombre },
      { responseType: 'blob' }
    );
    
    // Crear blob y descargar
    const blob = new Blob([response.data], { type: 'application/pdf' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `Reporte_Alojamientos_${new Date().toISOString().slice(0,10)}.pdf`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
    
    return { success: true };
  } catch (error) {
    console.error('Error generando reporte de alojamientos:', error);
    return { success: false, error: error.message };
  }
}
