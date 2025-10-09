
import { Link, useLocation } from "react-router-dom";

export default function Navbar({ persona = "Cliente", nombre = "Nombre" }){
  const { pathname } = useLocation();
  const is = (p) => pathname === p;
  return (
    <div className="nav">
      <div className="nav-inner">
        <div className="brand">UniRumbo</div>
        <div className="tabs">
          <Link className={`tab ${is('/')?'active':''}`} to="/">Rutas</Link>
          <Link className={`tab ${is('/mis-rutas')?'active':''}`} to="/mis-rutas">Mis Rutas</Link>
          <Link className={`tab ${is('/rutas/crear')?'active':''}`} to="/rutas/crear">Crear Ruta</Link>
          <Link className={`tab ${is('/solicitudes')?'active':''}`} to="/solicitudes">Solicitudes</Link>
        </div>
        <div style={{marginLeft:"auto", color:"#475569"}}>Nombre {persona}</div>
      </div>
    </div>
  );
}
