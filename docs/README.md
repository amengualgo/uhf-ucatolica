# Documentación de Integración Koha

Documentación técnica para entender y trabajar con **Koha ILS** (Integrated Library System).

## Índice

| # | Documento | Descripción |
|---|-----------|-------------|
| 1 | [¿Qué es Koha?](01-que-es-koha.md) | Visión general, arquitectura, módulos y versiones |
| 2 | [Módulos y Funcionalidades](02-modulos-funcionalidades.md) | Detalle de cada módulo de Koha |
| 3 | [API REST](03-api-rest.md) | Endpoints, autenticación OAuth2, ejemplos de requests |
| 4 | [Protocolos de Integración](04-protocolos-integracion.md) | Z39.50, SIP2, OAI-PMH, ILS-DI, LDAP, EDI |
| 5 | [Qué puedes integrar](05-que-puedes-integrar.md) | Lo que SÍ y NO es posible integrar con Koha |
| 6 | [Ejemplos Prácticos](06-ejemplos-practicos.md) | Código de integración real (Python, JS, curl) |
| 7 | [Plugins](07-plugins.md) | Sistema de plugins y cómo desarrollar el tuyo |

## Rutas rápidas

### Quiero conectar mi app web al catálogo
→ Lee [API REST](03-api-rest.md) y [Ejemplos Prácticos](06-ejemplos-practicos.md)

### Quiero autenticar usuarios con LDAP/SSO universitario
→ Lee [Protocolos de Integración](04-protocolos-integracion.md) sección LDAP/CAS/Shibboleth

### Quiero indexar el catálogo en Google Scholar o repositorio
→ Lee [Protocolos de Integración](04-protocolos-integracion.md) sección OAI-PMH

### Quiero conectar equipos físicos (autopréstamo, RFID)
→ Lee [Protocolos de Integración](04-protocolos-integracion.md) sección SIP2

### Quiero saber qué NO es posible hacer
→ Lee [Qué puedes integrar](05-que-puedes-integrar.md) sección "Lo que NO puedes hacer"

### Quiero extender Koha con funcionalidad nueva
→ Lee [Plugins](07-plugins.md)

## Recursos oficiales

- **Sitio oficial**: https://koha-community.org
- **Documentación**: https://koha-community.org/documentation/
- **Wiki**: https://wiki.koha-community.org
- **API interactiva (Swagger)**: `https://TU_KOHA/api/v1/.html`
- **GitLab**: https://gitlab.com/koha-community/Koha
- **Plugins**: https://koha-community.org/koha-plugins/
- **Bugzilla**: https://bugs.koha-community.org
