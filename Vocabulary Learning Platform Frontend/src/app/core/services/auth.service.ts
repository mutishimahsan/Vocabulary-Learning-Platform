import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { tap } from "rxjs";

@Injectable({ providedIn: 'root' })
export class AuthService {
  private api = 'https://localhost:7200/api/auth';

  constructor(private http: HttpClient) {}

  login(data: { email: string; password: string }) {
    return this.http.post<{ token: string }>(`${this.api}/login`, data)
      .pipe(tap(res => localStorage.setItem('token', res.token)));
  }

  register(data: any) {
    return this.http.post(`${this.api}/register`, data);
  }

  logout() {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getUserId(): string {
  const token = localStorage.getItem('token');
  if (!token) return '';

  const payload = JSON.parse(atob(token.split('.')[1]));
  return payload.nameid;
}

}
