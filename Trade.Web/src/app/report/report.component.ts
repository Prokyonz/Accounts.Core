import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Table } from 'primeng/table';
import { SharedService } from '../common/shared.service';
import { RememberCompany } from '../shared/component/companyselection/companyselection.component';
import { Message, MessageService } from 'primeng/api';
import { Customer, item, purchase, purchaseReport, saleReport, stockReport, user } from '../Model/models';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})

export class ReportComponent implements OnInit {
  PageTitle: string = "Report";
  reportIndex: number | undefined = 0;
  customers: Customer[];
  users: user[];
  purchaseData: purchaseReport[];
  saleData: saleReport[];
  itemData: item[];
  stockData: stockReport[];

  loading = false;

  constructor(private rote: Router, private activateRoute: ActivatedRoute, private sharedService: SharedService, private messageService: MessageService) {
    this.reportIndex = activateRoute.snapshot.params['id'];
  }

  ngOnInit() {
    this.activateRoute.paramMap.subscribe(params => {
      const reportId = params.get('id'); // Assuming 'id' is the parameter name in your route
      if (reportId) {
        this.reportIndex = parseInt(reportId);

        this.loading = false;
        if (this.reportIndex == 1) {
          this.PageTitle = "Customer Report";
          this.getCustomer();
        }
        else if (this.reportIndex == 2) {
          this.PageTitle = "User Report";
          this.getUser();
        }
        else if (this.reportIndex == 3) {
          this.PageTitle = "Purchase Report";
          this.getPurchase();
        }
        else if (this.reportIndex == 4) {
          this.PageTitle = "Sale Report";
          this.getSale();
        }
        else if (this.reportIndex == 5) {
          this.PageTitle = "Stock Report";
          this.getStock();
        }
      }
    });
    // else if (this.reportIndex == 4) {
    //   this.PageTitle = "Item Report";
    //   this.getItem();
    // }
  }

  getCustomer() {
    this.loading = true;
    this.sharedService.customGetApi1<Customer[]>('Customer').subscribe(
      (data: Customer[]) => {
        this.customers = data; // Data is directly returned here as an array of User objects
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching customers:', error);
      }
    );
  }

  getUser() {
    this.loading = true;
    this.sharedService.customGetApi1<user[]>('UserMaster').subscribe(
      (data: user[]) => {
        this.users = data; // Data is directly returned here as an array of User objects
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching users:', error);
      }
    );
  }

  getPurchase() {
    this.loading = true;
    this.sharedService.customGetApi1<purchaseReport[]>('PurchaseMaster/PurchaseReport').subscribe(
      (data: purchaseReport[]) => {
        this.purchaseData = data; // Data is directly returned here as an array of User objects
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching purchase details:', error);
      }
    );
  }

  getSale() {
    this.loading = true;
    this.sharedService.customGetApi1<saleReport[]>('Sales/SaleReport').subscribe(
      (data: saleReport[]) => {
        this.saleData = data; // Data is directly returned here as an array of User objects
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching sale details:', error);
      }
    );
  }

  getStock() {
    this.loading = true;
    this.sharedService.customGetApi1<stockReport[]>('PurchaseMaster/StockReport').subscribe(
      (data: stockReport[]) => {
        this.stockData = data; // Data is directly returned here as an array of User objects
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching stock details:', error);
      }
    );
  }

  getItem() {
    this.loading = true;
    this.sharedService.customGetApi1<item[]>('ItemMaster').subscribe(
      (data: item[]) => {
        this.itemData = data; // Data is directly returned here as an array of User objects
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching item details:', error);
      }
    );
  }

  goBack() {
    this.rote.navigate(['/dashboard']);
  }

  showMessage(type: string, message: string) {
    this.messageService.add({ severity: type, summary: message });
  }
}
