
import Navbar from "../components/Navbar";
export default function Placeholder({ title }){
  return (<>
    <Navbar/>
    <div className="container">
      <div className="card"><h3>{title}</h3><p className="helper">Esta pestaña es decorativa para el prototipo HU3.</p></div>
    </div>
  </>);
}
