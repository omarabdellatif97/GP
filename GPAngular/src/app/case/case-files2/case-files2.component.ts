import { HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ICaseFile } from 'src/app/models/case-file';
import { IUploadingFile } from 'src/app/models/uploading-file';
import { FileService } from 'src/app/services/file-service.service';


interface HTMLInputEvent extends Event {
  target: HTMLInputElement & EventTarget;
}

@Component({
  selector: 'app-case-files2',
  templateUrl: './case-files2.component.html',
  styleUrls: ['./case-files2.component.css']
})
export class CaseFiles2Component implements OnInit {

  @ViewChild('file2', { static: false })
  file: any;
  addFiles() {

    this.file.nativeElement.click();
  }

  files: (ICaseFile | IUploadingFile)[] = []
  internalCaseFiles: ICaseFile[] = [];


  @Input()
  set caseFiles(_caseFiles: ICaseFile[]) {
    this.internalCaseFiles = _caseFiles;
    for (let i = 0; i < _caseFiles.length; i++) {
      this.files.push(_caseFiles[i]);
    }
  }

  @Output()
  caseFilesChange: EventEmitter<ICaseFile[]> = new EventEmitter<ICaseFile[]>();


  checkUploading(file: ICaseFile | IUploadingFile) {
    return 'file' in file;
  }
  getFileName(file: ICaseFile | IUploadingFile) {
    if ('file' in file) {
      return file.file.name;
    } else {
      return file.fileName;
    }
  }
  getFileURL(file: ICaseFile | IUploadingFile) {
    if ('file' in file) {
      return "";
    } else {
      return file.url;
    }
  }

  getProgress(file: ICaseFile | IUploadingFile): number {
    if ('fileProgress' in file) {
      return file.fileProgress;
    } else {
      return 0;
    }
  }


  onFilesChange(event: Event) {
    if (event.target instanceof HTMLInputElement && event.target.files) {
      let files = event.target.files;
      for (let i = 0; i < files.length; i++) {
        let obs = this.fileService.saveFilewithProgress(files[i]);

        let obj: IUploadingFile = {
          file: files[i],
          fileObservable: obs,
          fileProgress: 0
        };
        this.files.push(obj);

        obs.subscribe(event => {
          if (event.type == HttpEventType.UploadProgress) {
            if (event.total) {
              obj.fileProgress = Math.round((100 * event.loaded) / event.total);
            }
          }

          if (event.type === HttpEventType.Response) {
            if (event.body) {
              let caseFile: ICaseFile = event.body;
              let ind = this.files.indexOf(obj);
              if (ind >= 0) {
                this.files[ind] = caseFile;
              }
            }
          }
        })

      }
    }
  }

  constructor(private fileService: FileService) { }

  ngOnInit(): void {
  }

}
