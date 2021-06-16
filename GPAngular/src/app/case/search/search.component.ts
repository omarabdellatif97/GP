import { Component, OnInit } from '@angular/core';
import { ICase } from 'src/app/models/case';
import { CaseService } from 'src/app/services/case-service.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  constructor(private _caseService: CaseService) { }
  pageTitle = "Page Search";
  Title = "";
  Tags = [];
  description = "";


  public casesArray: ICase[] = [];

  onSubmit() {
    this._caseService.searchProfiles(this.Title, this.description, this.Tags)
      .subscribe(
        (data) => this.casesArray = data,
        (error) => console.log("There is an Error!" + error)
      );
  }

  ngOnInit(): void {
  }

}
