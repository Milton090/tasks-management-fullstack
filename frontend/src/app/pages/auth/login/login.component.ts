import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Auth } from '../../../interfaces';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-login',
	standalone: true,
	imports: [
		ReactiveFormsModule,
		RouterModule,
		MatInputModule,
		CommonModule
	],
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent {

	userData: FormGroup;

	constructor(
		private fb: FormBuilder,
		private api: AuthService,
		private router: Router
	) {
		this.userData = this.fb.group({
			email: ['', Validators.required],
			password: ['', Validators.required]
		});
	}


	login() {
		console.log(this.userData.valid);
		const user: Auth = {
			email: this.userData.value.email,
			password: this.userData.value.password
		};

		this.api.login(user).subscribe({
			next: () => {
				this.router.navigate(['/tasks']);
			},
		});
	}
}
