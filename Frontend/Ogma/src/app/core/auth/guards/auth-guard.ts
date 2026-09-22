import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import Keycloak from 'keycloak-js';

export const canActivateAuth: CanActivateFn = async (route, state) => {
  const keycloak = inject(Keycloak);
  const router = inject(Router);

  // 1. Check if the user is authenticated via Keycloak
  if (keycloak.authenticated) {
    return true;
  }

  // 2. If not authenticated, redirect the browser to Keycloak's login screen
  // passing the attempted URL so Keycloak sends the user back after login
  const targetUrl = window.location.origin + state.url;

  await keycloak.login({
    redirectUri: targetUrl,
  });

  // 3. Cancel the current Angular routing attempt while browser redirects
  return false;
};
