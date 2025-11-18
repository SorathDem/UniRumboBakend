
import React from "react";
import ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import RutaCrear from "./pages/RutaCrear.jsx";
import MisRutas from "./pages/MisRutas.jsx";
import RutaEditar from "./pages/RutaEditar.jsx";
import Reportes from "./pages/Reportes.jsx";
import "./styles.css";

const router = createBrowserRouter([
  { path: "/", element: <MisRutas/> },
  { path: "/rutas/editar/:id", element: <RutaEditar/> },
  { path: "/rutas/crear", element: <RutaCrear/> },
  { path: "/mis-rutas", element: <MisRutas/> },
  { path: "/reportes", element: <Reportes/> }
]);

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode><RouterProvider router={router}/></React.StrictMode>
);
