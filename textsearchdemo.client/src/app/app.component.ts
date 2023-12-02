import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  public searchResult: string = "";

  constructor(private http: HttpClient) {}

  search(text: string) {
    if (!text) {
      this.searchResult = "";
      return;
    }

    this.http.get<string>('/search?text='+text).subscribe(
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
