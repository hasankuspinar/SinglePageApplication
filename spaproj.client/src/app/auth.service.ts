import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authStatusUrl = 'https://localhost:7233/api/Auth/isLoggedIn';

  constructor(private httpClient: HttpClient) { }

  isLoggedIn(): Observable<boolean> {
    return this.httpClient.get<{ message: string }>(this.authStatusUrl, { withCredentials: true })
      .pipe(
        map(response => true),
        
        map(() => true, () => false)
      );
  }
}
