import { MutationTree } from "vuex";
import { PlatformState, Platform } from "./types";

export const mutations: MutationTree<PlatformState> = {
  platformLoaded(state, payload: Platform[]) {
    state.error = false;
    state.platforms = payload;
  },
  platformError(state, message) {
    state.error = true;
    state.platforms = undefined;
    state.errorMessage = message;
  },
};
