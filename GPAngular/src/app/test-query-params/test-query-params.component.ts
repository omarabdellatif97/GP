import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Params, Router } from '@angular/router';
import { ICase } from 'src/app/models/case';
import { CaseService } from 'src/app/services/case-service';

@Component({
  selector: 'app-test-query-params',
  templateUrl: './test-query-params.component.html',
  styleUrls: ['./test-query-params.component.css']
})
export class TestQueryParamsComponent implements OnInit {

  constructor(
    private _caseService: CaseService,
    private route: ActivatedRoute,
    private router: Router
    ) { }
  pageTitle = "Page Search";
  Title = "";
  Tags = [];
  description = "";


  public casesArray: ICase[] = [];


  onSubmit() {
    this.router.navigate(['/heroes', {}])
    let params: Params = {
      tags:this.Tags,
      description:this.description,
      title:this.Title
    };
    this.router.navigate(['/cases'], {
      queryParams:params
    });
  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(
      (params) => {
        let title:string = params.get("title") || "";
        let description:string = params.get("description") || "";
        let tags:string[] = params.getAll("tag");
        if (title != this.Title || description != this.description || tags.length != this.Tags.length || (tags.some((val, index) => val != this.Tags[index]))) {
          // this._caseService.searchProfiles(this.Title, this.description, this.Tags)
          //   .subscribe(
          //     (data) => this.casesArray = data,
          //     (error) => console.log("There is an Error!" + error)
          //   );
          console.log('changed');
        }
      }
    )
  }

}
