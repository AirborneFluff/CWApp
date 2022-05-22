import { BOM } from "./bom";

export interface Product {
    id: number;
    name: string;
    description: string;
    boms: BOM[];
}