import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterModule } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { CommonModule } from '@angular/common';
import { AlertService } from '../../services/alert.service';
import { SweetAlertResult } from 'sweetalert2';
import { AuthService } from '../../services/auth.service';


@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [MatIconModule, MatButtonModule, RouterModule, MatSidenavModule, MatListModule, MatToolbarModule, CommonModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})

export class MenuComponent {

  modules: any[] = [];

  constructor(
    private router: Router,
    private alert: AlertService,
    private authService: AuthService
  ) {

    this.modules = [
      { name: 'Tareas', route: '/tasks' },
      { name: 'Estados', route: '/states' },
    ];

  }


  logout() {
    this.alert.question('¿Estás seguro de cerrar sesión?').then((result: SweetAlertResult) => {
      if (result.isConfirmed) {
        this.router.navigate(['/login']);
        this.alert.success('Sesión cerrada', 'Sesión cerrada correctamente');
        this.authService.logout();
      }
    });
  }
}
