# ¿Qué es Koha?

## Definición

Koha es el **primer sistema de gestión de bibliotecas (ILS - Integrated Library System) libre y de código abierto del mundo**. Fue creado en 1999 en Nueva Zelanda por la empresa Katipo Communications para la Horowhenua Library Trust, y desde entonces ha crecido hasta convertirse en uno de los ILS más utilizados globalmente.

- Sitio oficial: https://koha-community.org
- Documentación: https://koha-community.org/documentation/
- Repositorio: https://git.koha-community.org/koha-community/koha
- Wiki: https://wiki.koha-community.org

## Para qué tipo de institución es

Koha está diseñado para **cualquier tipo de biblioteca**:

| Tipo | Descripción |
|------|-------------|
| Públicas | Bibliotecas municipales, departamentales |
| Académicas | Universidades, colegios, institutos |
| Escolares | Bibliotecas de colegios y primarias |
| Especializadas | Empresas, hospitales, organismos gubernamentales |
| Consorcios | Redes de múltiples bibliotecas |

Es escalable: desde una biblioteca pequeña con pocos miles de registros hasta consorcios con millones de ítems.

## Arquitectura general

```
┌─────────────────────────────────────────────────┐
│                  KOHA ILS                        │
│                                                  │
│  ┌──────────────┐    ┌──────────────────────┐   │
│  │  OPAC         │    │  Interfaz Staff       │   │
│  │ (Catálogo     │    │  (Administración y    │   │
│  │  público)     │    │   Circulación)        │   │
│  └──────┬───────┘    └──────────┬───────────┘   │
│         │                       │                │
│  ┌──────▼───────────────────────▼───────────┐   │
│  │            Núcleo Koha                    │   │
│  │  (Zebra/Elasticsearch + MySQL/MariaDB)    │   │
│  └──────────────────────────────────────────┘   │
└─────────────────────────────────────────────────┘
```

## Componentes tecnológicos principales

| Componente | Tecnología |
|-----------|------------|
| Lenguaje backend | Perl |
| Base de datos | MySQL / MariaDB |
| Motor de búsqueda | Zebra (MARC21/UNIMARC) o Elasticsearch |
| Frontend | HTML5, CSS3, Bootstrap, JavaScript |
| Servidor web | Apache |
| Protocolo de registros | MARC21 o UNIMARC |

## Versiones y ciclo de releases

- **Versión estable actual**: 25.11 (noviembre 2025)
- **LTS (Long Term Support)**: 22.11 "Rosalie"
- Releases cada 6 meses (mayo y noviembre)
- Numeración: `AÑO.MES` (ej: 24.11 = noviembre 2024)

## Licencia

Koha se distribuye bajo la licencia **GNU General Public License (GPL) v3**, lo que significa:
- Uso libre y gratuito
- Modificación permitida
- Redistribución permitida bajo la misma licencia
- Sin costos de licenciamiento

## Comunidad

- Más de **15.000 bibliotecas** activas en el mundo
- Presente en más de **100 países**
- Comunidad activa de desarrolladores, bibliotecarios y empresas de soporte
- Conferencia anual: **KohaCon** (27 ediciones)
