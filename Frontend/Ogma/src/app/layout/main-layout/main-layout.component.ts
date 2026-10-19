  import { Component, computed, inject, OnInit, signal, Signal } from '@angular/core';
  import { AuthService } from '../../core/auth/services/auth.service';
  import { RouterLink, RouterOutlet } from '@angular/router';
  import { Menubar } from 'primeng/menubar';
  import { MenuItem } from 'primeng/api';
  import { Menu } from 'primeng/menu';
  import { PrimeTemplate } from 'primeng/api';
  import { ButtonModule } from 'primeng/button';
  import { DrawerModule } from 'primeng/drawer';
  import { UserProfile } from '../../core/auth/models/user-profile.model';

  @Component({
    selector: 'app-main-layout',
    imports: [RouterOutlet, RouterLink, Menubar, Menu, PrimeTemplate, ButtonModule, DrawerModule],
    templateUrl: './main-layout.component.html',
    styleUrl: './main-layout.component.scss'
  })
  export class MainLayoutComponent implements OnInit {
    private readonly authService = inject(AuthService);

    readonly userProfile: Signal<UserProfile | null> = this.authService.currentUser;
    
    readonly displayName = computed<string>(() => {
      const profile = this.userProfile(); 
      if (!profile) return 'Guest';
      
      if (profile.firstName || profile.lastName) {
        return `${profile.firstName ?? ''} ${profile.lastName ?? ''}`.trim();
      }
      return profile.username ?? profile.email ?? 'User';
    });

  navItems = signal<MenuItem[]>([]);
  userMenuItems = signal<MenuItem[]>([]);
  sidebarVisible = signal<boolean>(false);

  ngOnInit(): void {
    this.navItems.set([
      { label: 'Dashboard', icon: 'pi pi-home', routerLink: '/app/dashboard' }
    ]);

    this.userMenuItems.set([
      { 
        label: 'Logout', 
        icon: 'pi pi-power-off', 
        command: () => this.authService.logout() 
      }
    ]);
  }

  toggleSidebar() {
    this.sidebarVisible.update(visible => !visible);
  }

  }
