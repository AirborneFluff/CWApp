import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Pagination } from 'src/app/_models/pagination';
import { Part } from 'src/app/_models/part';
import { PartParams } from 'src/app/_models/partParams';
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
  pagination: Pagination;
  partParams: PartParams;

  constructor(private partService: PartsService) {
    this.partParams = new PartParams();
  }

  ngOnInit(): void {
    this.loadParts();
  }

  loadParts() {
    console.log(this.partParams);
    this.partService.getPaginatedParts(this.partParams).subscribe(response => {
      this.parts = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any) {
    this.pagination.currentPage = event.page;
  }
}
