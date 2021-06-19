import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestQueryParamsComponent } from './test-query-params.component';

describe('TestQueryParamsComponent', () => {
  let component: TestQueryParamsComponent;
  let fixture: ComponentFixture<TestQueryParamsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TestQueryParamsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TestQueryParamsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
