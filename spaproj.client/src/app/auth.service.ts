import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private registerUrl = 'https://localhost:7233/api/Auth/register';
  private loginUrl = 'https://localhost:7233/api/Auth/login';
  private authStatusUrl = 'https://localhost:7233/api/Auth/isLoggedIn';
  private authUserInfoUrl = 'https://localhost:7233/api/Auth/UserInfo';
  private getDataUrl = 'https://localhost:7233/api/Auth/getData';

  constructor(private httpClient: HttpClient) { }

  isLoggedIn(): Observable<boolean> {
    return this.httpClient.get<{ message: string }>(this.authStatusUrl, { withCredentials: true })
      .pipe(
        map(response => response.message === "User is logged in")
      );
  }
  getUserRole(): Observable<string> {
    return this.httpClient.get<{ role: string }>(this.authUserInfoUrl, { withCredentials: true }).pipe(
      map(response => response.role)
    );
  }
  register(username: string, password: string, role: string): Observable<any> {
    const registerData = { username, password, role };
    return this.httpClient.post(this.registerUrl, registerData);
  }

  login(username: string, password: string): Observable<any> {
    const loginData = { username, password };
    return this.httpClient.post(this.loginUrl, loginData, { withCredentials: true });
  }
}
