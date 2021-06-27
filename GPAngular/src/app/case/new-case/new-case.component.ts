import { Component, OnInit, ViewChild } from '@angular/core';
import { ICase } from 'src/app/models/case';
import { AppConsts } from 'src/app/app-consts';
import { NgForm } from '@angular/forms';
import { CaseService } from 'src/app/services/case-service.service';
import { FileService } from 'src/app/services/file-service.service';
import { ICaseFile } from 'src/app/models/case-file';
import { ITag } from 'src/app/models/tag';
import { ApplicationService } from 'src/app/services/application-service.service';
import { IApplication } from 'src/app/models/application';
import { TagService } from 'src/app/services/tag-service.serivce';
import { NotificationService } from 'src/app/services/notification-service.service';
import { forkJoin, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-new-case',
  templateUrl: './new-case.component.html',
  styleUrls: ['./new-case.component.css']
})
export class NewCaseComponent implements OnInit {
  @ViewChild('frm')
  public userFrm!: NgForm;

  myCase: ICase = {
    description: "",
    steps: [],
    tags: [],
    title: "",
    caseFiles: [],
    applications: []
  };
  fileUploadURL = AppConsts.fileUploadURL;
  allApps: IApplication[] = [];
  allTags: ITag[] = [];
  isLoading = true;
  submitting = false;

  constructor(private caseService: CaseService,
    private notify: NotificationService,
    private fileService: FileService,
    private appService: ApplicationService,
    private tagService: TagService) { }

  ngOnInit(): void {
    this.isLoading = true;
    const allObs = forkJoin([
      this.tagService.getAllTags().pipe(catchError((err) => { return of(null); })),
      this.appService.getAllApps().pipe(catchError((err) => { return throwError(err); })),
    ]).subscribe(([tags, apps]) => {
      if (tags) {
        this.allTags = [];
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
      } else {
        this.notify.show('Failed to get tags to support autocomplete', 'close', {
          duration: 2000
        })
      }
      this.allApps = apps;
      this.isLoading = false;
    }, (err) => {
      this.notify.show('Error has occured please try again', 'close', {
        duration: 2000
      })
    });
  }

  imageUploadHandler = (blobInfo: any, success: any, failure: any, progress: any) => {
    console.log(blobInfo);
    this.fileService.saveFile(blobInfo).subscribe(
      (caseFile: ICaseFile) => {
        this.notify.show('Succeeded to upload image', 'close', {
          duration: 2000,
        });
        success(caseFile.url);
      },
      (err: Error) => {
        this.notify.show('Failed to upload image', 'close', {
          duration: 2000,
        });
        failure('failed to upload image');
      }
    )
  }

  onSubmit(event: Event) {
    this.submitting = true;
    if (this.userFrm?.valid) {
      this.caseService.addCase(this.myCase).pipe().subscribe({
        next: () => {
          this.notify.show('Case added successfully', 'close', {
            duration: 2000,
          });
          this.myCase = {
            description: "",
            steps: [],
            tags: [],
            title: "",
            caseFiles: [],
            applications: []
          };
          this.submitting = false;

        },
        error: () => {
          this.notify.show('Failed to add case', 'close', {
            duration: 2000,
          });
          this.submitting = false;
        }
      });
    } else {
      this.notify.show('Invalid inputs', 'close', {
        duration: 2000,
      });
      this.submitting = false;
    }
  }
}
