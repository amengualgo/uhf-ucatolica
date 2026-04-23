# Protocolos de Integración de Koha

## Resumen de protocolos disponibles

| Protocolo | Propósito | Dirección | Estado |
|-----------|-----------|-----------|--------|
| **REST API** | Integración general moderna | Bidireccional | Activo, recomendado |
| **Z39.50** | Búsqueda y recuperación de catálogo | Cliente/Servidor | Estable, legacy |
| **SRU/SRW** | Búsqueda basada en web (sucesor Z39.50) | Servidor | Estable |
| **OAI-PMH** | Cosecha de metadatos | Servidor (Koha expone) | Estable |
| **SIP2** | Integración con equipos físicos | Bidireccional | Estable |
| **ILS-DI** | Interfaz servicios para portales | Servidor | Disponible |
| **LDAP** | Autenticación de usuarios | Cliente | Estable |
| **EDI** | Intercambio con proveedores | Bidireccional | Disponible |
| **NCIP** | Préstamo interbibliotecario | Bidireccional | Parcial |

---

## 1. Z39.50 (Servidor y Cliente)

### Koha como SERVIDOR Z39.50
Permite que otras aplicaciones busquen en el catálogo de Koha.

- Puerto por defecto: `9999`
- Requiere habilitar el daemon `zebraqueue`
- Configuración en `koha-conf.xml`

```xml
<listen id="biblioserver">tcp:@:9999</listen>
```

Bases de datos disponibles:
- `biblios` — registros bibliográficos
- `authorities` — registros de autoridades

### Koha como CLIENTE Z39.50
Permite importar registros desde catálogos externos al catalogar.

Servidores preconfigurados: Library of Congress, BNE, OCLC WorldCat, y más.

Configuración: `Administración > Servidores Z39.50/SRU`

**Caso de uso**: Al catalogar un libro, buscar el registro en la Library of Congress y copiarlo a Koha.

---

## 2. SRU / SRW (Search/Retrieve via URL)

Versión web del protocolo Z39.50, basado en HTTP y XML.

### URL de búsqueda SRU:
```
https://TU_KOHA/cgi-bin/koha/z3950/search.pl?
  database=biblios
  &operation=searchRetrieve
  &query=dc.title=sistemas
  &maximumRecords=10
  &recordSchema=marcxml
```

**Caso de uso**: Portales web que necesitan buscar en el catálogo Koha sin usar la API REST.

---

## 3. OAI-PMH (Open Archives Initiative - Protocol for Metadata Harvesting)

Koha actúa como **proveedor de datos (data provider)**. Otros sistemas pueden "cosechar" (harvest) los metadatos del catálogo.

### URL base OAI-PMH:
```
https://TU_KOHA/cgi-bin/koha/oai.pl
```

### Verbos disponibles:

| Verbo | Descripción |
|-------|-------------|
| `Identify` | Información del repositorio |
| `ListMetadataFormats` | Formatos disponibles |
| `ListSets` | Conjuntos (sets) disponibles |
| `ListIdentifiers` | Lista de identificadores |
| `ListRecords` | Lista de registros completos |
| `GetRecord` | Obtener un registro específico |

### Ejemplo de request:
```
GET /cgi-bin/koha/oai.pl?verb=ListRecords&metadataPrefix=oai_dc
GET /cgi-bin/koha/oai.pl?verb=GetRecord&identifier=oai:koha:123&metadataPrefix=marcxml
```

### Formatos de metadatos soportados:
- `oai_dc` — Dublin Core
- `marcxml` — MARCXML
- `marc21` — MARC21

**Habilitación**: `Administración > Preferencias del sistema > OAI-PMH = Habilitar`

**Caso de uso**: Google Scholar, repositorios institucionales, agregadores de metadatos que necesitan indexar el catálogo.

---

## 4. SIP2 (Standard Interchange Protocol v2)

Protocolo para comunicación con **equipos físicos** de la biblioteca.

### Equipos que usan SIP2:
- Terminales de autopréstamo (self-checkout)
- Portales de renovación
- Sistemas de control de acceso
- Sorters automáticos
- AMH (Automated Materials Handling)
- Sistemas de pago de multas
- Máquinas RFID

### Configuración:
El servidor SIP2 de Koha usa el puerto `6001` por defecto.

```bash
# Archivo de configuración
/etc/koha/sites/NOMBRE/SIPconfig.xml
```

```xml
<login id="sip_user" password="sip_pass" 
       institution="BIBLIOTECA" 
       implemented="1"/>
```

**Caso de uso**: Una máquina de autopréstamo en la biblioteca se conecta a Koha vía SIP2 para verificar usuarios, registrar préstamos y devoluciones.

---

## 5. ILS-DI (ILS Discovery Interface)

Interfaz pensada para que **portales de descubrimiento** (discovery layers) consulten Koha.

### URL base:
```
https://TU_KOHA/cgi-bin/koha/ilsdi.pl
```

### Servicios disponibles:

| Servicio | Descripción |
|----------|-------------|
| `GetAvailability` | Disponibilidad de ítems |
| `GetRecords` | Obtener registros |
| `GetAuthorityRecords` | Registros de autoridad |
| `LookupPatron` | Buscar usuario |
| `AuthenticatePatron` | Autenticar usuario |
| `GetPatronInfo` | Información del usuario |
| `GetPatronStatus` | Estado del usuario |
| `GetServices` | Servicios disponibles |
| `RenewLoan` | Renovar préstamo |
| `HoldTitle` | Reservar un título |
| `HoldItem` | Reservar un ítem |
| `CancelHold` | Cancelar reserva |

### Ejemplo:
```
GET /cgi-bin/koha/ilsdi.pl?service=GetAvailability&id=00023&id_type=biblio
```

**Habilitación**: `Administración > Preferencias del sistema > ILS-DI = Habilitar`

---

## 6. LDAP / Active Directory

Koha puede autenticar usuarios contra un servidor LDAP externo.

### Configuración en `koha-conf.xml`:
```xml
<useldapserver>1</useldapserver>
<ldapserver id="ldap">
  <hostname>ldap.universidad.edu</hostname>
  <port>389</port>
  <version>3</version>
  <base>dc=universidad,dc=edu</base>
  <filterstring>(&amp;(objectClass=person)(uid=%s))</filterstring>
  <auth_by_bind>1</auth_by_bind>
  <replicate>1</replicate>  <!-- sincronizar datos del usuario -->
  <update>1</update>
</ldapserver>
```

**Caso de uso**: Los usuarios de la universidad inician sesión en el OPAC con su correo/contraseña institucional.

---

## 7. EDI (Electronic Data Interchange)

Para automatizar el intercambio de órdenes de compra con proveedores de libros.

- Estándar: **EDIFACT (ISO 9735)**
- Mensajes: ORDERS, ORDRSP, DESADV, INVOIC
- Conexión vía FTP/SFTP con el proveedor

**Caso de uso**: Enviar órdenes de compra directamente desde el módulo de adquisiciones de Koha al proveedor (ej: Carvajal, Océano).

---

## 8. Webhooks (Koha 22.11+)

Desde Koha 22.11, el módulo **ERM** y áreas específicas soportan webhooks salientes.

En versiones más recientes, se está expandiendo mediante el sistema de **Koha::Plugins** para crear webhooks personalizados.

---

## 9. Autenticación OAuth / SSO (CAS, Shibboleth)

| Método | Soporte |
|--------|---------|
| **CAS** (Central Authentication Service) | Sí, nativo |
| **Shibboleth** (SAML) | Sí, nativo |
| **OAuth2** (para la API) | Sí |
| **Google OAuth** | Vía plugin |
| **SAML genérico** | Vía plugin |

**Caso de uso para universidades**: SSO institucional para que los estudiantes entren al OPAC con sus credenciales universitarias.

---

## Diagrama de integraciones

```
                    ┌─────────────────────────────────┐
                    │           KOHA ILS               │
                    │                                  │
   Aplicación web ──┤◄── REST API (JSON)               │
   Móvil/App     ──┤                                  │
                    │                                  │
   Máquinas RFID ──┤◄── SIP2 (TCP port 6001)          │
   Autopréstamo  ──┤                                  │
                    │                                  │
   Google Scholar──┤◄── OAI-PMH (HTTP)                │
   Repositorios  ──┤                                  │
                    │                                  │
   Discovery     ──┤◄── ILS-DI (HTTP)                 │
   Layers        ──┤◄── SRU/Z39.50                    │
                    │                                  │
   LDAP/AD       ──┤──► Autenticación LDAP            │
   SSO/CAS       ──┤──► CAS / Shibboleth              │
                    │                                  │
   Proveedores   ──┤◄── EDI/EDIFACT                   │
   libros        ──┤                                  │
                    └─────────────────────────────────┘
```
