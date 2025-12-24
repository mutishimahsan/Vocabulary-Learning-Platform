import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Deck, DeckService } from '../../core/services/deck.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-decks',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './decks.component.html',
  styleUrl: './decks.component.css'
})
export class DecksComponent {
  decks: Deck[] = [];
  showForm = false;
  deckForm!: FormGroup;

  
  constructor(
    private deckService: DeckService,
    private fb: FormBuilder,
    private router: Router
  ) {}
  
  ngOnInit(): void {
    this.deckForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      category: ['']
    });
    this.loadDecks();
  }

  loadDecks(): void {
    this.deckService.getAll().subscribe({
      next: decks => this.decks = decks,
      error: err => console.error('Failed to load decks', err)
    });
  }

  toggleForm() {
    this.showForm = !this.showForm;
  }

  createDeck(): void {
    if (this.deckForm.invalid) return;

    this.deckService.create(this.deckForm.value).subscribe({
      next: () => {
        this.deckForm.reset();
        this.showForm = false;
        this.loadDecks();
      }
    });
  }

  deleteDeck(id: string): void {
    if (!confirm('Delete this deck?')) return;

    this.deckService.delete(id).subscribe({
      next: () => this.loadDecks()
    });
  }

  // ✅ navigate to deck words page
  openDeck(deckId: string): void {
    this.router.navigate(['/decks', deckId]);
  }
}
