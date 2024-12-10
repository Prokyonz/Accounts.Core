import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { SharedService } from 'src/app/common/shared.service';
import { item } from 'src/app/Model/models';

@Component({
  selector: 'app-additem',
  templateUrl: './additem.component.html',
  styleUrls: ['./additem.component.scss']
})
export class AdditemComponent implements OnInit {
  PageTitle = 'Add Item';
  item: item;
  loading: boolean = false;

  constructor(private fb: FormBuilder, private router: Router, private messageService: MessageService, private sharedService: SharedService) {
    this.item = new item();
  }

  ngOnInit(): void {

  }

  // Function to handle the form submission
  showDetails() {
    this.sharedService.customPostApi("ItemMaster", this.item)
      .subscribe((data: any) => {
        if (data != null) {
          this.showMessage('success', 'Item Save Successfully');
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
  }

  showMessage(type: string, message: string) {
    this.messageService.add({ severity: type, summary: message });
  }

  clearForm() {
    this.item = new item();
    this.loading = false;
  }
}
