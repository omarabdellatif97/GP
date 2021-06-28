import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { IApplication } from 'src/app/models/application';
import { ICase } from 'src/app/models/case';
import { ITag } from 'src/app/models/tag';
import { ApplicationService } from 'src/app/services/application-service.service';
import { CaseService } from 'src/app/services/case-service.service';
import { FileService } from 'src/app/services/file-service.service';
import { NotificationService } from 'src/app/services/notification-service.service';
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
  isLoading = true;

  constructor(private caseService: CaseService,
    private notify: NotificationService,
    private fileService: FileService,
    private appService: ApplicationService,
    private tagService: TagService,
    private router: Router,
    private myRoute: ActivatedRoute) {
    this.id = myRoute.snapshot.params.id;
  }

  getApps(_case: ICase): string {
    return _case.applications.map(app => app.name).join(', ');
  }

  ngOnInit(): void {
    this.isLoading = true;
    this.caseService.getCaseById(this.id).subscribe(
      (sentCase: ICase) => {
        sentCase.caseFiles = sentCase.caseFiles.filter(c => c.isDescriptionFile == false);
        this.myCase = sentCase;
        this.isLoading = false;

      },
      (err) => {
        this.notify.show('Error has happened. Please try again.', 'close', {
          duration: 2000
        });
      }
    )
  }
}
