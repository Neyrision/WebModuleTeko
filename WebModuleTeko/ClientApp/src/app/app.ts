import { MatToolbarModule } from '@angular/material/toolbar';
import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    MatToolbarModule,
    MatSidenavModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatListModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  
  protected title = 'WebModuleTeko';
  protected showMenu = false;

  constructor(private readonly router: Router) {
  }


  protected onHomeClick(): void {
    this.router.navigate(['/']);
  }
  protected onClick(): void {
    this.router.navigate(['login']);
  }

}
