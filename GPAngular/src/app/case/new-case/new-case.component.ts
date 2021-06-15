import { Component, OnInit } from '@angular/core';
import { ICase } from 'src/app/models/case';

@Component({
  selector: 'app-new-case',
  templateUrl: './new-case.component.html',
  styleUrls: ['./new-case.component.css']
})
export class NewCaseComponent implements OnInit {

  case: ICase = {
    description: "",
    steps: [],
    tags: [],
    title: "",
    caseFiles: []
  };

  constructor() { }

  ngOnInit(): void { }

}
