import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {ICase} from "src/app/models/case"
import { IStep } from "src/app/models/step";


@Component({
  selector: 'app-steps',
  templateUrl: './steps.component.html',
  styleUrls: ['./steps.component.css']
})
export class StepsComponent implements OnInit {

  @Input()
  caseSteps: IStep[] = [];
  @Output()
  caseStepsChange: EventEmitter<IStep[]> = new EventEmitter<IStep[]>();
  constructor() { }

  ngOnInit(): void {
  }


  }








