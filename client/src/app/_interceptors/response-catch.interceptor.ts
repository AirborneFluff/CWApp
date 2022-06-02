import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ResponseCatchInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService) {}
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    /*
    return next.handle(request).pipe(map(resp => resp), catchError<Http>(error => {
      console.log(error)
      if (error?.status === 0) {
        this.toastr.warning("Server not responding")
      }
      if (error?.status === 401) {
        this.toastr.warning("Unauthorized, try logging in")
      }
      return thor
    }))
  */
    return next.handle(request);
  }
}
