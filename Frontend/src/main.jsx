
import React from "react";
import ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import RutaIndex from "./pages/RutaIndex.jsx";
import RutaDetalle from "./pages/RutaDetalle.jsx";
import RutaCrear from "./pages/RutaCrear.jsx";
import MisRutas from "./pages/_Placeholder.jsx";
import Solicitudes from "./pages/_Placeholder.jsx";
import "./styles.css";

const router = createBrowserRouter([
  { path: "/", element: <RutaIndex/> },
  { path: "/rutas/:id", element: <RutaDetalle/> },
  { path: "/rutas/crear", element: <RutaCrear/> },
  { path: "/mis-rutas", element: <MisRutas title="Mis Rutas (demo HU3)"/> },
  { path: "/solicitudes", element: <Solicitudes title="Solicitudes (demo HU3)"/> }
]);

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode><RouterProvider router={router}/></React.StrictMode>
);
