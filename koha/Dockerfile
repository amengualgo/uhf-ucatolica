FROM debian:12-slim

ENV DEBIAN_FRONTEND=noninteractive
ENV KOHA_INSTANCE=library

# Dependencias base
RUN apt-get update && apt-get install -y \
    curl \
    gnupg2 \
    apt-transport-https \
    ca-certificates \
    sudo \
    mariadb-server \
    && rm -rf /var/lib/apt/lists/*

# Repositorio oficial de Koha
RUN curl -fsSL https://debian.koha-community.org/koha/gpg.asc | gpg --dearmor -o /usr/share/keyrings/koha.gpg && \
    echo "deb [signed-by=/usr/share/keyrings/koha.gpg] https://debian.koha-community.org/koha stable main" \
    > /etc/apt/sources.list.d/koha.list

# Instalar Koha y memcached (requerido por la REST API)
RUN apt-get update && apt-get install -y koha-common memcached && \
    rm -rf /var/lib/apt/lists/*

EXPOSE 80 8080

COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh
CMD ["/entrypoint.sh"]
