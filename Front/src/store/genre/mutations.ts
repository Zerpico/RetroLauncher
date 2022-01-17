import { MutationTree } from "vuex";
import { GenreState, Genre } from "./types";

export const mutations: MutationTree<GenreState> = {
  genreLoaded(state, payload: Genre[]) {
    state.error = false;
    state.genres = payload;
  },
  genreError(state) {
    state.error = true;
    state.genres = undefined;
  },
};
