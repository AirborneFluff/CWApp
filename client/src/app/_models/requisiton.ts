export interface Requisition {
    id: number;
    part: PartialPart;
    user: PartialUser;
    outboundOrder: PartialOutboundOrder;
    quantity: number;
    urgent: boolean;
    date: string;
}

export interface CreateRequisition {
    partId: number;
    quantity: number;
    stockRemaining: number;
    forBuffer: boolean;
    urgent: boolean;
    date: string;
}

interface PartialPart {
    partCode: string;
    description: string;
    stockUnits: string;
}

interface PartialUser {
    initials: string;
}

interface PartialOutboundOrder {
    id: number;
}