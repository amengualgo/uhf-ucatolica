# Qué PUEDES y NO PUEDES integrar con Koha

## ✅ LO QUE SÍ PUEDES HACER

### Integración con aplicaciones web / móvil

| Caso de uso | Cómo | Protocolo |
|-------------|------|-----------|
| Buscar libros en el catálogo | Consultar biblios | REST API / SRU |
| Verificar disponibilidad de un ítem | Query a items con `status` | REST API / ILS-DI |
| Registrar/autenticar un usuario | CRUD de patrons | REST API / ILS-DI |
| Hacer una reserva de un libro | POST a holds | REST API / ILS-DI |
| Renovar un préstamo | PUT checkout renewal | REST API / ILS-DI |
| Ver historial de préstamos | GET checkouts por patron | REST API |
| Crear usuarios desde sistema externo | POST patrons | REST API |
| Sincronizar usuarios desde ERP/SIS | POST/PUT patrons en lote | REST API |
| Mostrar catálogo en portal institucional | Consumir biblios | REST API / OAI-PMH |

### Integración con sistemas institucionales

| Sistema | Tipo de integración | Mecanismo |
|---------|---------------------|-----------|
| Sistema de información estudiantil (SIS) | Sincronizar usuarios | REST API (batch patrons) |
| Active Directory / LDAP universitario | Autenticación SSO | LDAP nativo |
| Portal universitario / Intranet | Catálogo embebido | REST API / ILS-DI |
| Sistema de pagos | Cobro de multas | REST API (fines) |
| ERP institucional | Adquisiciones | REST API / EDI |
| Sistemas de gestión documental | Metadatos | OAI-PMH / REST API |

### Integración con hardware

| Hardware | Protocolo | Funcionalidad |
|----------|-----------|---------------|
| Máquinas de autopréstamo | SIP2 | Préstamo y devolución automática |
| Portales RFID de seguridad | SIP2 | Verificación de salida |
| Sorters / clasificadoras | SIP2 | Clasificación automática en devolución |
| Impresoras de etiquetas | Directo via herramientas Koha | Generación de etiquetas |

### Indexación y descubrimiento

| Sistema | Cómo | Protocolo |
|---------|------|-----------|
| Google Scholar | Koha expone metadatos | OAI-PMH |
| Repositorios OAI | Cosecha de registros | OAI-PMH |
| Discovery layers (VuFind, Primo) | Búsqueda integrada | Z39.50 / SRU / REST API |
| VuFind | Integración profunda documentada | Z39.50 + ILS-DI |

### Autenticación unificada

| Sistema | Soporte |
|---------|---------|
| CAS (Jasig CAS, Apereo) | Nativo en Koha |
| Shibboleth (SAML) | Nativo en Koha |
| LDAP / Active Directory | Nativo en Koha |
| OAuth2 para APIs | Nativo |

### Plugins y extensibilidad

Koha tiene un **sistema de plugins** (desde v3.12) que permite:
- Agregar nuevos endpoints REST
- Agregar nuevas preferencias de sistema
- Modificar el comportamiento del OPAC
- Agregar hooks en eventos (alta de usuario, préstamo, etc.)
- Integrar servicios externos

Repositorio de plugins: https://koha-community.org/koha-plugins/

---

## ❌ LO QUE NO PUEDES HACER (o es muy limitado)

### Limitaciones de la REST API

| Limitación | Detalle |
|-----------|---------|
| No hay búsqueda full-text via REST | La búsqueda avanzada del catálogo (Zebra/ES) no está expuesta directamente por REST; hay endpoints de biblios pero no el motor de búsqueda completo |
| Sin webhooks nativos masivos | No hay sistema nativo de webhooks para todos los eventos (préstamos, altas, etc.) — requiere plugin o polling |
| Sin streaming de eventos | No hay WebSockets ni eventos en tiempo real por defecto |
| Modificar configuración vía API | Las preferencias de sistema no son modificables vía REST API |
| Gestión de permisos vía API | No puedes asignar permisos de usuarios libremente via API sin superlibrarian |

### Limitaciones de integración

| Limitación | Detalle |
|-----------|---------|
| No es un repositorio institucional | Koha gestiona materiales físicos principalmente; para objetos digitales se necesita integración con DSpace, ePrints, etc. |
| Sin DRM nativo para ebooks | No gestiona préstamos de ebooks con DRM (Adobe, etc.) sin plugin externo |
| OverDrive / CloudLibrary | No hay integración nativa; requiere plugin externo |
| Sin gestión de salones/salas | No gestiona reserva de espacios físicos (salas de estudio) de forma nativa |
| Facturación compleja | El módulo de adquisiciones no reemplaza un ERP completo |
| Sin e-commerce nativo | No tiene pasarela de pago nativa para multas en línea (requiere plugin) |

### Limitaciones de protocolos

| Protocolo | Limitación |
|-----------|------------|
| NCIP | Soporte parcial; no todos los mensajes implementados |
| OpenURL | Soporte básico; no es un link resolver completo |
| BIBFRAME | No soporta BIBFRAME nativamente (solo MARC) |
| Linked Data / RDF | No expone datos como Linked Data por defecto |

### Limitaciones de arquitectura

| Área | Limitación |
|------|------------|
| Multitenancy | Koha no fue diseñado para SaaS multi-tenant nativo; cada instancia es independiente (aunque servicios como ByWater ofrecen hosting multibiblioteca) |
| Alta concurrencia | Perl/Apache no escala como Node.js o Go para miles de peticiones simultáneas |
| API GraphQL | No existe; solo REST |

---

## Tabla resumen: Posibilidades de integración

```
INTEGRACIÓN                    FACILIDAD    SOPORTE
─────────────────────────────────────────────────
Aplicación web (consultas)     ★★★★★      REST API
Autenticación LDAP/SSO         ★★★★★      Nativo
Sincronizar usuarios            ★★★★☆      REST API
Autopréstamo RFID               ★★★★☆      SIP2
Cosecha OAI-PMH                 ★★★★☆      Nativo
Discovery layer (VuFind)        ★★★★☆      Z39.50/SRU
Reservas desde portal web       ★★★★☆      REST API/ILS-DI
Importar catálogo Z39.50        ★★★★☆      Nativo
Webhooks en tiempo real         ★★☆☆☆      Plugin necesario
Gestión ebooks/DRM              ★★☆☆☆      Plugin externo
Repositorio digital             ★★☆☆☆      Integración externa
Linked Data / BIBFRAME          ★☆☆☆☆      No disponible
GraphQL API                     ☆☆☆☆☆      No existe
```
