import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { SharedService } from '../common/shared.service';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { user } from '../Model/models';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent {
  PageTitle: string = "Add User";
  loading: boolean = false;
  user: user;
  userList = [
    { id: 1, name: 'Dhanani Anand' },
    { id: 2, name: 'Dhanani Monika' },
    { id: 3, name: 'Sharma Mayur' },
  ];

  constructor(private fb: FormBuilder, private router: Router, private messageService: MessageService, private sharedService: SharedService) {
    this.user = new user();
    this.getUsers();
  }

  ngOnInit(): void {
    
  }

  // getUsers(){
  //   this.sharedService.customGetApi("UserMaster").subscribe((t) => {
  //     if (t.success == true){
  //       if (t.data != null && t.data.length > 0){
  //         t.data = [
  //           { name: '-Select-', id: '' },
  //           ...t.data
  //         ];
  //         this.userList = t.data;
  //       }
  //     }
  //   }); 
  // }

  getUsers() {
    this.sharedService.customGetApi("UserMaster").subscribe({
      next: (response) => {
        if (response.success) {
          if (response.data && response.data.length > 0) {
            // Add a default item to the list (without mutating the original data)
            this.userList = [
              { name: '-Select-', id: '' },
              ...response.data
            ];
          }
        }
      },
      error: (err) => {
        console.error('Error fetching users', err);
        // Handle error appropriately
      }
    });
  }

  showDetails() {
    var a = this.user;
    this.sharedService.customPostApi("UserMaster",this.user)
          .subscribe((data: any) => {
                if (data.success == true){                  
                  this.showMessage('success',data.message);
                  this.clearForm();
                }
                else{
                  this.loading = false;
                  this.showMessage('error','Something went wrong...');
                }
              }, (ex: any) => {
                this.loading = false;
                this.showMessage('error',ex);
            });

    this.showMessage('success','User details added successfully');
  }

  showMessage(type: string, message: string){
    this.messageService.add({severity: type, summary:message});
  }

  clearForm(){
    this.user = new user();
    this.loading = false;
  }
}
