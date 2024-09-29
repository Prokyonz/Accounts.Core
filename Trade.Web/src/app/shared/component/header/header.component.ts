import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})

export class HeaderComponent implements OnInit {
  @Input() title:string = "MyHeader";
  @Input() iconName:string = "pi-align-justify";
  @Input() leftIconName:string = "pi-align-justify";
  @Input() showSideBar:boolean = false;
  @Output() onClickMainIcon = new EventEmitter();
  @Output() onClickLeftIcon = new EventEmitter();

  sidebarVisible!: boolean;
  items: MenuItem[] = [
    {
        label: 'Home',
        icon: 'pi pi-fw pi-file',
        routerLink: "/dashboard",
        command: () => {
          this.sidebarVisible = false;  
        }
    },
    {
        label: 'Calculator',
        icon: 'pi pi-fw pi-calculator',
        routerLink: "/viewcts",
        expanded: false,
        command: () => {
          this.sidebarVisible = false;
        }
    },
    {
        label: 'Reports',        
        expanded: true,
        items: [
            {
                label: 'Purchase',
                icon: 'pi pi-fw pi-user-plus',
                routerLink: "/report/1",
                command: () => {
                  this.sidebarVisible = false;
                }
            },
            {
                label: 'Sales',
                icon: 'pi pi-fw pi-user-minus',
                routerLink: "/report/2",
                command: () => {
                  this.sidebarVisible = false;
                }
            },
            {
                label: 'Payment',
                icon: 'pi pi-fw pi-users',
                routerLink: "/report/3",
                command: () => {
                  this.sidebarVisible = false;
                }
            },
            {
                label: 'Receipt',
                icon: 'pi pi-fw pi-users',
                routerLink: "/report/4",
                command: () => {
                  this.sidebarVisible = false;
                }
            }
        ]
    },
    {
        label: 'Settings',
        icon: 'pi pi-spin pi-cog',
        expanded: true,
        items: [
            {
              label: 'Change Company',
              icon: 'pi pi-fw pi-user-plus',
              routerLink: "/companyselection/header",
              command: () => {
                this.sidebarVisible = false;
              }
          },
        ]
    },
    {
        label: 'Logout',
        icon: 'pi pi-fw pi-calendar',
        routerLink: "/login",
        command: () => {
          this.sidebarVisible = false;
        }
    }
  ];

  ngOnInit(): void {
    
  }  

  iconClick() {
    this.sidebarVisible = this.showSideBar;
    this.onClickMainIcon.emit();
  }

  leftIconClick() {
    this.onClickLeftIcon.emit();
  }
}