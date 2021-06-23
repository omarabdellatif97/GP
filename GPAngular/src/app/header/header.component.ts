import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {
  constructor(
    public _AuthService:AuthService
  ) { }

  ngOnInit() {
  }

  //IsAuth = localStorage.getItem('token');

}
