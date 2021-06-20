import { HttpClient, HttpErrorResponse, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { catchError, tap } from "rxjs/operators"
import { AppConsts } from "../app-consts";
import { IApplication } from "../models/application";
import { ICase } from "../models/case";
import { ITag } from "../models/tag";


@Injectable({
    providedIn: 'root'
})
export class ApplicationService {
    url: string = `${AppConsts.apiUrl}/api/apps`;

    constructor(private http: HttpClient) { }

    getAllApps(): Observable<IApplication[]> {
        return this.http.get<IApplication[]>(this.url);
    }
}