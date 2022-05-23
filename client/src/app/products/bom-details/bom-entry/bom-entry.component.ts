import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BOM } from 'src/app/_models/bom';
import { BOMEntry } from 'src/app/_models/bomEntry';
import { BomsService } from 'src/app/_services/boms.service';

@Component({
  selector: 'app-bom-entry',
  templateUrl: './bom-entry.component.html',
  styleUrls: ['./bom-entry.component.css']
})
export class BomEntryComponent implements OnInit {
  @Output() bomEmitter = new EventEmitter<BOM>();
  @Input() entry: BOMEntry;
  entryInput: BOMEntry;
  editting = false;
  editError = false;

  constructor(private bomService: BomsService, private route: ActivatedRoute) { }

  ngOnInit(): void {
  }

  editEntry() {
    this.entryInput = JSON.parse(JSON.stringify(this.entry));
    this.editting = true;
  }

  cancelEdit() {
    this.entryInput = null;
    this.editting = false;
  }

  saveEntry() {
    this.updateEntry();
  }

  updateEntry() {
    let productId = parseInt(this.route.snapshot.paramMap.get("id"));
    this.bomService.updateBomEntry(productId, this.entryInput).subscribe(response => {
      this.bomEmitter.emit(response as BOM);
      this.editting = false;
    }, error => {
      this.editError = true;
    });
  }

  deleteEntry() {
    let productId = parseInt(this.route.snapshot.paramMap.get("id"));
    this.bomService.deleteBomEntry(productId, this.entry).subscribe(response => {
      this.bomEmitter.emit(response);
    }, error => {
      this.editError = true;
    });
  }

}
