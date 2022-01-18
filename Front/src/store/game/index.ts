// profile/index.ts
import { Module } from "vuex";
import { getters } from "./getters";
import { actions } from "./actions";
import { mutations } from "./mutations";
import { GameState } from "./types";
import { RootState } from "../types";

export const state: GameState = {
  games: undefined,
  currentPage: 0,
  maxPage: 0,
  error: false,
};

//const namespaced = true;

export const game: Module<GameState, RootState> = {
  state,
  getters,
  actions,
  mutations,
};
