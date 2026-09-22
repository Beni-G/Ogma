  import { Component, computed, inject, OnInit, signal, Signal } from '@angular/core';
  import { AuthService } from '../../core/auth/services/auth.service';
  import { RouterOutlet } from '@angular/router';
  import { Menubar } from 'primeng/menubar';
  import { MenuItem } from 'primeng/api';
  import { Menu } from 'primeng/menu';
  import { PrimeTemplate } from 'primeng/api';
  import { ButtonModule } from 'primeng/button';
  import { UserProfile } from '../../core/auth/models/user-profile.model';

  @Component({
    selector: 'app-main-layout',
    imports: [RouterOutlet, Menubar, Menu, PrimeTemplate, ButtonModule],
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

  }
