import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ICaseFile } from 'src/app/models/case-file';
import { AppConsts } from 'src/app/app-consts';

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
  urlUpload: string = AppConsts.fileUploadURL;


  // myUploader() {

  // }

  onUpload(event: any) {
    console.log(event.originalEvent.body.data);
    this.caseFiles.push({
      fileURL: "https://filesamples.com/samples/document/txt/sample1.txt",
      fileName: "sample1.txt"
    });
  }

  constructor() { }

  ngOnInit(): void {
  }

}
