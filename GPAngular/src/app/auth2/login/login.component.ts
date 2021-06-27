import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { Observable } from 'rxjs';
import { NotificationService } from 'src/app/services/notification-service.service';
import { AuthResponseData, AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  isLoginMode = true;
  isLoading = false;

  constructor(private authService: AuthService, private router: Router, private notify: NotificationService) { }
  ngOnInit(): void { }

  onSwitchMode() {
    this.isLoginMode = !this.isLoginMode;
  }

  onSubmit(form: NgForm) {
    if (!form.valid) {
      this.notify.show('All fields are required', 'close', {
        duration: 2000,
      });
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
        this.notify.show('Successfully Logged In', 'close', {
          duration: 2000,
        });
        this.router.navigate(['/cases']);
      },
      errorMessage => {
        this.notify.show('Failed to login', 'close', {
          duration: 2000,
        });
        this.isLoading = false;
      }
    );

    // form.reset();
  }
}
