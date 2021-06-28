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
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from 'src/app/services/notification-service.service';
import { forkJoin, Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
@Component({
  selector: 'app-edit-case',
  templateUrl: './edit-case.component.html',
  styleUrls: ['./edit-case.component.css']
})
export class EditCaseComponent implements OnInit {
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
  id: number = 0;

  constructor(private caseService: CaseService,
    private notify: NotificationService,
    private fileService: FileService,
    private appService: ApplicationService,
    private tagService: TagService,
    private router: Router,
    private myRoute: ActivatedRoute) {
    this.id = myRoute.snapshot.params.id;
  }

  ngOnInit(): void {
    this.isLoading = true;
    const allObs = forkJoin([
      this.tagService.getAllTags().pipe(catchError((err) => { return of(null); })),
      this.appService.getAllApps().pipe(catchError((err) => { return throwError(err); })),
      this.caseService.getCaseById(this.id).pipe(catchError((err) => { return throwError(err); }))
    ]).subscribe(([tags, apps, _case]) => {
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
      _case.caseFiles = _case.caseFiles.filter(c => c.isDescriptionFile == false);
      this.myCase = _case;
      this.myCase.applications = this.allApps.filter(c => this.myCase.applications.map(a => a.id).indexOf(c.id) != -1);
      this.isLoading = false;
    }, (err) => {
      this.notify.show('Error has occured please try again', 'close', {
        duration: 2000
      })
    });
  }

  imageUploadHandler = (blobInfo: any, success: any, failure: any, progress: any) => {
    this.fileService.saveFile(blobInfo).subscribe(
      (caseFile: ICaseFile) => {
        success(caseFile.url);
      },
      (err: Error) => {
        this.notify.show('Failed to upload image', 'close', {
          duration: 2000
        });
        failure('Failed to upload image');
      }
    )
  }

  onSubmit(event: Event) {
    this.submitting = true;
    if (this.userFrm?.valid) {
      this.caseService.updateCase(this.myCase).pipe().subscribe({
        next: () => {
          this.notify.show('Case edited successfully', 'close', {
            duration: 2000
          });
          this.submitting = false;

        },
        error: () => {
          this.notify.show('Failed to edit case', 'close', {
            duration: 2000
          });
          this.submitting = false;

        }
      });
    } else {
      this.notify.show('Invalid inputs', 'close', {
        duration: 2000
      });
      this.submitting = false;

    }
  }
}
