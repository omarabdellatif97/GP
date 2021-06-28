import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTable } from '@angular/material/table';
import { ActivatedRoute, NavigationExtras, Params, Router } from '@angular/router';
import { ICase } from 'src/app/models/case';
import { CaseService } from 'src/app/services/case-service.service';
import { IApplication } from '../models/application';
import { ITag } from '../models/tag';
import { ApplicationService } from '../services/application-service.service';
import { TagService } from '../services/tag-service.serivce';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NotificationService } from '../services/notification-service.service';
import { forkJoin, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-test-query-params',
  templateUrl: './test-query-params.component.html',
  styleUrls: ['./test-query-params.component.css']
})
export class TestQueryParamsComponent implements OnInit {
  @ViewChild('mytable') mytable!: MatTable<ICase>;
  myCase: ICase = {
    description: "",
    steps: [],
    tags: [],
    title: "",
    caseFiles: [],
    applications: []
  };
  isLoading = true;
  isLoadingCases = false;
  pageTitle = "Page Search";
  Title = "";
  description = "";
  Tags = [];
  allApps: IApplication[] = [];
  allTags: ITag[] = [];
  public casesArray: ICase[] = [];

  constructor(
    private caseService: CaseService,
    private tagService: TagService,
    private appService: ApplicationService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
    private notify: NotificationService,
  ) { }

  onSubmit() {
    let params: Params = {};

    if (this.myCase.tags.length != 0) {
      params['tags'] = this.myCase.tags.map(t => t.name);
    }
    if (this.myCase.applications.length != 0) {
      params['applications'] = this.myCase.applications.map(t => t.name);
    }
    if (this.myCase.title != "") {
      params['title'] = this.myCase.title;
    }
    if (this.myCase.title != "") {
      params['description'] = this.myCase.description;
    }

    this.router.navigate(['/cases'], {
      queryParams: params,
      replaceUrl: true
    });
  }

  getDate(_case: ICase): string {
    if (_case.publishDate) {
      console.log(_case.publishDate);
      const caseDate = new Date(_case.publishDate);
      return `${caseDate.getDate()}/${caseDate.getMonth() + 1}/${caseDate.getFullYear()}`;
    } else {
      return "";
    }
  }

  removeCase(eve: Event, _case: ICase) {
    this.dialog
      .open(DialogConfimationComponent, {
        data: `Are you sure you want to delete this case?`
      })
      .afterClosed()
      .subscribe((confirm: Boolean) => {
        if (confirm) {
          if (_case.id) {
            this.isLoadingCases = true;
            this.caseService.deleteCase(_case.id).subscribe((res) => {
              if (_case.id) {
                let ind = this.casesArray.indexOf(_case);
                if (ind >= 0) {
                  this.casesArray.splice(ind, 1);
                  // this.notifier.notify('success', 'Case deleted successfully');
                  this.notify.show('Case deleted successfully', 'close', {
                    duration: 2000,
                  });
                  this.mytable?.renderRows();
                  this.isLoadingCases = false;
                }
              }
            },(err) => {
              this.notify.show('Failed to delete case', 'close', {
                duration: 2000,
              });
              this.isLoadingCases = false;
            });
          }
        }
      });
  }

  getApplications(_case: ICase): string {
    return _case.applications.map(app => app.name).join(', ');
  }

  ngOnInit(): void {
    this.isLoading = true;
    const allObs = forkJoin([
      this.tagService.getAllTags().pipe(catchError((err) => { return of(null); })),
      this.appService.getAllApps().pipe(catchError((err) => { return of(null); })),
    ]).subscribe(([tags, apps]) => {
      this.allTags = []
      if (tags) {
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
      if (apps) {
        this.allApps = apps;
      } else {
        this.notify.show('Failed to load apps', 'close', {
          duration: 2000
        })
      }
      this.isLoading = false;
    }, (err) => {
      this.notify.show('Error has occured please try again', 'close', {
        duration: 2000
      })
    });

    this.route.queryParamMap.subscribe(
      (params) => {
        this.isLoadingCases = true;
        let title: string = params.get("title") || "";
        let description: string = params.get("description") || "";
        let tags: string[] = params.getAll("tags");
        let applications: string[] = params.getAll("applications");


        this.myCase.title = title;
        this.myCase.description = description;
        this.myCase.tags = tags.map(t => { return { name: t } });
        this.myCase.applications = this.allApps.filter((app) => applications.indexOf(app.name) != -1);

        this.caseService.searchCases(title, description, tags, applications)
          .subscribe(
            (data) => {
              this.casesArray = data;
              this.isLoadingCases = false;
            },
            (error) => {
              this.notify.show('Failed to load cases', 'close', {
                duration: 2000
              });
            }
          );

      }, (err) => console.log(err)
    );
  }
}



@Component({
  selector: 'app-dialog-confirmation',
  template: `
    <div mat-dialog-content>
        <p>{{message}}</p>
    </div>
    <button type="button" mat-flat-button color="primary" (click)="closeDialog()" [style.margin-right.px]="10" >No</button>
    <button type="button" mat-flat-button color="primary" (click)="confirm()" cdkFocusInitial>Yes</button>
  `
})
export class DialogConfimationComponent implements OnInit {

  constructor(
    public dialog: MatDialogRef<DialogConfimationComponent>,
    @Inject(MAT_DIALOG_DATA) public message: string) { }

  closeDialog(): void {
    this.dialog.close(false);
  }
  confirm(): void {
    this.dialog.close(true);
  }

  ngOnInit() {
  }

}