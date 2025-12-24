import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {
    path: '',
    loadComponent: () =>
      import('./features/dashboard/dashboard.component')
        .then(m => m.DashboardComponent),
    canActivate: [authGuard]
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/dashboard.component')
        .then(m => m.DashboardComponent),
    canActivate: [authGuard]
  },
  {
    path: 'decks',
    loadComponent: () =>
      import('./features/decks/decks.component')
        .then(m => m.DecksComponent),
    canActivate: [authGuard]
  },
  {
    path: 'decks/:id',
    loadComponent: () =>
      import('./features/decks/deck-words/deck-words.component')
        .then(m => m.DeckWordsComponent),
    canActivate: [authGuard]
  },
  {
    path: 'analytics',
    loadComponent: () =>
      import('./features/analytics/analytics.component')
        .then(m => m.AnalyticsComponent),
    canActivate: [authGuard]
  },
  {
    path: 'review',
    loadComponent: () =>
      import('./features/review/review.component')
        .then(m => m.ReviewComponent),
    canActivate: [authGuard]
  },
  {
    path: 'leaderboard',
    loadComponent: () =>
      import('./features/leaderboard/leaderboard.component')
        .then(m => m.LeaderboardComponent)
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component')
        .then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register.component')
        .then(m => m.RegisterComponent)
  }
];
