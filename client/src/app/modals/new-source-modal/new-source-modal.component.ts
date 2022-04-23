import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { map } from 'rxjs/operators';
import { SupplySource } from 'src/app/_models/part';
import { Supplier } from 'src/app/_models/supplier';
import { PartsService } from 'src/app/_services/parts.service';
import { SuppliersService } from 'src/app/_services/suppliers.service';

@Component({
  selector: 'app-new-source-modal',
  templateUrl: './new-source-modal.component.html',
  styleUrls: ['./new-source-modal.component.css']
})
export class NewSourceModalComponent {
  @Input() updateSources = new EventEmitter();
  supplierNameValid: Boolean = true;
  supplierName: string;
  modalRef?: BsModalRef;
  partCode: string;
  supplierNames: string[];

  constructor(private modalService: BsModalService, private partService: PartsService) {
   }
  
  hideModal() {
    this.modalService.hide();
  }

  onSupplierChange($event) {
    if ($event?.type === undefined) {
      this.supplierNameValid = true;
    } else {
      if (this.supplierNames.find(x => x.toLocaleUpperCase() === this.supplierName.toLocaleUpperCase()) === undefined)
        this.supplierNameValid = false;
      else this.supplierNameValid = true;
    }
  }

  submit() {
    if (!this.supplierNameValid) return

    this.partService.addSupplySource(this.partCode, this.supplierName).subscribe(() => this.updateSources.emit());
    this.modalService.hide();
  }

}
