import Vue from "vue";
import Vuex from "vuex";
import { StoreOptions } from "vuex";
import { RootState } from "./types";
import { genre } from "./genre/index";
import { platform } from "./platform/index";
import { game } from "./game/index";

Vue.use(Vuex);

const store: StoreOptions<RootState> = {
  state: {
    version: "1.0.0", // a simple property
  },
  modules: {
    genre,
    platform,
    game
  },
};

export default new Vuex.Store<RootState>(store);
