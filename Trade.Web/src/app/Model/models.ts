export class Customer {
    name: string;
    address: string;
    mobile: string;
    email: string;
    aadharcardNo: string;
    pancardNo: string;
    imageAadharcardFront: string;
    imageAadharcardBack: string;
    imagePancard: string;
    imageSignature: string;
}

export class purchase {
    date: Date;
    party: string;
    description: string = '';
    billAmount: number = 0;
    items: purchaseItems[];
}

export class purchaseItems {
    itemName: string = '';
    itemdescription: string = '';
    qty: number = 1;
    rate: number = 0;
    gst: number = 0;
    total: number = 0;
}

export class user {
    id: number;
    parentUserId: number = 0;
    firstName: string = '';
    lastName: string = '';
    mobileNo: number = 0;
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