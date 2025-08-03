import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { AuthenticationApiService } from '../../../../api/api-services';
import { AuthService } from '../../../services/auth.service';
import { MessageService } from '../../../services/message.service';

@Component({
  imports: [
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    ReactiveFormsModule,
  ],
  selector: 'app-login-component',
  styleUrls: ['./login.component.scss'],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  protected readonly formGroup = new FormGroup({
    username: new FormControl<string | null>(null, Validators.required),
    password: new FormControl<string | null>(null, Validators.required),
  });
  protected readonly tfaFormControl = new FormControl<string | null>(
    null,
    Validators.required
  );

  protected show2FaInput: boolean = false;

  constructor(
    private readonly messageService: MessageService,
    private readonly authService: AuthService,
    private readonly authenticationService: AuthenticationApiService,
    private readonly router: Router
  ) {}

  protected onLoginClick(): void {
    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) {
      this.messageService.show('Invalid input', 'error');
      return;
    }

    this.show2FaInput = true;
  }

  protected onCompleteLogin(): void {
    if (this.tfaFormControl.invalid) {
      this.messageService.show('Invalid input', 'error', -1);
      return;
    }

    const login = this.formGroup.getRawValue();

    this.authenticationService
      .getToken(login.username!, login.password!, this.tfaFormControl.getRawValue()!)
      .subscribe((authenticatedUser) => {
        this.authService.setToken(authenticatedUser);
        this.router.navigate(['/']);
      });
  }

  protected onBackClick(): void {
    this.tfaFormControl.setValue(null);
    this.show2FaInput = false;
  }

  protected onRegisterClick(): void {
    this.router.navigate(['register']);
  }
}
