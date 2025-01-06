import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Table } from 'primeng/table';
import { SharedService } from '../common/shared.service';
import { RememberCompany } from '../shared/component/companyselection/companyselection.component';
import { Message, MessageService } from 'primeng/api';
import { Customer, filterCriteria, item, purchase, purchaseReport, saleReport, stockReport, user } from '../Model/models';

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
  groupedSaleData: any[] = [];
  loading = false;
  showFilter = false;
  filteredSaleData = []; // Data after applying filters
  logInUserID: string;
  filterCriteria: filterCriteria;

  constructor(private rote: Router, private activateRoute: ActivatedRoute, private sharedService: SharedService, private messageService: MessageService) {
    this.reportIndex = activateRoute.snapshot.params['id'];
    this.logInUserID = localStorage.getItem('userid') ?? '0';
    this.filterCriteria = {
      fromDate: new Date(new Date().setDate(new Date().getDate() - 30)), // Default: 30 days ago
      toDate: new Date(), // Default: Today
      name: ''
    };
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
    const params = {
      name: this.filterCriteria.name,
    };
    this.sharedService.customGetApi1<Customer[]>('Customer/CustomerReport', params).subscribe(
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
    this.loading = true;
    const params = {
      name: this.filterCriteria.name,
    };
    this.sharedService.customGetApi1<user[]>('UserMaster/UserReport', params).subscribe(
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
    const params = {
      //userId: this.logInUserID,
      fromDate: this.filterCriteria.fromDate != null ? this.filterCriteria.fromDate.toISOString().split('T')[0] : null,
      toDate: this.filterCriteria.toDate != null ? this.filterCriteria.toDate.toISOString().split('T')[0] : null,
      name: this.filterCriteria.name,
    };
    this.sharedService.customGetApi1<purchaseReport[]>('PurchaseMaster/PurchaseReport', params).subscribe(
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
    const params = {
      //userId: this.logInUserID,
      fromDate: this.filterCriteria.fromDate != null ? this.filterCriteria.fromDate.toISOString().split('T')[0] : null,
      toDate: this.filterCriteria.toDate != null ? this.filterCriteria.toDate.toISOString().split('T')[0] : null,
      name: this.filterCriteria.name,
    };

    this.sharedService.customGetApi1<saleReport[]>('Sales/SaleReport', params).subscribe(
      (data: saleReport[]) => {
        this.saleData = data; // Data is directly returned here as an array of User objects
        const groupedMap = new Map<string, any>();

        this.saleData.forEach((sale) => {
          const key = `${sale.invoiceNo}-${sale.invoiceDate}-${sale.partyName}-${sale.billAmount}`;
          if (!groupedMap.has(key)) {
            groupedMap.set(key, {
              invoiceNo: sale.invoiceNo,
              invoiceDate: sale.invoiceDate,
              partyName: sale.partyName,
              billAmount: sale.billAmount,
              details: [],
            });
          }
          groupedMap.get(key).details.push({
            paymentNo: sale.paymentNo,
            paymentMode: sale.paymentMode,
            cardNo: sale.cardNo,
            paidAmount: sale.paidAmount,
          });
        });

        this.groupedSaleData = Array.from(groupedMap.values());

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

  showFilterPopup() {
    this.showFilter = !this.showFilter;
    this.filterCriteria.fromDate = new Date();
    this.filterCriteria.toDate = new Date();
    this.filterCriteria.name = '';
  }

  filterSearch() {
    //this.showFilter = false;
    if (this.reportIndex == 1) {
      this.getCustomer();
    }
    else if (this.reportIndex == 2) {
      this.getUser();
    }
    else if (this.reportIndex == 3) {
      this.getPurchase();
    }
    else if (this.reportIndex == 4) {
      this.getSale();
    }
  }

  filterCancel() {
    this.showFilter = false;
    this.filterCriteria = new filterCriteria();
    this.filterCriteria.fromDate = null;
    this.filterCriteria.toDate = null;
    if (this.reportIndex == 1) {
      this.getCustomer();
    }
    else if (this.reportIndex == 2) {
      this.getUser();
    }
    else if (this.reportIndex == 3) {
      this.getPurchase();
    }
    else if (this.reportIndex == 4) {
      this.getSale();
    }
  }
}
