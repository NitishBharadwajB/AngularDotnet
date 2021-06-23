import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Angular App';
  users: any;
/**
 *
 */
constructor(private http: HttpClient) {
}
  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(){
    this.http.get('http://localhost:5000/api/Users').subscribe(response => {
      this.users = response;
      console.log(this.users);
    },error => {
      console.error(error);
    }
    );
  }

}
