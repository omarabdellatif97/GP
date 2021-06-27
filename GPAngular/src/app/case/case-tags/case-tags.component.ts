import { COMMA, ENTER, TAB } from '@angular/cdk/keycodes';
import { Component, ElementRef, forwardRef, Input, OnInit, ViewChild } from '@angular/core';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { Observable } from 'rxjs';
import { filter, map, startWith } from 'rxjs/operators';
import { ITag } from 'src/app/models/tag';

const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CaseTagsComponent),
  multi: true
};


@Component({
  selector: 'app-case-tags',
  templateUrl: './case-tags.component.html',
  styleUrls: ['./case-tags.component.css'],
  providers: [CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR]
})
export class CaseTagsComponent implements OnInit, ControlValueAccessor {

  tags: ITag[] = [];
  propagateChange = (_: any) => { };
  @Input() allTags: ITag[] = [];
  @Input() readonly: boolean = false;
  separatorKeysCodes: number[] = [ENTER, COMMA, TAB];
  tagCtrl = new FormControl();
  filteredTags: Observable<ITag[]>;
  @ViewChild('tagInput') tagInput!: ElementRef<HTMLInputElement>;

  constructor() {
    this.filteredTags = this.tagCtrl.valueChanges.pipe(
      startWith(null),
      filter(x => typeof x === 'string'),
      map((tag: string | null) => tag ? this._filter(tag) : this.allTags.slice(0, 5)));
  }

  ngOnInit(): void { }

  writeValue(obj: any): void {
    if (obj !== undefined) {
      this.tags = obj;
    }
  }
  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }
  registerOnTouched(fn: any): void { }



  private _filter(value: string): ITag[] {
    let filterValue = value.toLowerCase();
    return this.allTags.filter(tag => {
      return tag.name.toLowerCase().includes(filterValue)
        && !this.tags.map(t => t.name.toLowerCase()).includes(tag.name.toLowerCase())
    }).slice(0, 5);
  }

  addTag(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (value) {
      this.tags.push({ name: value });
      this.propagateChange(this.tags);
    }
    event.chipInput!.clear();
    this.tagCtrl.setValue(null);
  }

  removeTag(tag: ITag): void {
    const index = this.tags.indexOf(tag);
    if (index >= 0) {
      this.tags.splice(index, 1);
      this.propagateChange(this.tags);
    }
  }

  selectTag(event: MatAutocompleteSelectedEvent): void {
    this.tags.push(event.option.value);
    this.propagateChange(this.tags);
    this.tagInput.nativeElement.value = '';
    this.tagCtrl.setValue(null);
  }
}
