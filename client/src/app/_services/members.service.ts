import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
   baseURL = environment.apiURL;


  constructor(private http: HttpClient) { }

  getMembers(){
    return this.http.get<Member[]>(this.baseURL + 'users');
  }

  getMember(userName: string){
    return this.http.get<Member>(this.baseURL + 'users/' + userName);
  }

  updateMember(member: Member){
    return this.http.put(this.baseURL + 'users' , member);  
  }

}
