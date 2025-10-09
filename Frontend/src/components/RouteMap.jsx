
import { useEffect, useRef } from "react";
import L from "leaflet";

export default function RouteMap({ featureCollection }){
  const mapRef = useRef(null); const divRef = useRef(null);
  useEffect(()=>{
    if(!mapRef.current){
      mapRef.current = L.map(divRef.current).setView([4.65,-74.1], 12);
      L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {maxZoom:19}).addTo(mapRef.current);
    }
    mapRef.current.eachLayer(l=>{ if(!(l instanceof L.TileLayer)) mapRef.current.removeLayer(l); });
    if(featureCollection?.features?.length){
      const layer = L.geoJSON(featureCollection).addTo(mapRef.current);
      try{ mapRef.current.fitBounds(layer.getBounds(), {padding:[20,20]}); }catch{}
    }
  },[featureCollection]);
  return <div ref={divRef} style={{height:400, width:"100%", borderRadius:16}}/>;
}
