import { GetterTree } from "vuex";
import { GameState } from "./types";
import { RootState } from "../types";

export const getters: GetterTree<GameState, RootState> = {
  gamelist(state) {
    const { games } = state;   
    return games;
  },
};
