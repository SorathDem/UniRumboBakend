    
import { useEffect, useState } from "react";
import Navbar from "../components/Navbar";
import RouteCard from "../components/RouteCard";
import { buscarRutas } from "../api/rutas";

export default function RutaIndex(){
  const [rutas,setRutas]=useState([]);
  const [q,setQ]=useState({origen:"",destino:"",fecha:""});
  const [loading,setLoading]=useState(false);
  const [error,setError]=useState("");

  const cargar = async()=>{
    setLoading(true); setError("");
    try{
      const data = await buscarRutas({
        origen: q.origen || undefined,
        destino: q.destino || undefined,
        fecha: q.fecha || undefined
      });
      setRutas(data);
    }catch(e){
      setError("No se pudieron cargar las rutas");
    }finally{ setLoading(false); }
  };

  useEffect(()=>{ cargar(); },[]);
  const onSubmit=(e)=>{ e.preventDefault(); cargar(); };

  return (
    <>
      <Navbar persona="Cliente" />
      <div className="container">
        <div className="header-slab">
          <div className="title">Rutas</div>
        </div>

        <div className="card" style={{marginBottom:16}}>
          <div className="filters">
            <input className="input" placeholder="Ciudad / Origen" value={q.origen} onChange={e=>setQ({...q,origen:e.target.value})}/>
            <input className="input" placeholder="Destino" value={q.destino} onChange={e=>setQ({...q,destino:e.target.value})}/>
            <input className="input" type="date" value={q.fecha} onChange={e=>setQ({...q,fecha:e.target.value})}/>
            <button className="btn primary" onClick={onSubmit}>Buscar</button>
          </div>
          <div className="helper">Listado de rutas disponibles</div>
        </div>

        {loading && <div className="card">Cargando rutas…</div>}
        {error && <div className="card">{error}</div>}

        <div className="grid">
          {rutas.map(r => <RouteCard key={r.id} r={r}/>)}
        </div>

        {!loading && !error && rutas.length===0 && (
          <div className="empty">No hay rutas que coincidan con tu búsqueda.</div>
        )}
      </div>
    </>
  );
}
