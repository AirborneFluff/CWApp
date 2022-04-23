import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SourcePrice, SupplySource } from 'src/app/_models/part';
import { PartsService } from 'src/app/_services/parts.service';
import { SuppliersService } from 'src/app/_services/suppliers.service';

@Component({
  selector: 'app-supply-source',
  templateUrl: './supply-source.component.html',
  styleUrls: ['./supply-source.component.css']
})
export class SupplySourceComponent implements OnInit {
  @Input() source: SupplySource;
  @Input() partCode: string;
  @Input() supplierNames: string[] = [];
  @Output() removeSource = new EventEmitter;
  priceStrings: string[] = [];
  supplierNameValid: Boolean = true;

  constructor(private partService: PartsService, private supplierService: SuppliersService) { }

  ngOnInit(): void {
    this.source.prices.forEach(price => {
      this.priceStrings.push(`${price.quantity}+ ${price.unitPrice}`);
    });
  }

  onSupplierChange($event) {
    if ($event?.type === undefined) {
      this.supplierNameValid = true;
    } else {
      if (this.supplierNames.find(x => x.toLocaleUpperCase() === this.source.supplier.name.toLocaleUpperCase()) === undefined)
        this.supplierNameValid = false;
      else this.supplierNameValid = true;
    }
  }

  onPriceChange(event: any, price: string) {
    this.priceStrings[this.priceStrings.indexOf(price)] = event.target.value;
    this.parsePriceStrings();
    this.partService.updatePrices(this.partCode, this.source.id, this.source.prices).subscribe();
  };

  onSourceChange($event: any) {
    this.partService.updateSupplySource(this.partCode, this.source).subscribe();
  };

  parsePriceStrings() {
    let newPrices: SourcePrice[] = [];
    this.priceStrings.forEach(str => {
      let arr = str.split('+');
      let price: SourcePrice = {
        unitPrice: 0,
        quantity: 0,
        priceString: undefined
      };
      price.quantity = Number.parseFloat(arr[0].trim());
      price.unitPrice = Number.parseFloat(arr[1].trim());
      newPrices.push(price);
    });
    this.source.prices = newPrices;
  }

  addPrice() {
    this.priceStrings.push("");
  }

  deleteSource() {
    this.partService.removeSupplySource(this.partCode, this.source.id).subscribe();
    this.removeSource.emit(this.source);
  }
}
