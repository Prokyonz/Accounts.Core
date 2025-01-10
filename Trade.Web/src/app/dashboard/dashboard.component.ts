import { Component, OnInit } from '@angular/core';
import { color } from 'html2canvas/dist/types/css/types/color';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})

export class DashboardComponent implements OnInit {
  dashBoardReportItems: any[] = [];

  constructor(){}

  ngOnInit(): void {
    this.getDashBoardData();
  }

  openMenu() {
  }

  onSeach(event: any) {
    console.log(event);
  }

  getDashBoardData(): void {
    this.dashBoardReportItems.push(
      {
        label: '',  // Group label
        children: [  // Child items under the group
          {
            label: 'Customer',
            icon: 'pi pi-fw pi-user-plus',
            routerLink: "/addcustomer",
            color: '#4CAF50', // Vibrant green for positivity
            index: 1
          },
          {
            label: 'Sale',
            icon: 'pi pi-fw pi-chart-line',
            routerLink: "/sale",
            color: '#FF5722', // Bright orange for dynamic action
            index: 2
          }
        ]
      },
      {
        label: 'Report',  // Group label 
        children: [  // Child items under the group
          {
            label: 'Stock',
            icon: 'pi pi-fw pi-list',
            routerLink: "/report/5",
            color: '#607D8B', // Cool gray-blue for data
            index: 1
          },
          {
            label: 'Sale',
            icon: 'pi pi-fw pi-chart-line',
            routerLink: "/report/4",
            color: '#E91E63', // Bold pink for emphasis
            index: 2
          },
          {
            label: 'Purchase',
            icon: 'pi pi-fw pi-shopping-cart',
            routerLink: "/report/3",
            color: '#03A9F4', // Calm blue for reliability
            index: 3
          },
          {
            label: 'Customer',
            icon: 'pi pi-fw pi-user-plus',
            routerLink: "/report/1",
            color: '#8BC34A', // Fresh green for connection
            index: 4
          },
          {
            label: 'User',
            icon: 'pi pi-fw pi-user',
            routerLink: "/report/2",
            color: '#FFC107', // Energetic yellow for activity
            index: 5
          }
        ]
      },
      {
        label: 'Master',  // Group label
        children: [  // Child items under the group
          {
            label: 'User',
            icon: 'pi pi-fw pi-user',
            routerLink: "/user",
            color: '#FF9800', // Warm orange for priority
            index: 1
          },
          {
            label: 'Purchase',
            icon: 'pi pi-fw pi-shopping-cart',
            routerLink: "/purchase",
            color: '#3F51B5', // Solid blue for trust
            index: 2
          },
          {
            label: 'Item',
            icon: 'pi pi-fw pi-box',
            routerLink: "/additem",
            color: '#673AB7', // Rich purple for creativity
            index: 3
          },
          {
            label: 'POS',
            icon: 'pi pi-fw pi-credit-card',
            routerLink: "/pos",
            color: '#009688', // Teal for technology
            index: 4
          },
          {
            label: 'Series',
            icon: 'pi pi-fw pi-tag',
            routerLink: "/series",
            color: '#795548', // Earthy brown for categorization
            index: 5
          }
        ]
      });        
  }
}
