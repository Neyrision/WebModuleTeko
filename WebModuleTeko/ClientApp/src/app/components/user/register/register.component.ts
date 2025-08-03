import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { AuthenticationApiService } from '../../../../api/api-services';
import { MessageService } from '../../../services/message.service';

@Component({
  imports: [
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    ReactiveFormsModule,
  ],
  selector: 'app-register-component',
  styleUrls: ['./register.component.scss'],
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  protected readonly formGroup = new FormGroup({
    email: new FormControl<string | null>(null, [
      Validators.required,
      Validators.email,
    ]),
    username: new FormControl<string | null>(null, [
      Validators.required,
      Validators.maxLength(32),
    ]),
    password: new FormControl<string | null>(null, [
      Validators.required,
      Validators.maxLength(100),
    ]),
    repeatedPassword: new FormControl<string | null>(null, Validators.required),
  });

  constructor(
    private readonly router: Router,
    private readonly messageService: MessageService,
    private readonly authenticationService: AuthenticationApiService
  ) {}

  protected onLoginClick(): void {
    this.router.navigate(['login']);
  }

  protected onRegisterClick(): void {
    const passwordRepeatValidator = Validators.pattern(
      this.formGroup.controls.password.value ?? ''
    );
    this.formGroup.controls.repeatedPassword.setValidators([
      passwordRepeatValidator,
      Validators.required,
    ]);

    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) {
      this.messageService.show('Invalid input', 'error');
      return;
    }

    const model = this.formGroup.getRawValue();
    this.authenticationService
      .registerNewUser({
        email: model.email!,
        password: model.password!,
        username: model.username!,
      })
      .subscribe({
        next: (tfaModel) => {
          this.router.navigate(['2fa'], { state: tfaModel });
        },
        error: (error) => {
          this.messageService.show('Couldn\'t register user. User already exists or service is temporary unavailable. Please try again later.', 'error');
        }
      });
  }
}
