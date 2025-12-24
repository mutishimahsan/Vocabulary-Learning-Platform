import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, forkJoin, map } from 'rxjs';

export interface Word {
  id: string;
  deckId: string;
  term: string;
  definition: string;
  exampleSentence: string;
  partOfSpeech: string;
  pronunciationUrl: string;
}

export interface CreateWordDto {
  term: string;
  definition: string;
  exampleSentence: string;
  partOfSpeech: string;
  pronunciationUrl: string;
}

interface ApiResponse<T> {
  success: boolean;
  message?: string;
  word?: T;
  words?: T[];
}

@Injectable({
  providedIn: 'root'
})
export class WordService {
  private apiUrl = 'https://localhost:7200/api/word';

  constructor(private http: HttpClient) { }

  // ===============================
  // Create a new word in a deck
  // ===============================
  createWord(deckId: string, wordData: CreateWordDto): Observable<Word> {
    return this.http
      .post<ApiResponse<Word>>(`${this.apiUrl}/${deckId}`, wordData)
      .pipe(map(res => res.word!));
  }

  // ===============================
  // Get all words in a deck
  // ===============================
  getWordsByDeck(deckId: string): Observable<Word[]> {
    return this.http.get<ApiResponse<Word>>(`${this.apiUrl}/deck/${deckId}`)
      .pipe(
        map(res => (res.words ?? []).map(w => ({
          ...w,
          partOfSpeech: (w as any).partsOfSpeech // in case API uses PascalCase
        })))
      );
  }

  // ===============================
  // Get a specific word by ID
  // ===============================
  getWordById(wordId: string): Observable<Word> {
    return this.http
      .get<ApiResponse<Word>>(`${this.apiUrl}/${wordId}`)
      .pipe(map(res => res.word!));
  }

  // ===============================
  // Update a word
  // ===============================
  updateWord(wordId: string, wordData: CreateWordDto): Observable<Word> {
    return this.http
      .put<ApiResponse<Word>>(`${this.apiUrl}/${wordId}`, wordData)
      .pipe(map(res => res.word!));
  }

  // ===============================
  // Delete a word
  // ===============================
  deleteWord(wordId: string): Observable<void> {
    return this.http
      .delete<ApiResponse<null>>(`${this.apiUrl}/${wordId}`)
      .pipe(map(() => void 0));
  }

  // ===============================
  // FIXED: Batch create words using forkJoin
  // ===============================
  createWordsBatch(deckId: string, words: CreateWordDto[]): Observable<Word[]> {
    const wordObservables = words.map(word => this.createWord(deckId, word));
    return forkJoin(wordObservables);
  }

  // ===============================
  // Alternative: Sequential batch creation
  // ===============================
  createWordsSequentially(deckId: string, words: CreateWordDto[]): Observable<Word[]> {
    return new Observable<Word[]>(observer => {
      const createdWords: Word[] = [];
      let index = 0;

      const createNext = () => {
        if (index >= words.length) {
          observer.next(createdWords);
          observer.complete();
          return;
        }

        this.createWord(deckId, words[index]).subscribe({
          next: word => {
            createdWords.push(word);
            index++;
            createNext();
          },
          error: err => observer.error(err)
        });
      };

      createNext();
    });
  }

  // ===============================
  // Search words in a deck (if you implement this endpoint)
  // ===============================
  searchWords(deckId: string, searchTerm: string): Observable<Word[]> {
    return this.http
      .get<ApiResponse<Word>>(`${this.apiUrl}/deck/${deckId}/search?term=${searchTerm}`)
      .pipe(map(res => res.words ?? []));
  }
}