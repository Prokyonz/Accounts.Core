import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { SharedService } from 'src/app/common/shared.service';
import { series } from 'src/app/Model/models';

@Component({
  selector: 'app-series',
  templateUrl: './series.component.html',
  styleUrls: ['./series.component.scss']
})
export class SeriesComponent {
  PageTitle = 'Series Details';
  seriesItem: series;
  loading: boolean = false;
  isSaveButton: boolean = false;
  isEditMode = false;
  seriesItemData: series[];

  constructor(private route: ActivatedRoute, private fb: FormBuilder, private router: Router, private messageService: MessageService, private sharedService: SharedService) {
    this.seriesItem = new series();
  }


  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const itemId = params.get('seriesId'); // Assuming 'id' is the parameter name in your route

      if (itemId) {
        this.isEditMode = true;
        this.isSaveButton = true;
        this.loadItem(itemId); // Fetch the item by ID if editing
      } else {
        this.isEditMode = false;
        this.getItem();
      }
    });
  }

  loadItem(seriesId: string) {
    this.loading = true;
    this.sharedService.customGetApi1<series[]>('SeriesMaster/GetSeriesMaster/' + seriesId).subscribe(
      (data: any) => {
        this.seriesItem = data; // Data is directly returned here as an array of User objects
        this.isEditMode = true;
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching series details:', error);
      }
    );
  }

  myfunction() {
    if (this.isSaveButton) {
      this.isSaveButton = false;
      this.PageTitle = "Series Details";
      this.clearForm();
      this.router.navigate(["series"]);
    }
    else {
      this.router.navigate(["dashboard"]);
    }
  }

  onAddIconClick() {
    this.PageTitle = "Add Series"
    this.isSaveButton = true;
  }

  getItem() {
    this.loading = true;
    this.sharedService.customGetApi1<series[]>('SeriesMaster').subscribe(
      (data: series[]) => {
        this.seriesItemData = data; // Data is directly returned here as an array of User objects
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.showMessage('Error fetching series details:', error);
      }
    );
  }

  // Function to handle the form submission
  showDetails() {
    if (this.seriesItem.name.length == 0) {
      this.showMessage('error', 'Please enter Series Name');
      return;
    }

    if (this.isEditMode) {
      this.updateItem();
    } else {
      this.createItem();
    }
    this.router.navigate(['/series']);
  }

  createItem() {
    this.sharedService.customPostApi("SeriesMaster", this.seriesItem)
      .subscribe((data: any) => {
        if (data != null) {
          this.showMessage('success', 'Series Save Successfully');
          this.clearForm();
          this.getItem();
        }
        else {
          this.loading = false;
          this.showMessage('error', 'Something went wrong...');
        }
      }, (ex: any) => {
        this.loading = false;
        this.showMessage('error', ex);
      });
  }

  updateItem() {
    this.sharedService.customPutApi("SeriesMaster", this.seriesItem)
      .subscribe((data: any) => {
        if (data != null) {
          this.showMessage('success', 'Series Updated Successfully');
          this.clearForm();
          this.getItem();
        }
        else {
          this.loading = false;
          this.showMessage('error', 'Something went wrong...');
        }
      }, (ex: any) => {
        this.loading = false;
        this.showMessage('error', ex);
      });
  }

  deleteItem(item: any) {
    const confirmDelete = window.confirm('Are you sure you want to delete this item?');
    if (confirmDelete) {
      this.sharedService.customDeleteApi('SeriesMaster/' + item.id).subscribe(
        (response: any) => {
          this.showMessage('success', 'Series Deleted Successfully');
          this.getItem();
        },
        (error) => {
          this.showMessage('error', 'Something went wrong...');
        }
      );
    }
  }

  showMessage(type: string, message: string) {
    this.messageService.add({ severity: type, summary: message });
  }

  clearForm() {
    this.seriesItem = new series();
    this.isEditMode = false;
    this.isSaveButton = false;
    this.loading = false;
  }
}
