import { Component, OnInit } from '@angular/core';
import { NotifierService } from 'angular-notifier';
import { ICase } from 'src/app/models/case';
import { CaseService } from 'src/app/services/case-service.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  constructor(private _caseService: CaseService, private notifier: NotifierService) { }
  pageTitle = "Page Search";
  Title = "";
  Tags = [];
  description = "";

  public casesArray: ICase[] = [];

  onSubmit() {
    this._caseService.searchCases(this.Title, this.description, this.Tags)
      .subscribe(
        (data) => {
          this.casesArray = data
        },
        (error) => {
          this.notifier.notify('error', 'Failed to fetch data');
        }
      );
  }

  ngOnInit(): void {
  }

}
