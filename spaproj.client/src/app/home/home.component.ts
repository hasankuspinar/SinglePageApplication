import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface Person {
  id: number;
  name: string;
  surname: string;
  dob: Date;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
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

