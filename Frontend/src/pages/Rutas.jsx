import { useEffect, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { buscarRutas } from "../api/rutas";

export default function Rutas(){
  const [rutas,setRutas]=useState([]); 
  const [params,setParams]=useSearchParams();
  const [q,setQ]=useState({ origen: params.get("origen")||"", destino: params.get("destino")||"", fecha: params.get("fecha")||"" });

  const cargar = async()=> setRutas(await buscarRutas({
    origen:q.origen||undefined, destino:q.destino||undefined, fecha:q.fecha||undefined
  }));
  useEffect(()=>{ cargar(); },[]);

  const submit=(e)=>{ e.preventDefault(); setParams(q); cargar(); };

  return (
    <div className="container">
      <h2>Rutas disponibles</h2>

      <form onSubmit={submit} style={{display:"flex", gap:10, margin:"12px 0"}}>
        <input placeholder="Origen" value={q.origen} onChange={e=>setQ({...q,origen:e.target.value})}/>
        <input placeholder="Destino" value={q.destino} onChange={e=>setQ({...q,destino:e.target.value})}/>
        <input type="date" value={q.fecha} onChange={e=>setQ({...q,fecha:e.target.value})}/>
        <button type="submit">Buscar</button>
      </form>

      <div className="grid">
        {rutas.map(r=>(
          <div key={r.id} className="card">
            <h3>{r.titulo}</h3>
            <p><b>Estado:</b> {r.estado}</p>
            <p>🚩 <b>Origen:</b> {r.origen}<br/>🎯 <b>Destino:</b> {r.destino}</p>
            <p>🚗 {r.tipoVehiculo} &nbsp;&nbsp; 👥 {r.cupos} cupos</p>
            <p>🕒 <b>Salida:</b> {new Date(r.horaSalida).toLocaleString()}</p>
            {r.horaRegreso && <p>↩ <b>Regreso:</b> {new Date(r.horaRegreso).toLocaleString()}</p>}
            <div style={{marginTop:10}}>
              <Link to={`/rutas/${r.id}`}>Ver detalle</Link>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}