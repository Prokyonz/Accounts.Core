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