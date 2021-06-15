import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ICase } from 'src/app/models/case';
import { ICaseFile } from 'src/app/models/case-file';

@Component({
  selector: 'app-case-files',
  templateUrl: './case-files.component.html',
  styleUrls: ['./case-files.component.css']
})
export class CaseFilesComponent implements OnInit {


  @Input()
  caseFiles: ICaseFile[] = [];
  @Output()
  caseFilesChange: EventEmitter<ICaseFile[]> = new EventEmitter<ICaseFile[]>();

  @Input()
  urlUpload: string = "";

  constructor() { }

  ngOnInit(): void {
  }

}
