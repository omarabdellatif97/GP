import { Component, OnInit, ViewChild } from '@angular/core';
import { ICase } from 'src/app/models/case';
import { AppConsts } from 'src/app/app-consts';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-edit-case',
  templateUrl: './edit-case.component.html',
  styleUrls: ['./edit-case.component.css']
})
export class EditCaseComponent implements OnInit {

  @ViewChild('frm')
  public userFrm: NgForm | null = null;

  case: ICase = {
    description: "",
    steps: [],
    tags: [],
    title: "",
    caseFiles: []
  };

  fileUploadURL = AppConsts.fileUploadURL;

  onSubmit(event: Event) {
    console.log(this.userFrm);
    console.log(this.case);
  }

  constructor() { }

  ngOnInit(): void { }

}
