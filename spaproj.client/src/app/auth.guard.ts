import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private httpClient: HttpClient, private router: Router) { }

  canActivate(): Observable<boolean> {
    return this.httpClient.get('https://localhost:7233/api/Auth/isLoggedIn', { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.message === 'User is logged in') {
            return true;
          } else {
            this.router.navigate(['/']);
            return false;
          }
        }),
        catchError(() => {
          this.router.navigate(['/']);
          return of(false);
        })
      );
  }
}


