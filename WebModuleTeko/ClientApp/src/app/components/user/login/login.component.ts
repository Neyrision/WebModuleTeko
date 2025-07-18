import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';

@Component({
  imports: [
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    ReactiveFormsModule
  ],
  selector: 'app-login-component',
  styleUrls: ['./login.component.scss'],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  
  protected readonly formGroup = new FormGroup({
    email: new FormControl<string | null>(null, Validators.required),
    password: new FormControl<string | null>(null, Validators.required)
  });

  constructor(private readonly router: Router) {
  }

  protected onLoginClick(): void {
    this.formGroup.markAllAsTouched();

    if(this.formGroup.invalid) return;

    console.log(this.formGroup.getRawValue());
  }

  protected onRegisterClick(): void {
    this.router.navigate(['register']);
  }

}
