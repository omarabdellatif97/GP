import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler
} from '@angular/common/http';
import { take, exhaustMap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    console.log("interceptor");
    const modifiedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer VY4D6JB.FANQ4CY-1Z442QZ-G57P0MT-XN8ZEJ6`
      }
    });
    return next.handle(modifiedReq);
  }
}
