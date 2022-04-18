import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Part } from 'src/app/_models/part';
import { PartsService } from 'src/app/_services/parts.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-part-browse',
  templateUrl: './part-browse.component.html',
  styleUrls: ['./part-browse.component.css']
})
export class PartBrowseComponent implements OnInit {
  baseUrl = environment.baseUrl;
  parts: Part[] = [];

  constructor(private partService: PartsService) { }

  ngOnInit(): void {
    this.loadParts();
  }

  loadParts() {
    this.partService.getParts().subscribe(parts => {
      this.parts = parts;
      console.log(this.parts);
    })
  }
}
