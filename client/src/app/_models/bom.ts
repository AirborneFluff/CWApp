import { BOMEntry } from "./bomEntry";

export interface BOM {
    id: number;
    productId: number;
    title: string;
    description: string;
    dateCreated: string;
    parts: BOMEntry[];
}