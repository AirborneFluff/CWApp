import { Component, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-login-modal',
  templateUrl: './login-modal.component.html',
  styleUrls: ['./login-modal.component.css']
})
export class LoginModalComponent implements OnInit {
  users: User[] = [];

  constructor(private modalService: BsModalService, public accountService: AccountService) { }

  ngOnInit(): void {
    this.accountService.getAllUsers().subscribe(x => {
      this.users = x;
    });
  }

  login(user: User) {
    this.accountService.login(user.username, "0314").subscribe(() => {
      this.modalService.hide();
    });
  }
}


