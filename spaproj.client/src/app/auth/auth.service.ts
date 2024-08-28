import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpService } from './auth.httpservice';
interface Account {
  accountNumber: string;
  balance: number;
}
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private registerUrl = '/auth/register';
  private loginUrl = '/auth/login';
  private authStatusUrl = '/auth/isLoggedIn';
  private userInfoUrl = '/auth/UserInfo';
  private getDataUrl = '/auth/getData';
  private logoutUrl = '/auth/logout';
  private updateUserStatusUrl = '/auth/update-status';
  private getAccountsUrl = '/auth/getaccounts';

  constructor(private httpService: HttpService) { }

  isLoggedIn(): Observable<boolean> {
    return this.httpService.get<{ message: string }>(this.authStatusUrl, true)
      .pipe(
        map(response => response.message === "User is logged in")
      );
  }

  getUserRole(): Observable<string> {
    return this.httpService.get<{ role: string }>(this.userInfoUrl, true)
      .pipe(
        map(response => response.role)
      );
  }

  register(username: string, password: string, role: string): Observable<any> {
    const registerData = { username, password, role };
    return this.httpService.post(this.registerUrl, registerData);
  }

  login(username: string, password: string): Observable<any> {
    const loginData = { username, password };
    return this.httpService.post(this.loginUrl, loginData, true);
  }

  logout(): Observable<any> {
    return this.httpService.post(this.logoutUrl, {}, true);
  }

  userInfo(): Observable<{ username: string, role: string }> {
    return this.httpService.get<{ username: string, role: string }>(this.userInfoUrl, true);
  }

  updateUserStatus(username: string, status: number): Observable<any> {
    const statusUpdate = { username, newStatus: status };
    return this.httpService.put(this.updateUserStatusUrl, statusUpdate, true);
  }
  getAccounts(): Observable<Account[]> {
    return this.httpService.get<Account[]>(this.getAccountsUrl, true);
  }
}


