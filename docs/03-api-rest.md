# API REST de Koha

## Descripción general

Koha ofrece una **API REST moderna** basada en OpenAPI (Swagger) disponible desde la versión 17.05. Es la vía principal para integrar Koha con aplicaciones externas.

- URL base: `https://TU_KOHA/api/v1/`
- Documentación interactiva (Swagger UI): `https://TU_KOHA/api/v1/.html`
- Especificación OpenAPI: `https://TU_KOHA/api/v1/`

> **Nota importante**: La URL exacta de la API depende de tu instalación. En muchos casos es `/api/v1/` relativo al dominio raíz de Koha.

## Autenticación

### Método 1: HTTP Basic Auth
```http
GET /api/v1/patrons
Authorization: Basic base64(usuario:contraseña)
```

Debe habilitarse en: `Administración > Preferencias del sistema > RESTBasicAuth = Permitir`

### Método 2: OAuth2 (Client Credentials)
```http
POST /api/v1/oauth/token
Content-Type: application/x-www-form-urlencoded

grant_type=client_credentials&client_id=TU_CLIENT_ID&client_secret=TU_SECRET
```

Respuesta:
```json
{
  "access_token": "eyJ...",
  "token_type": "Bearer",
  "expires_in": 3600
}
```

Uso posterior:
```http
GET /api/v1/patrons
Authorization: Bearer eyJ...
```

### Método 3: Cookie de sesión
```http
POST /api/v1/auth/session
Content-Type: application/json

{ "userid": "usuario", "password": "contraseña" }
```

## Endpoints principales

### Usuarios (Patrons)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/v1/patrons` | Listar usuarios |
| `POST` | `/api/v1/patrons` | Crear usuario |
| `GET` | `/api/v1/patrons/{patron_id}` | Obtener usuario por ID |
| `PUT` | `/api/v1/patrons/{patron_id}` | Actualizar usuario |
| `DELETE` | `/api/v1/patrons/{patron_id}` | Eliminar usuario |
| `GET` | `/api/v1/patrons/{patron_id}/checkouts` | Préstamos activos del usuario |
| `GET` | `/api/v1/patrons/{patron_id}/holds` | Reservas del usuario |

### Registros bibliográficos (Biblios)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/v1/biblios` | Listar registros |
| `POST` | `/api/v1/biblios` | Crear registro MARC |
| `GET` | `/api/v1/biblios/{biblio_id}` | Obtener registro |
| `PUT` | `/api/v1/biblios/{biblio_id}` | Actualizar registro |
| `DELETE` | `/api/v1/biblios/{biblio_id}` | Eliminar registro |
| `GET` | `/api/v1/biblios/{biblio_id}/items` | Ítems de un registro |

### Ítems (Items)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/v1/items` | Listar ítems |
| `GET` | `/api/v1/items/{item_id}` | Obtener ítem |
| `PATCH` | `/api/v1/items/{item_id}` | Actualizar ítem |
| `DELETE` | `/api/v1/items/{item_id}` | Eliminar ítem |

### Circulación (Checkouts)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/v1/checkouts` | Listar préstamos |
| `POST` | `/api/v1/checkouts` | Crear préstamo |
| `GET` | `/api/v1/checkouts/{checkout_id}` | Obtener préstamo |
| `PUT` | `/api/v1/checkouts/{checkout_id}/renewal` | Renovar préstamo |
| `DELETE` | `/api/v1/checkouts/{checkout_id}` | Devolver ítem |

### Reservas (Holds)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/v1/holds` | Listar reservas |
| `POST` | `/api/v1/holds` | Crear reserva |
| `DELETE` | `/api/v1/holds/{hold_id}` | Cancelar reserva |
| `PUT` | `/api/v1/holds/{hold_id}/priority` | Cambiar prioridad |

### Endpoints públicos (sin autenticación o autenticación de usuario)

Bajo el prefijo `/api/v1/public/`:

| Endpoint | Descripción |
|----------|-------------|
| `GET /api/v1/public/biblios/{id}` | Ver registro bibliográfico |
| `GET /api/v1/public/biblios/{id}/items` | Ver ítems disponibles |
| `GET /api/v1/public/patrons/{id}/checkouts` | Ver préstamos del usuario autenticado |

## Filtrado y búsqueda

La API usa **sintaxis de consulta JSON**:

```http
GET /api/v1/patrons?q={"surname":"García"}
GET /api/v1/patrons?q={"cardnumber":{"like":"%2024%"}}
GET /api/v1/biblios?q={"title":{"like":"%sistemas%"}}
```

Operadores disponibles: `=`, `!=`, `<`, `>`, `<=`, `>=`, `-like`, `-in`, `-regexp`

Lógica AND/OR:
```json
{"and": [{"surname": "García"}, {"firstname": "Juan"}]}
{"or":  [{"surname": "García"}, {"surname": "López"}]}
```

## Formatos de registros MARC

Para endpoints de biblios, se puede pedir el registro en diferentes formatos:

```http
Accept: application/json           # JSON nativo de Koha
Accept: application/marcxml+xml    # MARCXML
Accept: application/marc-in-json   # MARC-in-JSON
Accept: application/marc           # MARC binario
```

## Paginación

```http
GET /api/v1/patrons?_per_page=20&_page=2
```

Headers de respuesta:
```
X-Total-Count: 1500
Link: </api/v1/patrons?_page=3>; rel="next"
```

## Ejemplo completo: Crear un usuario

```bash
curl -X POST https://koha.ejemplo.com/api/v1/patrons \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "firstname": "Juan",
    "surname": "García",
    "cardnumber": "USR001",
    "categorycode": "STUDENT",
    "branchcode": "CENTRAL",
    "email": "jgarcia@ejemplo.com",
    "dateexpiry": "2025-12-31"
  }'
```

## Manejo de errores

```json
{
  "error": "Object not found",
  "error_code": "not_found"
}
```

Códigos HTTP usados: `200`, `201`, `204`, `400`, `401`, `403`, `404`, `409`, `500`

## Permisos necesarios

La API respeta el sistema de permisos de Koha. El usuario de API debe tener los permisos correspondientes:

| Permiso Koha | Acceso API |
|-------------|------------|
| `circulate` | Préstamos y devoluciones |
| `catalogue` | Lectura del catálogo |
| `editcatalogue` | Modificar registros bibliográficos |
| `borrowers` | Gestión de usuarios |
| `reserveforothers` | Gestión de reservas |
| `superlibrarian` | Acceso total |
