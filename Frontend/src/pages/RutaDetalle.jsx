
import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import Navbar from "../components/Navbar";
import RouteMap from "../components/RouteMap";
import { rutaPorId, geoRuta, crearSolicitud } from "../api/rutas";

const USUARIO_DEMO = 2;

export default function RutaDetalle(){
  const { id } = useParams();
  const [ruta,setRuta]=useState(null);
  const [geo,setGeo]=useState(null);
  const [loading,setLoading]=useState(true);
  const [msg,setMsg]=useState("");
  const [sending,setSending]=useState(false);

  useEffect(()=>{ (async()=>{
    try{
      const r = await rutaPorId(id);
      setRuta(r);
      try{ setGeo(await geoRuta(id)); }catch{}
    } finally { setLoading(false); }
  })(); },[id]);

  const solicitar = async ()=>{
    setSending(true); setMsg("");
    try{
      await crearSolicitud({ idUsuario: USUARIO_DEMO, idRuta: Number(id), idEstado: 1 });
      setMsg("Solicitud enviada ✅");
    }catch{ setMsg("No se pudo enviar la solicitud"); }
    finally{ setSending(false); }
  };

  if(loading) return (<><Navbar persona="Cliente"/><div className="container"><div className="card">Cargando…</div></div></>);
  if(!ruta) return (<><Navbar persona="Cliente"/><div className="container"><div className="card">No se encontró la ruta.</div></div></>);

  return (
    <>
      <Navbar persona="Cliente"/>
      <div className="container">
        <div className="header-slab">
          <div className="title">Ruta - {ruta.destino}</div>
          <Link className="btn outline" to="/">Volver</Link>
        </div>

        <div className="grid">
          <div className="card">
            <div className="kv">📍 <b>Origen:</b> {ruta.origen}</div>
            <div className="kv">🎯 <b>Destino:</b> {ruta.destino}</div>
            <div className="kv">🕒 <b>Salida:</b> {new Date(ruta.horaSalida).toLocaleString()}</div>
            {ruta.horaRegreso && <div className="kv">↩ <b>Regreso:</b> {new Date(ruta.horaRegreso).toLocaleString()}</div>}
            <div className="kv">👥 <b>Cupos:</b> {ruta.cupos}</div>

            <div className="footer-actions">
              <button className="btn primary" disabled={sending} onClick={solicitar}>Solicitar</button>
              {msg && <div className="helper" style={{marginLeft:8}}>{msg}</div>}
            </div>
          </div>

          <div className="mapWrap">
            <h3 style={{margin:"6px 0 12px"}}>Mapa de la ruta</h3>
            <RouteMap featureCollection={geo}/>
            {!geo && <div className="helper" style={{marginTop:8}}>No se pudo calcular el recorrido automáticamente.</div>}
          </div>
        </div>
      </div>
    </>
  );
}
