export class Customer {
    id: number = 0;
    firstName: string = '';
    lastName: string = '';
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
    dealerName: string;
    description: string = '';
    brokerId: number = 0;
    discountAmount: number = 0;
    billAmount: number = 0;
    purchaseDetails: purchaseItems[];
    createdDate: Date = new Date();
    createdBy: number = 0;
    updatedDate: Date = new Date();
    updatedBy: number = 0;
}

export class purchaseItems {
    id: number = 0;
    itemId: number = 0;
    itemDescription: string = '';
    quantity: number = 1;
    rate: number = 0;
    gSTAmount: number = 0;
    gSTPer: number = 0;
    sGST: number = 0;
    cGST: number = 0;
    total: number = 0;
    createdDate: Date = new Date();
    createdBy: number = 0;
    updatedDate: Date = new Date();
    updatedBy: number = 0;
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
    permissions: permissions[];
}

export class permission {
    displayName: string;
    keyName: string;
}

export class permissions {
    userId: number;
    keyName: string;
}

export class sale {
    id: number = 0;
    customerId: number = 0;
    invoiceDate: Date = new Date();
    entryDate: Date = new Date();
    discountAmount: number = 0;
    amount: number = 0;
    salesDetails: salesDetails[];
    amountReceived: amountReceived[];
    createdDate: Date = new Date();
    createdBy: number = 0;
    updatedDate: Date = new Date();
    updatedBy: number = 0;
}

export class salesDetails {
    id: number = 0;
    salesMasterId: number = 0;
    itemId: number = 0;
    carratQty: number = 1;
    rate: number = 0;
    total: number = 0;
    sgst: number = 0;
    cgst: number = 0;
    igst: number = 0;
    gstper: number = 18;
    rowNum: number = 0;
    totalAmount: number = 0;
    createdDate: Date = new Date();
    createdBy: number = 0;
    updatedDate: Date = new Date();
    updatedBy: number = 0;
}

export class amountReceived {
    id: number = 0;
    salesMasterId: number = 0;
    paymentMode: string = 'Cash';
    cardNo: string = '';
    nameOnCard: string = '';
    amount: number = 0;
    createdDate: Date = new Date();
    createdBy: number = 0;
    updatedDate: Date = new Date();
    updatedBy: number = 0;
}

export class item {
    id: number = 0;
    name: string = '';
    description: string = '';
    hsnCode: string = '';
    gstPercentage: number = 0;
    isActive: boolean = true;
    createdDate: Date = new Date();
    createdBy: number = 0;
    updatedDate: Date = new Date();
    updatedBy: number = 0;
}

export class purchaseReport {
    purchaseSlipNo: number = 0;
    invoiceDate: Date = new Date();
    dealerName: string = '';
    totalItems: number = 0;
    billAmount: number = 0;
}

export class saleReport {
    saleSlipNo: number = 0;
    invoiceDate: Date = new Date();
    partyName: string = '';
    totalItems: number = 0;
    billAmount: number = 0;
}

export class stockReport {
    rowNum: number = 0;
    itemId: number = 0;
    name: string = '';
    quantity: number = 0;
    rate: number = 0;
    gstPer: number = 0;
}