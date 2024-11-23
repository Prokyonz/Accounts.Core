import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { SharedService } from 'src/app/common/shared.service';

@Component({
  selector: 'app-additem',
  templateUrl: './additem.component.html',
  styleUrls: ['./additem.component.scss']
})
export class AdditemComponent implements OnInit{
  PageTitle = 'Add Item';
  item = {
    itemName: '',
    itemDescription: ''
  };

  constructor(private fb: FormBuilder, private router: Router, private messageService: MessageService, private sharedService: SharedService) {
    
  }

  ngOnInit(): void {
    
  }

  // Function to handle the form submission
  showDetails() {
    var a = this.item;
    this.showMessage('success','Items details added successfully');
  }

  showMessage(type: string, message: string){
    this.messageService.add({severity: type, summary:message});
  }
}
