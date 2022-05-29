export interface Requisition {
    id: number;
    partId: number;
    userId: number;
    outboundOrderId: number;
    quantity: number;
    stockRemaining: number;
    forBuffer: boolean;
    urgent: boolean;
    date: string;
}