export interface Platform {
  id: number;
  name: number;
  alias: number;
}

export interface PlatformState {
  platforms?: Platform[];
  error: boolean;
  errorMessage: string;
}
