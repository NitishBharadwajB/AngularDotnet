import { Component, OnInit } from '@angular/core';
import { LikedParams } from '../_models/LikesParams';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members: Partial<Member[]>;
  likedParams = new LikedParams('liked');
  pagination: Pagination;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes(){
    this.memberService.getLikes(this.likedParams).subscribe(members => {
      this.members = members.result;
      this.pagination = members.pagination;
    })
  }

  pageChanged(event: any){
    this.likedParams.pageNumber = event.page;
    this.loadLikes();
  }

}
