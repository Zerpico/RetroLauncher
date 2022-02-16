export interface Genre {
  code: number;
  status: string;
  data: [];
}

export interface GenreState {
  genres?: Genre[];
  error: boolean;
}
