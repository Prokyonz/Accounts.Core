import { Component, HostListener } from '@angular/core';
import { Filesystem, Directory, Encoding } from '@capacitor/filesystem';
import { Share } from '@capacitor/share';
import { Plugins } from '@capacitor/core';
const { Permissions} = Plugins;
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { Camera, CameraResultType, CameraSource } from '@capacitor/camera';

@Component({
  selector: 'app-sale-bill',
  templateUrl: './sale-bill.component.html',
  styleUrls: ['./sale-bill.component.scss']
})
export class SaleBillComponent {
  PageTitle: string = "Sale Bill";
  isMobile: boolean = false;
  loading = false;

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
  async clickme() {
    const element = document.getElementById('content-to-export');
    if (element) {
      html2canvas(element).then(canvas => {
        // Convert canvas to image
        const imgData = canvas.toDataURL('image/png');
        console.log("Captured Image Data URL:", imgData);

        // Create a new PDF document
        const pdf = new jsPDF('l', 'mm', 'a4'); // Portrait, millimeters, A4 size

        // Calculate dimensions to fit the image in an A4 page
        const pdfWidth = pdf.internal.pageSize.getWidth();
        const pdfHeight = pdf.internal.pageSize.getHeight();

        // Add image to PDF (fit to A4 size)
        pdf.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight);

        // Save the PDF
        pdf.save('exported-document.pdf');
      });
    }
  }

  async generatePdfFromBase64(base64data: string) {
    // Create a new PDF document
    const pdf = new jsPDF('p', 'mm', 'a4'); // Portrait, millimeters, A4 size

    // Calculate dimensions to fit the image in an A4 page
    const pdfWidth = pdf.internal.pageSize.getWidth();
    const pdfHeight = pdf.internal.pageSize.getHeight();

    // Add the base64 image to the PDF
    pdf.addImage(base64data, 'PNG', 0, 0, pdfWidth, pdfHeight);

    // Save the PDF
    //pdf.save('exported-document.pdf');


    const pdfBlob = pdf.output('blob');

    // Use Capacitor Filesystem to save the PDF on the device
    const fileName = 'invoice.pdf';
    // pdf.save(fileName);
    // return;
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
  }

  //Final Method to pic & share image
  shapePic(){
    Camera.getPhoto({
      resultType: CameraResultType.Uri, // We will use the URI for the image
      source: CameraSource.Prompt, // Prompt user to choose between Camera or Gallery
      quality: 90, // Optional: Adjust image quality
      allowEditing: false, // Optional: Set to true if you want users to edit the image
      width: 600, // Optional: Set image width
      height: 600, // Optional: Set image height
    }).then((photo) => {
      console.log(photo);
      alert('Photo pic');
      try {
        // Attempt to share the PDF file (this will open the native sharing options, including PDF viewer)
        Share.share({
          title: 'Invoice PDF',
          text: 'Here is your invoice.',
          url: photo.path,
          dialogTitle: 'Share Pic/',
        });
        alert('Photo pic done');
      } catch (err) {
        console.error('Error sharing PDF:', err);
        alert('Photo pic error');
      }
    }).catch((error) => {
      console.error('Error picking photo:', error);
      alert('Photo pic error1');
    });
  }

  async sharePDF() {
    this.loading = true;
    const element = document.getElementById('content-to-export');
    if (element) {
      // Use html2canvas to capture the content of the div as a canvas
      html2canvas(element).then(async (canvas) => {
        // Create a new jsPDF instance
        const pdf = new jsPDF('p', 'mm', 'a4'); // Portrait, millimeters, A4 size

        // Convert the canvas to an image and add it to the PDF
        const imgData = canvas.toDataURL('image/png');
        const imgWidth = 794; // Number(element.style.width); // A4 width in mm
        const imgHeight = canvas.height * imgWidth / canvas.width; // Scale the height accordingly
        const scaleFactor = 2; //this.isMobile ? 0.75 : 2; 
        const scaledImgHeight = imgHeight * scaleFactor;
        pdf.addImage(imgData, 'PNG', 0, 0, imgWidth, scaledImgHeight);

        // Convert PDF to blob for mobile handling
        const pdfBlob = pdf.output('blob');

        // Use Capacitor Filesystem to save the PDF on the device
        const fileName = 'invoice.pdf';
        if (!this.isMobile) {
          pdf.save(fileName);
        }
        else {
          const requestPermissions = async () => {
            const permissionStatus = await Filesystem.requestPermissions();
            console.log('Permission requested:', permissionStatus);
            return permissionStatus.publicStorage === 'granted';
          };
          Filesystem.writeFile({
            path: fileName,
            data: await this.convertBlobToBase64(pdfBlob),
            directory:  Directory.Cache,//Directory.Documents, // Save to documents directory
            //encoding: Encoding.UTF8,
          })
            .then(async (writeFileResult) => {
              const fileUri = await Filesystem.getUri({
                directory: Directory.Cache,
                path: fileName,
              });
              // On success, share the PDF file
              try {
                // Attempt to share the PDF file (this will open the native sharing options, including PDF viewer)
                await Share.share({
                  title: 'Invoice PDF',
                  text: 'Here is your invoice.',
                  url: fileUri.uri,//writeFileResult.uri,
                  dialogTitle: 'Share PDF',
                });
              } catch (err) {
                console.error('Error sharing PDF:', err);
                this.loading = false;
              }
            })
            .catch((error) => {
              console.error('Error writing file to device', error);
              this.loading = false;
            });
        }
      });
    }
    this.loading = false;
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
