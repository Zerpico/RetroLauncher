// profile/index.ts
import { Module } from "vuex";
import { getters } from "./getters";
import { actions } from "./actions";
import { mutations } from "./mutations";
import { PlatformState } from "./types";
import { RootState } from "../types";

export const state: PlatformState = {
  platforms: undefined,
  error: false,
  errorMessage: ""
};

//const namespaced = true;

export const platform: Module<PlatformState, RootState> = {
  state,
  getters,
  actions,
  mutations,
};
