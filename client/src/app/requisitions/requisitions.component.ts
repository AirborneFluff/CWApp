import { Component, OnInit } from '@angular/core';
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

  req_partcode: string = "";
  req_partCodeValid: boolean = true;
  req_partAlreadyOrdered: boolean = false;

  req_description: string = "";
  req_forBuffer: boolean = false;
  req_quantity: number;
  req_quantityRemaining: number;

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
        this.req_description = x.description
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
    if (urgent) {

    } else {
      
    }
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
