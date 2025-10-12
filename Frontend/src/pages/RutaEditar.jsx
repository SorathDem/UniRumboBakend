import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Navbar from "../components/Navbar";
import L from "leaflet";
import { editarRuta, rutaPorId } from "../api/rutas";

export default function RutaEditar(){
  const { id } = useParams();
  const navigate = useNavigate();
  const [form,setForm]=useState({
    PuntoOrigen:"", PuntoDestino:"", CuposIda:1, CuposVuelta:null,
    HoraSalida:"", HoraRegreso:"", IdUsuario:2, IdVehiculo:1
  });
  const [msg,setMsg]=useState(""); const [loading,setLoading]=useState(true);
  const mapRef = useRef(null); const divRef = useRef(null);

  useEffect(()=>{ (async()=>{
    try{
      const r = await rutaPorId(id);
      setForm({
        IdRuta: r.id,
        PuntoOrigen: r.origen,
        PuntoDestino: r.destino,
        CuposIda: r.cupos,
        CuposVuelta: null,
        HoraSalida: new Date(r.horaSalida).toISOString().slice(0,16),
        HoraRegreso: r.horaRegreso ? new Date(r.horaRegreso).toISOString().slice(0,16) : "",
        IdUsuario: 2, IdVehiculo: 1
      });
    } finally { setLoading(false); }
  })(); },[id]);

  useEffect(()=>{ if(!mapRef.current){
    mapRef.current = L.map(divRef.current).setView([4.65,-74.1], 12);
    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {maxZoom:19}).addTo(mapRef.current);
  } },[]);

  const onSubmit = async (e)=>{
    e.preventDefault(); setMsg("");
    try{
      const payload = {
        IdRuta: form.IdRuta,
        PuntoOrigen: form.PuntoOrigen,
        PuntoDestino: form.PuntoDestino,
        HoraSalida: new Date(form.HoraSalida).toISOString(),
        HoraRegreso: form.HoraRegreso ? new Date(form.HoraRegreso).toISOString() : null,
        CuposIda: Number(form.CuposIda), CuposVuelta: form.CuposVuelta,
        IdUsuario: form.IdUsuario, IdVehiculo: form.IdVehiculo
      };
      await editarRuta(payload);
      setMsg('Ruta actualizada');
      setTimeout(()=>navigate('/mis-rutas'), 700);
    }catch(e){ setMsg('No se pudo actualizar'); }
  };

  if(loading) return (<><Navbar persona="Conductor"/><div className="container"><div className="card">Cargando…</div></div></>);
  return (
    <>
      <Navbar persona="Conductor"/>
      <div className="container">
        <div className="header-slab"><div className="title">Editar Ruta</div></div>
        <div className="grid">
          <div className="card">
            <form className="form" onSubmit={onSubmit}>
              <div className="kv"><b>Ciudad de origen</b><input className="input" value={form.PuntoOrigen} onChange={e=>setForm({...form,PuntoOrigen:e.target.value})}/></div>
              <div className="kv"><b>Ciudad de destino</b><input className="input" value={form.PuntoDestino} onChange={e=>setForm({...form,PuntoDestino:e.target.value})}/></div>
              <div className="kv"><b>Número de cupos</b><input className="input" type="number" min={1} value={form.CuposIda} onChange={e=>setForm({...form,CuposIda:e.target.value})}/></div>
              <div className="kv"><b>Hora salida</b><input className="input" type="datetime-local" value={form.HoraSalida} onChange={e=>setForm({...form,HoraSalida:e.target.value})}/></div>
              <div className="kv"><b>Hora regreso</b><input className="input" type="datetime-local" value={form.HoraRegreso} onChange={e=>setForm({...form,HoraRegreso:e.target.value})}/></div>
              <div className="footer-actions">
                <button className="btn primary" type="submit">Guardar</button>
                {msg && <div className="helper" style={{marginLeft:8}}>{msg}</div>}
              </div>
            </form>
          </div>
          <div className="mapWrap"><h3 style={{margin:"6px 0 12px"}}>Mapa de la ruta</h3><div ref={divRef} style={{height:420, width:"100%", borderRadius:16}}/></div>
        </div>
      </div>
    </>
  );
}
