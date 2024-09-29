import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AddctsComponent } from './calculator/addcts/addcts.component';
import { ViewctsComponent } from './calculator/viewcts/viewcts.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CalculatorComponent } from './calculator/calculator.component';
import { ReportComponent } from './report/report.component';
import { CompanyselectionComponent } from './shared/component/companyselection/companyselection.component';

const routes: Routes = [
  {path:'',component:LoginComponent},
  {path:'login',component:LoginComponent},
  {path:'dashboard', component:DashboardComponent},
  {path:'addcts', component:AddctsComponent},
  {path:'viewcts', component:ViewctsComponent},
  {path: 'calculator', component: CalculatorComponent},
  {path: 'report/:id', component: ReportComponent},
  {path: 'companyselection/:value', component: CompanyselectionComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
