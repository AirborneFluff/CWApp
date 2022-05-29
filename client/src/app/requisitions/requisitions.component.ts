import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { PageParams } from '../_models/pageParams';
import { Part } from '../_models/part';
import { Requisition } from '../_models/requisiton';
import { PartsService } from '../_services/parts.service';
import { RequisitionsService } from '../_services/requisitions.service';

@Component({
  selector: 'app-requisitions',
  templateUrl: './requisitions.component.html',
  styleUrls: ['./requisitions.component.css']
})
export class RequisitionsComponent implements OnInit {
  partCodes: string[] = [];
  requisitions: Requisition[] = [];
  defaultStockUnits = "pcs";

  req_partcode: string = "";
  req_partId: number;
  req_partDescription = "";
  req_partStockUnits = this.defaultStockUnits;
  req_partBufferValue: number;
  req_partCodeValid: boolean = true;
  req_partAlreadyOrdered: boolean = false;

  req_forBuffer: boolean = false;
  private _quantRem: number;
  private _quantReq: number;


  public set req_quantityRemaining(value: number) {
    this._quantRem = value

    if (this.req_forBuffer) {
      this._quantReq = this.req_partBufferValue - this._quantRem;
    }
  }

  public set req_quantityRequired(value: number) {
    this._quantReq = value

    if (this.req_forBuffer) {
      this._quantRem = this.req_partBufferValue - this._quantReq;
    }
  }

  public get req_quantityRemaining() {
    return this._quantRem
  }
  public get req_quantityRequired() {
    return this._quantReq
  }

  constructor(private partService: PartsService, private requisitionService: RequisitionsService) { }

  ngOnInit(): void {
    this.partService.getAllPartcodes().subscribe(() => {
      this.partCodes = this.partService.partCodes
    })
    this.loadRequisitions();
  }

  onPartcodeChanged($event) {
    if ($event.constructor.name === "TypeaheadMatch") {
      this.req_partCodeValid = true;
      this.partService.getPart(this.req_partcode).subscribe(x => {
        this.req_partDescription = x.description;
        this.req_partBufferValue = x.bufferValue;
        this.req_partId = x.id;

        this._quantRem = undefined;
        this._quantReq = undefined;

        if (x.stockUnits === "") this.req_partStockUnits = this.defaultStockUnits
        else this.req_partStockUnits = x.stockUnits

        if (x.bufferValue > 0) this.req_forBuffer = true;
        else this.req_forBuffer = false;

        if (x.requisitions?.length > 0) this.req_partAlreadyOrdered = true;
        else this.req_partAlreadyOrdered = false;
      })
    } else {
      if (!this.partCodes.find(x => x == this.req_partcode)) {
        this.req_partCodeValid = false;
        this.req_partAlreadyOrdered = false;
      } else this.req_partCodeValid = true;
    }
  }

  sendRequest(urgent: boolean) {
    let req: Requisition = {
      id: undefined,
      partId: this.req_partId,
      userId: 1,
      outboundOrderId: undefined,
      quantity: this.req_quantityRequired,
      stockRemaining: this.req_quantityRemaining,
      forBuffer: this.req_forBuffer,
      urgent: false,
      date: undefined
    };

    if (urgent) req.urgent = true;

    this.requisitionService.sendRequisition(req).subscribe(response => {
      this.requisitions.push(response as Requisition);
    });
  }

  loadRequisitions() {
    let params = new PageParams;
    params.pageNumber = 1;
    params.pageSize = 50;
    this.requisitionService.getPaginatedRequisitions(params).subscribe(response => {
      this.requisitions = response.result;
      console.log(this.requisitions)
    })
  }
}
