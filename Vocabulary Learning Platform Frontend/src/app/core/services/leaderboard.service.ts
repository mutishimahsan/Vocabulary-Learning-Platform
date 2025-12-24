import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LeaderboardService {

  private apiUrl = 'https://localhost:7200/api/review/leaderboard';

  constructor(private http: HttpClient) {}

  // ===============================
  // GET: /api/review/leaderboard
  // ===============================
  getLeaderboard(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
