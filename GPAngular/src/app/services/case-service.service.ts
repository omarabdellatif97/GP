import { HttpClient, HttpErrorResponse, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { catchError, tap } from "rxjs/operators"
import { AppConsts } from "../app-consts";
import { ICase } from "../models/case";
import { ITag } from "../models/tag";


@Injectable({
  providedIn: 'root'
})
export class CaseService {
  url: string = `${AppConsts.apiUrl}/api/case`;
  constructor(private http: HttpClient) { }


  searchCases(title: string, description: string, tags: string[]): Observable<ICase[]> {
    var params = new HttpParams();
    for (let i = 0; i < tags.length; i++) {
      params.set(`tags`, tags[i]);
    }
    if (title != "") {
      params.set(`title`, title);
    }
    // if (description != "") {
    //   params.set(`description`, description);
    // }
    return this.http.get<ICase[]>(this.url, {
      params: params
    });
  }

  getAllCases(): Observable<ICase[]> {
    return this.http.get<ICase[]>(this.url);
  }

  getCaseById(id: number) {
    return this.http.get<ICase>(`${this.url}/${id}`);
  }

  addCase(myCase: ICase) {
    console.log(myCase);
    return this.http.post(`${this.url}`, myCase);
  }

  deleteCase(id: number) {
    return this.http.delete(`${this.url}/${id}`);
  }

  updateCase(myCase: ICase) {
    return this.http.put(`${this.url}/${myCase.id}`, myCase);
  }
}