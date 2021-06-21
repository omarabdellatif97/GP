import { Component, Input, OnInit } from '@angular/core';

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

  ngOnInit(): void {
  }

}