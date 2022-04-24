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
  @Output() removePrice = new EventEmitter;
  supplierNameValid: Boolean = true;
  edittingPrices: Boolean = false;

  constructor(private partService: PartsService, private supplierService: SuppliersService) { }

  ngOnInit(): void {
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

  onPriceChange(event: any, price: SourcePrice) {
    let newPriceVal = this.parsePriceString(event.target.value);
    newPriceVal.priceString = event.target.value;
    newPriceVal.id = price.id;
    this.source.prices[this.source.prices.findIndex(x => x.id === price.id)] = newPriceVal;
    console.log(this.source.prices);

    this.partService.updatePrices(this.partCode, this.source.id, this.source.prices).subscribe();
  };

  onSourceChange($event: any) {
    this.partService.updateSupplySource(this.partCode, this.source).subscribe();
  };

  parsePriceString(priceString: string) {
    let newPrice: SourcePrice = {
      id: 0,
      unitPrice: 0,
      quantity: 0,
      priceString: ''
    };
    
    let arr = priceString.split('+');
    newPrice.quantity = Number.parseFloat(arr[0].trim());
    newPrice.unitPrice = Number.parseFloat(arr[1].trim());

    return newPrice;
  }

  addPrice() {
    this.partService.addSourcePrice(this.partCode, this.source.id, 0, 1).subscribe(response => {
      this.source.prices.push(response);
      this.partService.updatePrices(this.partCode, this.source.id, this.source.prices).subscribe();
    })
  }

  deleteSource() {
    this.partService.removeSupplySource(this.partCode, this.source.id).subscribe();
    this.removeSource.emit(this.source);
  }

  deletePrice(price) {
    console.log(price)
    this.partService.removeSourcePrice(this.partCode, this.source.id, price.id).subscribe(() => {
      this.source.prices.splice(this.source.prices.indexOf(price), 1);
    });
  }

  editPrices() {
    this.edittingPrices = !this.edittingPrices;
  }
}
