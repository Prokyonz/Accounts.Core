import { Component } from '@angular/core';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';

@Component({
  selector: 'app-sale-bill',
  templateUrl: './sale-bill.component.html',
  styleUrls: ['./sale-bill.component.scss']
})
export class SaleBillComponent {
  PageTitle: string = "Sale Bill";
  exportToPDF() {
    // const element = document.getElementById('content-to-export');

    // if (element?.nodeName) {
    //   html2canvas(element).then(canvas => {
    //     const pdf = new jsPDF();
    //     const imgData = canvas.toDataURL('image/png');
    //     pdf.addImage(imgData, 'PNG', 10, 10, canvas.width / 5, canvas.height / 5);
    //     pdf.save('exported-content.pdf');
    //   });
    // } else {
    //   console.error('Element not found!');
    // }

    const element = document.getElementById('content-to-export');
    
    if (element) {
      // Use html2canvas to capture the content of the div as a canvas
      html2canvas(element).then((canvas) => {
        // Create a new jsPDF instance
        const pdf = new jsPDF('p', 'mm', 'a4'); // Portrait, millimeters, A4 size
        
        // Convert the canvas to an image and add it to the PDF
        const imgData = canvas.toDataURL('image/png');
        const imgWidth = 210; // A4 width in mm
        const imgHeight = canvas.height * imgWidth / canvas.width; // Scale the height accordingly

        // Add image to the PDF
        pdf.addImage(imgData, 'PNG', 0, 0, imgWidth, imgHeight);
        
        // Save the PDF
        pdf.save('invoice.pdf');
      });
    }
  }
}
