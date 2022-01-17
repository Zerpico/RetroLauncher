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
  error: boolean;
}
