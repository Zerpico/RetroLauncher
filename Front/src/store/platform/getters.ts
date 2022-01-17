import { GetterTree } from "vuex";
import { PlatformState } from "./types";
import { RootState } from "../types";

export const getters: GetterTree<PlatformState, RootState> = {
  platformlist(state) {
    const { platforms } = state;   
    return platforms;
  },
};
