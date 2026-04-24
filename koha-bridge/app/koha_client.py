import time
import json
import httpx
from typing import Optional
from functools import lru_cache
from pydantic_settings import BaseSettings


class Settings(BaseSettings):
    koha_url: str
    koha_client_id: str
    koha_client_secret: str
    koha_patron_search_field: str = "cardnumber"

    class Config:
        env_file = ".env"


@lru_cache
def get_settings() -> Settings:
    return Settings()


TIMEOUT = httpx.Timeout(30.0)


class KohaClient:
    def __init__(self):
        self._token: Optional[str] = None
        self._token_expires_at: float = 0

    def _settings(self) -> Settings:
        return get_settings()

    def _base(self) -> str:
        return f"{self._settings().koha_url}/api/v1"

    async def _get_token(self) -> str:
        s = self._settings()
        async with httpx.AsyncClient(timeout=TIMEOUT) as client:
            r = await client.post(
                f"{self._base()}/oauth/token",
                data={
                    "grant_type": "client_credentials",
                    "client_id": s.koha_client_id,
                    "client_secret": s.koha_client_secret,
                },
            )
            r.raise_for_status()
            data = r.json()
            self._token = data["access_token"]
            self._token_expires_at = time.time() + data["expires_in"] - 30
            return self._token

    async def _auth_header(self) -> dict:
        if not self._token or time.time() >= self._token_expires_at:
            await self._get_token()
        return {"Authorization": f"Bearer {self._token}"}

    async def get_item_by_barcode(self, barcode: str) -> Optional[dict]:
        headers = await self._auth_header()
        async with httpx.AsyncClient(timeout=TIMEOUT) as client:
            r = await client.get(
                f"{self._base()}/items",
                params={"external_id": barcode},
                headers=headers,
            )
            r.raise_for_status()
            items = r.json()
            return items[0] if items else None

    async def get_item_by_epc(self, epc: str) -> Optional[dict]:
        headers = await self._auth_header()
        q = json.dumps({"inventory_number": epc})
        async with httpx.AsyncClient(timeout=TIMEOUT) as client:
            r = await client.get(
                f"{self._base()}/items",
                params={"q": q},
                headers=headers,
            )
            r.raise_for_status()
            items = r.json()
            return items[0] if items else None

    async def update_epc(self, item: dict, epc: str) -> dict:
        headers = await self._auth_header()
        biblio_id = item["biblio_id"]
        item_id = item["item_id"]
        body = {
            "inventory_number": epc,
            "home_library_id": item["home_library_id"],
            "holding_library_id": item["holding_library_id"],
            "item_type_id": item["item_type_id"],
        }
        async with httpx.AsyncClient(timeout=TIMEOUT) as client:
            r = await client.put(
                f"{self._base()}/biblios/{biblio_id}/items/{item_id}",
                json=body,
                headers=headers,
            )
            r.raise_for_status()
            return r.json()

    async def get_patron(self, identifier: str) -> Optional[dict]:
        headers = await self._auth_header()
        field = self._settings().koha_patron_search_field
        async with httpx.AsyncClient(timeout=TIMEOUT) as client:
            r = await client.get(
                f"{self._base()}/patrons",
                params={field: identifier},
                headers=headers,
            )
            r.raise_for_status()
            patrons = r.json()
            return patrons[0] if patrons else None


koha = KohaClient()
