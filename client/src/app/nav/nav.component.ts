import { Component, OnInit } from '@angular/core';
import { error } from 'protractor';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  public model:any = {};
  public currentUser$: Observable<User>;
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
  }

  login() {
      this.accountService.login(this.model).subscribe(result => {
          this.model = {};
      }, error => {
        console.log(error);
      });
  }

  logout(){
    this.accountService.logout();
  }

}
