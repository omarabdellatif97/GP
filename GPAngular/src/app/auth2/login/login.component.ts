import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { Observable } from 'rxjs';
import { AuthResponseData, AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  isLoginMode = true;
  isLoading = false;

  constructor(private authService: AuthService, private router: Router, private notifier: NotifierService) { }
  ngOnInit(): void { }

  onSwitchMode() {
    this.isLoginMode = !this.isLoginMode;
  }

  onSubmit(form: NgForm) {
    if (!form.valid) {
      this.notifier.notify('warning', 'All fields are required')
      return;
    }

    const email = form.value.email;
    const password = form.value.password;

    let authObs: Observable<AuthResponseData>;
    this.isLoading = true;

    authObs = this.authService.login(email, password);

    authObs.subscribe(
      resData => {
        this.isLoading = false;
        this.notifier.notify('success', "Successfully Logged In")
        this.router.navigate(['/cases']);
      },
      errorMessage => {
        this.notifier.notify('error', errorMessage ? errorMessage : "Failed to login")
        this.isLoading = false;
      }
    );

    // form.reset();
  }
}
