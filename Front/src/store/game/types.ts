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
  links: Link[];
}

export interface Link {
  url: string;
  type: string;
}

export interface GameState {
  games?: Game[];
  currentPage: number;
  maxPage: number;
  error: boolean;
}

export interface GameRequest {
  name: string | undefined | null;
  page: string | undefined | null;
  genre: number | undefined | null;
  platform: number | undefined | null;
}
