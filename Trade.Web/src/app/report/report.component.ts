import { Component, HostListener, OnInit } from '@angular/core';
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
  columnArray: any[] = [];
  selectedColumnArray: any[] = [];
  isFilter: boolean = false;
  PurchaseReportList: any[];
  PurchaseReportCloneList: any[];
  filterColumn: string[] = [];
  isAdminView: boolean = false;
  isMobile: boolean = false;

  constructor(private rote: Router, private activateRoute: ActivatedRoute, private sharedService: SharedService, private messageService: MessageService) {
    this.checkIfMobile();
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
    if (this.user?.isAdmin && !this.isMobile) {
      this.isAdminView = true;
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

          this.columnArray = [
            { "displayName": "View", "dataType": "button", "fieldName": "id", "ishidefilter": true, "minWidth": "2", "sortIndex": "-1" },
            { "displayName": "User", "dataType": "text", "fieldName": "userName", "minWidth": "10", "sortIndex": "0" },
            { "displayName": "Date", "dataType": "Date", "fieldName": "invoiceDate", "ishidefilter": true, "minWidth": "7", "sortIndex": "1" },
            { "displayName": "Invoice No", "dataType": "text", "fieldName": "invoiceNo", "minWidth": "10", "sortIndex": "2" },
            { "displayName": "Party", "dataType": "text", "fieldName": "partyName", "minWidth": "20", "sortIndex": "3" },
            { "displayName": "Address", "dataType": "text", "fieldName": "address", "minWidth": "25", "sortIndex": "4" },
            { "displayName": "Pincode", "dataType": "numeric", "fieldName": "pincode", "minWidth": "10", "sortIndex": "5" },
            { "displayName": "Pancard/IT", "dataType": "text", "fieldName": "panNo", "minWidth": "10", "sortIndex": "6" },
            { "displayName": "Item Name", "dataType": "text", "fieldName": "itemName", "minWidth": "15", "sortIndex": "7" },
            { "displayName": "Item Qty", "dataType": "numeric", "fieldName": "carratQty", "minWidth": "15", "sortIndex": "8" },
            { "displayName": "Item Rate", "dataType": "numeric", "fieldName": "rate", "minWidth": "15", "sortIndex": "9" },
            { "displayName": "Item Amount", "dataType": "numeric", "fieldName": "amount", "minWidth": "15", "sortIndex": "10" },
            { "displayName": "SGST", "dataType": "numeric", "fieldName": "sgst", "minWidth": "10", "sortIndex": "11" },
            { "displayName": "CGST", "dataType": "numeric", "fieldName": "cgst", "minWidth": "10", "sortIndex": "12" },
            { "displayName": "Discount", "dataType": "numeric", "fieldName": "discount", "minWidth": "10", "sortIndex": "13" },
            { "displayName": "Bill Amount", "dataType": "numeric", "fieldName": "billAmount", "minWidth": "20", "sortIndex": "14" },
            { "displayName": "Payment1_Mode", "dataType": "text", "fieldName": "payment1_Mode", "minWidth": "20", "sortIndex": "15" },
            { "displayName": "Payment1_CardNo", "dataType": "numeric", "fieldName": "payment1_CardNo", "minWidth": "10", "sortIndex": "16" },
            { "displayName": "Payment1_Amount", "dataType": "numeric", "fieldName": "payment1_Amount", "minWidth": "10", "sortIndex": "17" },

            { "displayName": "Payment2_Mode", "dataType": "text", "fieldName": "payment2_Mode", "minWidth": "20", "sortIndex": "18" },
            { "displayName": "Payment2_CardNo", "dataType": "numeric", "fieldName": "payment2_CardNo", "minWidth": "10", "sortIndex": "19" },
            { "displayName": "Payment2_Amount", "dataType": "numeric", "fieldName": "payment2_Amount", "minWidth": "10", "sortIndex": "20" },

            { "displayName": "Payment3_Mode", "dataType": "text", "fieldName": "payment3_Mode", "minWidth": "20", "sortIndex": "21" },
            { "displayName": "Payment3_CardNo", "dataType": "numeric", "fieldName": "payment3_CardNo", "minWidth": "10", "sortIndex": "22" },
            { "displayName": "Payment3_Amount", "dataType": "numeric", "fieldName": "payment3_Amount", "minWidth": "10", "sortIndex": "23" },

            { "displayName": "Payment4_Mode", "dataType": "text", "fieldName": "payment4_Mode", "minWidth": "20", "sortIndex": "24" },
            { "displayName": "Payment4_CardNo", "dataType": "numeric", "fieldName": "payment4_CardNo", "minWidth": "10", "sortIndex": "25" },
            { "displayName": "Payment4_Amount", "dataType": "numeric", "fieldName": "payment4_Amount", "minWidth": "10", "sortIndex": "26" }
          ];
          this.selectedColumnArray = this.columnArray.map(item => ({ ...item }));
          this.filterColumn = this.selectedColumnArray.filter(e => e.dataType == "text" || e.dataType == "numeric").map(column => column.fieldName).filter(Boolean);
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

  checkIfMobile() {
    this.isMobile = window.innerWidth <= 768;  // Adjust the width as needed
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
      userId: !this.user?.isAdmin ? Number.parseInt(this.logInUserID) : 0,
      name: this.filterCriteria.name,
    };
    this.sharedService.customGetApi1<Customer[]>('Customer/CustomerReport', params).subscribe(
      (data: Customer[]) => {
        this.customers = data; // Data is directly returned here as an array of User objects
        this.customers = data.sort((a, b) => b.id - a.id);
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
        this.users = data.sort((a, b) => b.id - a.id);
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
        this.purchaseData = data.sort((a, b) => {
          const dateA = new Date(a.invoiceDate);  // Convert to Date object
          const dateB = new Date(b.invoiceDate);  // Convert to Date object

          const dateComparison = dateB.getTime() - dateA.getTime();

          if (dateComparison !== 0) {
            return dateComparison;  // If dates are different, return the result
          }

          return b.id - a.id;
        });
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching purchase details:', error);
      }
    );
  }

  getSale() {
    if (this.isAdminView) {
      this.getSaleForAdmin();
      return;
    }

    this.loading = true;
    const params = {
      userId: !this.user?.isAdmin ? Number.parseInt(this.logInUserID) : 0,
      fromDate: this.filterCriteria.fromDate != null ? this.filterCriteria.fromDate.toISOString().split('T')[0] : null,
      toDate: this.filterCriteria.toDate != null ? this.filterCriteria.toDate.toISOString().split('T')[0] : null,
      name: this.filterCriteria.name,
    };

    this.sharedService.customGetApi1<saleReport[]>('Sales/SaleReport', params).subscribe(
      (data: saleReport[]) => {
        this.saleData = data; // Data is directly returned here as an array of User objects
        this.saleData = data.sort((a, b) => {
          const dateA = new Date(a.invoiceDate);  // Convert to Date object
          const dateB = new Date(b.invoiceDate);  // Convert to Date object

          const dateComparison = dateB.getTime() - dateA.getTime();

          if (dateComparison !== 0) {
            return dateComparison;  // If dates are different, return the result
          }

          return b.id - a.id;
        });
        const groupedMap = new Map<string, any>();

        this.saleData.forEach((sale) => {
          const key = `${sale.invoiceNo}-${sale.invoiceDate}-${sale.partyName}-${sale.billAmount}`;
          if (!groupedMap.has(key)) {
            groupedMap.set(key, {
              id: sale.id,
              invoiceNo: sale.invoiceNo,
              invoiceDate: sale.invoiceDate,
              partyName: sale.partyName,
              userName: sale.userName,
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

  getSaleForAdmin() {
    this.loading = true;
    const params = {
      userId: !this.user?.isAdmin ? Number.parseInt(this.logInUserID) : 0,
      fromDate: this.filterCriteria.fromDate != null ? this.filterCriteria.fromDate.toISOString().split('T')[0] : null,
      toDate: this.filterCriteria.toDate != null ? this.filterCriteria.toDate.toISOString().split('T')[0] : null,
      name: this.filterCriteria.name,
    };

    this.sharedService.customGetApi1<saleReport[]>('Sales/SaleReportForAdmin', params).subscribe(
      (data: saleReport[]) => {
        this.PurchaseReportList = data;
        this.PurchaseReportList = data.sort((a, b) => {
          const dateA = new Date(a.invoiceDate);  // Convert to Date object
          const dateB = new Date(b.invoiceDate);  // Convert to Date object

          const dateComparison = dateB.getTime() - dateA.getTime();

          if (dateComparison !== 0) {
            return dateComparison;  // If dates are different, return the result
          }

          return b.id - a.id;
        });
        this.PurchaseReportCloneList = [...this.PurchaseReportList];
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
        this.deleteSale(Id);
      }
    }
  }

  deleteSale(Id: number) {
    this.sharedService.customDeleteApi('Sales/' + Id).subscribe(
      (response: any) => {
        this.showMessage('success', 'Sales Deleted Successfully');
        this.getSale();
      },
      (error) => {
        this.showMessage('error', 'Something went wrong...');
      }
    );
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

  exportExcel() {
  }

  exportPdf() {
  }

  toggle() {
    this.isFilter = !this.isFilter;
  }

  onSelectedColumnsChange(selectedColumns: any[]) {
    // Retrieve existing saved columns from localStorage
    let savedColumns = localStorage.getItem('selectedColumns' + this.reportIndex);

    if (savedColumns) {
      // If saved columns exist, parse them and update the selection
      let parsedColumns = JSON.parse(savedColumns);
      parsedColumns = selectedColumns.sort((a, b) => a.sortIndex - b.sortIndex);
      // Save the updated selection back to localStorage
      localStorage.setItem('selectedColumns' + this.reportIndex, JSON.stringify(parsedColumns));
    } else {
      // If no saved columns exist, create a new entry with the current selection
      localStorage.setItem('selectedColumns' + this.reportIndex, JSON.stringify(selectedColumns));
    }
  }

  getDistinctColumnValues(fieldName: string): { fieldName: string, fieldValue: any }[] {
    const data = this.PurchaseReportCloneList;
    const distinctArray = Array.from(new Set(data.map(item => item[fieldName])))
      .filter(fieldValue => fieldValue !== undefined) // Filter out undefined values
      .map(fieldValue => ({ fieldName: fieldName, fieldValue: fieldValue }));
    return distinctArray;
  }

  applyFilter(event: any, fieldName: string) {
    if (!event || !event.value || event.value.length === 0) {
      this.PurchaseReportList = [...this.PurchaseReportCloneList];
      return;
    }

    const selectedValues = event?.value.map((item: any) => item.fieldValue);
    if (selectedValues != null) {
      this.PurchaseReportList = this.PurchaseReportCloneList.filter(row => {
        return selectedValues.includes(row[fieldName]);
      });
    }
    else {
      this.PurchaseReportList = [...this.PurchaseReportCloneList];
    }
  }

  getSelectedItemsLabel(fieldName: any): string {
    const selectedValues = this.PurchaseReportCloneList[fieldName];
    if (selectedValues && selectedValues.length > 0) {
      // Change the label format as needed
      return `${selectedValues.length} selected`;
    } else {
      return 'Select';
    }
  }

  formatIndianNumber(amount: number, isSymbol: boolean = true): string {
    const formatter = new Intl.NumberFormat('en-IN', { maximumFractionDigits: 2 });
    return isSymbol ? '₹' + formatter.format(amount) : formatter.format(amount);
  }

  calculateColumnSum(columnName: string): number {
    let sum = 0;
    let count = 0;
    // Use the filteredValue directly without reassigning it to PurchaseReportList
    // if (this.dataTable != undefined && this.dataTable.filteredValue !== undefined && this.dataTable.filteredValue !== null) {
    //   this.PurchaseReportList = this.dataTable.filteredValue;
    // }

    if (this.PurchaseReportList === undefined) {
      return 0;
    }

    try {
      for (const item of this.PurchaseReportList) {
        // Check if the property exists before summing
        if (item.hasOwnProperty(columnName) && !isNaN(parseFloat(item[columnName]))) {
          sum += parseFloat(item[columnName]);
          count++;
        }
      }
    } catch (ex) {
      this.loading = false;
      alert(ex);
    }
    return sum;
  }

  exportLedger(type: string, itemData: any) {

  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.checkIfMobile();
  }

  viewDetails(row: any) {
    console.log('Viewing details for: ', row);
    // You can add logic to display a modal, navigate, or show details
  }
}
