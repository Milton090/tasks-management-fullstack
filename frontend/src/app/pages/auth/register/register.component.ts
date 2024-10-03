import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Auth } from '../../../interfaces';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-register',
	standalone: true,
	imports: [
		ReactiveFormsModule, 
		RouterModule, 
		MatInputModule,
		CommonModule,
	],
	templateUrl: './register.component.html',
	styleUrls: ['./register.component.css']
})
export class RegisterComponent {

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

	register() {
		const user: Auth = {
			email: this.userData.value.email,
			password: this.userData.value.password
		};

		this.api.register(user).subscribe({
			next: () => {
				this.router.navigate(['/login']);
			},
		});
	}
}
