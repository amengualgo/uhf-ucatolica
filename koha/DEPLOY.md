# Despliegue de Koha ILS

## Requisitos previos
- Docker y Docker Compose instalados
- Red externa `proxy-net` creada: `docker network create proxy-net`

## Instalación desde cero

### 1. Levantar el contenedor
```bash
docker compose up -d
```
La primera vez tarda ~10 minutos (descarga paquetes Debian + Koha).  
Los reinicios posteriores tardan ~30 segundos.

### 2. Obtener credenciales del instalador web
```bash
docker exec koha_app grep -E 'user|pass' /etc/koha/sites/library/koha-conf.xml | head -10
```
Buscar las líneas:
```
<user>koha_library</user>
<pass>CONTRASEÑA_GENERADA</pass>
```
> La contraseña se genera aleatoriamente en cada instalación limpia.

### 3. Abrir el instalador web
```
http://<IP_SERVIDOR>:8082/cgi-bin/koha/installer/install.pl
```

### 4. Pasos del instalador web
1. **Choose language** → `en`
2. **Check Perl dependencies** → Continue
3. **Database settings** → verificar y Continue
4. **Set up database** → esperar ~2 minutos
5. **MARC flavor** → `MARC21`
6. **Selecting default settings** → ver selección detallada abajo → clic **Import** → esperar ~2 minutos
7. **Onboarding**:
   - Create a library → código: `UCB` / nombre: `Biblioteca Universidad Católica`
   - Create a patron category → `Staff`
   - Create Koha administrator patron → completar datos, categoría `Staff`
   - Create a new item type → ej: `BK` / Books
   - Create a circulation rule → puede omitirse (configurar después)

#### Selecting default settings — detalle de selección

**MARC frameworks: Mandatory** — ya vienen marcados, no tocar:
- ✅ Default MARC21 Standard Authority types (tipos de autoridad: nombres, títulos, temas, etc.)
- ✅ MARC21 Default and Acquisitions bibliographic frameworks

**MARC frameworks: Optional** — marcar todos:
- ✅ `marc21_default_matching_rules` — reglas para detectar duplicados por ISBN/ISSN al importar
- ✅ `marc21_sample_fastadd_framework` — formulario rápido para catalogar sobre la marcha
- ✅ `marc21_simple_bib_frameworks` — plantillas por tipo de material (BKS libros, CF recursos digitales, SR audio, VR video, SER revistas, etc.)

**Other data: Mandatory** — ya vienen marcados, no tocar:
- ✅ Credit/Debit types, Authorised values, Classification sources, ILL statuses, Patron restriction types, Sample frequencies/notices/numbering

**Other data: Optional** — marcar estos:
- ✅ Some basic default authorised values (estados de ítems: perdido, dañado, ubicaciones)
- ✅ CSV profiles (perfiles de exportación)
- ✅ Z39.71 coded values
- ✅ MARC code list for relators (roles: autor, editor, ilustrador, etc.)
- ✅ Some basic currencies (USD como moneda base para adquisiciones)
- ✅ Useful patron attribute types (SHOW_BCODE: mostrar código de barras en ficha de usuario)
- ✅ Sample patron types and categories (PT Patron, ST Student, S Staff, T Teacher, etc.)
- ✅ A set of default item types (tipos de ítem predefinidos)
- ✅ Allow access to Z39.50 servers (Library of Congress, Columbia, BNF — para importar registros catalogados)

**No marcar:**
- ☐ Sample label and patron card data
- ☐ Sample libraries
- ☐ Sample holidays
- ☐ Sample news items
- ☐ Sample patrons
- ☐ Sample quotes

### 5. Acceder a Koha
| Interfaz | URL |
|---|---|
| OPAC (usuarios) | `http://<IP>:8081` |
| Staff (bibliotecarios) | `http://<IP>:8082` |

---

## Resetear contraseña de un usuario Koha

Si el usuario no puede entrar (contraseña incorrecta al crearlo):

```bash
docker exec koha_app bash -c "KOHA_CONF=/etc/koha/sites/library/koha-conf.xml PERL5LIB=/usr/share/koha/lib perl -e \"use Koha::Patrons; my \\\$p = Koha::Patrons->find({userid=>'USUARIO'}); \\\$p->set_password({password=>'NUEVA_CLAVE', skip_validation=>1}); \\\$p->store(); print 'Listo\n';\""
```

Reemplazar `USUARIO` y `NUEVA_CLAVE`. Los warnings de memcached son normales, ignorarlos.

---

## Comandos útiles

```bash
# Ver logs en tiempo real
docker logs -f koha_app

# Reiniciar contenedor (sin perder datos)
docker compose restart

# Destruir todo y empezar desde cero
docker compose down -v && docker compose up -d

# Ver estado de Apache dentro del contenedor
docker exec koha_app apache2ctl -S

# Ver puertos en escucha
docker exec koha_app ss -tlnp
```

---

## Arquitectura

- **Contenedor único**: MariaDB + Apache + Koha dentro del mismo container
- **Imagen base**: `debian:12-slim` + paquetes oficiales `debian.koha-community.org`
- **Puertos**: 80 (OPAC) y 8080 (Staff) → mapeados a 8081 y 8082 en el host
- **Volúmenes persistentes**:
  - `koha_data` → `/var/lib/koha`
  - `koha_logs` → `/var/log/koha`
  - `koha_db` → `/var/lib/mysql`
  - `koha_etc` → `/etc/koha`

## Notas técnicas
- Koha requiere `mpm_itk` para crear la instancia pero no puede correr con él en Docker.
  El entrypoint crea la instancia con itk habilitado y luego cambia a `mpm_prefork`.
- `/etc/apache2` no está en volumen — el entrypoint restaura `library.conf` desde `/etc/koha/library-apache.conf` en cada arranque.
- La contraseña de la BD se genera aleatoriamente con `koha-create`. Consultarla siempre con el comando de credenciales arriba.
