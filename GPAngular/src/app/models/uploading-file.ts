import { HttpEvent } from "@angular/common/http";
import { Observable } from "rxjs";
import { ICaseFile } from "./case-file";

export interface IUploadingFile {
    file: File;
    fileObservable: Observable<HttpEvent<ICaseFile>>;
    fileProgress: number;
}