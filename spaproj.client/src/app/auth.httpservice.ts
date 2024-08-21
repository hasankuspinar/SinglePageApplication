import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  private baseUrl = '/api';

  constructor(private httpClient: HttpClient) { }

  getBaseUrl(): string {
    return this.baseUrl;
  }

  get<T>(url: string, withCredentials: boolean = false): Observable<T> {
    return this.httpClient.get<T>(`${this.baseUrl}${url}`, {  withCredentials });
  }

  post(url: string, body: any, withCredentials: boolean = false): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}${url}`, body, { withCredentials });
  }

  put(url: string, body: any, withCredentials: boolean = false): Observable<any> {
    return this.httpClient.put(`${this.baseUrl}${url}`, body, { withCredentials });
  }
}

