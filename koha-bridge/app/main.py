from fastapi import FastAPI
from app.routers import tagging, circulation, gate, inventory

app = FastAPI(title="Koha RFID Bridge", version="0.1.0")

app.include_router(tagging.router)
app.include_router(circulation.router)
app.include_router(gate.router)
app.include_router(inventory.router)


@app.get("/health")
async def health():
    return {"status": "ok"}
