from fastapi import APIRouter, HTTPException
from pydantic import BaseModel
from app.koha_client import koha
from app.sip2_client import sip2, SIP2Error

router = APIRouter(prefix="/api/v1", tags=["circulation"])


class CheckoutRequest(BaseModel):
    epc: str
    cedula: str


class CheckoutResponse(BaseModel):
    item_id: int
    patron_id: int
    patron_name: str
    due_date: str | None
    message: str


class CheckinRequest(BaseModel):
    epc: str


class CheckinResponse(BaseModel):
    item_id: int
    message: str


@router.post("/checkout", response_model=CheckoutResponse)
async def checkout(req: CheckoutRequest):
    """UC2 — Préstamo: EPC + cédula → checkout vía SIP2."""
    epc = req.epc.strip().upper()

    patron = await koha.get_patron(req.cedula)
    if not patron:
        raise HTTPException(status_code=404, detail=f"Usuario {req.cedula!r} no encontrado en Koha")

    item = await koha.get_item_by_epc(epc)
    if not item:
        raise HTTPException(status_code=404, detail=f"Ítem con EPC {epc!r} no encontrado en Koha")

    if item.get("checked_out_date"):
        raise HTTPException(status_code=409, detail="El ítem ya tiene un préstamo activo")

    barcode = item.get("external_id")
    if not barcode:
        raise HTTPException(status_code=422, detail="El ítem no tiene código de barras (external_id)")

    patron_cardnumber = patron.get("cardnumber")
    if not patron_cardnumber:
        raise HTTPException(status_code=422, detail="El usuario no tiene número de tarjeta (cardnumber)")

    try:
        result = await sip2.checkout(barcode, patron_cardnumber)
    except SIP2Error as e:
        raise HTTPException(status_code=409, detail=str(e))

    patron_name = f"{patron.get('firstname', '')} {patron.get('surname', '')}".strip()

    return CheckoutResponse(
        item_id=item["item_id"],
        patron_id=patron["patron_id"],
        patron_name=patron_name,
        due_date=result.get("due_date"),
        message="Préstamo registrado correctamente",
    )


@router.post("/checkin", response_model=CheckinResponse)
async def checkin(req: CheckinRequest):
    """UC3 — Devolución: EPC → checkin vía SIP2."""
    epc = req.epc.strip().upper()

    item = await koha.get_item_by_epc(epc)
    if not item:
        raise HTTPException(status_code=404, detail=f"Ítem con EPC {epc!r} no encontrado en Koha")

    if not item.get("checked_out_date"):
        raise HTTPException(status_code=409, detail="El ítem no tiene préstamo activo")

    barcode = item.get("external_id")
    if not barcode:
        raise HTTPException(status_code=422, detail="El ítem no tiene código de barras (external_id)")

    try:
        await sip2.checkin(barcode)
    except SIP2Error as e:
        raise HTTPException(status_code=409, detail=str(e))

    return CheckinResponse(
        item_id=item["item_id"],
        message="Devolución registrada correctamente",
    )
