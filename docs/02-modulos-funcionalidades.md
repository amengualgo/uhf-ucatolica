# Módulos y Funcionalidades de Koha

## Módulos principales

### 1. Catalogación (Cataloguing)

Gestión completa de registros bibliográficos.

- Edición de registros MARC21 / UNIMARC
- Importación masiva por Z39.50 / SRU desde catálogos externos (Library of Congress, BNE, etc.)
- Importación/exportación de archivos MARC (`.mrc`)
- Importación de registros Dublin Core
- Control de autoridades (personas, entidades, materias)
- Plantillas MARC personalizables
- Validación de campos MARC

### 2. Circulación (Circulation)

Gestión del préstamo y devolución de materiales.

- Préstamo, devolución y renovación de ítems
- Gestión de reservas (holds)
- Transferencias entre sucursales
- Políticas de préstamo configurables por tipo de usuario y material
- Multas y cobros automáticos
- Préstamos interbibliotecarios (ILL)
- Alertas y notificaciones a usuarios (email, SMS)

### 3. OPAC (Online Public Access Catalog)

Catálogo público accesible por los usuarios finales.

- Búsqueda simple y avanzada
- Facetas de filtrado (autor, materia, año, formato)
- Listas de lectura personales
- Renovaciones en línea por el usuario
- Reservas en línea
- Historial de préstamos
- Integración con redes sociales
- Responsive (mobile-friendly)

### 4. Adquisiciones (Acquisitions)

Gestión de compras y presupuestos.

- Gestión de proveedores
- Órdenes de compra y presupuestos
- Seguimiento de pedidos
- Recepción de materiales
- Informes financieros
- EDI (Electronic Data Interchange) con proveedores

### 5. Publicaciones Seriadas (Serials)

Control de revistas y publicaciones periódicas.

- Suscripciones a revistas
- Control de fascículos recibidos
- Alertas de números esperados
- Patrones de publicación personalizables

### 6. Gestión de Usuarios (Patrons)

Administración de los miembros de la biblioteca.

- Registro y gestión de usuarios
- Categorías de usuarios (estudiante, docente, externo, etc.)
- Atributos extendidos personalizables
- Deduplicación de registros
- Importación masiva de usuarios (CSV)
- Integración con LDAP/Active Directory
- Autoregistro en OPAC

### 7. Informes (Reports)

Generación de estadísticas y reportes.

- Informes predefinidos
- Constructor de informes SQL personalizado
- Informes guardados y programables
- Exportación a CSV, Excel

### 8. Herramientas administrativas (Tools)

- Gestión de noticias y banners del OPAC
- Calendario de días festivos
- Perfiles de impresión (etiquetas, código de barras)
- Gestión de registros MARC en lote
- Registros de auditoría (logs)

### 9. ERM (Electronic Resource Management)

Módulo para gestión de recursos electrónicos.

- Gestión de licencias de bases de datos y e-books
- Acuerdos con proveedores
- Plataformas de acceso
- Paquetes de recursos electrónicos

### 10. ILL (Interlibrary Loan)

Préstamos entre bibliotecas.

- Solicitudes de préstamo interbibliotecario
- Gestión de peticiones entrantes y salientes
- Integración con backends externos (BLDSS, RapidILL)

## Resumen de capacidades por módulo

```
Catalogación   ████████████████████  Completo
Circulación    ████████████████████  Completo
OPAC           ████████████████████  Completo
Usuarios       ████████████████░░░░  Muy bueno
Adquisiciones  ████████████████░░░░  Muy bueno
Seriadas       ██████████████░░░░░░  Bueno
ERM            ████████████░░░░░░░░  En desarrollo activo
ILL            ████████████░░░░░░░░  Bueno
```
