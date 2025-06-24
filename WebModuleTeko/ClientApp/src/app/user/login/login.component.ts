import { Component } from '@angular/core';
import { MatFormFieldModule, MatInputModule } from '@angular/material';

@Component({
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule
  ],
  selector: 'app-login-component',
  styleUrls: ['./login.component.scss'],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  
}
