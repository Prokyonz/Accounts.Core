import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { SharedService } from '../common/shared.service';

@Component({
  selector: 'app-sale',
  templateUrl: './sale.component.html',
  styleUrls: ['./sale.component.scss'],
  providers: [MessageService]
})
export class SaleComponent implements OnInit{
  PageTitle: string = "Sale";
  loading: boolean = false;
  saleData: any = {
    date: '',
    party: '',
    billAmount: '',
    paymentMode: 'Cash',
    creditCardNo: '',
    creditCardPaidAmount: '',
    cashAmount: '',
    items: [
      {
        itemName: '',
        qty: 1,
        rate: 0,
        total: 0,
        sgst: 0,
        cgst: 0,
        igst: 0,
        grandTotal: 0
      }
    ]
  };

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

  }

  ngOnInit(): void {
  }

  addItem() {
    this.saleData.items.push({
      itemName: '',
      qty: 1,
      rate: 0,
      total: 0,
      sgst: 0,
      cgst: 0,
      igst: 0,
      grandTotal: 0
    });
  }

  // Method to calculate totals
  calculateTotal(item: any): void {
    item.total = item.qty * item.rate;
    item.sgst = item.total * 0.09; // Example SGST at 9%
    item.cgst = item.total * 0.09; // Example CGST at 9%
    item.igst = item.total * 0.18; // If SGST and CGST are applied, IGST is 0
    item.grandTotal = item.total + item.sgst + item.cgst; // + item.igst;
    this.saleData.billAmount = this.getBillAmount();
    this.calculateCashAmount();
  }

  getBillAmount(): number {
    return this.saleData.items.reduce((sum: any, item: { grandTotal: any; }) => sum + item.grandTotal, 0);
  }

  calculateCashAmount(): void {
    if(this.saleData.paymentMode == undefined){
      this.saleData.paymentMode = 'Cash';
    }

    this.saleData.cashAmount = this.saleData.billAmount - this.saleData.creditCardPaidAmount;
  }

  // Check if the form is valid
  isFormValid(): boolean {
    return this.saleData.date && this.saleData.party && this.saleData.billAmount && this.saleData.paymentMode && this.isItemsValid();
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
    console.log('Editing item:', this.saleData.items[index]);
  }

  // Delete item method
  deleteItem(index: number): void {
    this.saleData.items.splice(index, 1);
  }

  showDetails() {
    var a = this.saleData.item;
    //this.showMessage('success','Sales details added successfully');
    this.router.navigate(['salebill']);
  }

  showMessage(type: string, message: string){
    this.messageService.add({severity: type, summary:message});
  }
}
