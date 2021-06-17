import { Component, OnInit, ViewChild } from '@angular/core';
import { ICase } from 'src/app/models/case';
import { AppConsts } from 'src/app/app-consts';
import { NgForm } from '@angular/forms';
import { CaseService } from 'src/app/services/case-service.service';
import { MessageService } from 'primeng/api';
import { NotifierService } from 'angular-notifier';
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
    if (this.userFrm?.valid) {
      this.caseService.updateCase(this.case);
    } else {
      this.notifier.notify('error', 'Invalid inputs')
    }
  }

  constructor(private caseService: CaseService, private notifier: NotifierService) { }

  ngOnInit(): void { }

}
