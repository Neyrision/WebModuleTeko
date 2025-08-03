import { Injectable } from '@angular/core';
import { AuthenticatedUserModel } from '../../api/api-services';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  protected readonly tokenKey = 'auth_user';

  authenticationChanged = new ReplaySubject();

  constructor() {
    this.authenticationChanged.next(null);
  }

  setToken(authenticatedUser: AuthenticatedUserModel): void {
    localStorage.setItem(this.tokenKey, JSON.stringify(authenticatedUser));
    this.authenticationChanged.next(null);
  }

  clearToken(): void {
    localStorage.removeItem(this.tokenKey);
    this.authenticationChanged.next(null);
  }

  getToken(): string | null {
    return this.getAuthenticatedUser()?.token ?? null;
  }

  getUsername(): string | null {
    return this.getAuthenticatedUser()?.username ?? null;
  }

  isAuthenticated(): boolean {
    return this.getToken() != null;
  }

  protected getAuthenticatedUser(): AuthenticatedUserModel | null {
    const json = localStorage.getItem(this.tokenKey);

    if (json == null) return null;

    return (JSON.parse(json) as AuthenticatedUserModel) ?? null;
  }
}
