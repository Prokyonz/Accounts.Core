import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { SharedService } from '../common/shared.service';
import { amountReceived, sale, salesDetails } from '../Model/models';

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

  parties = [
    { id: 1, name: 'Dhanani Anand' },
    { id: 2, name: 'Dhanani Monika' },
    { id: 3, name: 'Sharma Mayur' },
  ];

  itemsList = [
    { id: 1, name: 'Item 1' },
    { id: 2, name: 'Item 2' },
    { id: 3, name: 'Item 3' },
    { id: 4, name: 'Item 4' },
    { id: 5, name: 'Item 5' },
    // Add more items here
  ];

  constructor(private router: Router, private messageService: MessageService, private sharedService: SharedService) {
    this.saleData = new sale();
    this.saleData.invoiceDate = new Date();
    this.saleData.salesDetails = [];
    this.saleData.amountReceived = [];

    this.saleData.salesDetails.push(new salesDetails());
    this.saleData.amountReceived.push(new amountReceived);
  }

  ngOnInit(): void {
  }

  addItem() {
    this.saleData.salesDetails.push(new salesDetails());
  }

  // Method to calculate totals
  calculateTotal(item: any): void {
    item.total = item.carratQty * item.rate;
    item.sgst = item.total * 0.09; // Example SGST at 9%
    item.cgst = item.total * 0.09; // Example CGST at 9%
    item.igst = item.total * 0.18; // If SGST and CGST are applied, IGST is 0
    item.totalAmount = item.total + item.sgst + item.cgst; // + item.igst;
    this.saleData.amount = this.getBillAmount();
    this.calculateCashAmount();
  }

  getBillAmount(): number {
    return this.saleData.salesDetails.reduce((sum: any, item: { totalAmount: any; }) => sum + item.totalAmount, 0);
  }

  calculateCashAmount(): void {
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
    const cashAmount = this.saleData.amount - totalPaymentAmount;

    // Ensure the cash amount is not negative
    if (this.saleData.amountReceived.length == 0) {
      this.saleData.amountReceived.push(new amountReceived);
    }

    this.saleData.amountReceived.forEach(payment => {
      if (payment.paymentMode === 'Cash') {
        payment.amount = cashAmount;  // Set the calculated cash amount
      }
    });
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
  }

  showDetails() {
    var a = this.saleData.salesDetails;
    //this.showMessage('success','Sales details added successfully');
    this.router.navigate(['salebill']);
  }

  showMessage(type: string, message: string) {
    this.messageService.add({ severity: type, summary: message });
  }

  addPayment() {
    this.saleData.amountReceived.push(new amountReceived());
  }

  removePayment(index: number) {
    this.saleData.amountReceived.splice(index, 1);
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
    let cashPayment = this.saleData.amountReceived.find(payment => payment.paymentMode === 'Cash');
    if (cashPayment) {
      cashPayment.amount = remainingAmount;
    }
  }
}
