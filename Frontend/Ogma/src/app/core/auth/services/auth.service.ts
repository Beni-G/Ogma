import { inject, Injectable, signal } from '@angular/core';
import Keycloak from 'keycloak-js';
import { UserProfile } from '../models/user-profile.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private keycloak = inject(Keycloak);

  // Reactive state using Angular Signals
  currentUser = signal<UserProfile | null>(null);

  constructor() {
    if (this.keycloak.authenticated) {
      this.loadUserProfile();
    }
  }

  // Get raw JWT Access Token (for manual API headers if needed)
  get token(): string | undefined {
    return this.keycloak.token;
  }

  // Check user authentication status
  get isAuthenticated(): boolean {
    return !!this.keycloak.authenticated;
  }

  // Load profile details from ID token / Keycloak
  async loadUserProfile(): Promise<UserProfile | null> {
    if (!this.keycloak.authenticated) return null;

    const profile = await this.keycloak.loadUserProfile();
    const user: UserProfile = {
      username: profile.username,
      email: profile.email,
      firstName: profile.firstName,
      lastName: profile.lastName,
    };

    this.currentUser.set(user);
    return user;
  }

  // Check if user has a specific realm role
  hasRole(role: string): boolean {
    return this.keycloak.hasRealmRole(role);
  }

  // Logout and clear Keycloak session
  logout(): void {
    this.keycloak.logout({
      redirectUri: window.location.origin,
    });
  }
}
