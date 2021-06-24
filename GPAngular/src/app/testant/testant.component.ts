import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-testant',
  templateUrl: './testant.component.html',
  styleUrls: ['./testant.component.css']
})
export class TestantComponent implements OnInit {

  listOfOption: Array<{ label: string; value: string }> = [];
  listOfTagOptions = [];

  ngOnInit(): void {
    const children: Array<{ label: string; value: string }> = [];
    for (let i = 10; i < 36; i++) {
      children.push({ label: i.toString(36) + i, value: i.toString(36) + i });
    }
    this.listOfOption = children;
  }

}

