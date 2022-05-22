import { Part } from "./part";

export interface BOM {
    id: number;
    productId: number;
    title: string;
    description: string;
    dateCreated: string;
    parts: Part[];
}