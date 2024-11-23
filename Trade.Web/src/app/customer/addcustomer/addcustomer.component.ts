import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { SharedService } from 'src/app/common/shared.service';
import { Customer } from 'src/app/Model/models';

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
  selectedFile: File | null = null;
  imageUrl: string | null = null;
  currentDocumentType: string = '';
  // customerDetails = {
  //   name: '',
  //   address: '',
  //   mobile: '',
  //   email: '',
  //   aadhar: '',
  //   pancard: '',
  //   imageAadharcardFront: '',
  //   imageAadharcardBack: '',
  //   imagePancard: '',
  //   imageSignature: '',
  // };
  customerDetails: Customer;

  constructor(private fb: FormBuilder, private router: Router, private messageService: MessageService, private sharedService: SharedService) {
    this.customerDetails = new Customer();
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

  // Show upload component method
  showUploadComponent() {
    this.isUploadVisible = true;
  }

  // Show details method
  showDetails() {
    var a = this.customerDetails;
    this.showMessage('success','Customer details added successfully');
  }

  isFormValid() {
    return this.customerDetails.name && this.customerDetails.mobile && this.customerDetails.email;
  }

  clearForm() {
    this.loading = false;
  }

  triggerFileInput(documentType: string) {
    this.currentDocumentType = documentType;
    this.imageUrl = '';
    if (this.currentDocumentType == 'AadharcardFront' && this.customerDetails.imageAadharcardFront != undefined) {
      this.imageUrl = this.customerDetails.imageAadharcardFront;
      this.isUploadVisible = true;
      return;
    }
    else if (this.currentDocumentType == 'AadharcardBack' && this.customerDetails.imageAadharcardBack != undefined) {
      this.imageUrl = this.customerDetails.imageAadharcardBack;
      this.isUploadVisible = true;
      return;
    }
    else if (this.currentDocumentType == 'Pancard' && this.customerDetails.imagePancard != undefined) {
      this.imageUrl = this.customerDetails.imagePancard;
      this.isUploadVisible = true;
      return;
    }
    else if (this.currentDocumentType == 'Signature' && this.customerDetails.imageSignature != undefined) {
      this.imageUrl = this.customerDetails.imageSignature;
      this.isUploadVisible = true;
      return;
    }
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    fileInput.click();
  }

  onFileSelected(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length) {
      this.selectedFile = target.files[0];
      this.onUpload(false);
      this.isUploadVisible = true;
    }
  }

  onUpload(isUpload: boolean): void {
    if (this.selectedFile) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        // Convert ArrayBuffer to binary string
        const arrayBuffer = e.target.result as ArrayBuffer;
        const binaryData = new Uint8Array(arrayBuffer);
        this.imageUrl = this.arrayBufferToBase64(binaryData); // Convert to base64

        if (isUpload) {
          if (this.currentDocumentType == 'AadharcardFront') {
            this.customerDetails.imageAadharcardFront = this.imageUrl;
          }
          else if (this.currentDocumentType == 'AadharcardBack') {
            this.customerDetails.imageAadharcardBack = this.imageUrl;
          }
          else if (this.currentDocumentType == 'Pancard') {
            this.customerDetails.imagePancard = this.imageUrl;
          }
          else if (this.currentDocumentType == 'Signature') {
            this.customerDetails.imageSignature = this.imageUrl;
          }
          this.isUploadVisible = false;
        }
      };
      reader.readAsArrayBuffer(this.selectedFile); // Read the file as ArrayBuffer
    }
  }

  arrayBufferToBase64(buffer: Uint8Array): string {
    let binary = '';
    const len = buffer.byteLength;
    for (let i = 0; i < len; i++) {
      binary += String.fromCharCode(buffer[i]);
    }
    return 'data:image/jpeg;base64,' + btoa(binary); // Add the appropriate MIME type
  }

  onDialogClose() {
    this.isUploadVisible = false;
  }

  showMessage(type: string, message: string){
    this.messageService.add({severity: type, summary:message});
  }
}
