import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { environment } from 'src/environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { LoginModalComponent } from './modals/login-modal/login-modal.component';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'CW Controls';
  contentTitle: string;
  bsModalRef: BsModalRef;
  currentUser: string;

  constructor(public accountService: AccountService,
    private modalService: BsModalService) {}

  ngOnInit(): void {
    this.setCurrentUser();
  }

  switchUser(user) {
    this.bsModalRef = this.modalService.show(LoginModalComponent);
  }

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user'));

    if (user) {
      this.accountService.setCurrentUser(user);
      this.currentUser = user.firstName;
    }
  }

  
}
