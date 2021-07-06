import { HttpClient } from '@angular/common/http';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators'
import { environment } from 'src/environments/environment';
import { User } from '../_models/User';
import { UserParams } from '../_models/userParams';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
   baseURL = environment.apiURL;
   private currentUserSource = new ReplaySubject<User>(1);
   currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model:any){
        return this.http.post(this.baseURL + 'account/login', model).pipe(
          map((response: User) => {
            const user = response;
            if(user){
              this.setCurrentUser(user);
            }
          })
        );
  }

  register(model:any){
    return this.http.post(this.baseURL + 'account/register', model).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
        }
        return user;
      })
    );
}

  setCurrentUser(user: User)
  {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;

    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  getDecodedToken(token){
    return JSON.parse(atob(token.split(".")[1]))
  }
 
}
