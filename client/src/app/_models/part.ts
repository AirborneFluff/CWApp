import { Supplier } from "./supplier";

export interface Part {
    id: number,
    partCode: string,
    description: string,
    notes: string,
    bufferValue: number,
    bufferUnit: string;
    buffer: string,
    supplySources: SupplySource[]
}

export interface SupplySource {
    id: number,
    partId: number,
    supplier: Supplier,
    supplierSKU: string,
    manufacturerSKU: string,
    packSize: number,
    minimumOrderQuantity: number,
    notes: string,
    roHS: boolean,
    prices: SourcePrice[]
}

export interface SourcePrice {
    id: number,
    unitPrice: number,
    quantity: number,
    priceString: string
}