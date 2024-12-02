export class Customer {
    id: number = 0;
    firstName: string = '';
    lastName: string= '';
    address: string = '';
    mobileNo: string = '';
    emailId: string = '';
    aadharNo: string = '';
    panNo: string = '';
    aadharImageFrontData: string = '';
    aadhbarImageBackData: string = '';
    aadharImageFileName: string = '';
    panImageFileName: string = '';
    panImageData: string = '';
    signatureFileName: string = '';
    signatureImageData: string = '';
    createdDate: Date = new Date();
    createdBy: number = 0;
    updatedDate: Date = new Date();
    updatedBy: number = 0;
}

export class purchase {
    id: number = 0;
    purchaseSlipNo: number = 0;
    invoiceDate: Date = new Date();
    EntryDate: Date = new Date();
    customerId: string;
    description: string = '';
    brokerId: number = 0;
    discountAmount: number = 0;
    billAmount: number = 0;
    purchaseDetails: purchaseItems[];
}

export class purchaseItems {
    id: number = 0;
    itemId: number = 0;
    itemDescription: string = '';
    quantity: number = 1;
    rate: number = 0;
    gSTAmount: number = 0;
    total: number = 0;
}

export class user {
    id: number = 0;
    parentUserId: number = 0;
    firstName: string = '';
    lastName: string = '';
    mobileNo: string = '';
    emailId: string = '';
    password: string = '';
    isAgent: boolean = false;
    createdDate: Date = new Date();
    createdBy: number = 0;
    updatedDate: Date = new Date();
    updatedBy: number = 0;
}

export class sale {
    date: Date;
    party: string = '';
    billAmount: number = 0;
    paymentMode: string = 'Cash';
    creditCardNo: string = '';
    creditCardPaidAmount: string = '';
    cashAmount: number = 0;
    items: saleItems[];
}

export class saleItems {
    itemName: string = '';
    qty: number = 1;
    rate: number = 0;
    total: number = 0;
    sgst: number = 0;
    cgst: number = 0;
    igst: number = 0;
    grandTotal: number = 0;
}

export class item {
    id: number = 0;
    name: string = '';
    description: string = '';
    createdDate: Date = new Date();
    createdBy: number = 0;
    updatedDate: Date = new Date();
    updatedBy: number = 0;
}