import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Part } from 'src/app/_models/part';
import { PartsService } from 'src/app/_services/parts.service';

@Component({
  selector: 'app-part-details',
  templateUrl: './part-details.component.html',
  styleUrls: ['./part-details.component.css']
})
export class PartDetailsComponent implements OnInit {
  part: Part;

  constructor(private partService: PartsService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadPart();
  }

  loadPart() {
    this.partService.getPart(this.route.snapshot.paramMap.get("partcode")).subscribe(part => {
      this.part = part;
      console.log(this.part);
    })
  }

}
