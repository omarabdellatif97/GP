import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';


@Component({
  selector: 'app-file',
  templateUrl: './file.component.html',
  styleUrls: ['./file.component.css']
})
export class FileComponent implements OnInit {

  constructor() { }

  @Input()
  value: number = 0;
  @Input()
  fileName: string = "";
  @Input()
  isUploaded: boolean = false;
  @Input()
  fileUrl: string = "";
  @Input()
  id: number = 0;

  @Output()
  fileRemoved: EventEmitter<number> = new EventEmitter<number>();

  removeFile(eve: Event) {
    this.fileRemoved.emit(this.id);
  }

  ngOnInit(): void {
  }

}
