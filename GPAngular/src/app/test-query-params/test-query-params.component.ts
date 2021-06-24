import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Params, Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { ConfirmationService } from 'primeng/api';
import { ICase } from 'src/app/models/case';
import { CaseService } from 'src/app/services/case-service.service';
import { IApplication } from '../models/application';
import { ITag } from '../models/tag';
import { ApplicationService } from '../services/application-service.service';
import { TagService } from '../services/tag-service.serivce';

@Component({
  selector: 'app-test-query-params',
  templateUrl: './test-query-params.component.html',
  styleUrls: ['./test-query-params.component.css']
})
export class TestQueryParamsComponent implements OnInit {

  constructor(
    private caseService: CaseService,
    private tagService: TagService,
    private appService: ApplicationService,
    private notifier: NotifierService,
    private route: ActivatedRoute,
    private router: Router,
    private confirmationService: ConfirmationService
  ) { }
  myCase: ICase = {
    description: "",
    steps: [],
    tags: [],
    title: "",
    caseFiles: [],
    applications: []
  };

  pageTitle = "Page Search";
  Title = "";
  Tags = [];
  description = "";
  allApps: IApplication[] = [];
  allTags: ITag[] = [];

  public casesArray: ICase[] = [];

  onSubmit() {
    let params: Params = {
      // tags: this.myCase.tags.map(t => t.name),
      // description: this.myCase.description,
      // title: this.myCase.title,
      // applications: this.myCase.applications.map(t => t.name)
    };
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

  removeCase(eve: Event, _case: ICase) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this case?',
      accept: () => {
        if (_case.id) {
          this.caseService.deleteCase(_case.id).subscribe((res) => {
            if (_case.id) {
              let ind = this.casesArray.indexOf(_case);
              if (ind >= 0) {
                this.casesArray.splice(ind, 1);
                this.notifier.notify('success', 'Case deleted successfully');
              }
            }
          });
        }
      }
    });
  }

  getApplications(_case: ICase): string {
    return _case.applications.map(app => app.name).join(', ');
  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(
      (params) => {
        let title: string = params.get("title") || "";
        let description: string = params.get("description") || "";
        let tags: string[] = params.getAll("tags");
        let applications: string[] = params.getAll("applications");
        this.myCase.title = title;
        this.myCase.description = description;
        this.myCase.tags = tags.map(t => { return { name: t } });
        this.myCase.applications = applications.map(app => { return { name: app } });
        console.log(this.myCase.applications);

        this.caseService.searchCases(title, description, tags, applications)
          .subscribe(
            (data) => {
              this.casesArray = data;
            },
            (error) => this.notifier.notify('error', 'Failed to load cases')
          );

      }, (err) => console.log(err)
    );
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
