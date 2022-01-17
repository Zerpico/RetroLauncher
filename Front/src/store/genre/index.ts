// profile/index.ts
import { Module } from "vuex";
import { getters } from "./getters";
import { actions } from "./actions";
import { mutations } from "./mutations";
import { GenreState } from "./types";
import { RootState } from "../types";

export const state: GenreState = {
  genres: undefined,
  error: false,
};

//const namespaced = true;

export const genre: Module<GenreState, RootState> = {
  state,
  getters,
  actions,
  mutations,
};
