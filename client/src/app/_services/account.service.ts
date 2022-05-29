import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User, UserResponse } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.baseUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();
  users: User[] = [];

  constructor(private http: HttpClient) { }

  login(username: string, password: string) {
    let body = {
      userName: username,
      password: password
    }
    return this.http.post(this.baseUrl + "account/login", body).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  setCurrentUser(user: User) {
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

  getAllUsers() {
  return this.http.get(this.baseUrl + "account").pipe(
    map((users: UserResponse[]) => {
      let userArr: User[] = [];
      users.forEach(x => {
        let initials = x.firstName[0].toUpperCase() + x.lastName[0].toUpperCase();
        let userEntry: User = {
          userId: undefined,
          firstName: x.firstName,
          lastName: x.lastName,
          initials: initials,
          username: initials,
          token: undefined,
          roles: undefined
        }
        userArr.push(userEntry);
      })
      return userArr;
    }));
  }

  /*
  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if (user) {
          localStorage.setItem("user", JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }
  */

  getDecodedToken(token) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
