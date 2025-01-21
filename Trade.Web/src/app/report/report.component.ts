import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Table } from 'primeng/table';
import { SharedService } from '../common/shared.service';
import { RememberCompany } from '../shared/component/companyselection/companyselection.component';
import { Message, MessageService } from 'primeng/api';
import { Customer, filterCriteria, item, purchase, purchaseReport, saleReport, stockReport, user } from '../Model/models';
import { Directory, Filesystem } from '@capacitor/filesystem';
import { Share } from '@capacitor/share';

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
  isImageVisible: boolean = false;
  imageUrl: string | null = null;
  imageDetails: string | null = null;
  customerId: number | null = 0;
  user: user;
  isAllowtoEditDelete: boolean = false;

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
    var userDetail = localStorage.getItem('AuthorizeData');
    if (userDetail) {
      this.user = JSON.parse(userDetail);
    }

    this.activateRoute.paramMap.subscribe(params => {
      const reportId = params.get('id'); // Assuming 'id' is the parameter name in your route
      if (reportId) {
        this.reportIndex = parseInt(reportId);

        this.loading = false;
        if (this.reportIndex == 1) {
          this.PageTitle = "Customer Report";
          this.isAllowtoEditDelete = this.hasPermission('CustomerManage');
          this.getCustomer();
        }
        else if (this.reportIndex == 2) {
          this.PageTitle = "User Report";
          this.isAllowtoEditDelete = this.hasPermission('MasterManage');
          this.getUser();
        }
        else if (this.reportIndex == 3) {
          this.PageTitle = "Purchase Report";
          this.isAllowtoEditDelete = this.hasPermission('MasterManage');
          this.getPurchase();
        }
        else if (this.reportIndex == 4) {
          this.PageTitle = "Sale Report";
          this.isAllowtoEditDelete = this.hasPermission('SaleManage');
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

  hasPermission(permissionKey: string): boolean {
    if (this.user?.isAdmin) {
      return true;
    }
    else {
      return this.user.permissions.some(permission => permission.keyName === permissionKey);
    }
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
              id: sale.id,
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
    this.sharedService.customGetApi1<stockReport[]>('PurchaseMaster/StockReport/0').subscribe(
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

  deleteItem(Id: any) {
    const confirmDelete = window.confirm('Are you sure you want to delete this item?');
    if (confirmDelete) {
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

  onDialogClose() {
    this.isImageVisible = false;
  }

  imageClick(imageData: string, imageDetails: string, id: number) {
    this.imageUrl = imageData;
    this.imageDetails = imageDetails;
    this.customerId = id;
    this.isImageVisible = true;
  }

  async shareImage() {
    this.loading = true;
    const fileName = `${this.imageDetails?.replace(/\s+/g, '')}_${this.customerId?.toString()}.png`;
    const requestPermissions = async () => {
      const permissionStatus = await Filesystem.requestPermissions();
      console.log('Permission requested:', permissionStatus);
      return permissionStatus.publicStorage === 'granted';
    };
    Filesystem.writeFile({
      path: fileName,
      data: this.imageUrl?.split(',')[1] ?? '',
      directory: Directory.Cache,//Directory.Documents, // Save to documents directory
      //encoding: Encoding.UTF8,
    })
      .then(async (writeFileResult) => {
        const fileUri = await Filesystem.getUri({
          directory: Directory.Cache,
          path: fileName,
        });
        // On success, share the PDF file
        try {
          await Share.share({
            title: this.imageDetails ?? 'Image',
            text: `${this.imageDetails} (Cust ID: ${this.customerId?.toString()})`,
            url: fileUri.uri,
            dialogTitle: 'Share Image',
          });
        } catch (err) {
          console.error('Error sharing image:', err);
          this.loading = false;
        }
      })
      .catch((error) => {
        console.error('Error writing file to device', error);
        this.loading = false;
      });
    this.loading = false;
  }

  onCheckChange(event: any, item: any): void {
    this.loading = true;
    if (this.users) {
      item.updatedBy = this.logInUserID;
      item.updatedDate = new Date();
      this.sharedService.customPutApi("UserMaster/ActiveUser", item)
        .subscribe((data: any) => {
          if (data != null) {
            if (event.checked) {
              this.showMessage('success', `${item.name} is now active.`);
            } else {
              this.showMessage('success', `${item.name} is now inactive.`);
            }
            this.loading = false;
          }
          else {
            this.loading = false;
            this.showMessage('error', 'Something went wrong...');
          }
        }, (ex: any) => {
          this.loading = false;
          this.showMessage('error', ex);
        });
    }
  }
}
