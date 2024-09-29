import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Table } from 'primeng/table';
import { SharedService } from '../common/shared.service';
import { RememberCompany } from '../shared/component/companyselection/companyselection.component';
import { Message, MessageService } from 'primeng/api';

interface Customer {
  name: string,
  country: string,
  date: string,
  balance: number,
  verified: boolean
}

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})

export class ReportComponent implements OnInit {

  PageTitle: string = "Purchase Report";
  reportIndex: number = 0;
  RememberCompany: RememberCompany = new RememberCompany();
  PurchaseReportList : any[];
  //Table variables
  customers: Customer[] = [
    { name: 'Abhishek', country: 'India', date: '12/12/2014', balance: 25000, verified: true },
    { name: 'Anand', country: 'India', date: '12/12/2014', balance: 28000, verified: false },
    { name: 'Shaielsh', country: 'USA', date: '01/11/2015', balance: 45000, verified: false },
    { name: 'Akshay', country: 'UK', date: '01/11/2017', balance: 145000, verified: false },
    { name: 'Abhishek', country: 'India', date: '12/12/2014', balance: 25000, verified: true },
    { name: 'Anand', country: 'India', date: '12/12/2014', balance: 28000, verified: false },
    { name: 'Shaielsh', country: 'USA', date: '01/11/2015', balance: 45000, verified: false },
    { name: 'Akshay', country: 'UK', date: '01/11/2017', balance: 145000, verified: false },
    { name: 'Abhishek', country: 'India', date: '12/12/2014', balance: 25000, verified: true },
    { name: 'Anand', country: 'India', date: '12/12/2014', balance: 28000, verified: false },
    { name: 'Shaielsh', country: 'USA', date: '01/11/2015', balance: 45000, verified: false },
    { name: 'Akshay', country: 'UK', date: '01/11/2017', balance: 145000, verified: false },
  ];

  loading: boolean = true;

  constructor(private rote: Router, private activateRoute: ActivatedRoute,private sharedService: SharedService, private messageService: MessageService) {
    this.reportIndex = activateRoute.snapshot.params['id'];
  }

  ngOnInit() {
    this.loading = false;
    this.getCompanyData();
    this.purchseReport();
  }

  goBack() {
    this.rote.navigate(['/dashboard']);
  }

  //Tabel functions
  clear(table: Table) {
      table.clear();
  }

  myfunc(event: any): string {
    return event.target.value;
  }

  onSeach(event: any) {
    console.log(event);
  }

  purchseReport(){
    this.sharedService.customGetApi("Report/GetPurchaseReport?CompanyId=" + this.RememberCompany.company.id + "&FinancialYearId=" + this.RememberCompany.financialyear.id +"&FromDate=2022-05-01&ToDate=2023-05-20")
    .subscribe((data: any) => {
          this.PurchaseReportList = data.data;
          console.log(this.PurchaseReportList);
        }, (ex: any) => {
          this.showMessage('error',ex);
      });
  }

  showMessage(type: string, message: string){
    this.messageService.add({severity: type, summary:message});
  }

  getCompanyData(){
    debugger;
    const data = localStorage.getItem("companyremember");
    if (data != null){
      this.RememberCompany = this.sharedService.JsonConvert<RememberCompany>(data)
    }
  }

}
