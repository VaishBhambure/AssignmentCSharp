import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { importProvidersFrom } from '@angular/core';
import { FormsModule } from '@angular/forms';  // Ensure FormsModule is imported
import { provideHttpClient, withFetch } from '@angular/common/http';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), // Add the router configuration
    provideHttpClient(withFetch()), // If you need HTTP client
    importProvidersFrom(FormsModule), provideAnimationsAsync(), // Add FormsModule to enable ngModel
  ]
};
