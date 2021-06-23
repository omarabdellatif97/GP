import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestantComponent } from './testant.component';

describe('TestantComponent', () => {
  let component: TestantComponent;
  let fixture: ComponentFixture<TestantComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TestantComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TestantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
