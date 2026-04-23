#!/bin/bash

# Iniciar MariaDB
service mariadb start
sleep 3

# Habilitar módulos Apache necesarios (antes de koha-create)
a2enmod rewrite cgi headers proxy_http

# Crear instancia Koha solo si no existe (requiere mpm_itk habilitado para el check)
if [ ! -f /etc/koha/sites/library/koha-conf.xml ]; then
    echo "Creando instancia Koha por primera vez..."

    # Deshabilitar AssignUserID antes de crear la instancia
    sed -i 's/AssignUserID/#AssignUserID/g' /etc/koha/apache-shared.conf 2>/dev/null || true

    # Configurar puerto 8080 para intranet
    sed -i 's/^INTRAPORT=.*/INTRAPORT="8080"/' /etc/koha/koha-sites.conf

    koha-create --create-db library

    # Guardar el config de Apache en volumen persistente
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

# Permisos de cache, sesiones, logs y config de Koha
mkdir -p /var/cache/koha/library
mkdir -p /var/lib/koha/library/tmp
chown -R www-data:www-data /var/cache/koha/
chown -R www-data:www-data /var/lib/koha/library/
chown -R www-data:www-data /var/log/koha/library/ 2>/dev/null || true
chown -R www-data:www-data /etc/koha/sites/library/ 2>/dev/null || true

# Habilitar sitio Koha y deshabilitar default
a2ensite library 2>/dev/null || true
a2dissite 000-default 2>/dev/null || true

# Iniciar/reiniciar Apache (koha-create lo arranca internamente, hay que recargar con la config final)
service apache2 restart || service apache2 start

echo "=== Koha listo ==="
echo "OPAC:  puerto 80  → host 8081"
echo "Staff: puerto 8080 → host 8082"

tail -f /var/log/koha/library/opac-error.log /var/log/koha/library/intranet-error.log 2>/dev/null || tail -f /dev/null
