import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { SharedService } from 'src/app/common/shared.service';
import { item } from 'src/app/Model/models';

@Component({
  selector: 'app-additem',
  templateUrl: './additem.component.html',
  styleUrls: ['./additem.component.scss']
})
export class AdditemComponent implements OnInit {
  PageTitle = 'Item Details';
  item: item;
  loading: boolean = false;
  isSaveButton: boolean = false;
  isEditMode = false;
  itemData: item[];

  constructor(private route: ActivatedRoute,private fb: FormBuilder, private router: Router, private messageService: MessageService, private sharedService: SharedService) {
    this.item = new item();
  }

  
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const itemId = params.get('itemId'); // Assuming 'id' is the parameter name in your route

      if (itemId) {
        this.isEditMode = true;
        this.isSaveButton = true;
        this.loadItem(itemId); // Fetch the item by ID if editing
      } else {
        this.isEditMode = false;
        this.getItem();
      }
    });
  }

  loadItem(itemId: string) {
    this.loading = true;
    this.sharedService.customGetApi1<item[]>('ItemMaster/GetItemMaster/'+itemId).subscribe(
      (data: any) => {
        this.item = data; // Data is directly returned here as an array of User objects
        this.isEditMode = true;
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching item details:', error);
      }
    );
  }

  myfunction() {
    if (this.isSaveButton) {
      this.isSaveButton = false;
      this.PageTitle = "Item Details";
      this.clearForm();
    }
    else {
      this.router.navigate(["dashboard"]);
    }
  }

  onAddIconClick() {
    this.PageTitle = "Add Item"
    this.isSaveButton = true;
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

  // Function to handle the form submission
  showDetails() {
    if(this.item.name.length == 0){
      this.showMessage('error', 'Please enter Item Name');
      return;
    }

    if (this.isEditMode) {
      this.updateItem();
    } else {
      this.createItem();
    }
    this.router.navigate(['/additem']);
  }

  createItem() {
    this.sharedService.customPostApi("ItemMaster", this.item)
      .subscribe((data: any) => {
        if (data != null) {
          this.showMessage('success', 'Item Save Successfully');
          this.clearForm();
          this.getItem();
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

  updateItem(){
    this.sharedService.customPutApi("ItemMaster", this.item)
    .subscribe((data: any) => {
      if (data != null) {
        this.showMessage('success', 'Item Updated Successfully');
        this.clearForm();
        this.getItem();
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

  showMessage(type: string, message: string) {
    this.messageService.add({ severity: type, summary: message });
  }

  clearForm() {
    this.item = new item();
    this.isEditMode = false;
    this.isSaveButton = false;
    this.loading = false;
  }
}
