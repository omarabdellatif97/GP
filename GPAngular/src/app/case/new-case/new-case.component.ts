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
  styleUrls: ['./new-case.component.css']
})
export class NewCaseComponent implements OnInit {
  @ViewChild('frm')
  public userFrm: NgForm | null = null;

  myCase: ICase = {
    description: "",
    steps: [],
    tags: [],
    title: "",
    caseFiles: []
  };

  temp2: string[] = [];

  set tempTags(arr: string[]) {
    this.temp2 = arr;
    this.myCase.tags = arr.map((item) => {
      return { name: item };
    });
  }

  get tempTags(): string[] {
    return this.temp2;
  }

  fileUploadURL = AppConsts.fileUploadURL;

  imageUploadHandler = (blobInfo: any, success: any, failure: any, progress: any) => {
    console.log(blobInfo);
    this.fileService.saveFile(blobInfo).subscribe(
      (caseFile: ICaseFile) => {
        this.notifier.notify('success', 'succeeded to upload image');
        success(caseFile.url);
      },
      (err: Error) => {
        this.notifier.notify('error', 'Failed to upload image');
        failure('failed to upload image');
      }
    )
  }

  onSubmit(event: Event) {
    console.log(this.myCase);
    if (this.userFrm?.valid) {
      this.caseService.addCase(this.myCase).pipe().subscribe({
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

  constructor(private caseService: CaseService, private notifier: NotifierService, private fileService: FileService) {
    console.log(this.fileService);
  }

  ngOnInit(): void { }

}
