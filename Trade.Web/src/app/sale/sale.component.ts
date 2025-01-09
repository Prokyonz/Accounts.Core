import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { SharedService } from '../common/shared.service';
import { amountReceived, Customer, item, pos, sale, salesDetails, stockReport } from '../Model/models';

@Component({
  selector: 'app-sale',
  templateUrl: './sale.component.html',
  styleUrls: ['./sale.component.scss'],
  providers: [MessageService]
})
export class SaleComponent implements OnInit {
  PageTitle: string = "Sale";
  loading: boolean = false;
  saleData: sale;
  logInUserID: string;
  isEditMode = false;

  parties: Customer[];
  itemsList: stockReport[];
  posList: pos[];

  constructor(private route: ActivatedRoute, private router: Router, private messageService: MessageService, private sharedService: SharedService) {
    this.saleData = new sale();
    this.saleData.invoiceDate = new Date();
    this.saleData.salesDetails = [];
    this.saleData.amountReceived = [];
    this.saleData.amountReceived.push(new amountReceived);
    let cashAmount = new amountReceived();
    cashAmount.paymentMode = "Cash"
    this.saleData.amountReceived.push(cashAmount);
    this.logInUserID = localStorage.getItem('userid') ?? '0';

    this.getCustomer();
    this.getPOS();
    this.addItem();
    this.getItem();
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const itemId = params.get('salesId'); // Assuming 'id' is the parameter name in your route

      if (itemId) {
        this.isEditMode = true;
        this.loadItem(itemId); // Fetch the item by ID if editing
      } else {
        this.isEditMode = false;
        this.getItem();
      }
    });
  }

  loadItem(salesId: string) {
      this.loading = true;
      this.sharedService.customGetApi1<sale[]>('Sales/GetSale/' + salesId).subscribe(
        (data: any) => {
          this.saleData = data; // Data is directly returned here as an array of User objects
          this.isEditMode = true;
          this.loading = false;
        },
        (error) => {
          this.loading = false;
          this.showMessage('Error fetching Sales details:', error);
        }
      );
    }

  getCustomer() {
    this.loading = true;
    this.sharedService.customGetApi1<Customer[]>('Customer').subscribe(
      (data: Customer[]) => {
        this.parties = data; // Data is directly returned here as an array of User objects
        this.parties = this.parties.map(user => ({
          ...user,
          fullName: `${user.firstName} ${user.lastName}` // Concatenate first and last name
        }));
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching customers:', error);
      }
    );
  }

  getItem() {
    this.loading = true;
    this.sharedService.customGetApi1<stockReport[]>('PurchaseMaster/StockReport').subscribe(
      (data: stockReport[]) => {
        this.itemsList = data; // Data is directly returned here as an array of User objects
        this.itemsList = this.itemsList.map(item => ({
          ...item,
          stockDisplayName: `${item.name} - ${item.rate}(${item.quantity})` // Concatenate first and last name
        }));
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching customers:', error);
      }
    );
  }

  getPOS() {
    this.loading = true;
    this.sharedService.customGetApi1<pos[]>('POSMaster/GetPOSByUser/'+this.logInUserID).subscribe(
      (data: pos[]) => {
        this.posList = data;
        this.posList = this.posList.map(pos => ({
          ...pos,
          posName: `${pos.tidNumber} (${pos.tidBankName})` // Concatenate first and last name
        }));
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching pos details:', error);
      }
    );
  }

  addItem() {
    this.saleData.salesDetails.push(new salesDetails());
  }

  // Method to calculate totals
  calculateTotal(item: any): void {
    var selectedStock = this.itemsList.find(x => x.rowNum === item.rowNum);
    if (selectedStock != null && selectedStock.quantity < item.carratQty) {
      this.showMessage('error', `Quantity can not be more then available quantity. Available quantity is ${selectedStock.quantity}.`);
      setTimeout(() => {
        item.carratQty = '';  // Reset quantity if invalid
        item.total = 0;       // Reset total as well
        item.sgst = 0;        // Reset SGST
        item.cgst = 0;        // Reset CGST
        item.igst = 0;        // Reset IGST
        item.totalAmount = 0; // Reset totalAmount
        this.saleData.amount = this.getBillAmount();
        this.calculateCashAmount(true); // Recalculate Cash if needed
      }, 500);
      return;
    }

    item.total = item.carratQty * item.rate;
    let gSTAmount = (item.total * item.gstper) / 100;
    item.sgst = parseFloat((gSTAmount / 2).toFixed(2));
    item.cgst = parseFloat((gSTAmount / 2).toFixed(2));
    item.igst = parseFloat(gSTAmount.toFixed(2));
    item.totalAmount = Math.ceil(item.total + item.sgst + item.cgst);
    this.saleData.amount = this.getBillAmount();
    this.calculateCashAmount(true);
  }

  getBillAmount(): number {
    return this.saleData.salesDetails.reduce((sum: any, item: { totalAmount: any; }) => sum + item.totalAmount, 0);
  }

  calculateCashAmount(isAddDefaultPayment: boolean, payment: any = null): void {
    if (isAddDefaultPayment) {
      this.saleData.amountReceived = [];
      this.saleData.amountReceived.push(new amountReceived);
      let cashAmount = new amountReceived();
      cashAmount.paymentMode = "Cash"
      this.saleData.amountReceived.push(cashAmount);
    }
    // if(this.saleData.amountReceived.find(x=>x.paymentMode == "Cash"){
    //   this.saleData.paymentMode = 'Cash';
    // }

    // this.saleData.cashAmount = this.saleData.billAmount - this.saleData.creditCardPaidAmount;

    let totalPaymentAmount = 0;

    // Sum up all amounts from non-cash payment modes (Creditcard, Debitcard, etc.)
    this.saleData.amountReceived.forEach(payment => {
      if (payment.paymentMode !== 'Cash') {
        totalPaymentAmount += payment.amount || 0;  // Only sum amounts for non-Cash payments
      }
    });

    // The cash amount is the remaining balance after non-cash payments
    const creditCardAmount = this.saleData.amount - totalPaymentAmount;

    if (payment != null && creditCardAmount < 0) {
      this.showMessage('error', 'Ensure the total amount paid does not exceed the bill amount.');
      payment.amount = '';
      return;
    }
    // Ensure the cash amount is not negative
    if (this.saleData.amountReceived.length == 0) {
      this.saleData.amountReceived.push(new amountReceived);
    }

    this.saleData.amountReceived.forEach(payment => {
      if (payment.paymentMode === 'Cash') {
        payment.amount = creditCardAmount;  // Set the calculated cash amount
      }
    });
  }

  onAmountChange(payment: any, index: number) {
    this.calculateCashAmount(false, payment);
  }
  // Check if the form is valid
  isFormValid(): boolean {
    //eturn this.saleData.invoiceDate && this.saleData.customerId && this.saleData.amount && this.isItemsValid();
    return true;
  }

  // Check if all items are valid
  isItemsValid(): boolean {
    //return this.saleData.items.every(x => x.itemName && x.qty > 0 && x.rate > 0);
    return true;
  }

  // Handle form submission
  onSubmit() {
    if (this.isFormValid()) {
      console.log('Form Submitted:', this.saleData);
    }
  }

  editItem(index: number): void {
    // You can add further functionality for editing an item if required
    console.log('Editing item:', this.saleData.salesDetails[index]);
  }

  // Delete item method
  deleteItem(index: number): void {
    this.saleData.salesDetails.splice(index, 1);
    this.calculateCashAmount(false);
  }

  showDetails() {
    var a = this.saleData.salesDetails;
    if (this.saleData.customerId == 0) {
      this.showMessage('error', 'Please select Party.');
      return;
    }
    else if (this.saleData.amount == 0) {
      this.showMessage('error', 'Please select Sale Item(s).');
      return;
    }
    var paidAmount = this.saleData.amountReceived.reduce((total, payment) => total + payment.amount, 0)
    if (this.saleData.amount < paidAmount) {
      this.showMessage('error', 'Paid amount is lesser than bill amount.');
      return;
    }

    this.saleData.createdBy = parseInt(this.logInUserID);
    this.saleData.createdDate = new Date();
    this.saleData.updatedBy = parseInt(this.logInUserID);
    this.saleData.updatedDate = new Date();
    this.sharedService.customPostApi("Sales", this.saleData)
      .subscribe((data: any) => {
        if (data != null) {
          this.showMessage('success', `Invoice No: ${data.seriesName + data.invoiceNo} - Sale details added successfully`);
          this.clearForm();
        }
        else {
          this.loading = false;
          this.showMessage('error', 'Something went wrong...');
        }
      }, (ex: any) => {
        this.loading = false;
        this.showMessage('error', ex);
      });

    this.router.navigate(['salebill']);
  }

  clearForm() {
    this.saleData = new sale();
    this.saleData.invoiceDate = new Date();
    this.saleData.salesDetails = [];
    this.saleData.amountReceived = [];
    this.saleData.amountReceived.push(new amountReceived);
    let cashAmount = new amountReceived();
    cashAmount.paymentMode = "Cash"
    this.saleData.amountReceived.push(cashAmount);
    this.getItem();
    this.addItem();
    this.loading = false;
  }

  showMessage(type: string, message: string) {
    this.messageService.add({ severity: type, summary: message });
  }

  addPayment() {
    this.saleData.amountReceived.push(new amountReceived());
  }

  removePayment(index: number) {
    this.saleData.amountReceived.splice(index, 1);
    this.calculateCashAmount(false);
  }

  onPaymentModeChange(payment: amountReceived, index: number) {
    if (payment.paymentMode === 'Cash') {
      // If 'Cash' is selected, handle the remaining amount automatically
      this.updateRemainingAmount();
    }
  }

  // Ensure the remaining amount is handled with 'Cash'
  updateRemainingAmount() {
    let totalPaid = this.saleData.amountReceived.reduce((acc, payment) => acc + payment.amount, 0);
    let remainingAmount = this.saleData.amount - totalPaid;

    // If there's a remaining amount, set it to Cash
    let creditcardPayment = this.saleData.amountReceived.find(payment => payment.paymentMode === 'Cash');
    if (creditcardPayment) {
      creditcardPayment.amount = remainingAmount;
    }
  }

  onItemSelect(itemL: any) {
    const selectedItem = this.itemsList.find(item => item.rowNum === parseInt(itemL.rowNum.toString()));
    if (selectedItem) {
      this.saleData.salesDetails.forEach(x => {
        if (x.rowNum.toString() === selectedItem.rowNum.toString()) {
          x.itemId = selectedItem.itemId;
          x.rate = selectedItem.rate;
          x.gstper = selectedItem.gstPer;
        }
      });
      this.calculateTotal(itemL);
    }
  }
}
