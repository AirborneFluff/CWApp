import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { PageParams } from '../_models/pageParams';
import { Part } from '../_models/part';
import { CreateRequisition, Requisition } from '../_models/requisiton';
import { AccountService } from '../_services/account.service';
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
  newRequisition: CreateRequisition = new CreateRequisition();
  selectedPart: Part;
  selectedPartCode: string;
  selectedDescription: string;
  defaultStockUnits = "pcs";

  constructor(private partService: PartsService, private requisitionService: RequisitionsService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.partService.getAllPartcodes().subscribe(() => {
      this.partCodes = this.partService.partCodes
    })
    this.loadRequisitions();
  }

  onPartcodeChanged($event) {
    if (this.partCodes.includes(this.selectedPartCode)) {
      this.partService.getPart(this.selectedPartCode).subscribe(x => {
        console.log(x)
        if (x.stockUnits === "") x.stockUnits = this.defaultStockUnits
        this.selectedPart = x;

        this.newRequisition = new CreateRequisition();
        this.newRequisition.partId = x.id;

        if (x.bufferValue > 0) this.newRequisition.forBuffer = true;
        else this.newRequisition.forBuffer = false;

        if (x.requisitions?.length > 0) this.showOldRequestDetails(x.requisitions[0])
      })
    } else this.resetModel()
  }

  sendRequest() {
    console.log(this.newRequisition);
    this.requisitionService.sendRequisition(this.newRequisition).subscribe(response => {
      this.requisitions.unshift(response as Requisition);
      this.selectedPartCode = undefined;
      this.resetModel()
    });
  }

  loadRequisitions() {
    let params = new PageParams;
    params.pageNumber = 1;
    params.pageSize = 50;
    this.requisitionService.getPaginatedRequisitions(params).subscribe(response => {
      this.requisitions = response.result;
    })
  }

  showOldRequestDetails(request: Requisition) {
  }

  updateRequest() {
    this.requisitionService.updateRequisition(this.newRequisition).subscribe(response => {
      let oldReqIndex = this.requisitions.findIndex(r => r.id == (response as Requisition).id)
      this.requisitions.splice(oldReqIndex, 1);
      this.requisitions.unshift(response as Requisition);
      this.selectedPartCode = undefined;
      this.resetModel()
    });
  }

  resetModel() {
    this.selectedPart = undefined;
    this.selectedDescription = undefined;
    this.newRequisition = new CreateRequisition();
  }

  public set quantityRemaining(value: number) {
    this.newRequisition.stockRemaining = Number(value)

    if (this.newRequisition.forBuffer) {
      this.newRequisition.quantity = this.selectedPart.bufferValue - this.newRequisition.stockRemaining;
    }
  }

  public set quantityRequired(value: number) {
    this.newRequisition.quantity = Number(value)

    if (this.newRequisition.forBuffer) {
      this.newRequisition.stockRemaining = this.selectedPart.bufferValue - this.newRequisition.quantity;
    }
  }

  public get quantityRemaining() {
    return this.newRequisition.stockRemaining
  }
  public get quantityRequired() {
    return this.newRequisition.quantity
  }
}
