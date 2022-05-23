import { Part } from "./part";

export interface BOMEntry {
    partId: number;
    bomId: number;
    part: Part;
    quantity: number;
    componentLocation: string;
}

export interface NewBOMEntry {
    partId: number;
    quantity: number;
    componentLocation: string;
}