import { Component, OnInit, ViewChild } from '@angular/core';
import { ICase } from 'src/app/models/case';
import { AppConsts } from 'src/app/app-consts';
import { NgForm } from '@angular/forms';
import { CaseService } from 'src/app/services/case-service.service';
import { NotifierService } from 'angular-notifier';
import { FileService } from 'src/app/services/file-service.service';
import { ICaseFile } from 'src/app/models/case-file';
import { ITag } from 'src/app/models/tag';
import { ApplicationService } from 'src/app/services/application-service.service';
import { IApplication } from 'src/app/models/application';
import { TagService } from 'src/app/services/tag-service.serivce';

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
    caseFiles: [],
    applications: []
  };


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
    if (this.userFrm?.valid) {
      this.caseService.addCase(this.myCase).pipe().subscribe({
        next: () => {
          this.notifier.notify('success', 'Case added successfully');
          this.myCase = {
            description: "",
            steps: [],
            tags: [],
            title: "",
            caseFiles: [],
            applications: []
          };

        },
        error: () => {
          this.notifier.notify('error', 'Failed to add case')
        }
      });
    } else {
      this.notifier.notify('error', 'Invalid inputs')
    }
  }

  allApps: IApplication[] = [];
  allTags: ITag[] = [];

  constructor(private caseService: CaseService,
    private notifier: NotifierService,
    private fileService: FileService,
    private appService: ApplicationService,
    private tagService: TagService) { }

  ngOnInit(): void {
    this.tagService.getAllTags().subscribe(
      (tags) => {
        for (let i = 0; i < tags.length; i++) {
          let found = false;
          for (let j = 0; j < i; j++) {
            if (tags[i].name.toLowerCase() == tags[j].name.toLowerCase()) {
              found = true;
              break;
            }
          }
          if (!found) {
            this.allTags.push(tags[i]);
          }
        }
      },
      (error) => {
        this.notifier.notify('error', 'Failed to get tags');
      }
    );
    this.appService.getAllApps().subscribe(
      (apps) => {
        this.allApps = apps;
      },
      (error) => {
        this.notifier.notify('error', 'Failed to get apps');
      }
    );
  }
}
