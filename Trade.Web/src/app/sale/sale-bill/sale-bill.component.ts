import { Component, HostListener } from '@angular/core';
import { Filesystem, Directory, Encoding } from '@capacitor/filesystem';
import { Share } from '@capacitor/share';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';

@Component({
  selector: 'app-sale-bill',
  templateUrl: './sale-bill.component.html',
  styleUrls: ['./sale-bill.component.scss']
})
export class SaleBillComponent {
  PageTitle: string = "Sale Bill";
  isMobile: boolean = false;

  constructor() {
    this.checkIfMobile();
  }
  // exportToPDF() {
  //   const element = document.getElementById('content-to-export');

  //   if (element) {
  //     // Use html2canvas to capture the content of the div as a canvas
  //     html2canvas(element).then((canvas) => {
  //       // Create a new jsPDF instance
  //       const pdf = new jsPDF('p', 'mm', 'a4'); // Portrait, millimeters, A4 size

  //       // Convert the canvas to an image and add it to the PDF
  //       const imgData = canvas.toDataURL('image/png');
  //       const imgWidth = 210; // A4 width in mm
  //       const imgHeight = canvas.height * imgWidth / canvas.width; // Scale the height accordingly

  //       // Add image to the PDF
  //       pdf.addImage(imgData, 'PNG', 0, 0, imgWidth, imgHeight);

  //       // Save the PDF
  //       pdf.save('invoice.pdf');
  //     });
  //   }
  // }

  async exportToPDF() {
    const element = document.getElementById('content-to-export');

    if (element) {
      // Use html2canvas to capture the content of the div as a canvas
      html2canvas(element).then(async (canvas) => {
        // Create a new jsPDF instance
        const pdf = new jsPDF('p', 'mm', 'a4'); // Portrait, millimeters, A4 size

        // Convert the canvas to an image and add it to the PDF
        const imgData = canvas.toDataURL('image/png');
        const imgWidth = 210; // A4 width in mm
        const imgHeight = canvas.height * imgWidth / canvas.width; // Scale the height accordingly
        const scaleFactor = this.isMobile ? 0.75 : 2; 
        const scaledImgHeight = imgHeight * scaleFactor;
        pdf.addImage(imgData, 'PNG', 0, 0, imgWidth, scaledImgHeight);
        // Add image to the PDF
        //pdf.addImage(imgData, 'PNG', 0, 0, imgWidth, imgHeight);

        // Convert PDF to blob for mobile handling
        const pdfBlob = pdf.output('blob');

        // Use Capacitor Filesystem to save the PDF on the device
        const fileName = 'invoice.pdf';
        pdf.save(fileName);
        return;
        if (!this.isMobile) {
          pdf.save(fileName);
        }
        else {
          Filesystem.writeFile({
            path: fileName,
            data: await this.convertBlobToBase64(pdfBlob),
            directory: Directory.Documents, // Save to documents directory
            encoding: Encoding.UTF8,
          })
            .then(async (writeFileResult) => {
              // On success, share the PDF file
              try {
                // Attempt to share the PDF file (this will open the native sharing options, including PDF viewer)
                await Share.share({
                  title: 'Invoice PDF',
                  text: 'Here is your invoice.',
                  url: writeFileResult.uri,
                  dialogTitle: 'Share PDF',
                });
              } catch (err) {
                console.error('Error sharing PDF:', err);
              }
            })
            .catch((error) => {
              console.error('Error writing file to device', error);
            });
        }
      });
    }
  }

  // Helper function to convert blob to base64 for saving the PDF
  convertBlobToBase64(blob: Blob): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onloadend = () => resolve(reader.result as string);
      reader.onerror = (error) => reject(error);
      reader.readAsDataURL(blob);
    });
  }

  checkIfMobile() {
    this.isMobile = window.innerWidth <= 768;  // Adjust the width as needed
  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.checkIfMobile();
  }
}
