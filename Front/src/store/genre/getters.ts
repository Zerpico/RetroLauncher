import { GetterTree } from "vuex";
import { GenreState } from "./types";
import { RootState } from "../types";

export const getters: GetterTree<GenreState, RootState> = {
  genrelist(state) {
    const { genres } = state;   
    return genres;
  },
};
