import { ApplicationConfig, provideAppInitializer, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import Keycloak from 'keycloak-js';
import Aura from '@primeng/themes/aura';

import { routes } from './app.routes';
import { bearerTokenInterceptor } from './core/auth/interceptors/bearer-token-interceptor';
import { providePrimeNG } from 'primeng/config';

export const keycloak = new Keycloak({
  url: 'http://localhost:8080',
  realm: 'ogma',
  clientId: 'ogma-frontend'
});

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),

    // Deprecated in Angular 19+, but required by PrimeNG overlay triggers 
    // (@overlayAnimation) to prevent runtime error NG05105 on component destroy/logout.
    provideAnimationsAsync(),

    providePrimeNG({
      theme: {
        preset: Aura, 
        options: {
          darkModeSelector: 'system' // Or false / '.my-app-dark'
        }
      }
    }),
    
    // Attach the Bearer Token Interceptor to HttpClient
    provideHttpClient(
      withInterceptors([bearerTokenInterceptor])
    ),

    // Provide Keycloak for Dependency Injection (So guards & services can inject it)
    { 
      provide: Keycloak, 
      useValue: keycloak 
    },

    // Initialize Keycloak before Angular boots
    provideAppInitializer(() => {
      return keycloak.init({
        onLoad: 'check-sso',
        pkceMethod: 'S256', 
        silentCheckSsoRedirectUri: `${window.location.origin}/silent-check-sso.html`
      });
    })
  ]
};