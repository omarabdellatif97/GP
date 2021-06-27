import { Component, OnInit, forwardRef, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatTable, MatTableModule } from '@angular/material/table';
import { IStep } from 'src/app/models/step';

const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
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

  @ViewChild('mytable') mytable!: MatTable<IStep>;
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
    if (this.newStep != "") {
      this.steps.push({
        stepText: this.newStep
      });
      this.propagateChange(this.steps);
      this.newStep = "";
      this.mytable?.renderRows();
    }
    event.preventDefault();
  }
  removeStep(event: Event, stepId: number) {
    console.log(stepId);
    this.steps.splice(stepId, 1);
    this.mytable?.renderRows();
    this.propagateChange(this.steps);

  }
  stepedChanged(stepId: number) {
    this.propagateChange(this.steps);
  }

  // addStep2(eve: KeyboardEvent) {
  //   if (eve.key === 'Enter' || eve.code === 'Enter') {
  //     if (this.newStep != "") {
  //       this.steps.push({
  //         stepText: this.newStep
  //       });
  //       this.propagateChange(this.steps);
  //       this.newStep = "";
  //     }
  //     eve.preventDefault();
  //   }
  // }

  ngOnInit(): void { }
}
