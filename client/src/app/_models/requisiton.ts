export interface Requisition {
    id: number;
    forBuffer: boolean;
    part: PartialPart;
    user: PartialUser;
    outboundOrder: PartialOutboundOrder;
    quantity: number;
    urgent: boolean;
    date: string;
}

export class CreateRequisition {
    partId: number = null;
    quantity: number = null;
    stockRemaining: number = null;
    forBuffer: boolean = false;
    urgent: boolean = false;
    date: string = undefined;
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