import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BOM } from 'src/app/_models/bom';
import { ProductsService } from 'src/app/_services/products.service';

@Component({
  selector: 'app-bom-details',
  templateUrl: './bom-details.component.html',
  styleUrls: ['./bom-details.component.css']
})
export class BomDetailsComponent implements OnInit {
  bom: BOM;
  date: string;

  constructor(private productsService: ProductsService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getBOMDetails();
  }

  getBOMDetails() {
    this.productsService.getBom(parseInt(this.route.snapshot.paramMap.get("bomid")), parseInt(this.route.snapshot.paramMap.get("bomid"))).subscribe(response => {
      this.bom = response;
      this.date = new DatePipe('en-GB').transform(response.dateCreated, 'M/d/yy');
    });
  }

  bomUpdate($event: any) {
    this.bom = $event;
  }

}
