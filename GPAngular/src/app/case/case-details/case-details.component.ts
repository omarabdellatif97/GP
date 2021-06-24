import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { IApplication } from 'src/app/models/application';
import { ICase } from 'src/app/models/case';
import { ITag } from 'src/app/models/tag';
import { ApplicationService } from 'src/app/services/application-service.service';
import { CaseService } from 'src/app/services/case-service.service';
import { FileService } from 'src/app/services/file-service.service';
import { TagService } from 'src/app/services/tag-service.serivce';

@Component({
  selector: 'app-case-details',
  templateUrl: './case-details.component.html',
  styleUrls: ['./case-details.component.css']
})
export class CaseDetailsComponent implements OnInit {
  myCase: ICase = {
    description: "",
    steps: [],
    tags: [],
    title: "",
    caseFiles: [],
    applications: []
  };
  id: number = 0;
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

  getApplications(_case: ICase): string {
    return _case.applications.map(app => app.name).join(', ');
  }
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
}
