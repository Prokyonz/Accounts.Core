import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { SharedService } from '../common/shared.service';
import { Customer, item, purchase, purchaseItems } from '../Model/models';

@Component({
  selector: 'app-purchase',
  templateUrl: './purchase.component.html',
  styleUrls: ['./purchase.component.scss']
})
export class PurchaseComponent {
  PageTitle: string = "Purchase";
  loading: boolean = false;
  purchase: purchase;
  parties: Customer[];
  itemsList: item[];

  constructor(private router: Router, private messageService: MessageService, private sharedService: SharedService) {
    this.purchase = new purchase();
    this.purchase.date = new Date();
    this.purchase.items = [];
    this.getCustomer();
    this.addItem();
    this.getItem();
  }

  ngOnInit(): void {
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
    this.sharedService.customGetApi1<item[]>('ItemMaster').subscribe(
      (data: item[]) => {
        this.itemsList = data; // Data is directly returned here as an array of User objects
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching customers:', error);
      }
    );
  }
  
  addItem() {
    this.purchase.items.push(new purchaseItems());
  }

  // Method to calculate totals
  calculateTotal(item: any): void {
    item.total = (item.qty * item.rate) + item.gst;
    this.purchase.billAmount = this.getBillAmount();
  }

  getBillAmount(): number {
    return this.purchase.items.reduce((sum: any, item: { total: any; }) => sum + item.total, 0);
  }

  // Check if the form is valid
  isFormValid(): boolean {
    //return this.purchase.date && this.purchase.party && this.purchase.billAmount && this.isItemsValid();
    return true;
  }

  // Check if all items are valid
  isItemsValid(): boolean {
    //return this.purchase.items.every(x => x.itemName && x.qty > 0 && x.rate > 0);
    return true;
  }

  // Handle form submission
  onSubmit() {
    if (this.isFormValid()) {
      console.log('Form Submitted:', this.purchase);
    }
  }

  editItem(index: number): void {
    // You can add further functionality for editing an item if required
    console.log('Editing item:', this.purchase.items[index]);
  }

  // Delete item method
  deleteItem(index: number): void {
    this.purchase.items.splice(index, 1);
  }

  showDetails() {
    var a = this.purchase.items;
    this.showMessage('success', 'Sales details added successfully');
  }

  showMessage(type: string, message: string) {
    this.messageService.add({ severity: type, summary: message });
  }

  onItemSelect(itemname: string) {
    const selectedItem = this.itemsList.find(item => item.id === parseInt(itemname));
    if (selectedItem) {
      this.purchase.items.forEach(x => {
        if (x.itemName.toString() === selectedItem.id.toString()) {
          x.itemdescription = selectedItem.description;
        }
      });
    }
  }
}
