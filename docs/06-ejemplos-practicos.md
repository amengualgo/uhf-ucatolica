# Ejemplos Prácticos de Integración con Koha

## Ejemplo 1: Autenticar un usuario desde tu app

### Usando ILS-DI (sin credenciales de staff)
```http
GET /cgi-bin/koha/ilsdi.pl?service=AuthenticatePatron&username=jgarcia&password=mi_clave
```

Respuesta exitosa:
```xml
<AuthenticatePatron>
  <id>1042</id>
  <code>Success</code>
</AuthenticatePatron>
```

### Usando REST API (con token OAuth2)
```bash
# 1. Obtener token
curl -X POST https://koha.ejemplo.com/api/v1/oauth/token \
  -d "grant_type=client_credentials&client_id=APP_ID&client_secret=APP_SECRET"

# 2. Buscar usuario por número de carnet
curl -H "Authorization: Bearer TOKEN" \
  "https://koha.ejemplo.com/api/v1/patrons?q={\"cardnumber\":\"USR001\"}"
```

---

## Ejemplo 2: Buscar disponibilidad de un libro

```bash
# Por ISBN
curl -H "Authorization: Bearer TOKEN" \
  "https://koha.ejemplo.com/api/v1/biblios?q={\"isbn\":\"9780134685991\"}"

# Obtener ítems del registro (disponibilidad)
curl -H "Authorization: Bearer TOKEN" \
  "https://koha.ejemplo.com/api/v1/biblios/123/items"
```

Respuesta de ítem:
```json
[
  {
    "item_id": 456,
    "biblio_id": 123,
    "barcode": "BC001234",
    "location": "SALA-A",
    "callnumber": "004.678 G123",
    "onloan": null,        // null = disponible, fecha = prestado
    "notforloan": 0,       // 0 = disponible para préstamo
    "withdrawn": 0,
    "damaged": 0,
    "itemlost": 0,
    "homebranch": "CENTRAL"
  }
]
```

---

## Ejemplo 3: Sincronizar usuarios desde un SIS universitario

```python
import requests

API_BASE = "https://koha.universidad.edu/api/v1"
TOKEN = "eyJ..."

headers = {
    "Authorization": f"Bearer {TOKEN}",
    "Content-Type": "application/json"
}

# Datos provenientes del SIS
estudiante = {
    "firstname": "María",
    "surname": "Rodríguez",
    "cardnumber": "20240001",
    "categorycode": "STUDENT",   # categoría configurada en Koha
    "branchcode": "PRINCIPAL",
    "email": "m.rodriguez@universidad.edu",
    "dateexpiry": "2025-12-31",
    "userid": "20240001",
    "password": "password_inicial"
}

# Verificar si ya existe
resp = requests.get(
    f"{API_BASE}/patrons",
    params={"q": f'{{"cardnumber":"{estudiante["cardnumber"]}"}}'},
    headers=headers
)

if resp.json():
    # Actualizar
    patron_id = resp.json()[0]["patron_id"]
    requests.put(f"{API_BASE}/patrons/{patron_id}", json=estudiante, headers=headers)
else:
    # Crear
    requests.post(f"{API_BASE}/patrons", json=estudiante, headers=headers)
```

---

## Ejemplo 4: Hacer una reserva desde un portal web

```javascript
// JavaScript / fetch API
async function reservarLibro(patronId, biblioId, branchCode, token) {
  const response = await fetch('https://koha.ejemplo.com/api/v1/holds', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      patron_id: patronId,
      biblio_id: biblioId,      // reserva a nivel de título
      pickup_library_id: branchCode,
      expiration_date: '2025-06-30'
    })
  });

  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error);
  }

  return await response.json();
}
```

---

## Ejemplo 5: Cosechar registros via OAI-PMH (Python)

```python
from sickle import Sickle  # pip install sickle

sickle = Sickle('https://koha.biblioteca.edu/cgi-bin/koha/oai.pl')

# Listar todos los registros Dublin Core
records = sickle.ListRecords(metadataPrefix='oai_dc')

for record in records:
    print(record.header.identifier)
    print(record.metadata)
    # {'title': ['Python para todos'], 'creator': ['Lutz, Mark'], ...}
```

---

## Ejemplo 6: Configurar OAI-PMH en Koha

1. Ir a `Administración > Preferencias del sistema`
2. Buscar `OAI-PMH`
3. Habilitar: `OAI-PMH = Habilitar`
4. Configurar `OAI-PMH:AutoUpdateSets` si se usan sets

Sets personalizados en `Administración > Servidor OAI-PMH`:
```
Set spec: music
Set name: Colección de Música
Descripción: Todos los registros de la colección de música
```

---

## Ejemplo 7: Integración con VuFind (Discovery Layer)

VuFind se conecta a Koha vía Z39.50 para búsquedas y via ILS-DI para transacciones.

Configuración en VuFind (`config/vufind/Koha.ini`):
```ini
[Catalog]
host    = koha.biblioteca.edu
port    = 9999
db      = biblios
user    = z3950user
pass    = z3950pass

[ILSDI]
url = https://koha.biblioteca.edu/cgi-bin/koha/ilsdi.pl
```

---

## Ejemplo 8: Webhooks con el Plugin Event-driven

Para capturar eventos en tiempo real (requiere instalar plugin):

```perl
# En el plugin Koha personalizado
sub after_circ_action {
    my ($self, $params) = @_;
    
    if ($params->{action} eq 'checkout') {
        my $checkout = $params->{payload}{checkout};
        
        # Enviar a tu sistema externo
        my $ua = LWP::UserAgent->new;
        $ua->post('https://tu-sistema.com/webhook/koha', {
            event    => 'checkout',
            patron   => $checkout->patron->cardnumber,
            item     => $checkout->item->barcode,
            due_date => $checkout->date_due
        });
    }
}
```

---

## Configuración mínima para habilitar la API REST

En Koha, ir a `Administración > Preferencias del sistema`:

| Preferencia | Valor |
|-------------|-------|
| `RESTBasicAuth` | Habilitar |
| `RESTOAuth2ClientCredentials` | Habilitar |
| `RESTPublicAPI` | Habilitar |
| `ILS-DI` | Habilitar |
| `OAI-PMH` | Habilitar |

Crear cliente OAuth2 en `Administración > Credenciales API`:
1. Nombre: `Mi Aplicación`
2. Descripción: `Integración con portal web`
3. Guardar → anotar `client_id` y `client_secret`

Asignar permisos al usuario de API según los recursos que necesitas acceder.
