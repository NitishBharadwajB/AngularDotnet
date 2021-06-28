import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
   baseURL = environment.apiURL;
   members: Member[] = []


  constructor(private http: HttpClient) { }

  getMembers(){
    if(this.members.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseURL + 'users').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    );
  }

  getMember(userName: string){
    const member = this.members.find(x => x.username === userName);
    if(member && member !== undefined) return of(member);
    return this.http.get<Member>(this.baseURL + 'users/' + userName);
  }

  updateMember(member: Member){
    return this.http.put(this.baseURL + 'users' , member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );  
  }

  setMainPhoto(photoId: number){
    return this.http.put(this.baseURL + 'users/set-main-photo/' + photoId, {})
  }
  deletePhoto(photoId: number){
    return this.http.delete(this.baseURL + 'users/delete-photo/' + photoId)
  }

}
