import { MutationTree } from "vuex";
import { GameState, Game } from "./types";

export const mutations: MutationTree<GameState> = {
  gameLoaded(state, game: Game[]) {
    state.error = false;
    state.games = game;
  },
  gameLoadedCurrentPage(state, currentPage: number) {
    state.error = false;
    state.currentPage = currentPage;
  },
  gameLoadedMaxPage(state, maxPage: number) {
    state.error = false;
    state.maxPage = maxPage;
  },
  gameError(state) {
    state.error = true;
    state.games = undefined;
  },
};
