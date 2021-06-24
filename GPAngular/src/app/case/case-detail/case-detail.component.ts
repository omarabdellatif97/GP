import { Component, OnInit,Input } from '@angular/core';
import { ICase  } from '../../models/case';
import { ITag  } from '../../models/tag';
import { IStep  } from '../../models/step';
import { ICaseFile  } from '../../models/case-file';
import { Router, ActivatedRoute,ParamMap } from '@angular/router';
import { ApplicationService } from 'src/app/services/application-service.service';
import { CaseService } from 'src/app/services/case-service.service';
import { FileService } from 'src/app/services/file-service.service';
import { TagService } from 'src/app/services/tag-service.serivce';

@Component({
  selector: 'app-case-detail',
  templateUrl: './case-detail.component.html',
  styleUrls: ['./case-detail.component.css']
})
export class CaseDetailComponent implements OnInit {

  allTags: any;
  notifier: any;


  constructor(private caseService: CaseService,
   // private notifier: NotifierService,
    private fileService: FileService,
    private appService: ApplicationService,
    private tagService: TagService,
    private router: Router,
    private myRoute: ActivatedRoute) {
    this.id = myRoute.snapshot.params.id
  }



  id: number = 0;
  ngOnInit(): void {
    this.tagService.getAllTags().subscribe(
      (tags:ITag[]) => {
        this.allTags = tags;
      },
      () => {
        this.notifier.notify('error', 'Failed to get tags');
      }
    );

    this.caseService.getCaseById(this.id).subscribe(
      (sentCase: ICase) => {
        console.log(sentCase);
        this.myCase = sentCase;
      },
      () => {
        this.notifier.notify('error', 'Failed to load case');
      }
    )
  }
 // @Input()
  // @Input()
  // @Input()
  // @Input()
  // @Input()
  // @Input()
  // @Input()
  // @Input()
  myCase!: ICase;

}
