import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';

export interface Deck {
  id: string;
  title: string;
  description: string;
  category: string;
  ownerId: string;
  wordsCount?: number;
}

export interface CreateDeckDto {
  title: string;
  description: string;
  category: string;
}

interface ApiResponse<T> {
  success: boolean;
  message?: string;
  deck?: T;
  decks?: T[];
}

@Injectable({ 
  providedIn: 'root' 
})
export class DeckService {
  private apiUrl = 'https://localhost:7200/api/Deck'; // Fixed: 'deck' not 'decks'

  constructor(private http: HttpClient) {}

  // ✅ Get all decks
  getAll(): Observable<Deck[]> {
    return this.http
      .get<ApiResponse<Deck>>(this.apiUrl+'/get-deck')
      .pipe(map(res => res.decks ?? []));
  }

  // ✅ Create deck
  create(deck: CreateDeckDto): Observable<Deck> {
    return this.http
      .post<ApiResponse<Deck>>(this.apiUrl + '/create', deck)
      .pipe(map(res => res.deck!));
  }

  // ✅ Get deck by id
  getById(id: string): Observable<Deck> {
    return this.http
      .get<ApiResponse<Deck>>(`${this.apiUrl}/${id}`)
      .pipe(map(res => res.deck!));
  }

  // ✅ Update deck
  update(id: string, deck: CreateDeckDto): Observable<Deck> {
    return this.http
      .put<ApiResponse<Deck>>(`${this.apiUrl}/${id}`, deck)
      .pipe(map(res => res.deck!));
  }

  // ✅ Delete deck
  delete(id: string): Observable<void> {
    return this.http
      .delete<ApiResponse<null>>(`${this.apiUrl}/${id}`)
      .pipe(map(() => void 0));
  }
}