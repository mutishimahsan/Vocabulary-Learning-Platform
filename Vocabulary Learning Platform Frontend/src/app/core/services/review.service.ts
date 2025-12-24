import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ReviewRequest {
  isCorrect: boolean;
}

export interface ReviewProgress {
  name: string;
  totalXP: number;
  level: number;
  streak: number;
  totalReviews: number;
  masteredWords: number;
  accuracy: number;
}

export interface LeaderboardEntry {
  name: string;
  totalXP: number;
  level: number;
  streak: number;
}

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private apiUrl = 'https://localhost:7200/api/review';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  // Get today's reviews
  getTodayReviews(): Observable<any[]> {
    const headers = this.getHeaders();
    return this.http.get<any[]>(`${this.apiUrl}/today`, { headers });
  }

  // Submit a review for a word
  submitReview(wordId: string, isCorrect: boolean): Observable<any> {
    const headers = this.getHeaders();
    const request: ReviewRequest = { isCorrect };
    return this.http.post<any>(
      `${this.apiUrl}/${wordId}`, 
      request, 
      { headers }
    );
  }

  // Get user progress
  getUserProgress(): Observable<ReviewProgress> {
    const headers = this.getHeaders();
    return this.http.get<ReviewProgress>(`${this.apiUrl}/progress`, { headers });
  }

  // Get leaderboard
  getLeaderboard(): Observable<LeaderboardEntry[]> {
    return this.http.get<LeaderboardEntry[]>(`${this.apiUrl}/leaderboard`);
  }
}