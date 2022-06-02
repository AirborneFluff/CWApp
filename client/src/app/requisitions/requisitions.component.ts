import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
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
  oldRequisition: Requisition;
  selectedPart: Part;
  selectedPartCode: string;
  selectedDescription: string;
  defaultStockUnits = "pcs";
  _additionalRequired: number;
  _additionalRemaining: number;
  _oldReqRequired: number;
  _oldReqRemaining: number;

  constructor(private partService: PartsService, private requisitionService: RequisitionsService, private accountService: AccountService
    , private toastr: ToastrService) { }

  ngOnInit(): void {
    this.partService.getAllPartcodes().subscribe(() => {
      this.partCodes = this.partService.partCodes
    })
    this.loadRequisitions();
  }

  onPartcodeChanged($event) {
    if (this.partCodes.includes(this.selectedPartCode)) {
      this.partService.getPart(this.selectedPartCode).subscribe(x => {
        if (x.stockUnits === "") x.stockUnits = this.defaultStockUnits
        this.selectedPart = x;

        this.newRequisition = new CreateRequisition();
        this.newRequisition.partId = x.id;

        if (x.bufferValue > 0) this.newRequisition.forBuffer = true;
        else this.newRequisition.forBuffer = false;

        if (x.requisitions?.length > 0) {
          this.requisitionService.getOpenRequisition(x.partCode).subscribe(req => {
            this.oldRequisition = req;
            this.quantityRequired = req.quantity
            this._additionalRequired = undefined;
            this._oldReqRequired = req.quantity
          })
        }

      })
    } else this.resetModel()
  }

  sendRequest() {
    console.log(this.newRequisition);
    this.requisitionService.sendRequisition(this.newRequisition).subscribe(response => {
      this.requisitions.unshift(response as Requisition);
      this.selectedPartCode = undefined;
      this.resetModel()
    }, error => {
      this.toastr.error("Problem sending request")
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
    this.oldRequisition = request;
    this.newRequisition.urgent = request.urgent;
    this._oldReqRequired = request.quantity;

    if (request.forBuffer) {
      this._oldReqRemaining = this.selectedPart.bufferValue - request.quantity
    }

    this.quantityRequired = request.quantity;
    console.log(this.quantityRequired)
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
    this.oldRequisition = undefined;
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


  public set additionalRemaining(value: number) {
    this.quantityRemaining = Number(value)
    this._additionalRemaining = Number(value)

    if (this.newRequisition.forBuffer) {
      this._additionalRequired = this._oldReqRemaining - (this._additionalRemaining || 0)
    }
  }

  public set additionalRequired(value: number) {
    this._additionalRequired = Number(value)
    this.quantityRequired = Number(value) + this._oldReqRequired

    if (this.newRequisition.forBuffer) {
      this._additionalRemaining = this.newRequisition.stockRemaining
    }
  }

  public get additionalRemaining() {
    return this._additionalRemaining
  }
  public get additionalRequired() {
    return this._additionalRequired
  }
}
