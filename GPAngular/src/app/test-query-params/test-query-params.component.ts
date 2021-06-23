import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Params, Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';
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
    private router: Router
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
      tags: this.myCase.tags,
      description: this.myCase.description,
      title: this.myCase.title,
      applications: this.myCase.applications
    };
    this.router.navigate(['/cases'], {
      queryParams: params,
      replaceUrl: true
    });
  }

  removeCase(eve: Event, _case: ICase) {
    this.caseService.deleteCase(_case.id ?? 0).subscribe();
  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(
      (params) => {
        let title: string = params.get("title") || "";
        let description: string = params.get("description") || "";
        let tags: string[] = params.getAll("tags");
        let applications: string[] = params.getAll("applications");



        this.caseService.searchCases(title, description, tags, applications)
          .subscribe(
            (data) => {
              this.casesArray = data;
            },
            (error) => this.notifier.notify('error', 'Failed to load cases')
          );

      }, (err) => console.log(err)
    )
  }

}
