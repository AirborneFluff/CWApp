import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Part } from 'src/app/_models/part';
import { PartsService } from 'src/app/_services/parts.service';
import { SupplySource } from 'src/app/_models/part';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NewSourceModalComponent } from 'src/app/modals/new-source-modal/new-source-modal.component';
import { SuppliersService } from 'src/app/_services/suppliers.service';

@Component({
  selector: 'app-part-details',
  templateUrl: './part-details.component.html',
  styleUrls: ['./part-details.component.css']
})
export class PartDetailsComponent implements OnInit {
  part: Part;
  loaded = false;
  bsModalRef: BsModalRef;
  supplierNames: string[] = [];

  constructor(private partService: PartsService,
    private route: ActivatedRoute,
    private modalService: BsModalService,
    private supplierServce: SuppliersService,
    private router: Router) { }

  ngOnInit(): void {
    this.loadPart();
    this.loadSupplierNames();
  }

  loadPart() {
    this.partService.getPart(this.route.snapshot.paramMap.get("partcode")).subscribe(part => {
      this.part = part;
      this.loaded = true;
    })
  }

  loadSupplierNames() {
    this.supplierServce.getSuppliersNames().subscribe(response => {
      console.log(response)
      this.supplierNames = response;
    })
  }

  addSupplySource() {
    const initialState = {
      partCode: this.part.partCode,
      supplierNames: this.supplierNames
    }
    this.bsModalRef = this.modalService.show(NewSourceModalComponent, { initialState });
    this.bsModalRef.content.updateSources.subscribe(() => {
      this.loadPart();
    })
  }

  removeSupplySource($event: SupplySource) {
    this.part.supplySources.splice(this.part.supplySources.indexOf($event), 1);
  }
}
