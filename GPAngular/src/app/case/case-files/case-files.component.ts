import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ICase } from 'src/app/models/case';

@Component({
  selector: 'app-case-files',
  templateUrl: './case-files.component.html',
  styleUrls: ['./case-files.component.css']
})
export class CaseFilesComponent implements OnInit {


  @Input()
  caseFiles: ICase[] = [];
  @Output()
  caseFilesChange: EventEmitter<ICase[]> = new EventEmitter<ICase[]>();

  @Input()
  urlUpload: string = "";

  constructor() { }

  ngOnInit(): void {
  }

}
