import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';



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

  ngOnInit(): void {
  }

  

  title = 'spaproj.client';
}
