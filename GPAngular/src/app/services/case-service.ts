import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { catchError, tap } from "rxjs/operators"
import { AppConsts } from "../app-consts";
import { ICase } from "../models/case";


@Injectable({
  providedIn: 'root'
})
export class CaseService {
  url: string = `${AppConsts.apiUrl}/api/cases`;
  constructor(private http: HttpClient) { }

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
