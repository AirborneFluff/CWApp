import { Supplier } from "./supplier";

export interface Part {
    partId: number,
    partCode: string,
    description: string,
    notes: string,
    buffer: string,
    sources: Source[]
}

interface Source {
    sourceId: number,
    supplier: Supplier,
    supplierSKU: string,
    manufacturerSKU: string,
    packSize: number,
    minimumOrderQuantity: number,
    notes: string,
    rohs: boolean,
    prices: Price[]
}

interface Price {
    priceId: number,
    unitPrice: number,
    quantity: number
}