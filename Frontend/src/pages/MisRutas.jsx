import { useEffect, useState } from "react";
import Navbar from "../components/Navbar";
import { misRutas } from "../api/rutas";
import { Link } from "react-router-dom";

const USUARIO_DEMO = 2;

export default function MisRutas(){
  const [list,setList]=useState([]);
  const [loading,setLoading]=useState(true);
  const [error,setError]=useState("");

  useEffect(()=>{ (async()=>{
    setError(""); setLoading(true);
    try{ setList(await misRutas(USUARIO_DEMO)); }
    catch(e){ setError("No se pudieron cargar tus rutas"); }
    finally{ setLoading(false); }
  })(); },[]);

  return (
    <>
      <Navbar persona="Conductor"/>
      <div className="container">
        <div className="header-slab"><div className="title">Mis Rutas</div></div>
        {loading && <div className="card">Cargando…</div>}
        {error && <div className="card">{error}</div>}

        <div className="grid">
          {list.map(r => (
            <div key={r.id} className="card">
              <h3>{r.titulo}</h3>
              <div className="kv">📍 <b>Origen:</b> {r.origen}</div>
              <div className="kv">🎯 <b>Destino:</b> {r.destino}</div>
              <div className="kv">👥 <b>Cupos:</b> {r.cupos}</div>
              <div className="kv">🕒 <b>Salida:</b> {new Date(r.horaSalida).toLocaleString()}</div>
              {r.horaRegreso && <div className="kv">↩ <b>Regreso:</b> {new Date(r.horaRegreso).toLocaleString()}</div>}
              <div className="footer-actions">
                <Link className="btn outline" to={`/rutas/editar/${r.id}`}>Editar</Link>
              </div>
            </div>
          ))}
        </div>

        {!loading && !error && list.length===0 && (
          <div className="empty">Aún no tienes rutas creadas.</div>
        )}
      </div>
    </>
  );
}
