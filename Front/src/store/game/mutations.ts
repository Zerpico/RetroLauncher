import { MutationTree } from "vuex";
import { GameState, Game } from "./types";

export const mutations: MutationTree<GameState> = {
  gameLoaded(state, game: Game[]) {
    state.error = false;
    state.games = game;
  },
  gameError(state) {
    state.error = true;
    state.games = undefined;
  },
};
