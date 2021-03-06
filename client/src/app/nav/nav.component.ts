import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
  public user:User;
  constructor(private accountService: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
    this.currentUser$.subscribe(user => {
      this.user = user;
    })
  }

  login() {
      this.accountService.login(this.model).subscribe(result => {
          this.model = {};
          this.router.navigateByUrl("/members");
      });
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl("/");
  }

}
