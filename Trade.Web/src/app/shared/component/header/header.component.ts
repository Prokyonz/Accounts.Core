import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { user } from 'src/app/Model/models';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})

export class HeaderComponent implements OnInit {
  @Input() title: string = "MyHeader";
  @Input() iconName: string = "pi-align-justify";
  @Input() leftIconName: string = "pi-align-justify";
  @Input() showSideBar: boolean = false;
  @Output() onClickMainIcon = new EventEmitter();
  @Output() onClickLeftIcon = new EventEmitter();
  username: string = 'Demo User';
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
      label: 'Customer',
      icon: 'pi pi-fw pi-calculator',
      routerLink: "/addcustomer",
      expanded: false,
      command: () => {
        this.sidebarVisible = false;
      }
    },
    {
      label: 'Sale',
      icon: 'pi pi-fw pi-calculator',
      routerLink: "/sale",
      expanded: false,
      command: () => {
        this.sidebarVisible = false;
      }
    },
    // {
    //     label: 'Calculator',
    //     icon: 'pi pi-fw pi-calculator',
    //     routerLink: "/viewcts",
    //     expanded: false,
    //     command: () => {
    //       this.sidebarVisible = false;
    //     }
    // },
    {
      label: 'Reports',
      expanded: true,
      items: [
        {
          label: 'Customer',
          icon: 'pi pi-fw pi-user-plus',
          routerLink: "/report/1",
          command: () => {
            this.sidebarVisible = false;
          }
        },
        {
          label: 'User',
          icon: 'pi pi-fw pi-user-minus',
          routerLink: "/report/2",
          command: () => {
            this.sidebarVisible = false;
          }
        },
        // {
        //   label: 'Bank',
        //   icon: 'pi pi-fw pi-users',
        //   routerLink: "/report/3",
        //   command: () => {
        //     this.sidebarVisible = false;
        //   }
        // },
        // {
        //   label: 'Cash',
        //   icon: 'pi pi-fw pi-users',
        //   routerLink: "/report/4",
        //   command: () => {
        //     this.sidebarVisible = false;
        //   }
        // }
      ]
    },
    // {
    //   label: 'Settings',
    //   icon: 'pi pi-spin pi-cog',
    //   expanded: true,
    //   items: [
    //     {
    //       label: 'Change Company',
    //       icon: 'pi pi-fw pi-user-plus',
    //       routerLink: "/companyselection/header",
    //       command: () => {
    //         this.sidebarVisible = false;
    //       }
    //     },
    //   ]
    // },
    {
      label: 'Master',
      expanded: true,
      items: [
        {
          label: 'User',
          icon: 'pi pi-fw pi-user-plus',
          routerLink: "/user",
          command: () => {
            this.sidebarVisible = false;
          }
        },
        {
          label: 'Purchase',
          icon: 'pi pi-fw pi-user-plus',
          routerLink: "/purchase",
          command: () => {
            this.sidebarVisible = false;
          }
        },
        {
          label: 'Item',
          icon: 'pi pi-fw pi-user-plus',
          routerLink: "/additem",
          command: () => {
            this.sidebarVisible = false;
          }
        }
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
    var userDetail = localStorage.getItem('AuthorizeData');
    if (userDetail) {
      var user: user = JSON.parse(userDetail);
      this.username = user.firstName + ' ' + user.lastName; 
    }
  }

  iconClick() {
    this.sidebarVisible = this.showSideBar;
    this.onClickMainIcon.emit();
  }

  leftIconClick() {
    this.onClickLeftIcon.emit();
  }
}