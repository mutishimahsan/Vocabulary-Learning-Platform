import { Component } from '@angular/core';
import { Word, WordService } from '../../../core/services/word.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { WordFormComponent } from "../word-form/word-form.component";

@Component({
  selector: 'app-deck-words',
  standalone: true,
  imports: [CommonModule, WordFormComponent],
  templateUrl: './deck-words.component.html',
  styleUrl: './deck-words.component.css'
})
export class DeckWordsComponent {
  deckId!: string;
  words: Word[] = [];

  constructor(
    private route: ActivatedRoute,
    private wordService: WordService
  ) {}

  ngOnInit(): void {
    this.deckId = this.route.snapshot.paramMap.get('id')!;
    this.loadWords();
  }

  loadWords(): void {
    this.wordService.getWordsByDeck(this.deckId).subscribe({
      next: words => this.words = words,
      error: err => console.error('Failed to load words', err)
    });
  }
}
