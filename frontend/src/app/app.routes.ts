import { Routes } from '@angular/router';
import { LoginComponent } from './pages/auth/login/login.component';
import { RegisterComponent } from './pages/auth/register/register.component';
import { ListTasksComponent } from './pages/tasks/list-tasks/list-tasks.component';
import { ListStatesComponent } from './pages/states/list-states/list-states.component';
import { authGuard } from './guards/auth.guard';


export const routes: Routes = [
   { path: '', redirectTo: 'tasks', pathMatch: 'full' },

   { path: 'login', component: LoginComponent },
   
   { path: 'register', component: RegisterComponent },

   { path: 'tasks', component: ListTasksComponent, canActivate: [authGuard] },

   { path: 'states', component: ListStatesComponent, canActivate: [authGuard] },

   { path: '**', redirectTo: 'tasks' }
];
