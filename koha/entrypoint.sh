#!/bin/bash

# Iniciar memcached (requerido por la REST API y preferencias del sistema)
service memcached start

# Iniciar MariaDB
service mariadb start
sleep 3

# Crear usuario del sistema library-koha si no existe (requerido por SIP2)
if ! id library-koha &>/dev/null; then
    useradd -r -s /bin/false library-koha
fi

# Añadir www-data al grupo library-koha para compartir acceso a archivos
usermod -aG library-koha www-data 2>/dev/null || true

# Habilitar módulos Apache necesarios (antes de koha-create)
a2enmod rewrite cgi headers proxy_http

# Crear instancia Koha solo si no existe
if [ ! -f /etc/koha/sites/library/koha-conf.xml ]; then
    echo "Creando instancia Koha por primera vez..."

    sed -i 's/AssignUserID/#AssignUserID/g' /etc/koha/apache-shared.conf 2>/dev/null || true
    sed -i 's/^INTRAPORT=.*/INTRAPORT="8080"/' /etc/koha/koha-sites.conf

    koha-create --create-db library

    cp /etc/apache2/sites-available/library.conf /etc/koha/library-apache.conf
fi

# Cambiar de mpm-itk a mpm-prefork (itk no funciona en Docker)
a2dismod mpm_itk 2>/dev/null || true
a2enmod mpm_prefork

# Deshabilitar AssignUserID (directiva exclusiva de mpm-itk)
sed -i 's/AssignUserID/#AssignUserID/g' /etc/koha/apache-shared.conf 2>/dev/null || true

# Restaurar config de Apache (se pierde en cada reinicio porque /etc/apache2 no está en volumen)
if [ -f /etc/koha/library-apache.conf ]; then
    cp /etc/koha/library-apache.conf /etc/apache2/sites-available/library.conf
fi
sed -i 's/AssignUserID/#AssignUserID/g' /etc/apache2/sites-available/library.conf 2>/dev/null || true

# Configurar puerto 8080 si no está
if ! grep -q "Listen 8080" /etc/apache2/ports.conf; then
    echo "Listen 8080" >> /etc/apache2/ports.conf
fi

# Permisos — Apache (www-data) y SIP2 (library-koha) comparten acceso
mkdir -p /var/cache/koha/library
mkdir -p /var/lib/koha/library/tmp
chown -R www-data:www-data /var/cache/koha/
chown -R www-data:www-data /var/lib/koha/library/

# Config de Koha: library-koha como dueño, permisos de lectura para todos
chown -R library-koha:library-koha /etc/koha/sites/library/ 2>/dev/null || true
chmod -R a+rX /etc/koha/sites/library/ 2>/dev/null || true

# Logs: library-koha como dueño, www-data puede escribir vía grupo
chown -R library-koha:library-koha /var/log/koha/library/ 2>/dev/null || true
chmod -R a+rwX /var/log/koha/library/ 2>/dev/null || true

# CGIPassAuth On — permite que Apache pase el header Authorization a la REST API de Koha
grep -q 'CGIPassAuth' /etc/koha/apache-shared-intranet.conf 2>/dev/null || \
    python3 -c "
f='/etc/koha/apache-shared-intranet.conf'
c=open(f).read()
c=c.replace('AddHandler cgi-script .pl','AddHandler cgi-script .pl\n    CGIPassAuth On')
open(f,'w').write(c)
" 2>/dev/null || true

# Habilitar sitio Koha y deshabilitar default
a2ensite library 2>/dev/null || true
a2dissite 000-default 2>/dev/null || true

# Iniciar Apache
service apache2 restart || service apache2 start

# SIP2 — habilitar si no está habilitado
if [ ! -f /var/lib/koha/library/sip.enabled ]; then
    koha-sip --enable library 2>/dev/null || true
fi

# SIP2 — asegurar que el listener escuche en 0.0.0.0 (no solo localhost)
python3 -c "
f='/etc/koha/sites/library/SIPconfig.xml'
c=open(f).read()
c=c.replace('127.0.0.1:6001/tcp/IPv4','6001/tcp')
if \"ipv='4'\" not in c:
    c=c.replace(\"min_servers='1'\",\"min_servers='1' ipv='4'\")
open(f,'w').write(c)
" 2>/dev/null || true

# SIP2 — directorio de PID con permisos correctos
mkdir -p /var/run/koha/library
chown library-koha:library-koha /var/run/koha/library

# SIP2 — arrancar daemon
koha-sip --start library 2>/dev/null || true

echo "=== Koha listo ==="
echo "OPAC:  puerto 80  → host 8081"
echo "Staff: puerto 8080 → host 8082"
echo "SIP2:  puerto 6001"

tail -f /var/log/koha/library/opac-error.log /var/log/koha/library/intranet-error.log 2>/dev/null || tail -f /dev/null
