# Infraestructura fox-lab

## Servidor
- **Hostname:** fox-lab
- **IP local:** 192.168.88.10
- **OS:** Debian Linux
- **IP pública:** 186.112.144.130 (fija, casa)
- **ISP:** Movistar — router GPT-2741GNAC (port forwarding bloqueado)

## Dominio
- **Dominio:** `foxone-lab.dev` (comprado en Cloudflare Registrar ~$10/año)
- **DNS:** Cloudflare (registros A apuntando a 186.112.144.130 con proxy naranja)

## Acceso externo — Cloudflare Tunnel

No se puede abrir puertos en el Movistar. Se usa **Cloudflare Tunnel** (`cloudflared`) como reemplazo de ngrok — conexión saliente, sin abrir puertos, HTTPS automático, nodo en Bogotá.

### Servicios activos

| Subdominio | Servicio | Puerto local |
|---|---|---|
| `automation.foxone-lab.dev` | n8n | 5678 |
| `udlvstaff.foxone-lab.dev` | Koha Staff | 8082 |
| `udlvopac.foxone-lab.dev` | Koha OPAC | 8081 |
| `scan-rfid.foxone-lab.dev` | Shadow Scan API | 8000 |

### Archivos del túnel
```
/etc/cloudflared/config.yml                        ← configuración principal
/etc/cloudflared/55aa2276-05e6-4e0b-aa98-dd475ab3c825.json  ← credenciales (NO compartir)
~/.cloudflared/cert.pem                            ← certificado de autenticación
```

### Configuración del túnel (`/etc/cloudflared/config.yml`)
```yaml
tunnel: 55aa2276-05e6-4e0b-aa98-dd475ab3c825
credentials-file: /etc/cloudflared/55aa2276-05e6-4e0b-aa98-dd475ab3c825.json

ingress:
  - hostname: automation.foxone-lab.dev
    service: http://localhost:5678
  - hostname: udlvstaff.foxone-lab.dev
    service: http://localhost:8082
  - hostname: udlvopac.foxone-lab.dev
    service: http://localhost:8081
  - hostname: scan-rfid.foxone-lab.dev
    service: http://localhost:8000
  - service: http_status:404
```

### Servicio systemd
El túnel arranca automáticamente con el servidor:
```bash
sudo systemctl status cloudflared
sudo systemctl restart cloudflared
```

### Agregar un nuevo servicio
1. Editar `/etc/cloudflared/config.yml` — agregar nueva línea `hostname/service`
2. Crear el CNAME en Cloudflare:
   ```bash
   cloudflared tunnel route dns foxone-lab nuevo.foxone-lab.dev
   ```
3. Recargar:
   ```bash
   sudo systemctl restart cloudflared
   ```

---

## nginx (en standby)

nginx está corriendo pero no es el punto de entrada del tráfico externo — Cloudflare Tunnel enruta directamente a cada puerto. Se mantiene por si Movistar habilita port forwarding en el futuro (puertos 80/443 → 192.168.88.10).

---

## Red Docker
- Red externa `proxy-net` — todos los contenedores que necesitan comunicarse entre sí se unen a esta red
- Crear si no existe: `docker network create proxy-net`

## Mikrotik
- IP: 192.168.1.1
- Hace NAT de 192.168.1.62 → 192.168.88.10 (el servidor)
- El server está en la subred 192.168.88.0/24 detrás del Mikrotik
