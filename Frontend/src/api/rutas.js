
import api from "./client";
export const buscarRutas = (params)=>api.get(`/rutas`,{params}).then(r=>r.data);
export const rutaPorId = (id)=>api.get(`/rutas/${id}`).then(r=>r.data);
export const geoRuta = (id)=>api.get(`/rutas/${id}/geo`).then(r=>r.data);
export const crearSolicitud = (payload)=>api.post(`/solicitudesruta`, payload).then(r=>r.data);
export const crearRuta = (payload)=>api.post(`/rutas`, payload).then(r=>r.data);
