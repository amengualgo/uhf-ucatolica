import asyncio
from fastapi import APIRouter
from pydantic import BaseModel
from app.koha_client import koha

router = APIRouter(prefix="/api/v1/inventory", tags=["inventory"])


class InventoryRequest(BaseModel):
    epcs: list[str]


class ItemStatus(BaseModel):
    epc: str
    item_id: int | None
    status: str  # "available", "checked_out", "unknown"
    checkout_id: int | None


class InventoryResponse(BaseModel):
    total: int
    available: int
    checked_out: int
    unknown: int
    items: list[ItemStatus]


async def _check_epc(epc: str) -> ItemStatus:
    item = await koha.get_item_by_epc(epc)
    if not item:
        return ItemStatus(epc=epc, item_id=None, status="unknown", checkout_id=None)
    checked_out = bool(item.get("checked_out_date"))
    return ItemStatus(
        epc=epc,
        item_id=item["item_id"],
        status="checked_out" if checked_out else "available",
        checkout_id=None,
    )


@router.post("", response_model=InventoryResponse)
async def inventory(req: InventoryRequest):
    """UC5 — Inventario de estantería: verificar estado de lista de EPCs en Koha."""
    epcs = [e.strip().upper() for e in req.epcs]

    items_status = await asyncio.gather(*[_check_epc(epc) for epc in epcs])

    available = sum(1 for i in items_status if i.status == "available")
    checked_out = sum(1 for i in items_status if i.status == "checked_out")
    unknown = sum(1 for i in items_status if i.status == "unknown")

    return InventoryResponse(
        total=len(epcs),
        available=available,
        checked_out=checked_out,
        unknown=unknown,
        items=list(items_status),
    )
