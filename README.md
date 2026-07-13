# Desarrollo de un Mapa Interactivo para Presentación de Información Geológica
# Backend
> Sistema de información geológica para el museo petrográfico de la EPN: expone el inventario de muestras y la cartografía del Ecuador mediante una API REST de solo lectura, respaldada por PostgreSQL/PostGIS y GeoServer.

---

## Información General

| Campo | Información |
|--------|-------------|
| Institución | Escuela Politécnica Nacional |
| Facultad | Facultad de Ingeniería Eléctrica y Electrónica |
| Carrera | Ingeniería en Tecnologías de la Información |
| Asignatura | Trabajo de Integración Curricular |
| Autor | Alex Fernando Calderón Santín |
| Director | Raúl David Mejía Navarrete |
| Año | 2026 |
| Versión | 1.0 |

---

# Tabla de Contenidos

- [Descripción del Proyecto](#descripción-del-proyecto)
- [Objetivos](#objetivos)
- [Alcance](#alcance)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Arquitectura](#arquitectura)
- [Licencias](#licencias)

---

# Descripción del Proyecto

El museo petrográfico de la EPN conserva un inventario de muestras de rocas y formaciones geológicas del Ecuador que, hasta este proyecto, no contaba con un medio digital para consultarlo de forma georreferenciada. Este componente desarrolla el backend de un sistema web de información geológica: una base de datos espacial en PostgreSQL con PostGIS almacena cuatro tablas (ecuador, provincias, geologia, muestras_rocas, con 2.924 registros en total), GeoServer las publica como capas espaciales bajo los estándares WMS y WFS del OGC, y una API REST en ASP.NET Core actúa como único punto de acceso público, trasladando cada petición hacia GeoServer sin que el cliente llegue nunca directamente a GeoServer ni a PostGIS. Un cliente de validación construido con Leaflet.js consume esa API y permite comprobar visualmente que el backend funciona como se espera.

---

# Objetivos

## Objetivo General

Hay dos formas de plantearlo. Copio primero la que tienes en el anteproyecto y al lado la que yo usaría, porque la palabra "gestionar" sugiere capacidades de escritura que la API no tiene (son 13 endpoints GET, ninguno modifica datos):

- **Versión actual del anteproyecto:** "Desarrollar el backend de un sistema web de información geológica que integre una base de datos espacial PostgreSQL con PostGIS, un servidor de mapas GeoServer y una API REST en ASP.NET Core, organizados en una arquitectura multicapa, para gestionar y publicar el inventario de muestras del museo a través de un punto de acceso único, junto a un cliente de validación que permita demostrar la funcionalidad del backend."
- **Versión que recomiendo para el README:** Desarrollar el backend de un sistema web de información geológica que integre una base de datos espacial PostgreSQL con PostGIS, un servidor de mapas GeoServer y una API REST en ASP.NET Core, organizados en una arquitectura multicapa, para publicar y poner a disposición pública el inventario de muestras del museo a través de un único punto de acceso de solo lectura, junto a un cliente de validación que permita demostrar la funcionalidad del backend.

## Objetivos Específicos

- Estructurar el modelo de datos espacial en PostgreSQL con PostGIS para almacenar el inventario de muestras petrográficas y la cartografía geológica del Ecuador.
- Configurar en GeoServer los servicios de interoperabilidad geoespacial y publicar las capas espaciales mediante los estándares WMS y WFS del OGC.
- Construir una API REST en ASP.NET Core que actúe como punto de acceso público del sistema y traslade las peticiones del cliente hacia los servicios WMS y WFS de GeoServer.

---

# Alcance

El desarrollo se ejecutó bajo la metodología Scrum, en ocho sprints, cubriendo diseño, implementación y evaluación en un entorno de pruebas sobre Windows Server 2022. El backend integra PostgreSQL con PostGIS, GeoServer y una API REST en ASP.NET Core, con un cliente de validación construido en Leaflet.js para demostrar su funcionamiento.

Queda fuera del alcance:

- Cualquier operación de escritura o administración del inventario: los 13 endpoints expuestos son de solo lectura (GET); la gestión de las muestras ocurre directamente en la base de datos, fuera de la API.
- El despliegue en un servidor IIS de producción: la aplicación se ejecuta actualmente mediante `dotnet run`.
- La construcción de un frontend completo: el cliente de Leaflet es un stub que demuestra el consumo de la API, no una interfaz final para el museo.

---

# Tecnologías Utilizadas

| Tecnología | Versión |
|------------|---------|
| Windows Server | 2022 |
| PostgreSQL | 18 |
| PostGIS | 3.6 |
| GeoServer | 2.28.1 |
| .NET SDK / ASP.NET Core | 10 |
| Leaflet.js | 1.9.4 |
| Java (requerido por GeoServer) | 21 |

---

# Arquitectura

El sistema se organiza en cuatro capas de arquitectura, dispuestas en serie: la capa de presentación (el cliente de validación en Leaflet y las vistas de ASP.NET Core), la capa de servicio (la API REST, único punto de acceso público), la capa de lógica (GeoServer, expuesto solo internamente vía WMS/WFS) y la capa de acceso a datos (PostgreSQL con PostGIS). El cliente de validación nunca llega directamente a GeoServer ni a la base de datos: toda petición pasa por la API, que actúa como cliente HTTP interno hacia GeoServer.

<img width="631" height="639" alt="arquitecturafinal" src="https://github.com/user-attachments/assets/6aa5ca29-fc78-4ada-99c3-7c7b2d374400" />
---

# Licencia

Este proyecto fue desarrollado con fines académicos como parte del Trabajo de Integración Curricular de la carrera de Ingeniería en Tecnologías de la Información.

Su distribución y uso quedan sujetos a la normativa institucional correspondiente.



