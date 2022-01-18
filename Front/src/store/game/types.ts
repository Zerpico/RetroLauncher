export interface Game {
  id: number;
  name: string;
  alternative: string;
  year: string;
  publisher: string;
  annotation: string;
  genres: [];
  platform: number;
  rating: number;
  links: [];
}

export interface GameState {
  games?: Game[];
  currentPage: number;
  maxPage: number;
  error: boolean;
}

export interface GameRequest {
  name: string;
  genre: number | undefined | null;
  platform: number | undefined | null;
}