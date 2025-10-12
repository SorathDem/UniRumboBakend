
import api from "./client";
export const crearRuta = (payload)=>api.post(`/rutas`, payload).then(r=>r.data);
export const editarRuta = (payload)=>api.put(`/rutas`, payload).then(r=>r.data);
export const misRutas = (usuarioId)=>api.get(`/rutas/mias/${usuarioId}`).then(r=>r.data);
export const rutaPorId = (id)=>api.get(`/rutas/${id}`).then(r=>r.data);
