# Sistema de Plugins de Koha

## ¿Qué son los plugins de Koha?

Los plugins permiten **extender Koha sin modificar el código fuente**. Son la forma correcta de añadir funcionalidades personalizadas que no están en el núcleo.

- Disponibles desde Koha 3.12+
- Formato: archivos `.kpz` (zip renombrado)
- Repositorio oficial: https://koha-community.org/koha-plugins/

## Habilitar el sistema de plugins

En `koha-conf.xml`:
```xml
<pluginsdir>/var/lib/koha/NOMBRE/plugins</pluginsdir>
<enable_plugins>1</enable_plugins>
```

En `Administración > Preferencias del sistema`:
- `UseKohaPlugins = Habilitar`

## Tipos de hooks disponibles en plugins

| Hook | Cuándo se ejecuta |
|------|-------------------|
| `after_circ_action` | Después de préstamo/devolución/renovación |
| `after_hold_action` | Después de crear/cancelar/confirmar reserva |
| `after_patron_action` | Después de crear/modificar usuario |
| `before_send_messages` | Antes de enviar notificaciones |
| `opac_head` | Inyectar CSS/JS en cabecera del OPAC |
| `opac_js` | Inyectar JavaScript en el OPAC |
| `intranet_head` | Inyectar en cabecera del staff |
| `api_routes` | Agregar endpoints REST personalizados |
| `auth_client` | Personalizar autenticación |
| `cronjob_nightly` | Tarea programada nocturna |

## Plugins útiles y conocidos

| Plugin | Función |
|--------|---------|
| **Koha-Plugin-OPAC-Sliders** | Carruseles en el OPAC |
| **Koha-Plugin-CoverArt** | Portadas desde Google Books, OpenLibrary |
| **Koha-Plugin-PayViaPayPal** | Pago de multas con PayPal |
| **Koha-Plugin-Room-Reservations** | Reserva de salas |
| **Koha-Plugin-SMTP-From** | Configuración SMTP personalizada |
| **Koha-Plugin-Carrousel** | Novedades destacadas en OPAC |
| **Koha-Plugin-VirtualShelves** | Listas mejoradas |

## Desarrollar tu propio plugin

Estructura mínima de un plugin:
```
MiPlugin/
├── Koha/
│   └── Plugin/
│       └── MiPlugin.pm      # Clase principal Perl
└── CHANGELOG
```

Clase base del plugin (`MiPlugin.pm`):
```perl
package Koha::Plugin::MiPlugin;

use base qw(Koha::Plugins::Base);

our $VERSION = "1.0.0";
our $metadata = {
    name            => 'Mi Plugin Personalizado',
    author          => 'Tu Nombre',
    description     => 'Descripción del plugin',
    date_authored   => '2024-01-01',
    date_updated    => '2024-01-01',
    minimum_version => '22.11',
    maximum_version => undef,
    version         => $VERSION,
};

sub new {
    my ($class, $args) = @_;
    $args->{metadata} = $metadata;
    return $class->SUPER::new($args);
}

# Hook: después de un préstamo
sub after_circ_action {
    my ($self, $params) = @_;
    my $action  = $params->{action};   # 'checkout', 'checkin', 'renewal'
    my $payload = $params->{payload};
    
    if ($action eq 'checkout') {
        my $patron = $payload->{checkout}->patron;
        # hacer algo...
    }
}

# Agregar endpoint REST personalizado
sub api_routes {
    my ($self, $args) = @_;
    my $spec_file = $self->mbf_path('openapi.json');
    return decode_json(read_file($spec_file));
}

1;
```

## Empaquetar e instalar el plugin

```bash
# Empaquetar
zip -r MiPlugin.kpz Koha/ CHANGELOG

# Instalar via interfaz web:
# Administración > Plugins de Koha > Subir plugin > seleccionar .kpz
```
