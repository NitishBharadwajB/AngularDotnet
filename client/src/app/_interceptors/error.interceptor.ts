import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpClient
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private route: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if(error)
        {
          switch(error.status){
            case 400:
            if(error.error.errors)
            {
              //StateModalErrors is called as Validation type of errors in ASP.NET Errors
              const StateModalError = [];
              for(const key in error.error.errors)
              {
                if(error.error.errors[key]){
                  StateModalError.push(error.error.errors[key]);
                }
              }
              throw StateModalError.flat();
            } else if(typeof(error.errrs) === 'object'){
              this.toastr.error(error.statusText, error.status);
            } else{
              this.toastr.error(error.error, error.status);
            }
            break;
            case 401:
              this.toastr.error(error.statusText, error.status);
              break;
            case 404:
              this.route.navigateByUrl('/not-found');
              break;
            case 500:
              const navigateByExtras: NavigationExtras =  {state:{error:error.error}};
              this.route.navigateByUrl('/server-error', navigateByExtras);
              break;
            default :
              this.toastr.error('Something unexpected went wrong');
              console.log(error);
              break;

        }
      }
      return throwError(error);
      })
    )
  }
}
