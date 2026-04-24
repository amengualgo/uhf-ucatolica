import socket
import asyncio
from datetime import datetime
from functools import lru_cache
from pydantic_settings import BaseSettings


class SIP2Settings(BaseSettings):
    sip2_host: str = "koha_app"
    sip2_port: int = 6001
    sip2_login: str
    sip2_password: str
    sip2_institution: str = "UDLV"

    class Config:
        env_file = ".env"


@lru_cache
def get_sip2_settings() -> SIP2Settings:
    return SIP2Settings()


def _ts() -> str:
    """SIP2 timestamp: YYYYMMDD    HHMMSS (18 chars)"""
    return datetime.now().strftime("%Y%m%d    %H%M%S")


def _field(resp: str, code: str) -> str:
    """Extract variable-length field value from SIP2 response."""
    marker = f"|{code}"
    idx = resp.find(marker)
    if idx == -1:
        return ""
    start = idx + len(marker)
    end = resp.find("|", start)
    return resp[start:end] if end != -1 else resp[start:]


class SIP2Error(Exception):
    pass


class SIP2Client:
    def _open(self) -> socket.socket:
        s = get_sip2_settings()
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.settimeout(15)
        sock.connect((s.sip2_host, s.sip2_port))
        return sock

    def _exchange(self, sock: socket.socket, msg: str) -> str:
        sock.sendall(msg.encode("latin-1"))
        buf = b""
        while b"\r" not in buf:
            chunk = sock.recv(4096)
            if not chunk:
                break
            buf += chunk
        return buf.decode("latin-1").strip()

    def _login(self, sock: socket.socket) -> None:
        s = get_sip2_settings()
        msg = f"9300CN{s.sip2_login}|CO{s.sip2_password}|CP{s.sip2_institution}|\r"
        resp = self._exchange(sock, msg)
        if not resp.startswith("94") or resp[2:3] != "1":
            raise SIP2Error(f"Login SIP2 fallido: {resp!r}")

    def _do_checkout(self, item_barcode: str, patron_cardnumber: str) -> dict:
        s = get_sip2_settings()
        sock = self._open()
        try:
            self._login(sock)
            ts = _ts()
            msg = (
                f"11YN{ts}{ts}"
                f"AO{s.sip2_institution}|"
                f"AA{patron_cardnumber}|"
                f"AB{item_barcode}|"
                f"AC|\r"
            )
            resp = self._exchange(sock, msg)
            if not resp.startswith("12"):
                raise SIP2Error(f"Respuesta inesperada: {resp!r}")
            if resp[2:3] != "1":
                detail = _field(resp, "AF") or "Checkout rechazado por Koha"
                raise SIP2Error(detail)
            return {"due_date": _field(resp, "AH") or None}
        finally:
            sock.close()

    def _do_checkin(self, item_barcode: str) -> None:
        s = get_sip2_settings()
        sock = self._open()
        try:
            self._login(sock)
            ts = _ts()
            msg = (
                f"09N{ts}{ts}"
                f"AP{s.sip2_institution}|"
                f"AO{s.sip2_institution}|"
                f"AB{item_barcode}|"
                f"AC|\r"
            )
            resp = self._exchange(sock, msg)
            if not resp.startswith("10"):
                raise SIP2Error(f"Respuesta inesperada: {resp!r}")
            if resp[2:3] != "1":
                detail = _field(resp, "AF") or "Checkin rechazado por Koha"
                raise SIP2Error(detail)
        finally:
            sock.close()

    async def checkout(self, item_barcode: str, patron_cardnumber: str) -> dict:
        return await asyncio.to_thread(self._do_checkout, item_barcode, patron_cardnumber)

    async def checkin(self, item_barcode: str) -> None:
        await asyncio.to_thread(self._do_checkin, item_barcode)


sip2 = SIP2Client()
