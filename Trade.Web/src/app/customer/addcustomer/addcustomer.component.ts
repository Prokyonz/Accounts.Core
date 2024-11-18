import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { SharedService } from 'src/app/common/shared.service';

@Component({
  selector: 'app-addcustomer',
  templateUrl: './addcustomer.component.html',
  styleUrls: ['./addcustomer.component.scss'],
  providers: [MessageService]
})
export class AddcustomerComponent implements OnInit {
  PageTitle: string = "Add Customer";
  isUploadVisible = false;
  isSaveButton: boolean = true;
  loading: boolean = false;

  constructor(private router: Router, private messageService: MessageService, private sharedService: SharedService) {

  }

  ngOnInit(): void {
  }

  myfunction() {
    if (!this.isSaveButton) {
      this.isSaveButton = true;
      this.PageTitle = "History";
      this.clearForm();
    }
    else {
      this.router.navigate(["dashboard"]);
    }
  }

  showDetails() {
  }

  showUploadComponent(): void {
    this.isUploadVisible = true; // Show the upload component
  }

  clearForm() {
    this.loading = false;
  }
}
