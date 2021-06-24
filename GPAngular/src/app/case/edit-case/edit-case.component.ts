import { Component, OnInit, ViewChild } from '@angular/core';
import { ICase } from 'src/app/models/case';
import { AppConsts } from 'src/app/app-consts';
import { NgForm } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { CaseService } from 'src/app/services/case-service.service';
import { NotifierService } from 'angular-notifier';
import { FileService } from 'src/app/services/file-service.service';
import { ICaseFile } from 'src/app/models/case-file';
import { MatChipInputEvent } from '@angular/material/chips';
import { ITag } from 'src/app/models/tag';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
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

  // add(event: MatChipInputEvent): void {
  //   const value = (event.value || '').trim();
  //   if (value) {
  //     this.myCase.tags.push({ name: value });
  //   }
  //   event.chipInput!.clear();
  // }
  // readonly separatorKeysCodes = [ENTER, COMMA] as const;

  // remove(tag: ITag): void {
  //   const index = this.myCase.tags.indexOf(tag);

  //   if (index >= 0) {
  //     this.myCase.tags.splice(index, 1);
  //   }
  // }



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
    private tagService: TagService,
    private router: Router,
    private myRoute: ActivatedRoute) {
    this.id = myRoute.snapshot.params.id;
  }

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
      },
      (error) => {
        this.notifier.notify('error', 'Failed to get apps');
      }
    );
    this.caseService.getCaseById(this.id).subscribe(
      (sentCase: ICase) => {
        console.log(sentCase);
        this.myCase = sentCase;
      },
      (err) => {
        this.notifier.notify('error', 'Failed to load case');
      }
    )
  }



  // ngOnInit(): void {

  // }

}
