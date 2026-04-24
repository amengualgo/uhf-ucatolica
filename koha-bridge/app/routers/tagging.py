from fastapi import APIRouter, HTTPException
from pydantic import BaseModel
from app.koha_client import koha

router = APIRouter(prefix="/api/v1/tag", tags=["tagging"])


class TagRequest(BaseModel):
    barcode: str
    epc: str


class TagResponse(BaseModel):
    item_id: int
    biblio_id: int
    previous_epc: str | None
    new_epc: str
    message: str


@router.post("", response_model=TagResponse)
async def tag_item(req: TagRequest):
    """UC1 — Etiquetado inicial o reemplazo de tag RFID."""
    item = await koha.get_item_by_barcode(req.barcode)
    if not item:
        raise HTTPException(status_code=404, detail=f"Ítem con barcode {req.barcode!r} no encontrado en Koha")

    previous_epc = item.get("inventory_number")

    await koha.update_epc(item, req.epc)

    action = "reemplazado" if previous_epc else "asignado"
    return TagResponse(
        item_id=item["item_id"],
        biblio_id=item["biblio_id"],
        previous_epc=previous_epc,
        new_epc=req.epc,
        message=f"Tag RFID {action} correctamente",
    )


@router.get("/by-epc/{epc}")
async def get_item_by_epc(epc: str):
    """Buscar ítem en Koha por EPC del tag RFID."""
    item = await koha.get_item_by_epc(epc)
    if not item:
        raise HTTPException(status_code=404, detail=f"Ningún ítem tiene el EPC {epc!r}")
    return item
