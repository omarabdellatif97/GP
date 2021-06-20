import { HttpClient, HttpErrorResponse, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { catchError, tap } from "rxjs/operators"
import { AppConsts } from "../app-consts";
import { ITag } from "../models/tag";


@Injectable({
    providedIn: 'root'
})
export class TagService {
    url: string = `${AppConsts.apiUrl}/api/tags`;

    constructor(private http: HttpClient) { }

    getAllTags(): Observable<ITag[]> {
        return this.http.get<ITag[]>(this.url);
    }
}