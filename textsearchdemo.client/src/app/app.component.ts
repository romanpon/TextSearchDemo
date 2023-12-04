import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

interface Result {
  entityType: string,
  value: string,
  propertyName: string,
  weith: number,
  entity: [key: string]
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent {
  public searchResult: Result[] = {} as Result[];

  constructor(private http: HttpClient) {}

  search(text: string) {
    if (!text) {
      this.searchResult = {} as Result[];
      return;
    }

    this.http.get<Result[]>('/search?text='+text).subscribe(
      (result) => {
        this.searchResult = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  title = 'textsearchdemo.client';
}
