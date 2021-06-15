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
  url: string = `${AppConsts.apiUrl}/api/cases`;
  constructor(private http: HttpClient) { }


   searchProfiles(title: string, tags: ITag[]): Observable<ICase[]> {
    var params = new HttpParams();
    for (let i = 0; i < tags.length; i++) {
      params.set(`tags[]`, tags[i].name);
    }
    if (title != "") {
      params.set(`title`, title);
    }
    return this.http.get<ICase[]>(this.url, {
      params: params
    })
  }

  getAllProfiles(): Observable<ICase[]> {
    return this.http.get<ICase[]>(this.url);
  }

  getProfileById(id: number) {
    return this.http.get<ICase>(`${this.url}/${id}`);
  }

  addProfile(profile: ICase) {
    return this.http.post(`${this.url}`, profile);
  }

  deleteProfile(id: number) {
    return this.http.delete(`${this.url}/${id}`);
  }

  updateProfile(profile: ICase) {
    return this.http.put(`${this.url}/${profile.id}`, profile);
  }

}
