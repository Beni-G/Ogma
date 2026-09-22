import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import Keycloak from 'keycloak-js';

export const bearerTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const keycloak = inject(Keycloak);

  // Only attach header if authenticated and request is going to our API
  if (keycloak.authenticated && keycloak.token) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${keycloak.token}`,
      },
    });
    return next(authReq);
  }

  return next(req);
};
