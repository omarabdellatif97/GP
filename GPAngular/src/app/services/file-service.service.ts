import { HttpClient, HttpErrorResponse, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { catchError, tap } from "rxjs/operators"
import { AppConsts } from "../app-consts";
import { ICaseFile } from "../models/case-file";



@Injectable({
    providedIn: 'root'
})
export class FileService {

    url: string = `${AppConsts.fileUploadURL}`;

    constructor(private http: HttpClient) { }

    saveFiles(filesToUpload: File[]): Observable<HttpEvent<ICaseFile[]>> {
        const formData = new FormData();
        filesToUpload.forEach((file, index) => {
            formData.append('file' + index, file, file.name);
        });
        return this.http.post<ICaseFile[]>(this.url, formData, { reportProgress: true, observe: 'events' });
    }

    saveFile(fileToUpload: any): Observable<ICaseFile> {
        const formData = new FormData();
        formData.append('file', fileToUpload.blob(), fileToUpload.filename());
        return this.http.post<ICaseFile>(this.url, formData);
    }

    saveFilewithProgress(fileToUpload: File) {
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
        return this.http.post<ICaseFile>(this.url, formData, { reportProgress: true, observe: "events", });
    }
}
