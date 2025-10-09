
import { Link } from "react-router-dom";
export default function RouteCard({ r }){
  return (
    <div className="card">
      <h3>🚗 {r.titulo}</h3>
      <div className="helper">Conductor: Nombre</div>
      <div className="kv">📍 <b>Origen:</b> {r.origen}</div>
      <div className="kv">🎯 <b>Destino:</b> {r.destino}</div>
      <div className="kv">👥 <b>Cupos:</b> {r.cupos} &nbsp;&nbsp; 🕒 <b>Salida:</b> {new Date(r.horaSalida).toLocaleTimeString()}</div>
      {r.horaRegreso && <div className="kv">↩ <b>Regreso:</b> {new Date(r.horaRegreso).toLocaleTimeString()}</div>}
      <div className="footer-actions">
        <Link className="btn outline" to={`/rutas/${r.id}`}>Ver detalles</Link>
        <Link className="btn primary" to={`/rutas/${r.id}`}>Aplicar</Link>
      </div>
    </div>
  );
}
