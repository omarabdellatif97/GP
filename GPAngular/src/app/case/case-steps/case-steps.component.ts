import { Component, OnInit, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { IStep } from 'src/app/models/step';

export const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CaseStepsComponent),
  multi: true
};

@Component({
  selector: 'app-case-steps',
  templateUrl: './case-steps.component.html',
  styleUrls: ['./case-steps.component.css'],
  providers: [CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR]
})
export class CaseStepsComponent implements OnInit, ControlValueAccessor {

  steps: IStep[] = [];
  newStep: string = "";
  propagateChange = (_: any) => { };

  constructor() { }

  writeValue(obj: any): void {
    if (obj !== undefined) {
      this.steps = obj;
    }
  }

  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any): void { }

  addStep(event: Event) {
    console.log(this.newStep);
    if (this.newStep != "") {
      this.steps.push({
        stepText: this.newStep
      });
      this.propagateChange(this.steps);
      this.newStep = "";
    }
  }
  removeStep(event: Event, stepId: number) {
    this.steps.splice(stepId, 1);
    this.propagateChange(this.steps);
  }
  stepedChanged(event: Event, stepId: number) {
    this.propagateChange(this.steps);
  }

  ngOnInit(): void { }
}
