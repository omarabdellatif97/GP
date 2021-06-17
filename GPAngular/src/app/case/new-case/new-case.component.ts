import { Component, OnInit, ViewChild } from '@angular/core';
import { ICase } from 'src/app/models/case';
import { AppConsts } from 'src/app/app-consts';
import { NgForm } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { CaseService } from 'src/app/services/case-service.service';
import { NotifierService } from 'angular-notifier';
import { FileService } from 'src/app/services/file-service.service';
import { ICaseFile } from 'src/app/models/case-file';

@Component({
  selector: 'app-new-case',
  templateUrl: './new-case.component.html',
  styleUrls: ['./new-case.component.css'],
  providers: [MessageService]
})
export class NewCaseComponent implements OnInit {
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

  imageUploadHandler(blobInfo: File, success: any, failure: any, progress: any) {
    this.fileService.saveFile(blobInfo).subscribe(
      (caseFile: ICaseFile) => {
        this.notifier.notify('success', 'failed to upload file');
        success(caseFile.URL);
      },
      (err: Error) => {
        this.notifier.notify('error', 'Failed to upload image');
        failure('failed to upload image');
      }
    )
  }

  onSubmit(event: Event) {
    console.log(this.case);
    if (this.userFrm?.valid) {
      this.caseService.addCase(this.case).pipe().subscribe({
        next: () => {
          this.notifier.notify('success', 'Case added successfully')

        },
        error: () => {
          this.notifier.notify('error', 'Failed to add case')
        }
      });
    } else {
      this.notifier.notify('error', 'Invalid inputs')
    }
  }

  constructor(private caseService: CaseService, private notifier: NotifierService, private fileService: FileService) { }

  ngOnInit(): void { }

}
