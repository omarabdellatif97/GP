import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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
import { ActivatedRoute, Router } from '@angular/router';
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
    caseFiles: [],
    applications: []
  };


  constructor(private caseService: CaseService,
    private notifier: NotifierService,
    private fileService: FileService,
    private appService: ApplicationService,
    private tagService: TagService,
    private router: Router,
    private myRoute: ActivatedRoute) {
    this.id = myRoute.snapshot.params.id;

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
      this.caseService.updateCase(this.myCase).pipe().subscribe({
        next: () => {
          this.notifier.notify('success', 'Case edited successfully')

        },
        error: () => {
          this.notifier.notify('error', 'Failed to edit case')
        }
      });
    } else {
      this.notifier.notify('error', 'Invalid inputs')
    }
  }

  // used if the case recieved before all apps
  tempApps: IApplication[] | null = null;

  allApps: IApplication[] = [];
  allTags: ITag[] = [];
  id: number = 0;
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
        this.myCase.applications = this.allApps.filter(c => this.myCase.applications.map(a => a.id).indexOf(c.id) != -1);
      },
      (error) => {
        this.notifier.notify('error', 'Failed to get apps');
      }
    );
    this.caseService.getCaseById(this.id).subscribe(
      (sentCase: ICase) => {
        this.myCase = sentCase;
        if (this.allApps.length == 0) {
          this.allApps = this.myCase.applications;
        } else {
          this.myCase.applications = this.allApps.filter(c => this.myCase.applications.map(a => a.id).indexOf(c.id) != -1);
        }
      },
      (err) => {
        this.notifier.notify('error', 'Failed to load case');
      }
    )
  }


}
