import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Part } from 'src/app/_models/part';
import { PartsService } from 'src/app/_services/parts.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-part-browse',
  templateUrl: './part-browse.component.html',
  styleUrls: ['./part-browse.component.css']
})
export class PartBrowseComponent implements OnInit {
  searchValue: "";
  baseUrl = environment.baseUrl;
  filteredParts: Part[] = [];
  parts: Part[] = [];

  constructor(private partService: PartsService) { }

  ngOnInit(): void {
    this.loadParts();
  }

  loadParts() {
    this.partService.getParts().subscribe(parts => {
      this.parts = parts;
      this.updateResults();
    })
  }

  updateResults() {
    let partcodeFilter = this.parts.filter(p => {
      return p.partCode.includes(this.searchValue);
    });
    let wordFilter = this.parts.filter(p => {
      return p.description.toLocaleLowerCase().includes(this.searchValue.toLocaleLowerCase());
    });
    this.filteredParts = partcodeFilter;
    wordFilter.forEach(p => {
      if (!this.filteredParts.includes(p)) this.filteredParts.push(p);
    });
  }
}
