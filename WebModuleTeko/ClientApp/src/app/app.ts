import { MatToolbarModule } from '@angular/material/toolbar';
import {
  Component,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { AuthService } from './services/auth.service';
import { ReplaySubject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    MatToolbarModule,
    MatSidenavModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App implements OnInit, OnDestroy {
  private destroyed$: ReplaySubject<boolean> = new ReplaySubject(1);

  protected title = 'WebModuleTeko';
  protected showMenu = false;
  protected isAuthenticated = false;

  constructor(
    private readonly router: Router,
    private readonly authService: AuthService
  ) {}

  ngOnInit(): void {
    this.authService.authenticationChanged
      .pipe(takeUntil(this.destroyed$))
      .subscribe((_) => {
        this.isAuthenticated = this.authService.isAuthenticated();
      });
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }

  protected getDisplayName(): string {
    return this.authService.getUsername() ?? 'unknown';
  }

  protected onOpenProfile(): void {
    this.router.navigate(['profile']);
  }

  protected onHomeClick(): void {
    this.router.navigate(['/']);
  }

  protected onLogout(): void {
    this.authService.clearToken();
    this.router.navigate(['/']);
    window.location.reload();
  }

  protected onLogin(): void {
    this.router.navigate(['login']);
  }
}
