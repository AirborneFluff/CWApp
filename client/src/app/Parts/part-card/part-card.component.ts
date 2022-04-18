import { Component, Input, OnInit } from '@angular/core';
import { Part } from 'src/app/_models/part';

@Component({
  selector: 'app-part-card',
  templateUrl: './part-card.component.html',
  styleUrls: ['./part-card.component.css']
})
export class PartCardComponent implements OnInit {
  @Input() part: Part;

  constructor() { }

  ngOnInit(): void {
  }

}
