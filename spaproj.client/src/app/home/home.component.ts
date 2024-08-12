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
  

  ngOnInit(): void {
    
  }



  title = 'spaproj.client';
}

