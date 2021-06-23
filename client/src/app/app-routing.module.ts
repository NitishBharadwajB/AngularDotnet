import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MembersDetailComponent } from './members/members-detail/members-detail.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path:"",component: HomeComponent},
  {
    path:"",
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path:"messages",component: MessagesComponent, canActivate: [AuthGuard]},
      {path:"members",component: MembersListComponent},
      {path:"messages/:id",component: MembersDetailComponent},
      {path:"lists",component: ListsComponent}
      ]
  },
  
  {path:"**",component: HomeComponent, pathMatch: "full"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
