// artwork.model.ts
export interface Artwork {
    artworkID: number;
    title: string;
    description: string;
    creationDate: string;
    imageURL: string;
    artistID: number;
    artist: any;  // If you know the structure of artist, replace 'any' with the correct type
    artworkGalleries: any;  // If you know the structure of galleries, replace 'any' with the correct type
    favoritedBy: any;  // Replace 'any' if needed
  }
  
  export interface SearchResponse {
    $id: string;
    $values: Artwork[];
  }
  