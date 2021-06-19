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

  onUpload(eve: any) {
    var myCase: ICaseFile = <ICaseFile>(eve.originalEvent.body);
    this.caseFiles.push(myCase);
    // this.caseFiles.push({
    //   URL: "https://filesamples.com/samples/document/txt/sample1.txt",
    //   fileName: "sample1.txt"
    // });
  }

  constructor() { }

  ngOnInit(): void {
  }

}
