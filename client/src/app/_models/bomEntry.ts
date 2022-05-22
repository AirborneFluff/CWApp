import { Part } from "./part";

export interface BOMEntry {
    partId: number;
    bomId: number;
    part: Part;
    quantity: number;
    componentLocation: string;
}