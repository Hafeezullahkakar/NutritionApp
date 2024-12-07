import { Routes } from '@angular/router';
import { RegisterComponent } from './components/auth/register/register.component';
import { LoginComponent } from './components/auth/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AllFoodItemsComponent } from './components/allfooditems/allfooditems.component';

 export const routes: Routes = [
    { path: 'register', component: RegisterComponent },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'allfooditems', component: AllFoodItemsComponent },
    { path: '**', redirectTo: 'login' }
  ];
  
