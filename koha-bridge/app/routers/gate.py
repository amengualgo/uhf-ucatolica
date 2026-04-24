from fastapi import APIRouter
from pydantic import BaseModel
from app.koha_client import koha

router = APIRouter(prefix="/api/v1/gate", tags=["gate"])


class GateResponse(BaseModel):
    epc: str
    item_id: int | None
    alarm: bool
    reason: str


@router.get("/{epc}", response_model=GateResponse)
async def gate_check(epc: str):
    """UC4 — Puerta antihurto: verificar si el ítem debe disparar alarma."""
    epc = epc.strip().upper()

    item = await koha.get_item_by_epc(epc)
    if not item:
        return GateResponse(epc=epc, item_id=None, alarm=True, reason="EPC no registrado en Koha")

    checked_out = bool(item.get("checked_out_date"))

    return GateResponse(
        epc=epc,
        item_id=item["item_id"],
        alarm=checked_out,
        reason="Ítem en préstamo activo" if checked_out else "Ítem disponible",
    )
