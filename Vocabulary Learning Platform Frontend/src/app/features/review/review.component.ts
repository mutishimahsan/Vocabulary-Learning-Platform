import { animate, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { ReviewService } from '../../core/services/review.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-review',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './review.component.html',
  styleUrl: './review.component.css',
  animations: [
    trigger('flip', [
      transition(':increment', [
        animate('300ms ease-in', style({ transform: 'rotateY(180deg)' }))
      ])
    ])
  ]
})
export class ReviewComponent {
  words: any[] = [];
  index = 0;
  flipped = false;

  constructor(private reviewService: ReviewService) {}

  ngOnInit() {
    this.reviewService.getTodayReviews().subscribe((res: any[]) => this.words = res);
  }

  submit(isCorrect: boolean) {
    const word = this.words[this.index];
    this.reviewService.submitReview(word.id, isCorrect).subscribe(() => {
      this.index++;
      this.flipped = false;
    });
  }
}
