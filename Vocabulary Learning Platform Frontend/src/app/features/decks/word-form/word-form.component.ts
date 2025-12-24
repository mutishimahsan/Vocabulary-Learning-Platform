import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateWordDto, Word, WordService } from '../../../core/services/word.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-word-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './word-form.component.html',
  styleUrl: './word-form.component.css'
})
export class WordFormComponent {
  @Input() deckId!: string; // The deck to which word will be added
  @Output() wordCreated = new EventEmitter<Word>(); // Emit after successful creation

  wordForm!: FormGroup;
  submitting = false;

  constructor(private fb: FormBuilder, private wordService: WordService) {
    this.wordForm = this.fb.group({
      term: ['', Validators.required],
      definition: ['', Validators.required],
      exampleSentence: [''],
      partOfSpeech: [''],
      pronunciationUrl: ['']
    });
  }

  createWord(): void {
    if (this.wordForm.invalid || this.submitting) return;

    this.submitting = true;

    const wordData: CreateWordDto = this.wordForm.value;

    this.wordService.createWord(this.deckId, wordData).subscribe({
      next: (word) => {
        this.wordForm.reset();
        this.submitting = false;
        this.wordCreated.emit(word); // Notify parent
      },
      error: (err) => {
        console.error('Failed to create word', err);
        this.submitting = false;
      }
    });
  }
}
