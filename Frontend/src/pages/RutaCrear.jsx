
import { useEffect, useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import Navbar from "../components/Navbar";
import L from "leaflet";
import { crearRuta } from "../api/rutas";

const OSRM = "https://router.project-osrm.org/route/v1/driving";
const NOMINATIM = "https://nominatim.openstreetmap.org/search";

export default function RutaCrear(){
  const [form,setForm]=useState({
    nombre:"",
    tipoVehiculo:"Carro",
    origen:"",
    destino:"",
    cupos:1,
    precio:0,
    horaSalida: "",
    horaRegreso: "",
  });
  const [coords,setCoords]=useState({ o:null, d:null });
  const [loading,setLoading]=useState(false);
  const [msg,setMsg]=useState("");
  const [createdId,setCreatedId]=useState(null);
  const navigate = useNavigate();

  const mapRef = useRef(null); const divRef = useRef(null); const vectorRef = useRef(null);

  useEffect(()=>{
    if(!mapRef.current){
      mapRef.current = L.map(divRef.current).setView([4.65,-74.1], 12);
      L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {maxZoom:19}).addTo(mapRef.current);
    }
  },[]);

  const geocode = async (q)=>{
    const url = `${NOMINATIM}?format=json&q=${encodeURIComponent(q)}&limit=1`;
    const r = await fetch(url, { headers: { "Accept":"application/json" } });
    const j = await r.json();
    if(!j?.length) return null; const { lat, lon } = j[0];
    return [Number(lat), Number(lon)];
  };

  const traceRoute = async (o,d)=>{
    if(!o || !d) return;
    const url = `${OSRM}/${d[1]},${o[0]};${d[1]},${d[0]}?overview=full&geometries=geojson`;
    // Nota: OSRM espera lon,lat; corregimos orden
    const url2 = `${OSRM}/${o[1]},${o[0]};${d[1]},${d[0]}?overview=full&geometries=geojson`;
    const res = await fetch(url2); const j = await res.json();
    const geom = j?.routes?.[0]?.geometry; if(!geom) return;
    // limpiar capas
    mapRef.current.eachLayer(l=>{ if(!(l instanceof L.TileLayer)) mapRef.current.removeLayer(l); });
    const line = L.geoJSON(geom).addTo(mapRef.current);
    L.marker(o).addTo(mapRef.current);
    L.marker(d).addTo(mapRef.current);
    try{ mapRef.current.fitBounds(line.getBounds(), {padding:[20,20]}); }catch{}
  };

  const onLocate = async ()=>{
    setMsg("");
    const o = form.origen?.trim(); const d = form.destino?.trim();
    if(!o || !d){ setMsg("Completa origen y destino"); return; }
    setLoading(true);
    try{
      const co = await geocode(o); const cd = await geocode(d);
      setCoords({ o: co, d: cd });
      await traceRoute(co, cd);
    } finally { setLoading(false); }
  };

  const onSubmit = async (e)=>{
    e.preventDefault(); setMsg("");
    if(!coords.o || !coords.d){ setMsg("Primero localiza en el mapa"); return; }
    setLoading(true);
    try{
      const payload = {
        PuntoOrigen: form.origen,
        PuntoDestino: form.destino,
        HoraSalida: new Date(form.horaSalida).toISOString(),
        HoraRegreso: form.horaRegreso? new Date(form.horaRegreso).toISOString(): null,
        CuposIda: Number(form.cupos),
        CuposVuelta: null,
        IdUsuario: 2, // demo
        IdVehiculo: 1, // demo
        OrigenLat: coords.o[0], OrigenLon: coords.o[1],
        DestinoLat: coords.d[0], DestinoLon: coords.d[1],
      };
      const id = await crearRuta(payload);
      setCreatedId(id);
      setMsg(`Ruta creada (#${id}).`);
    } catch (e) {
      console.error('Error creando ruta', e);
      const apiMsg = e?.response?.data?.message || e?.response?.data || e?.message || 'Error desconocido';
      setMsg(`No se pudo crear la ruta: ${apiMsg}`);
    } finally { setLoading(false); }
  };

  return (
    <>
      <Navbar persona="Conductor"/>
      <div className="container">
        <div className="header-slab"><div className="title">Crear Ruta</div></div>
        <div className="grid">
          <div className="card">
            <form className="form" onSubmit={onSubmit}>
              <div className="kv"><b>Nombre de la ruta</b><input className="input" value={form.nombre} onChange={e=>setForm({...form,nombre:e.target.value})}/></div>
              <div className="kv"><b>Tipo de vehículo</b><input className="input" value={form.tipoVehiculo} onChange={e=>setForm({...form,tipoVehiculo:e.target.value})}/></div>
              <div className="kv"><b>Ciudad de origen</b><input className="input" value={form.origen} onChange={e=>setForm({...form,origen:e.target.value})}/></div>
              <div className="kv"><b>Ciudad de destino</b><input className="input" value={form.destino} onChange={e=>setForm({...form,destino:e.target.value})}/></div>
              <div className="kv"><b>Número de cupos</b><input className="input" type="number" min={1} value={form.cupos} onChange={e=>setForm({...form,cupos:e.target.value})}/></div>
              <div className="kv"><b>Precio por viaje</b><input className="input" type="number" min={0} value={form.precio} onChange={e=>setForm({...form,precio:e.target.value})}/></div>
              <div className="kv"><b>Hora salida</b><input className="input" type="datetime-local" value={form.horaSalida} onChange={e=>setForm({...form,horaSalida:e.target.value})}/></div>
              <div className="kv"><b>Hora regreso</b><input className="input" type="datetime-local" value={form.horaRegreso} onChange={e=>setForm({...form,horaRegreso:e.target.value})}/></div>
              <div className="footer-actions">
                <button className="btn outline" type="button" onClick={onLocate} disabled={loading}>Ubicar en mapa</button>
                <button className="btn primary" type="submit" disabled={loading}>Publicar Ruta</button>
                {msg && <div className="helper" style={{marginLeft:8}}>{msg} {createdId && (<>
                  · <Link className="btn link" to={`/rutas/${createdId}`}>Ver detalle</Link>
                  <button className="btn outline" type="button" onClick={()=>navigate(`/rutas/${createdId}`)} style={{marginLeft:6}}>Ir ahora</button>
                </>)}
                </div>}
              </div>
            </form>
          </div>
          <div className="mapWrap"><h3 style={{margin:"6px 0 12px"}}>Mapa de la ruta</h3><div ref={divRef} style={{height:420, width:"100%", borderRadius:16}}/></div>
        </div>
      </div>
    </>
  );
}
