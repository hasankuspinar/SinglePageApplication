import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


/*interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getForecasts();
  }

  getForecasts() {
    this.http.get<WeatherForecast[]>('/weatherforecast').subscribe(
      (result) => {
        this.forecasts = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  title = 'spaproj.client';
}*/

interface Person {
  id: number;
  name: string;
  surname: string;
  dob: Date;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  people: any = [];
  constructor(private httpClient: HttpClient) { }
  fetchPeople(): void {
    this.httpClient.get<Person[]>('https://localhost:7233/api/Person').subscribe({
      next: (data) => {
        this.people = data;
      },
      error: (error) => console.error('Failed to fetch people:', error)
    });

  }

  ngOnInit(): void {
    this.fetchPeople();
  }

  

  title = 'spaproj.client';
}
