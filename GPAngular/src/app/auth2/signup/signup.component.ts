import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { NotificationService } from 'src/app/services/notification-service.service';
import { AuthResponseData, AuthService } from '../auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {


  isLoading = false;

  constructor(private authService: AuthService, private router: Router, private notify: NotificationService) { }
  ngOnInit(): void { }



  onSubmit(form: NgForm) {
    if (!form.valid) {
      this.notify.show('All fields are required', 'close', {
        duration: 2000,
      });
      return;
    }

    const email = form.value.email;
    const userName = form.value.username;
    const password = form.value.password;
    const confirmPassword = form.value.confirmPassword;

    if (password != confirmPassword) {
      this.notify.show("Passwords Don't Match", 'close', {
        duration: 2000,
      });
      return;
    }

    let authObs: Observable<AuthResponseData>;

    this.isLoading = true;
    authObs = this.authService.signup(email, userName, password, confirmPassword);


    authObs.subscribe(
      resData => {
        this.isLoading = false;
        this.notify.show("Successfully Registered Please Login", 'close', {
          duration: 2000,
        });
      },
      errorMessage => {
        this.notify.show(errorMessage ? errorMessage : "Failed to register", 'close', {
          duration: 2000,
        });
        this.isLoading = false;
      }
    );
  }
}
