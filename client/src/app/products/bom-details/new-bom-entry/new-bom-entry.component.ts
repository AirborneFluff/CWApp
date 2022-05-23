import { ThrowStmt } from '@angular/compiler';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BOM } from 'src/app/_models/bom';
import { NewBOMEntry } from 'src/app/_models/bomEntry';
import { BomsService } from 'src/app/_services/boms.service';

@Component({
  selector: 'app-new-bom-entry',
  templateUrl: './new-bom-entry.component.html',
  styleUrls: ['./new-bom-entry.component.css']
})
export class NewBomEntryComponent implements OnInit {
  @Output() bomEmitter = new EventEmitter<BOM>();
  newEntry: NewBOMEntry = {} as NewBOMEntry;
  editting = false;

  constructor(private bomService: BomsService, private route: ActivatedRoute) { }

  ngOnInit(): void {

  }

  editEntry() {
    this.editting = true;
  }

  createEntry() {
    let productId = parseInt(this.route.snapshot.paramMap.get("id"));
    let bomId = parseInt(this.route.snapshot.paramMap.get("bomid"));
    this.bomService.addBomEntry(productId, bomId, this.newEntry).subscribe(response => {
      this.bomEmitter.emit(response as BOM);
      this.editting = false;
      this.cancelEdit();
    });
  }

  cancelEdit() {
    this.newEntry = {} as NewBOMEntry;
    this.editting = false;
  }

}
