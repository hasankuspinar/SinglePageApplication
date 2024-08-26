import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from './auth.service';
import { map, catchError, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.authService.isLoggedIn().pipe(
      switchMap(isLoggedIn => {
        if (!isLoggedIn) {
          this.router.navigate(['/login']);
          return of(false);
        }
        if (route.data['roles'] && route.data['roles'].includes('admin')) {
          return this.authService.getUserRole().pipe(
            map(role => {
              if (role === 'admin') {
                return true;
              } else {
                this.router.navigate(['/error']); 
                return false;
              }
            }),
            catchError(() => {
              this.router.navigate(['/error']);
              return of(false);
            })
          );
        }
        return of(true);
      }),
      catchError(() => {
        this.router.navigate(['/login']);
        return of(false);
      })
    );
  }
}


