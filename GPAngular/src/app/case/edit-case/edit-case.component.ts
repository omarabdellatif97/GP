import { Component, OnInit, ViewChild } from '@angular/core';
import { ICase } from 'src/app/models/case';
import { AppConsts } from 'src/app/app-consts';
import { NgForm } from '@angular/forms';
import { CaseService } from 'src/app/services/case-service.service';
import { MessageService } from 'primeng/api';
import { NotifierService } from 'angular-notifier';
import { ActivatedRoute, Router } from '@angular/router';
import { FileService } from 'src/app/services/file-service.service';
import { ICaseFile } from 'src/app/models/case-file';
@Component({
  selector: 'app-edit-case',
  templateUrl: './edit-case.component.html',
  styleUrls: ['./edit-case.component.css']
})
export class EditCaseComponent implements OnInit {

  @ViewChild('frm')
  public userFrm: NgForm | null = null;

  myCase: ICase = {
    description: "",
    steps: [],
    tags: [],
    title: "",
    caseFiles: []
  };

  fileUploadURL = AppConsts.fileUploadURL;

  onSubmit(event: Event) {
    console.log(this.myCase);
    if (this.userFrm?.valid) {
      this.caseService.updateCase(this.myCase).pipe().subscribe({
        next: () => {
          this.notifier.notify('success', 'Case updated successfully')

        },
        error: () => {
          this.notifier.notify('error', 'Failed to update case')
        }
      });
    } else {
      this.notifier.notify('error', 'Invalid inputs')
    }
  }
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
  id: number = 0;
  constructor(private caseService: CaseService, private notifier: NotifierService, private fileService: FileService, private router: Router, private myRoute: ActivatedRoute) {
    this.id = myRoute.snapshot.params.id;
  }

  ngOnInit(): void {
    this.caseService.getCaseById(this.id).subscribe(
      (sentCase: ICase) => {
        console.log(sentCase);
        // this.myCase.title = sentCase.title;
        // console.log(this.myCase.title);
        // console.log(sentCase);
        this.myCase = sentCase;
        this.temp2 = sentCase.tags.map((item) => item.name);
        // console.log(this.myCase);
      },
      (err) => {
        this.notifier.notify('error', 'Failed to load case');
      }
    )
  }

}
