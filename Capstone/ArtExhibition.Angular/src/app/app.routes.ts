import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import{ProfileComponent} from'./components/profile/profile.component';
import { ArtworkUploadComponent } from './components/artist/artist.component';
import { CreateGalleryComponent } from './components/create-gallery/create-gallery.component';
import { HeaderComponent } from './components/header/header.component';

import { GalleryComponent } from './components/gallery/gallery.component';

import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'artist', component: ArtworkUploadComponent  },
  {path:'gallery',component:GalleryComponent},
  { path: 'create-gallery', component: CreateGalleryComponent  },

  { path: '', redirectTo: '/login', pathMatch: 'full' }

];

@NgModule({
  
  imports: [RouterModule.forRoot(routes), HttpClientModule, 
    FormsModule],
  exports: [RouterModule]
})
export class AppRoutingModule {}
